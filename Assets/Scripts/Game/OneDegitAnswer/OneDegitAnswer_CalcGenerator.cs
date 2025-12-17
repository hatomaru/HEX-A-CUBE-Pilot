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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public  Gen()
}
