using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Threading;
using UnityEngine;

public class StageClearUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] RectTransform resultRect;
    [SerializeField] MatrixTextReveal matrixText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Hex生成アニメーションを再生する
    /// </summary>
    /// <param name="Hex">生成するHEX</param>
    public async UniTask PlayHexCreate(string Hex, CancellationToken token)
    {
        matrixText.tmp.text = "";
        LMotion.Create(0f, 1f, 0.25f)
              .WithEase(Ease.InSine)
              .BindToAlpha(canvasGroup)
              .AddTo(gameObject);
        LMotion.Create(Vector3.zero, new Vector3(4.68359375f, 4.52390003f, 4.52390003f), 0.65f)
             .WithEase(Ease.OutBack)
             .BindToLocalScale(resultRect)
             .AddTo(gameObject);
        await UniTask.Delay(450, cancellationToken: token);
        matrixText.SetAndPlay(Hex);
        await UniTask.Delay(1400, cancellationToken: token);
        await LMotion.Create(new Vector3(4.68359375f, 4.52390003f, 4.52390003f), Vector3.zero, 0.3f)
               .WithEase(Ease.InOutExpo)
               .BindToLocalScale(resultRect)
               .AddTo(gameObject);
        canvasGroup.blocksRaycasts = true;
    }
}
