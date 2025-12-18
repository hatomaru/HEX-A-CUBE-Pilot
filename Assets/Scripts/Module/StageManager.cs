using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    [SerializeField] UnityEvent<int> onInput;
    [SerializeField] UnityEvent onInit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    /// <summary>
    /// ステージ全体を初期化する関数
    /// </summary>
    public void Init()
    {
        onInit.Invoke();
    }

    /// <summary>
    /// 入力を受け取る関数
    /// </summary>
    /// <param name="input">入力</param>
    public void OnInput(int input)
    {
        onInput?.Invoke(input);
    }
}
