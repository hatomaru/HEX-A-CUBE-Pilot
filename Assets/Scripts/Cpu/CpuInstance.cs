using TMPro;
using UnityEngine;

public class CpuInstance : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cpuNumberText; // Cpu番号テキスト
    [SerializeField] TextMeshProUGUI answerText;    // 回答テキスト
    
    /// <summary>
    /// Cpuを初期化する
    /// </summary>
    /// <param name="cpuData">初期化するCpuデータ</param>
    public void Init(CpuData cpuData)
    {
        answerText.text = cpuData.answer.KeyName.ToString();
        cpuNumberText.text = $"CPU #{cpuData.cpuNumber}";
    }
}
