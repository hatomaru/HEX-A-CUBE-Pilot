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
        // 一時的にCpuの回答が未設定の場合、ランダムに設定する
        if (cpuData.answer == null)
        {
            InputData inputData = new InputData();
            int r = Random.Range(0, 10);
            inputData.Key = (Key)r;
            inputData.KeyName = r.ToString();
            cpuData.answer = inputData;
        }
        answerText.text = cpuData.answer.KeyName.ToString();
        cpuNumberText.text = $"CPU #{cpuData.cpuNumber}";
    }
}
