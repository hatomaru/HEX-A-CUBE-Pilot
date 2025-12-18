using UnityEngine;

public class OneDegitAnswer_Manager : MonoBehaviour
{
    private CalcData calcData;
    private OneDegitAnswer_CalcGenerator calcGenerator;

    private void Awake()
    {
        calcGenerator = GetComponent<OneDegitAnswer_CalcGenerator>();
        Init();
    }

    /// <summary>
    /// お題を初期化する
    /// </summary>
    public void Init()
    {
        calcGenerator = GetComponent<OneDegitAnswer_CalcGenerator>();
        // 計算問題を生成する
        calcData = calcGenerator.Gen();
        OneDegitAnswer_Drawer drawer = GetComponent<OneDegitAnswer_Drawer>();
        // 計算問題を描画する
        drawer.DrawQuestion(calcData);
    }

    /// <summary>
    /// 入力を受け取る関数
    /// </summary>
    /// <param name="input">入力</param>
    public void OnInput(int input)
    {
        Debug.Log("Input Number: " + input);
    }
}
