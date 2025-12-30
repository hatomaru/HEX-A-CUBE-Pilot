using UnityEngine;
using UnityEngine.InputSystem;

public class InputModule : MonoBehaviour
{
    StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    /// <summary>
    /// 入力した数字を受け取る
    /// </summary>
    /// <param name="number">入力した数字</param>
    void OnTapNumber(int number)
    {
        stageManager.OnInput(number);
    }

    /// <summary>
    /// 0を押した時の処理
    /// </summary>
    public void OnZero(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(0);
    }

    /// <summary>
    /// 1を押した時の処理
    /// </summary>
    public void OnOne(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(1);
    }

    /// <summary>
    /// 2を押した時の処理
    /// </summary>
    public void OnTwo(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(2);
    }

    /// <summary>
    /// 3を押した時の処理
    /// </summary>
    public void OnThree(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(3);
    }

    /// <summary>
    /// 4を押した時の処理
    /// </summary>
    public void OnFore(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(4);
    }

    /// <summary>
    /// 5を押した時の処理
    /// </summary>
    public void OnFive(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(5);
    }

    /// <summary>
    /// 6を押した時の処理
    /// </summary>
    public void OnSix(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(6);
    }

    /// <summary>
    /// 7を押した時の処理
    /// </summary>
    public void OnSeven(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(7);
    }

    /// <summary>
    /// 8を押した時の処理
    /// </summary>
    public void OnEight(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(8);
    }

    /// <summary>
    /// 9を押した時の処理
    /// </summary>
    public void OnNine(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        OnTapNumber(9);
    }
}
