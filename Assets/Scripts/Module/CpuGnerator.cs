using UnityEngine;

public class CpuGnerator : MonoBehaviour
{
    const int cpuMax = 100;                 // 最大Cpu数

    CpuData[] cpus = new CpuData[cpuMax];   // Cpuデータ配列
    float generateInterval = 0.01f;         // Cpu出現間隔

    // Update is called once per frame
    void Update()
    {
        
    /// <summary>
    /// Cpuを初期化する関数
    /// </summary>
    /// <param name="stageInfo"></param>
    public void Init(StageInfoData stageInfo)
    {
        generateInterval = stageInfo.TimeLimit / cpuMax;
        cpus = new CpuData[cpuMax];
        for (int i = 0; i < cpuMax; i++)
        {
            CpuType type = (CpuType)Random.Range(0, 4);
            float genDuration = generateInterval * (i + 1);
            // 終盤ではゆらぎを無効にする
            if (i <= cpuMax - 10)
            {
                // 出現までの時間にゆらぎを加える
                generateInterval *= Random.Range(0.8f, 1.2f);
            }
            cpus[i] = new CpuData(type, i + 1, genDuration);
        }
    }
}
