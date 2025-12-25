using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CpuInstance : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cpuNumberText; // Cpu番号テキスト
    [SerializeField] TextMeshProUGUI answerText;    // 回答テキスト
    [SerializeField] Image           iconImage;     // アイコンイメージ

    /// <summary>
    /// Cpuを初期化する
    /// </summary>
    /// <param name="cpuData">初期化するCpuデータ</param>
    public void Init(CpuData cpuData)
    {
        // 一時的にCpuの回答が未設定の場合、ランダムに設定する
        if (cpuData.answer == null)
        {
            InputData inputData = new InputData();
            int r = Random.Range(0, 10);
            inputData.Key = (Key)r;
            inputData.KeyName = r.ToString();
            cpuData.answer = inputData;
        }
        if (cpuData.answer.icon != null)
        {
            answerText.enabled = false;
            iconImage.enabled = true;
            iconImage.sprite = cpuData.answer.icon;
        }
        else
        {
            answerText.enabled = true;
            iconImage.enabled = false;
            answerText.text = ((int)cpuData.answer.Key).ToString();
        }
        cpuNumberText.text = $"CPU #{cpuData.cpuNumber}";
    }
}
