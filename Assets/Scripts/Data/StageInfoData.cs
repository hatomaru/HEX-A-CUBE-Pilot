using UnityEngine;

/// <summary>
/// ステージ情報を定義するクラス
/// </summary>
[CreateAssetMenu(menuName = "GameData/StageInfo", fileName = "StageInfo")]
public class StageInfoData : ScriptableObject
{
    public int StageNumber;      // ステージ番号
    public string StageName;     // ステージ名
    public float TimeLimit;      // 制限時間（秒）
    public int GameLevel;        // お題の難易度

    /// <summary>
    /// お題の難易度を変更する
    /// </summary>
    /// <param name="level">変更後の難易度</param>
    public void SetGameLevel(int level)
    {
        GameLevel = level;
    }
}
