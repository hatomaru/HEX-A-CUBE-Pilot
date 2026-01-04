using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class StageManager : MonoBehaviour
{
    const int BaseInputGuideDelay = 120;     // 基本操作案内の遅延時間
    const int AdditionalInputGuideDelay = 7; // 追加操作案内の遅延時間

    public static bool canProgressGame = false;                     // ゲーム進行可能フラグ
    public static bool isInGameLoop { get; private set; } = false;  // ゲームループ中フラグ
    public ReactiveProperty<bool> isInGame = new(false);            // ゲーム中フラグ

    public float stageTimmer { get; private set; } = 0f; // ステージタイマー
    public float performerTimer { get; set; } = 0f; // パフォーマータイマー

    InputData[] inputDatas = new InputData[10]; // 入力データ配列
    [SerializeField] public StageInfoData stageInfo;
    [SerializeField] UnityEvent<int> onInput;
    [SerializeField] UnityEvent onInit;
    [SerializeField] UnityEvent<Action<MagicData>> onMagicRequest;
    [SerializeField] UnityEvent onMagicRestore;
    [SerializeField] MissUI missUI;
    [SerializeField] StageClearUI stageClearUI;

    CpuGnerator cpuGnerator;
    StageUI stageUI;
    public AudioPlayer audioPlayer;
    public InputInstance[] inputInstances = new InputInstance[10]; // 入力インスタンス配列

    private void Awake()
    {
        cpuGnerator = GetComponent<CpuGnerator>();
        stageUI = GetComponent<StageUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public StageInfoData StageInfo => stageInfo;

    /// <summary>
    /// ゲームループ
    /// </summary>
    public async UniTask GameLoop(CancellationToken token)
    {
        // 操作案内をリセット
        foreach (var inputInstance in inputInstances)
        {
            if (inputInstance == null)
            {
                continue;
            }
            inputInstance.ResetInput();
        }
        cpuGnerator.syncConter.Reset();
        stageUI.ResetUI();
        await StageStart(token);
        isInGameLoop = true;
        isInGame.Value = true;
        while (!token.IsCancellationRequested)
        {
            if(!isInGameLoop)
            {
                return;
            }
            // タイマーが0以下になったらループを抜ける
            if (stageTimmer <= 0f)
            {
                break;
            }
            UpdateTimer();
            stageUI.UpdateUI();
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        await UniTask.Delay(300, cancellationToken: token);
        await StageMiss("Cpuが先に正解しました。",token);
    }

    /// <summary>
    /// タイマーを更新する関数
    /// </summary>
    private void UpdateTimer()
    {
        stageTimmer -= Time.deltaTime;
        performerTimer -= Time.deltaTime;
    }

    /// <summary>
    /// ステージ全体を初期化し開始する関数
    /// </summary>
    private async UniTask StageStart(CancellationToken token)
    {
        stageTimmer = stageInfo.TimeLimit;
        cpuGnerator.Init(stageInfo);
        onInit.Invoke();
        audioPlayer.PlayStageOpen();
        await stageUI.StageWindowPopuop(token);
        await UniTask.Delay(50, cancellationToken: token);
        await PlayInputGuide(token);
    }

    /// <summary>
    /// HEX生成機
    /// </summary>
    /// <param name="input">入力</param>
    /// <returns>md5後の数値</returns>
    public static string CalculateMD5(string input)
    {
        // MD5ハッシュオブジェクトを作成
        // Unity Editor上では`MD5.Create()`が推奨されることが多い
        using (MD5 md5 = MD5.Create())
        {
            // 入力文字列をバイト配列に変換
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // ハッシュ値を計算（16バイトの配列）
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // バイト配列を16進数文字列に変換
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2")); // "x2"で2桁の小文字16進数に
            }
            return sb.ToString(); // 32文字のMD5ハッシュ文字列
        }
    }

    /// <summary>
    /// ミス処理を行う関数
    /// </summary>
    public async UniTask StageMiss(string reason,CancellationToken token, InputData input = null)
    {
        cpuGnerator.SyncCpu(input, false);
        audioPlayer.PlayWarning();
        isInGameLoop = false;
        canProgressGame = false;
        await missUI.PlayMiss(reason,token);
        isInGame.Value = false;
    }

    /// <summary>
    /// ステージクリア処理を行う関数
    /// </summary>
    public async UniTask StageClear(CancellationToken token,InputData input)
    {
        cpuGnerator.SyncCpu(input, performerTimer >= 0);
        isInGameLoop = false;
        audioPlayer.PlayClear();
        await stageUI.StageWindowClose(token);
        cpuGnerator.AllDestoryCpu();
        await UniTask.Delay(200, cancellationToken: token);
        await stageClearUI.PlayHexCreate(CalculateMD5("Game#01" + input.Key.ToString() + input.KeyName), token);
        isInGame.Value = false;
    }

    /// <summary>
    /// 操作案内を表示する関数
    /// </summary>
    private async UniTask PlayInputGuide(CancellationToken token)
    {
        int enabledInputCount = 0;
        foreach (var inputInstance in inputInstances)
        {
            if (inputInstance != null)
            {
                enabledInputCount++;
                inputInstance.Init(true).Forget();
                //await UniTask.Delay(BaseInputGuideDelay + AdditionalInputGuideDelay * enabledInputCount, cancellationToken: token);
            }
        }
    }

    /// <summary>
    /// 入力を受け取る関数
    /// </summary>
    /// <param name="input">入力</param>
    public void OnInput(int input)
    {
        if(!isInGameLoop)
        {
            return;
        }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log("Input Number: " + input);
#endif
        if (inputInstances[input] != null)
        {
            inputInstances[input].OnPress(destroyCancellationToken).Forget();
        }
        onInput?.Invoke(input);
    }


    /// <summary>
    /// パフォーマーの魔法発動をリクエストし、成功した場合は魔法データを返す
    /// </summary>
    /// <returns>成功した場合は魔法データを返す</returns>
    public MagicData OnMagicRequest()
    {
        MagicData result = null;
        onMagicRequest?.Invoke(m => result = m);
        return result;
    }

    /// <summary>
    /// パフォーマーの魔法を元に戻す関数
    /// </summary>
    public void OnMagicRestore()
    {
        onMagicRestore?.Invoke();
    }

    /// <summary>
    /// お題の難易度を設定する関数
    /// </summary>
    /// <param name="level">お題の難易度</param>
    public void SetGameLevel(int level)
    {
        stageInfo.SetGameLevel(level);
    }

    /// <summary>
    /// お題の制限時間を設定する関数
    /// </summary>
    /// <param name="timeLimit">制限時間</param>
    public void SetStageTimeLimit(float timeLimit)
    {
        stageInfo.TimeLimit = timeLimit;
    }

    /// <summary>
    /// 答えを設定する関数
    /// </summary>
    public void SetAnswer(InputInstance collectAnswer,List<InputInstance> answerList)
    {
        cpuGnerator.SetAnswer(collectAnswer, answerList);
    }

    /// <summary>
    /// ステージ情報を取得する関数
    /// </summary>
    /// <returns>現在のステージ情報</returns>
    public StageInfoData GetStageInfo()
    {
        return stageInfo;
    }
}
