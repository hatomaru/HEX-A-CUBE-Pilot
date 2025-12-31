using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class OneDegitAnswer_Manager : MonoBehaviour
{
    private CalcData calcData;
    private OneDegitAnswer_CalcGenerator calcGenerator;
    // 有効な入力
    List<InputInstance> vaildInputInstances = new List<InputInstance>();
    [SerializeField] StageManager stageManager;

    private void Awake()
    {
        calcGenerator = GetComponent<OneDegitAnswer_CalcGenerator>();
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
        calcData = calcGenerator.Gen(stageManager.stageInfo);
        OneDegitAnswer_Drawer drawer = GetComponent<OneDegitAnswer_Drawer>();
        // 計算問題を描画する
        drawer.DrawQuestion(calcData);
        // 有効な入力インスタンスを設定する
        SetVaildInputInstances();
        stageManager.SetAnswer(stageManager.inputInstances[calcData.answer], vaildInputInstances);
    }

    /// <summary>
    /// 有効な入力インスタンスを設定する関数
    /// </summary>
    public void SetVaildInputInstances()
    {
        vaildInputInstances.Clear();
        int rnd = Random.Range(2, 3);
        // 答え方の候補を取得
        // レイヤー1: 正解の誤差
        if (calcData.answer - rnd >= 0 && calcData.answer - rnd != calcData.answer)
        {
            vaildInputInstances.Add(stageManager.inputInstances[calcData.answer - rnd]);
        }
        rnd = Random.Range(2, 3);
        if (calcData.answer + rnd <= 9 && calcData.answer + rnd != calcData.answer)
        {
            vaildInputInstances.Add(stageManager.inputInstances[calcData.answer + rnd]);
        }
        // レイヤー2: 別解釈(オペレーターの反転)
        switch (calcData.operatorChar)
        {
            case '+':
                if (calcData.firstNumber - calcData.secondNumber >= 0 && calcData.firstNumber - calcData.secondNumber != calcData.answer)
                {
                    vaildInputInstances.Add(stageManager.inputInstances[calcData.firstNumber - calcData.secondNumber]);
                }
                break;
            case '-':
                if (calcData.firstNumber + calcData.secondNumber <= 9 && calcData.firstNumber + calcData.secondNumber != calcData.answer)
                {
                    vaildInputInstances.Add(stageManager.inputInstances[calcData.firstNumber + calcData.secondNumber]);
                }
                break;
            case '*':
                if (calcData.firstNumber / calcData.secondNumber >= 0 && calcData.firstNumber / calcData.secondNumber != calcData.answer)
                {
                    vaildInputInstances.Add(stageManager.inputInstances[(int)(calcData.firstNumber / calcData.secondNumber)]);
                }
                break;
            case '/':
                if (calcData.firstNumber * calcData.secondNumber <= 9 && calcData.firstNumber * calcData.secondNumber != calcData.answer)
                {
                    vaildInputInstances.Add(stageManager.inputInstances[(int)(calcData.firstNumber * calcData.secondNumber)]);
                }
                break;
        }
        // レイヤー3: プラスマイナス1の誤差
        if (calcData.answer - 1 >= 0 && calcData.answer - 1 != calcData.answer)
        {
            vaildInputInstances.Add(stageManager.inputInstances[calcData.answer - 1]);
        }
        if (calcData.answer + 1 <= 9 && calcData.answer + 1 != calcData.answer)
        {
            vaildInputInstances.Add(stageManager.inputInstances[calcData.answer + 1]);
        }
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
            stageManager.StageMiss("不正解です...",destroyCancellationToken).Forget();
        }
    }
}
