using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeEffect : MonoBehaviour
{
    public static bool isSceneChanging = false; // シーン切り替え中かどうかのフラグ
    CanvasGroup canvas;

    private async void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.blocksRaycasts = false;
        // シーン切り替えを行っている場合はフェードインを実行
        if (isSceneChanging)
        {
            canvas.blocksRaycasts = true;
            await LMotion.Create(1f, 0f, 0.3f)
                .WithEase(Ease.InOutSine)
                .BindToAlpha(canvas);
            canvas.blocksRaycasts = false;
            isSceneChanging = false;
        }
    }

    /// <summary>
    /// フェードインエフェクトを再生
    /// </summary>
    /// <param name="noRay">レイキャストを無効にするかどうか</param>
    public async UniTask PlayFadeIn(CancellationToken token,bool noRay = false)
    {
        if (!noRay)
        {
            canvas.blocksRaycasts = true;
        }
        await LMotion.Create(0f, 1f, 0.2f)
            .WithEase(Ease.InOutSine)
            .BindToAlpha(canvas);
    }

    /// <summary>
    /// フェードアウトエフェクトを再生
    /// </summary>
    public async UniTask PlayFadeOut(CancellationToken token)
    {
        await UniTask.Delay(400,cancellationToken: token);
        await LMotion.Create(1f, 0f, 0.3f)
            .WithEase(Ease.InOutSine)
            .BindToAlpha(canvas);
        canvas.blocksRaycasts = false;
    }
}
