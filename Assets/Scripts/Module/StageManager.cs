using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    const int BaseInputGuideDelay = 120;     // 基本操作案内の遅延時間
    const int AdditionalInputGuideDelay = 7; // 追加操作案内の遅延時間

    public static bool canProgressGame = false;                     // ゲーム進行可能フラグ
    public static bool isInGameLoop { get; private set; } = false;  // ゲームループ中フラグ
    public ReactiveProperty<bool> isInGame = new(false);            // ゲーム中フラグ

    public float stageTimmer { get; private set; } = 0f; // ステージタイマー
    InputData[] inputDatas = new InputData[10]; // 入力データ配列
    [SerializeField] public StageInfoData stageInfo;
    [SerializeField] UnityEvent<int> onInput;
    [SerializeField] UnityEvent onInit;
    [SerializeField] MissUI missUI;
    [SerializeField] StageClearUI stageClearUI;

    CpuGnerator cpuGnerator;
    StageUI stageUI;
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
    }

    /// <summary>
    /// ステージ全体を初期化し開始する関数
    /// </summary>
    private async UniTask StageStart(CancellationToken token)
    {
        stageTimmer = stageInfo.TimeLimit;
        cpuGnerator.Init(stageInfo);
        onInit.Invoke();
        await stageUI.StageWindowPopuop(token);
        await UniTask.Delay(50, cancellationToken: token);
        await PlayInputGuide(token);
    }

    /// <summary>
    /// ミス処理を行う関数
    /// </summary>
    public async UniTask StageMiss(string reason,CancellationToken token)
    {
        isInGameLoop = false;
        canProgressGame = false;
        await missUI.PlayMiss(reason,token);
        isInGame.Value = false;
    }

    /// <summary>
    /// ステージクリア処理を行う関数
    /// </summary>
    public async UniTask StageClear(CancellationToken token)
    {
        isInGameLoop = false;
        await stageUI.StageWindowClose(token);
        await UniTask.Delay(200, cancellationToken: token);
        await stageClearUI.PlayHexCreate("6bab4dac5c809fc9bd5d3bea39c73d7d", token);
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
