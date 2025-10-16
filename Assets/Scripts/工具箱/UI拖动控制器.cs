// DraggableMaskController.cs
using UnityEngine;
using UnityEngine.EventSystems; // 使用 UI 事件系統必須引入！

public class DraggableMaskController : MonoBehaviour, IDragHandler
{
    [Header("要控制的目標")]
    public Transform targetMaskTransform; // 【需要拖曳】將你的 Sprite Mask 物件拖到這裡

    private RectTransform canvasRectTransform;

    void Start()
    {
        // 獲取 Canvas 的 RectTransform，用於座標轉換
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    // 當在這個 UI 元素上拖曳時，每一幀都會被呼叫
    public void OnDrag(PointerEventData eventData)
    {
        // 將滑鼠的螢幕座標轉換為 Canvas 內的局部座標
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint);

        // 因為我們的 DraggableArea 在螢幕中央，所以 Canvas 的局部座標可以直接當作世界座標的偏移量
        // （前提是 Canvas 是 Screen Space - Overlay 模式）
        if (targetMaskTransform != null)
        {
            // 將 Canvas 的 2D 座標直接賦值給世界中遮罩的 3D 座標
            targetMaskTransform.position = new Vector3(localPoint.x, localPoint.y, 0);
        }
    }
}