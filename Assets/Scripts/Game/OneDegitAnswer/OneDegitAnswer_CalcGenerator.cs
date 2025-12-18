using UnityEngine;

/// <summary>
/// 計算問題を定義するクラス
/// </summary>
public class CalcData
{
    public int firstNumber;
    public int secondNumber;
    public int answer;
    public char operatorChar; // '+', '-', '×', '÷'のいずれかが入る

    /// <summary>
    /// CalcDataのコンストラクタ
    /// </summary>
    /// <param name="first">左辺の値</param>
    /// <param name="second"></param>
    /// <param name="op"></param>
    public CalcData(int first, int second, char op)
    {
        firstNumber = first;
        secondNumber = second;
        operatorChar = op;
        // 計算結果を求める
        switch (operatorChar)
        {
            case '+':
                answer = firstNumber + secondNumber;
                break;
            case '-':
                answer = firstNumber - secondNumber;
                break;
            case '×':
                answer = firstNumber * secondNumber;
                break;
            case '÷':
                answer = firstNumber / secondNumber;
                break;
            default:
                Debug.LogError("Invalid operator");
                answer = 0;
                break;
        }
    }
}

public class OneDegitAnswer_CalcGenerator : MonoBehaviour
{
    /// <summary>
    /// 答えが一桁になる計算問題を生成する
    /// </summary>
    /// <returns>生成した計算問題</returns>
    public CalcData Gen()
    {
        int operatorIndex = Random.Range(0, 2);
        if(operatorIndex < 2)
        {
            // 演算子が足し算、引き算の場合
            int first = Random.Range(0, 8);
            int second = Random.Range(0, 8 - first);
            char op = operatorIndex == 0 ? '+' : '-';
            // 引き算の場合、答えが負にならないように調整
            if(op == '-' && first < second)
            {
                int temp = first;
                first = second;
                second = temp;
            }
            return new CalcData(first, second, op);
        }
        // 未実装の場合はダミーを返す
        return new CalcData(0,0,'+');
    }
}
