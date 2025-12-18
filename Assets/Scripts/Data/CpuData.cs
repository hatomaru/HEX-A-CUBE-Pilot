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
public class CpuData : ScriptableObject
{
    public CpuType type;       // Cpuの種類
    public int cpuNumber;      // Cpuの識別番号
    public float genDuration;  // 出現までの時間

    /// <summary>
    /// Cpuのデータを初期化するコンストラクタ
    /// </summary>
    /// <param name="type">Cpuの種類</param>
    /// <param name="cpuNumber">識別番号</param>
    /// <param name="genDuration">出現までの時間</param>
    CpuData(CpuType type, int cpuNumber, float genDuration)
    {
        this.type = type;
        this.cpuNumber = cpuNumber;
        this.genDuration = genDuration;
    }
}
