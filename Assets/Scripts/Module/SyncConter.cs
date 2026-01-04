using UnityEngine;
using unityroom.Api;

public class SyncConter : MonoBehaviour
{
    int syncCounter = 0;

    public void Increment()
    {
        syncCounter++;
    }

    public void GoldIncrement()
    {
        syncCounter += 2;
    }

    public void Reset()
    {
        syncCounter = 0;
    }

}
