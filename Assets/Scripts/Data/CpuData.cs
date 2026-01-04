using UnityEngine;

/// <summary>
/// Cpuの種類を定義
/// </summary>
public enum CpuType
{
    Misstake,
    Virus,
    Navigator,
    Performer
}

/// <summary>
/// Cpuの情報を定義するクラス
/// </summary>
[System.Serializable]
public class CpuData
{
    public CpuType type;        // Cpuの種類
    public InputData answer;    // Cpuの入力データ
    public int cpuNumber;       // Cpuの識別番号
    public float genDuration;   // 出現までの時間
    public bool isGened = false;// Cpuが出現したか
    public bool isAnimationEnded = false; // Cpuの警告アニメーションが終了したか
    public GameObject instance; // Cpuのインスタンスオブジェクト

    /// <summary>
    /// Cpuのデータを初期化するコンストラクタ
    /// </summary>
    /// <param name="type">Cpuの種類</param>
    /// <param name="cpuNumber">識別番号</param>
    /// <param name="genDuration">出現までの時間</param>
    public CpuData(CpuType type, int cpuNumber, float genDuration)
    {
        this.type = type;
        this.cpuNumber = cpuNumber;
        this.genDuration = genDuration;
        isGened = false;
    }
}
