using Cysharp.Threading.Tasks;
using UnityEngine;

public class OneDegitAnswer_Manager : MonoBehaviour
{
    private CalcData calcData;
    private OneDegitAnswer_CalcGenerator calcGenerator;
    [SerializeField] StageManager stageManager;

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
        // 入力インスタンスに入力データを設定する
        for (int i = 0; i < stageManager.inputInstances.Length; i++)
        {
            if (stageManager.inputInstances[i] == null)
                continue;
            stageManager.inputInstances[i].SetInputData(new InputData() { Key = (Key)i });
        }
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
        // 正解判定
        if (calcData.answer == input)
        {
            // 正解
            stageManager.StageClear(destroyCancellationToken).Forget();
        }
        else
        {
            // 不正解
            stageManager.StageMiss(destroyCancellationToken).Forget();
        }
    }
}
