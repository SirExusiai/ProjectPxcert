// ClickInteractor.cs
using UnityEngine;
using UnityEngine.Events;

public class ClickInteractor : MonoBehaviour
{
    [Header("滑鼠懸停時的游標 (可選)")]
    public Texture2D hoverCursor; // 拖曳一張圖片來作為懸停時的滑鼠游標

    [Header("訊號輸出")]
    // 當這個物件被滑鼠左鍵點擊時，發出此訊號
    public UnityEvent OnClick;

    // 當滑鼠指標進入這個物件的碰撞體範圍時被呼叫
    private void OnMouseEnter()
    {
        // 如果設定了懸停游標，就改變它
        if (hoverCursor != null)
        {
            Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 當滑鼠指標離開這個物件的碰撞體範圍時被呼叫
    private void OnMouseExit()
    {
        // 恢復成預設的系統游標
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // 當滑鼠左鍵在這個物件上按下時被呼叫
    private void OnMouseDown()
    {
        // 發出「我被點擊了！」的訊號
        OnClick.Invoke();
    }
}