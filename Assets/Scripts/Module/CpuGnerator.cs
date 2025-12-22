using DG.Tweening;
using UnityEngine;

public class CpuGnerator : MonoBehaviour
{
    const int cpuMax = 100;                 // 最大Cpu数

    [SerializeField] Transform frontParent;      // 前方親オブジェクト
    [SerializeField] Transform backParent;       // 後方親オブジェクト
    [SerializeField] Vector2 generateFrontRangeStart;  // Cpu前方出現開始範囲
    [SerializeField] Vector2 generateFrontRangeEnd;    // Cpu前方出現終了範囲
    [SerializeField] Vector2 generateBackRangeStart;   // Cpu後方出現開始範囲
    [SerializeField] Vector2 generateBackRangeEnd;     // Cpu後方出現終了範囲

    [SerializeField] GameObject[] cpuPrefabs;    // Cpuプレハブ配列
    [SerializeField] CpuData[] replaceCpus;      // 置き換え用Cpuデータ配列
    [SerializeField] CpuData[] cpus = new CpuData[cpuMax];   // Cpuデータ配列
    float generateInterval = 0.01f;         // Cpu出現間隔

    // Update is called once per frame
    void Update()
    {
        // 出現までの時間を減少させる
        for (int i = 0; i < cpuMax; i++)
        {
            if (cpus[i] == null || cpus[i].genDuration < 0)
                continue;
            cpus[i].genDuration -= Time.deltaTime;
            if(cpus[i].genDuration <= 0)
            {
                // Cpu出現処理
                GameObject cpuObj;
                Vector3 localScale;
                if (cpus[i].type == CpuType.Misstake)
                {
                    cpuObj = Instantiate(cpuPrefabs[(int)cpus[i].type], backParent);
                    localScale = cpuObj.GetComponent<RectTransform>().localScale;
                    cpuObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(generateBackRangeStart.x, generateBackRangeEnd.x),
                                                                                Random.Range(generateBackRangeStart.y, generateBackRangeEnd.y));
                    cpuObj.GetComponent<RectTransform>().localScale = Vector3.zero;
                    cpuObj.GetComponent<RectTransform>().DOScale(localScale, generateInterval * 2)
                        .SetEase(Ease.InOutBounce);
                    cpuObj.GetComponent<RectTransform>().DOAnchorPosY(500, generateInterval * 3)
                        .SetEase(Ease.InOutCirc)
                        .SetDelay(generateInterval * 2);
                    Destroy(cpuObj, generateInterval * 5);
                }
                else
                {
                    cpuObj = Instantiate(cpuPrefabs[(int)cpus[i].type], frontParent);
                    localScale = cpuObj.GetComponent<RectTransform>().localScale;
                    cpuObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(generateFrontRangeStart.x, generateFrontRangeEnd.x),
                                                                                Random.Range(generateFrontRangeStart.y, generateFrontRangeEnd.y));
                    cpuObj.GetComponent<RectTransform>().localScale = Vector3.zero;
                    cpuObj.GetComponent<RectTransform>().DOScale(localScale, generateInterval * 3)
                        .SetEase(Ease.InOutBounce);
                    Destroy(cpuObj, generateInterval * 15);
                }
                cpuObj.GetComponent<CpuInstance>().Init(cpus[i]);
                Debug.Log($"Cpu {cpus[i].cpuNumber} ({cpus[i].type.ToString()}) が出現しました。");
                cpus[i].isGened = true;
            }
        }
    }

    /// <summary>
    /// Cpuを初期化する関数
    /// </summary>
    /// <param name="stageInfo"></param>
    public void Init(StageInfoData stageInfo)
    {
        // Cpu出現間隔を計算
        generateInterval = (stageInfo.TimeLimit * 1.0f) / (cpuMax * 1.0f);
        cpus = new CpuData[cpuMax];
        // Cpuデータをランダム生成
        for (int i = 0; i < cpuMax; i++)
        {
            CpuType type = (CpuType)Random.Range(0, 2);
            float genDuration = generateInterval * (i + 1);
            // 終盤ではゆらぎを無効にする
            if (i <= cpuMax - 20)
            {
                // 出現までの時間にゆらぎを加える
                genDuration *= Random.Range(0.8f, 1.2f);
            }
            cpus[i] = new CpuData(type, i + 1, genDuration);
        }
        // 置き換え用Cpuデータで置き換え
        for (int i = 0; i < replaceCpus.Length; i++)
        {
            if (i >= cpuMax)
                break;
            cpus[i] = replaceCpus[i];
        }
    }
}
