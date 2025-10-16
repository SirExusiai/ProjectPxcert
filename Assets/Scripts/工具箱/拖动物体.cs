// DraggableObject.cs
using UnityEngine;
using UnityEngine.Events;

public class DraggableObject : MonoBehaviour
{
    [Header("拖曳設定")]
    // (可選) 設定邊界，防止物體被拖出螢幕
    public Transform minBounds; 
    public Transform maxBounds;

    [Header("訊號輸出")]
    // (可選) 當拖曳結束時發出的訊號
    public UnityEvent OnDragEnd;

    // 內部狀態變數
    private bool isDraggable = false; // 核心！控制此物件當前是否可被拖曳
    private Vector3 offset;

    // 這是一個公共方法，可以被 ConditionEvaluator 呼叫來「解鎖」拖曳功能
    public void EnableDragging()
    {
        isDraggable = true;
        Debug.Log(gameObject.name + " 的拖曳功能已被啟用！");
    }

    // (可選) 一個用來「上鎖」的方法
    public void DisableDragging()
    {
        isDraggable = false;
        Debug.Log(gameObject.name + " 的拖曳功能已被禁用。");
    }

    // 當滑鼠按下時被呼叫
    void OnMouseDown()
    {
        // 只有在「可拖曳」狀態下才執行
        if (!isDraggable) return;

        // 計算滑鼠點擊位置與物件中心的偏移量
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // 當滑鼠拖曳時被呼叫
    void OnMouseDrag()
    {
        // 只有在「可拖曳」狀態下才執行
        if (!isDraggable) return;

        // 計算滑鼠在世界中的新位置
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPosition.z = transform.position.z; // 確保Z軸不變

        // 如果設定了邊界，則限制位置
        if (minBounds != null && maxBounds != null)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.position.x, maxBounds.position.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.position.y, maxBounds.position.y);
        }

        // 更新物件位置
        transform.position = newPosition;
    }

    // 當滑鼠釋放時被呼叫
    void OnMouseUp()
    {
        // 只有在「可拖曳」狀態下才執行
        if (!isDraggable) return;

        // 發出「拖曳結束」的訊號
        OnDragEnd.Invoke();
    }
}