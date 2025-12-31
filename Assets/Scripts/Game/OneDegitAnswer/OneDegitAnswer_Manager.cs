using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class OneDegitAnswer_Manager : MonoBehaviour
{

    /// <summary>
    /// パフォーマーの魔法キー
    /// </summary>
    enum MagicKey
    {
        ChangeOperator,     // オペレーター変更
        ChangeRightNumbers, // 数字変更
        MaxNum
    }

    private CalcData restoreCalcData; // 魔法使用前の計算データを保存する変数
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
    /// パフォーマーの魔法復元を行う関数
    /// </summary>
    public void RestoreCalcData()
    {
        if (restoreCalcData == null)
        {
            return;
        }
        calcData = new CalcData(restoreCalcData.firstNumber, restoreCalcData.secondNumber, restoreCalcData.operatorChar);
        calcData.answer = restoreCalcData.answer;
        // 有効な入力インスタンスを設定する
        SetVaildInputInstances();
        stageManager.SetAnswer(stageManager.inputInstances[calcData.answer], vaildInputInstances);
        restoreCalcData = null;
    }

    /// <summary>
    /// パフォーマーの魔法発動をリクエストし、成功した場合は魔法データを返す
    /// </summary>
    /// <returns></returns>
    public void OnMagicRequest(System.Action<MagicData> callback)
    {
        restoreCalcData = new CalcData(calcData.firstNumber, calcData.secondNumber, calcData.operatorChar);
        restoreCalcData.answer = calcData.answer;
        // ランダムで魔法を選択
        MagicKey rndKey = (MagicKey)Random.Range(0, 2);
        MagicData magicData = null;
        for (int i = 0; i < (int)MagicKey.MaxNum; i++)
        {
            switch (rndKey)
            {
                case MagicKey.ChangeOperator:
                    // オペレーターを変更できるか判定
                    if (calcData.operatorChar == '+')
                    {
                        if (calcData.firstNumber - calcData.secondNumber >= 0 && calcData.firstNumber - calcData.secondNumber == (int)(calcData.firstNumber - calcData.secondNumber))
                        {
                            calcData.operatorChar = '-';
                            calcData.answer = (int)(calcData.firstNumber - calcData.secondNumber);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (calcData.operatorChar == '-')
                    {
                        if (calcData.firstNumber + calcData.secondNumber <= 9 && calcData.firstNumber - calcData.secondNumber == (int)(calcData.firstNumber - calcData.secondNumber))
                        {
                            calcData.operatorChar = '+';
                            calcData.answer = (int)(calcData.firstNumber + calcData.secondNumber);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (calcData.operatorChar == '×')
                    {
                        if (calcData.firstNumber / calcData.secondNumber >= 0)
                        {
                            calcData.operatorChar = '÷';
                            calcData.answer = (int)(calcData.firstNumber / calcData.secondNumber);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (calcData.operatorChar == '÷')
                    {
                        if (calcData.firstNumber * calcData.secondNumber <= 9)
                        {
                            calcData.operatorChar = '×';
                            calcData.answer = (int)(calcData.firstNumber * calcData.secondNumber);
                        }
                        else
                        {
                            break;
                        }
                    }
                    // オペレーター変更魔法を発動
                    magicData = new MagicData(rndKey.ToString(), calcData.operatorChar.ToString(), new Vector2(-34, 47), new Vector3(1, 1, 1));
                    break;
                case MagicKey.ChangeRightNumbers:
                    // 数字変更魔法を発動
                    calcData = calcGenerator.Gen(stageManager.stageInfo, calcData.firstNumber);
                    magicData = new MagicData(rndKey.ToString(), calcData.secondNumber.ToString(), new Vector2(24.4f, 47), new Vector3(1.31092167f, 1.19174695f, 1.19174695f));
                    break;
            }
            if (magicData != null)
            {
                break;
            }
            rndKey++;
            if (rndKey >= MagicKey.MaxNum)
            {
                rndKey = 0;
            }
        }
        // 有効な入力インスタンスを設定する
        SetVaildInputInstances();
        stageManager.SetAnswer(stageManager.inputInstances[calcData.answer], vaildInputInstances);
        callback(magicData);
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
                    vaildInputInstances.Add(stageManager.inputInstances[(int)(calcData.firstNumber - calcData.secondNumber)]);
                }
                break;
            case '-':
                if (calcData.firstNumber + calcData.secondNumber <= 9 && calcData.firstNumber + calcData.secondNumber != calcData.answer)
                {
                    vaildInputInstances.Add(stageManager.inputInstances[(int)(calcData.firstNumber + calcData.secondNumber)]);
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
            stageManager.StageMiss("不正解です...", destroyCancellationToken).Forget();
        }
    }
}
