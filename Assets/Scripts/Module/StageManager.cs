using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
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

}
