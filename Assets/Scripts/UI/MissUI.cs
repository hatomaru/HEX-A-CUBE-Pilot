using UnityEngine;
using LitMotion;
using Cysharp.Threading.Tasks;
using System.Threading;
using LitMotion.Extensions;

public class MissUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField]CanvasGroup commandCanvasGroup;
    [SerializeField] RectTransform resultRect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        commandCanvasGroup.alpha = 0f;

        PlayMiss(destroyCancellationToken).Forget();
    }

    /// <summary>
    /// ミス演出を再生する
    /// </summary>
    public async UniTask PlayMiss(CancellationToken token)
    {
        LMotion.Create(0f,1f,0.25f)
            .WithEase(Ease.InSine)
            .BindToAlpha(canvasGroup)
            .AddTo(gameObject);
        await LMotion.Create(Vector3.zero, new Vector3(3.58686352f, 3.59632206f, 3.58686352f), 0.8f)
            .WithEase(Ease.OutBack)
            .BindToLocalScale(resultRect)
            .AddTo(gameObject);
        await UniTask.Delay(100, cancellationToken: token);
        await LMotion.Create(0f, 1f, 0.15f)
            .WithEase(Ease.InSine)
            .BindToAlpha(commandCanvasGroup)
            .AddTo(gameObject);
        canvasGroup.blocksRaycasts = true;
    }
}
