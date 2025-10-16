// ToolButton.cs
using UnityEngine;
using UnityEngine.EventSystems; // 引入事件系统命名空间

// 实现接口来监听鼠标悬停和点击事件
public class ToolButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("工具标识")]
    public ToolType toolType; // <-- 新增！在Inspector中设置
    
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标悬停时放大
        transform.localScale = originalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开时恢复
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 呼叫新場景總監的 OnToolSelected 方法，並把自己傳過去
        FindObjectOfType<SceneInteractionManager>().SelectTool(this);
    }
}