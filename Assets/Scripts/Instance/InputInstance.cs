using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputInstance : MonoBehaviour
{
    RectTransform rect;
    Vector3 defaultScale = new Vector3(1.7f, 1.41f, 1.41f);

    [SerializeField] TextMeshProUGUI numberText;          // 入力テキスト
    [SerializeField] TextMeshProUGUI numberDetailText;    // 入力詳細テキスト
    [SerializeField] Image           numberImage;         // 入力画像
    [SerializeField] CanvasGroup     numberGroup;         // 入力テキストグループ
    [SerializeField] CanvasGroup     detailGroup;         // 入力詳細グループ

    async void Awake()
    {
        rect = GetComponent<RectTransform>();
        InputData inputData = new InputData();
        inputData.Key = Key.Key0;
        rect.localScale = Vector3.zero;
        await Init(inputData,true);
        Debug.Log("InputInstance Awake Finished");
    }

    /// <summary>
    /// 入力を初期化する
    /// </summary>
    /// <param name="inputData">初期化する入力データ</param>
    /// <param name="isAnimation">アニメーションの有無</param>
    public async UniTask Init(InputData inputData, bool isAnimation = false)
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
