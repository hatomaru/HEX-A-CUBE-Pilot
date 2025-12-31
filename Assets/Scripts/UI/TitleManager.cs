using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public SceneChangeEffect sceneChangeEffect;

    public async void StartGame()
    {
        await sceneChangeEffect.PlayFadeIn(destroyCancellationToken);
        SceneChangeEffect.isSceneChanging = true;
        SceneManager.LoadScene("Main");
    }
}
