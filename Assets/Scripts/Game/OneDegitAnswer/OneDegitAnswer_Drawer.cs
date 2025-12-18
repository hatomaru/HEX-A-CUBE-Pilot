using TMPro;
using UnityEngine;

public class OneDegitAnswer_Drawer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;

    /// <summary>
    /// 計算問題を描画する
    /// </summary>
    /// <param name="calcData">計算問題データ</param>
    public void DrawQuestion(CalcData calcData)
    {
        questionText.text = $"{calcData.firstNumber}{calcData.operatorChar}{calcData.secondNumber}=?";
    }
}
