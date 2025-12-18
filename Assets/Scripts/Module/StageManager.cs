using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    InputData[] inputDatas = new InputData[10]; // 入力データ配列
    [SerializeField] StageInfoData stageInfo;
    [SerializeField] UnityEvent<int> onInput;
    [SerializeField] UnityEvent onInit;
    CpuGnerator cpuGnerator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
        StageStart();
    }

    /// <summary>
    /// </summary>
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

    /// <summary>
    /// お題の難易度を設定する関数
    /// </summary>
    /// <param name="level">お題の難易度</param>
    public void SetGameLevel(int level)
    {
        stageInfo.SetGameLevel(level);
    }

    /// <summary>
    /// お題の制限時間を設定する関数
    /// </summary>
    /// <param name="timeLimit">制限時間</param>
    public void SetStageTimeLimit(float timeLimit)
    {
        stageInfo.TimeLimit = timeLimit;
    }

    /// <summary>
    /// ステージ情報を取得する関数
    /// </summary>
    /// <returns>現在のステージ情報</returns>
    public StageInfoData GetStageInfo()
    {
        return stageInfo;
    }
}
