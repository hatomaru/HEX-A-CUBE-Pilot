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
        calcData = calcGenerator.Gen();
        OneDegitAnswer_Drawer drawer = GetComponent<OneDegitAnswer_Drawer>();
        drawer.DrawQuestion(calcData);
    }
}
