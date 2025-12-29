using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    const int BaseInputGuideDelay = 120; // 基本操作案内の遅延時間
    const int AdditionalInputGuideDelay = 7; // 追加操作案内の遅延時間
    public float stageTimmer { get; private set; } = 0f; // ステージタイマー
    InputData[] inputDatas = new InputData[10]; // 入力データ配列
    [SerializeField] StageInfoData stageInfo;
    [SerializeField] UnityEvent<int> onInput;
    [SerializeField] UnityEvent onInit;
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
        GameLoop(destroyCancellationToken).Forget();
    }

    /// <summary>
    /// ゲームループ
    /// </summary>
    private async UniTask GameLoop(CancellationToken token)
    {
        await StageStart(token);
        while (!token.IsCancellationRequested)
        {
            // タイマーが0以下になったらループを抜ける
            if (stageTimmer <= 0f)
            {
                break;
            }
            UpdateTimer();
        }
        await stageUI.StageWindowClose(token);
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
        onInit.Invoke();
        await stageUI.StageWindowPopuop(token);
        await UniTask.Delay(50, cancellationToken: token);
        await PlayInputGuide(token);
        cpuGnerator.Init(stageInfo);
        await UniTask.Delay(20000, cancellationToken: token);
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
                await UniTask.Delay(BaseInputGuideDelay + AdditionalInputGuideDelay * enabledInputCount, cancellationToken: token);
            }
        }
    }

    /// <summary>
    /// 入力を受け取る関数
    /// </summary>
    /// <param name="input">入力</param>
    public void OnInput(int input)
    {
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
    /// ステージ情報を取得する関数
    /// </summary>
    /// <returns>現在のステージ情報</returns>
    public StageInfoData GetStageInfo()
    {
        return stageInfo;
    }
}
