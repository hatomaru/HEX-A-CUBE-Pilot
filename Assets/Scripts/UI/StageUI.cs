using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Threading;
using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] RectTransform stageWindow; // ステージウィンドウ
    [SerializeField] RectTransform stageTimerFill; // ステージタイマー
    [SerializeField] TextMeshProUGUI stageNoText;    // ステージ番号テキスト
    [SerializeField] TextMeshProUGUI stageCpuText;   // ステージCpu数テキスト

    StageManager stageManager;
    int cpuCount = 0;                     // 現在の残りCpu数
    void Awake()
    {
        stageManager = GetComponent<StageManager>();
        stageWindow.localScale = new Vector3(0f, 0.71122998f, 0.71122998f);
    }

    public void InitStageText(int cube)
    {
        stageNoText.text = $"<size=22>Cube</size> #{cube.ToString("00")}";
    }

    /// <summary>
    /// UIを初期化する関数
    /// </summary>
    public void ResetUI()
    {
        stageTimerFill.localScale = Vector3.one;
        cpuCount = CpuGnerator.cpuMax;
    }

    /// <summary>
    /// CPUカウントを進める
    /// </summary>
    public void CpuCountDown()
    {
        cpuCount--;
    }

    /// <summary>
    /// UIを更新する関数
    /// </summary>
    public void UpdateUI()
    {
        stageCpuText.text = $"あと<size=42>{cpuCount}</size>CPU";
        stageTimerFill.localScale = new Vector3(stageManager.stageTimmer / stageManager.GetStageInfo().TimeLimit, 1f, 1f);
    }

    /// <summary>
    /// ステージ画面を表示する関数
    /// </summary>
    /// <param name="token">キャンセルトークン</param>
    public async UniTask StageWindowPopuop(CancellationToken token)
    {
        await LMotion.Create(new Vector3(0f, 0.71122998f, 0.71122998f), new Vector3(0.71122998f, 0.71122998f, 0.71122998f), 0.35f)
            .WithEase(Ease.InOutBounce)
            .BindToLocalScale(stageWindow)
            .AddTo(gameObject);
    }

    /// <summary>
    /// ステージ画面を閉じる関数
    /// </summary>
    /// <param name="token">キャンセルトークン</param>
    public async UniTask StageWindowClose(CancellationToken token)
    {
        await LMotion.Create(new Vector3(0.71122998f, 0.71122998f, 0.71122998f), new Vector3(0f, 0.71122998f, 0.71122998f), 0.17f)
          .WithEase(Ease.OutBack)
          .BindToLocalScale(stageWindow)
          .AddTo(gameObject);
    }
}
