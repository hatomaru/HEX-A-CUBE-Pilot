using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CpuGnerator : MonoBehaviour
{
    bool isInit = false;                  // 初期化フラグ
    public static int cpuMax { get; private set; } = 100;                 // 最大Cpu数

    [SerializeField] Transform frontParent;      // 前方親オブジェクト
    [SerializeField] Transform backParent;       // 後方親オブジェクト
    [SerializeField] Vector2 generateFrontRangeStart;  // Cpu前方出現開始範囲
    [SerializeField] Vector2 generateFrontRangeEnd;    // Cpu前方出現終了範囲
    [SerializeField] Vector2 generateBackRangeStart;   // Cpu後方出現開始範囲
    [SerializeField] Vector2 generateBackRangeEnd;     // Cpu後方出現終了範囲

    [SerializeField] GameObject[] cpuPrefabs;    // Cpuプレハブ配列
    [SerializeField] CpuData[] replaceCpus;      // 置き換え用Cpuデータ配列
    [SerializeField] CpuData[] cpus = new CpuData[cpuMax];   // Cpuデータ配列
    StageUI stageUI;
    List<InputInstance> answerList = new List<InputInstance>(); // 答えの入力インスタンスリスト
    [SerializeField] StageManager stageManager;

    float generateInterval = 0.01f;         // Cpu出現間隔

    private void Awake()
    {
        stageUI = GetComponent<StageUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!StageManager.isInGameLoop || !isInit)
            return;
        // 出現までの時間を減少させる
        for (int i = 0; i < cpuMax; i++)
        {
            if (cpus[i] == null || cpus[i].genDuration < 0)
                continue;
            cpus[i].genDuration -= Time.deltaTime;
            if (cpus[i].genDuration <= 0)
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
                    cpuObj.GetComponent<RectTransform>().DOAnchorPosY(500, generateInterval * 7)
                        .SetEase(Ease.InOutCirc)
                        .SetDelay(generateInterval * 2);
                    Destroy(cpuObj, generateInterval * 10);
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
                stageUI.CpuCountDown();
                if (i == 99)
                {
                    isInit = false;
                }
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
        cpus = new CpuData[cpuMax];
        float cpuGenSec = stageInfo.TimeLimit * 0.15f;
        float beforeSec = 0;
        generateInterval = (cpuGenSec * 1.0f) / (20 * 1.0f);
        // Cpuデータをランダム生成
        for (int i = 0; i < 20; i++)
        {
            CpuType type = CpuType.Misstake;
            float genDuration = beforeSec + (generateInterval * (i + 1));
            // 終盤ではゆらぎを無効にする
            if (i <= cpuMax - 20)
            {
                // 出現までの時間にゆらぎを加える
                genDuration *= Random.Range(0.8f, 1.2f);
            }
            cpus[i] = new CpuData(type, i + 1, genDuration);
        }
        // Cpu出現間隔を計算
        cpuGenSec = stageInfo.TimeLimit * 0.55f;
        beforeSec = stageInfo.TimeLimit * 0.15f;
        generateInterval = (cpuGenSec * 1.0f) / (50 * 1.0f);
        // Cpuデータをランダム生成
        for (int i = 20; i < 70; i++)
        {
            CpuType type = CpuType.Misstake;
            if (Random.Range(0, 100) <= 40)
            {
                type = CpuType.Virus;
            }
            float genDuration = beforeSec + (generateInterval * (i - 19));
            // 終盤ではゆらぎを無効にする
            if (i <= cpuMax - 20)
            {
                // 出現までの時間にゆらぎを加える
                genDuration *= Random.Range(0.8f, 1.2f);
            }
            cpus[i] = new CpuData(type, i + 1, genDuration);
        }
        // Cpu出現間隔を計算
        cpuGenSec = stageInfo.TimeLimit * 0.15f;
        beforeSec = stageInfo.TimeLimit * 0.7f;
        generateInterval = (cpuGenSec * 1.0f) / (25 * 1.0f);
        // Cpuデータをランダム生成
        for (int i = 70; i < 95; i++)
        {
            CpuType type = CpuType.Misstake;
            if (Random.Range(0, 100) <= 80)
            {
                type = CpuType.Virus;
            }
            float genDuration = beforeSec + (generateInterval * (i - 69));
            // 終盤ではゆらぎを無効にする
            if (i <= cpuMax - 20)
            {
                // 出現までの時間にゆらぎを加える
                genDuration *= Random.Range(0.8f, 1.2f);
            }
            cpus[i] = new CpuData(type, i + 1, genDuration);
        }
        // Cpu出現間隔を計算
        cpuGenSec = stageInfo.TimeLimit * 0.15f;
        beforeSec = stageInfo.TimeLimit * 0.85f;
        generateInterval = (cpuGenSec * 1.0f) / (20 * 1.0f);
        // Cpuデータをランダム生成
        for (int i = 95; i < 100; i++)
        {
            CpuType type = CpuType.Misstake;
            if (Random.Range(0, 100) <= 80)
            {
                type = CpuType.Virus;
            }
            float genDuration = beforeSec + (generateInterval * (i - 94));
            // 出現までの時間にゆらぎを加える
            generateInterval *= 1.1f;
            cpus[i] = new CpuData(type, i + 1, genDuration);
        }
        // 置き換え用Cpuデータで置き換え
        for (int i = 0; i < stageManager.stageInfo.replaceCpus.Length; i++)
        {
            if (i >= cpuMax)
                break;
            cpus[i] = stageManager.stageInfo.replaceCpus[i];
        }
        isInit = true;
    }

    /// <summary>
    /// 答えを設定する関数
    /// </summary>
    public void SetAnswer(InputInstance collectAnswer, List<InputInstance> answerList)
    {
        this.answerList.Clear();
        // 回答候補からランダムに3つ答えを設定
        for (int i = 0;i < 3; i++)
        {
            int rmd = Random.Range(0, answerList.Count);
            this.answerList.Add(answerList[rmd]);
        }
        // 確率を公平にする
        this.answerList.Add(this.answerList[2]);
        // 答えをランダムにシャッフル
        for (int i = 0;i < cpuMax;i++)
        {
            if (cpus[i] == null || cpus[i].genDuration < 0)
                continue;
            cpus[i].answer = this.answerList[Random.Range(0, this.answerList.Count)].inputData;
        }
        // 最後のCpuを答えに設定
        cpus[cpuMax - 1].answer = collectAnswer.inputData;
    }
}
