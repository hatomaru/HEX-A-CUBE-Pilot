using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputInstance : MonoBehaviour
{
    readonly Color onColorFill = new Color(0.09967953f, 0.3880405f, 0.5031446f, 0.9f);
    readonly Color offColorFill = new Color(0.01542656f, 0.2393069f, 0.327044f, 0.9f);

    public InputData inputData { get; private set; }
    RectTransform rect;
    Vector3 defaultScale = new Vector3(1.7f, 1.41f, 1.41f);
    [SerializeField] Sprite defaultIcon;              // デフォルトアイコン
    [SerializeField] Image           backgroundImage;     // 背景画像
    [SerializeField] TextMeshProUGUI numberText;          // 入力テキスト
    [SerializeField] TextMeshProUGUI numberDetailText;    // 入力詳細テキスト
    [SerializeField] Image           numberImage;         // 入力画像
    [SerializeField] CanvasGroup     numberGroup;         // 入力テキストグループ
    [SerializeField] CanvasGroup     detailGroup;         // 入力詳細グループ

    async void Awake()
    {
        backgroundImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        inputData = new InputData();
        inputData.Key = Key.Key0;
        inputData.icon = defaultIcon;
        ResetInput();
    }

    /// <summary>
    /// 入力データをリセットする
    /// </summary>
    public void ResetInput()
    {
        rect.localScale = Vector3.zero;
    }

    /// <summary>
    /// 入力データを設定する
    /// </summary>
    /// <param name="data">データ</param>
    public void SetInputData(InputData data)
    {
        inputData = data;
    }

    /// <summary>
    /// 入力したときの処理
    /// </summary>
    public async UniTask OnPress(CancellationToken token)
    {
        backgroundImage.color = offColorFill;
        await UniTask.Delay(250,cancellationToken: token);
        backgroundImage.color = onColorFill;
    }

    /// <summary>
    /// 入力を初期化する
    /// </summary>
    /// <param name="isAnimation">アニメーションの有無</param>
    public async UniTask Init(bool isAnimation = false)
    {
        numberGroup.DOKill();
        detailGroup.DOKill();
        numberGroup.alpha = 0;
        detailGroup.alpha = 0;

        numberText.text = ((int)inputData.Key).ToString();
        numberDetailText.text = numberText.text;
        numberImage.sprite = inputData.icon;
        if (isAnimation)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(rect.DOScale(defaultScale, 0.2f).SetEase(Ease.OutBack));
            seq.Append(numberGroup.DOFade(1, 0.5f));
            seq.AppendInterval(0.5f);
            if (inputData.icon != null)
            {
                seq.Append(numberGroup.DOFade(0, 0.2f));
                seq.Join(detailGroup.DOFade(1, 0.2f));
            }
            await seq.Play().AsyncWaitForCompletion();
        }
        rect.localScale = defaultScale;
        if (inputData.icon != null)
        {
            numberGroup.alpha = 0;
            detailGroup.alpha = 1;
        }
        else
        {
            numberGroup.alpha = 1;
            detailGroup.alpha = 0;
        }
    }

    /// <summary>
    /// 入力を無効化する
    /// </summary>
    /// <param name="isAnimation">アニメーションの有無</param>
    public async UniTask DisableActive(bool isAnimation = false)
    {
        rect.DOKill();
        if (isAnimation)
        {
            Sequence seq = DOTween.Sequence();
            rect.DOScale(Vector3.zero, 0.15f).SetEase(Ease.OutBack);
            await seq.Play().AsyncWaitForCompletion();
        }
        rect.localScale = Vector3.zero;
    }
}
