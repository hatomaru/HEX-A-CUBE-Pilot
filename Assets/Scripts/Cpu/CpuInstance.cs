using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using System.Threading;
using LitMotion.Extensions;

public class CpuInstance : MonoBehaviour
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI cpuNumberText; // Cpu番号テキスト
    [SerializeField] TextMeshProUGUI answerText;    // 回答テキスト
    [SerializeField] Image           iconImage;     // アイコンイメージ
    [SerializeField] RectTransform syncEffectRect;
    [SerializeField] Image         syncEffectImage;
    [SerializeField] Sprite goldSyncSprite;
    [SerializeField] Sprite normalSyncSprite;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Cpuを初期化する
    /// </summary>
    /// <param name="cpuData">初期化するCpuデータ</param>
    public void Init(CpuData cpuData)
    {
        // 一時的にCpuの回答が未設定の場合、ランダムに設定する
        if (cpuData.answer == null)
        {
            InputData inputData = new InputData();
            int r = Random.Range(0, 10);
            inputData.Key = (Key)r;
            inputData.KeyName = r.ToString();
            cpuData.answer = inputData;
        }
        if (cpuData.answer.icon != null)
        {
            answerText.enabled = false;
            iconImage.enabled = true;
            iconImage.sprite = cpuData.answer.icon;
        }
        else
        {
            answerText.enabled = true;
            iconImage.enabled = false;
            answerText.text = ((int)cpuData.answer.Key).ToString();
        }
        cpuNumberText.text = $"CPU #{cpuData.cpuNumber}";
    }

    public void Destory()
    {
        LMotion.Create(rectTransform.localScale, rectTransform.localScale * 1.5f, 0.3f)
        .WithEase(Ease.InOutCirc)
        .BindToLocalScale(rectTransform)
        .AddTo(gameObject);
        LMotion.Create(0f, 1f, 0.05f)
          .WithEase(Ease.OutSine)
          .BindToAlpha(canvasGroup)
          .AddTo(gameObject);
        LMotion.Create(1f, 0f, 0.15f)
           .WithEase(Ease.OutSine)
           .WithDelay(0.15f)
           .BindToAlpha(canvasGroup)
           .AddTo(gameObject);
    }

    public void SyncEffect(CancellationToken token,bool isPerformer)
    {
        syncEffectImage.sprite = isPerformer ? goldSyncSprite : normalSyncSprite;
        LMotion.Create(new Vector3(1.03611183f, 1.03988707f, 0.973039269f),new Vector3(1.35357642f, 1.35850847f, 1.27117848f),0.3f)
            .WithEase(Ease.InOutCirc)
            .BindToLocalScale(syncEffectRect)
            .AddTo(gameObject);
        LMotion.Create(0f, 1f, 0.05f)
          .WithEase(Ease.OutSine)
          .BindToColorA(syncEffectImage)
          .AddTo(gameObject);
        LMotion.Create(1f, 0f, 0.15f)
           .WithEase(Ease.OutSine)
           .WithDelay(0.25f)
           .BindToColorA(syncEffectImage)
           .AddTo(gameObject);
    }
}
