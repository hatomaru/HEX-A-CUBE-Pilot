using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class StageUI : MonoBehaviour
{
    [SerializeField] RectTransform stageWindow; // ステージウィンドウ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageWindow.localScale = Vector3.zero;
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
