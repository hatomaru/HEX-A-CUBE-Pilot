using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] PilotGameLoop pilotGameLoop;
    [SerializeField] StageManager stageManager;
    [SerializeField] MissUI missUI;
    [SerializeField] StageUI stageUI;
    [SerializeField] SceneChangeEffect sceneChangeEffect;

    /// <summary>
    /// リトライする
    /// </summary>
    public async void OnRetry()
    {
        await missUI.HideMiss(destroyCancellationToken);
        await stageUI.StageWindowClose(destroyCancellationToken);
        await pilotGameLoop.InitGame(destroyCancellationToken);
    }

    /// <summary>
    /// タイトルに戻る
    /// </summary>
    public async void OnReturnToTitle()
    {
        await missUI.HideMiss(destroyCancellationToken);
        await stageUI.StageWindowClose(destroyCancellationToken);
        await sceneChangeEffect.PlayFadeIn(destroyCancellationToken);
        SceneChangeEffect.isSceneChanging = true;
        SceneManager.LoadScene("Title");
    }
}
