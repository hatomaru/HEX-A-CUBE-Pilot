using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using System.Threading;
using LitMotion.Extensions;

public class WarningEffect : MonoBehaviour
{
    public const float WarningDuration = 1.1f;
    Image bg;
    [SerializeField] StageManager stageManager;

    void Awake()
    {
        bg = GetComponent<Image>();
        // 色を指定
        bg.color = new Color(0.9764706f, 0.5882353f, 0.1843137f, 0f);
    }

    /// <summary>
    /// ウイルス活性化警告
    /// </summary>
    public async UniTask OnVirusWarning(CancellationToken token)
    {
        // 色を指定
        bg.color = new Color(0.9764706f, 0.2764286f, 0.1843137f, 0f);
        // 再生する
        for (int i = 0; i < 2; i++)
        {
            stageManager.audioPlayer.PlayLookWarning();
            await LMotion.Create(0f, 0.22f, 0.3f)
                .WithEase(Ease.InSine)
                .BindToColorA(bg)
                .AddTo(gameObject);
            await LMotion.Create(0.22f, 0f, 0.25f)
             .WithEase(Ease.InOutSine)
             .BindToColorA(bg)
             .AddTo(gameObject);
        }
    }

    /// <summary>
    /// パフォーマー活性化警告
    /// </summary>
    public async UniTask OnPerformerWarning(CancellationToken token)
    {
        // 色を指定
        bg.color = new Color(0.9764706f, 0.5882353f, 0.1843137f, 0f);
        // 再生する
        for(int i = 0; i < 2;i++)
        {
            stageManager.audioPlayer.PlayWarning();
            await LMotion.Create(0f, 0.22f, 0.3f)
                .WithEase(Ease.InSine)
                .BindToColorA(bg)
                .AddTo(gameObject);
            await LMotion.Create(0.22f, 0f, 0.25f)
             .WithEase(Ease.InOutSine)
             .BindToColorA(bg)
             .AddTo(gameObject);
        }
    }
}
