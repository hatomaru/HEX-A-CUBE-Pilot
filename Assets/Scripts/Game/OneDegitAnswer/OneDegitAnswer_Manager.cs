using UnityEngine;

public class OneDegitAnswer_Manager : MonoBehaviour
{
    private CalcData calcData;
    private OneDegitAnswer_CalcGenerator calcGenerator;

    private void Awake()
    {
        calcGenerator = GetComponent<OneDegitAnswer_CalcGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
