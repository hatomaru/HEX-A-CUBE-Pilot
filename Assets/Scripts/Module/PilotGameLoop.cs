using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using R3.Triggers;
using R3.Collections;
using R3;

public class PilotGameLoop : MonoBehaviour
{
    [SerializeField] StageManager stageManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // イベントを登録
        stageManager.isInGame
            .Where(x => x == false && StageManager.canProgressGame)
            .Subscribe(async _ =>
            {
                await GenStage(destroyCancellationToken);
            })
            .AddTo(this);
        InitGame(destroyCancellationToken).Forget();
    }

    /// <summary>
    /// ゲームループを初期化
    /// </summary>
    private async UniTask InitGame(CancellationToken token)
    {
        StageManager.canProgressGame = true;
        await GenStage(destroyCancellationToken);
    }

    /// <summary>
    /// ステージを生成
    /// </summary>
    private async UniTask GenStage(CancellationToken token)
    {
        await stageManager.GameLoop(token);
    }

}
