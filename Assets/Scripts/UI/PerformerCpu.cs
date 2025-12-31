using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerformerCpu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cpuNumberText; // Cpu番号テキスト
    [SerializeField] TextMeshProUGUI answerText;    // 回答テキスト
    [SerializeField] Image iconImage;     // アイコンイメージ
    [SerializeField] StageManager stageManager;

    /// <summary>
    /// Cpuを初期化する
    /// </summary>
    /// <param name="magicData">初期化する魔法データ</param>
    /// <param name="second">表示秒数</param>
    public async UniTask Init(CancellationToken token,MagicData magicData,float second)
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        if (magicData.icon != null)
        {
            answerText.enabled = false;
            iconImage.enabled = true;
            iconImage.sprite = magicData.icon;
        }
        else
        {
            answerText.enabled = true;
            iconImage.enabled = false;
            answerText.text =  magicData.text;
        }
        await UniTask.Delay((int)(second * 100),cancellationToken: token);
        stageManager.OnMagicRestore();
    }
}
