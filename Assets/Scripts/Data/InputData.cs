using UnityEngine;

/// <summary>
/// 入力キーを定義
/// </summary>
public enum Key
{
    Key0,
    Key1,
    Key2, 
    Key3, 
    Key4, 
    Key5, 
    Key6, 
    Key7, 
    Key8, 
    Key9,
}

/// <summary>
/// キー情報を定義するクラス
/// </summary>
[CreateAssetMenu(menuName = "InputData", fileName = "GameData/Input")]
public class InputData : ScriptableObject
{
    public Key Key;        // 操作を表す列挙体
    public string KeyName; // 操作の名前
    public Sprite icon;    // 操作アイコン
}
