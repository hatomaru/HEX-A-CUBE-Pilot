using UnityEngine;

/// <summary>
/// 計算問題を定義するクラス
/// </summary>
public class CalcData
{
    public float firstNumber;
    public float secondNumber;
    public int answer;
    public char operatorChar; // '+', '-', '×', '÷'のいずれかが入る

    /// <summary>
    /// CalcDataのコンストラクタ
    /// </summary>
    /// <param name="first">左辺の値</param>
    /// <param name="second"></param>
    /// <param name="op"></param>
    public CalcData(float first, float second, char op)
    {
        firstNumber = first;
        secondNumber = second;
        operatorChar = op;
        // 計算結果を求める
        switch (operatorChar)
        {
            case '+':
                answer = (int)(firstNumber + secondNumber);
                break;
            case '-':
                answer = (int)(firstNumber - secondNumber);
                break;
            case '×':
                answer = (int)(firstNumber * secondNumber);
                break;
            case '÷':
                answer = (int)(firstNumber / secondNumber);
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
    /// <param name="stageInfo"></param>ステージ情報データ</param>
    /// <param name="firstN">最初の数字を指定する場合、その値をセット。-1の場合はランダム生成</param>
    /// <returns>生成した計算問題</returns>
    public CalcData Gen(StageInfoData stageInfo, float firstN = -1)
    {
        int operatorIndex = Random.Range(0, 2);
        if (stageInfo.GameLevel == 3)
        {
            // ゲームレベル3では少数を含むたし算、ひき算のみ
            int decimalScale = 10;
            char op = Random.Range(0, 2) == 0 ? '+' : '-';

            while (true)
            {
                int answer = Random.Range(0, 8); // 一桁整数

                int answerInt = answer * decimalScale;

                int firstInt, secondInt;

                if (op == '+')
                {
                    firstInt = Random.Range(0, answerInt + 1);
                    if (firstInt != -1)
                    {
                        firstInt = (int)firstN;
                    }
                    secondInt = answerInt - firstInt;
                }
                else
                {
                    secondInt = Random.Range(0, 80);
                    firstInt = secondInt + answerInt;
                    if (firstInt != -1)
                    {
                        firstInt = (int)firstN;
                    }
                }

                // 表示範囲チェック
                if (firstInt < 10 || firstInt > 80) continue;
                if (secondInt < 0 || secondInt > 80) continue;

                float first = firstInt / (float)decimalScale;
                float second = secondInt / (float)decimalScale;

                return new CalcData(first, second, op);
            }
        }
        if (stageInfo.GameLevel == 2)
        {
            // ゲームレベル2ではかけ算、わり算のみ
            operatorIndex = Random.Range(2, 4);
        }
        if (operatorIndex < 2)
        {
            // 演算子が足し算、引き算の場合
            int first = Random.Range(1, 8);
            if (first != -1)
            {
                first = (int)firstN;
            }
            int second = Random.Range(1, 8 - first);
            char op = operatorIndex == 0 ? '+' : '-';
            // 引き算の場合、答えが負にならないように調整
            if (op == '-' && first < second)
            {
                int temp = first;
                first = second;
                second = temp;
            }
            return new CalcData(first, second, op);
        }
        else if (operatorIndex == 2)
        {
            // 演算子が掛け算の場合
            int first = Random.Range(1, 4);
            if (first != -1)
            {
                first = (int)firstN;
            }
            int second = Random.Range(1, 4);
            return new CalcData(first, second, '×');
        }
        else if (operatorIndex >= 3)
        {
            // 演算子が割り算の場合
            int second = Random.Range(1, 4);
            if (second != -1)
            {
                second = (int)firstN;
            }
            int answer = Random.Range(1, 4);
            int first = second * answer;
            return new CalcData(first, second, '÷');
        }
        // 未実装の場合はダミーを返す
        return new CalcData(0, 0, '+');
    }
}
