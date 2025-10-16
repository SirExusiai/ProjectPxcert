using UnityEngine;
using UnityEngine.EventSystems;

public class UI拖动控制器 : MonoBehaviour, IDragHandler // <--- 類別名稱已修正
{
    [Header("要控制的目標")]
    public Transform targetMaskTransform; 

    private RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint);
        
        if (targetMaskTransform != null)
        {
            targetMaskTransform.position = new Vector3(localPoint.x, localPoint.y, 0);
        }
    }
}