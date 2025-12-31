using UnityEngine;

/// <summary>
/// パフォーマーのデータクラス
/// </summary>
public class MagicData
{
    public string magicName;      // 発動魔法名
    public Vector2 genPos;        // 出現位置
    public Vector3 genScale;     // 出現スケール
    public string text;          // 魔法テキスト
    public Sprite icon;          // 魔法アイコン

    /// <summary>
    /// パフォーマーのコンストラクタ
    /// </summary>
    /// <param name="name">発動魔法名</param>
    /// <param name="text">魔法テキスト</param>
    /// <param name="pos">出現位置</param>
    /// <param name="scale">出現スケール</param>
    public MagicData(string name, string text, Vector2 pos, Vector3 scale)
    {
        this.text = text;
        magicName = name;
        genPos = pos;
        genScale = scale;
    }
}
