using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using UnityEngine;

public class PilotGameLoop : MonoBehaviour
{
    [SerializeField] StageManager stageManager;
    [SerializeField] StageUI stageUI;
    int cube = 1;

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
    public async UniTask InitGame(CancellationToken token)
    {
        cube = 0;
        stageUI.InitStageText(cube + 1);

        StageManager.canProgressGame = true;
        await UniTask.Delay(300, cancellationToken: token);
        await GenStage(destroyCancellationToken);
    }

    /// <summary>
    /// ステージを生成
    /// </summary>
    private async UniTask GenStage(CancellationToken token)
    {
        cube++;
        StageInfoData stageInfo = new StageInfoData()
        {
            StageName = "ヒトケタ計算",
            TimeLimit = 12f - (cube - 1) * 0.68f,
        };
        // ゲームレベルの設定
        if (cube >= 100)
        {
            stageInfo.GameLevel = 3;
            stageInfo.TimeLimit = 12f - 15 * 0.68f - 0.02f * 85;
        }
        else if (cube >= 15)
        {
            stageInfo.GameLevel = 3;
            stageInfo.TimeLimit = 12f - 15 * 0.68f - 0.02f * (cube - 15);
        }else if(cube >= 10)
        {
            stageInfo.GameLevel = 3;
        }
        else if(cube >= 5)
        {
            stageInfo.GameLevel = 2;
        } else{
            stageInfo.GameLevel = 1;
        }
        float performerRate = stageInfo.TimeLimit / 100;
        int cpuNo = Random.Range(30, 70);
        // パフォーマーを1体出現させる
        stageInfo.replaceCpus = new CpuData[]
        {
            new CpuData(CpuType.Performer, cpuNo, performerRate * cpuNo),
        };
        stageUI.InitStageText(cube);
        stageManager.stageInfo = stageInfo;
        await stageManager.GameLoop(token);
    }

}
