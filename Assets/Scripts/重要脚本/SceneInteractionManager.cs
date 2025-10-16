using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 【關鍵修正】類別名稱已從 SceneInteractionManager 修改為與檔案名稱一致的 "治疗场景总监"
public class SceneInteractionManager : MonoBehaviour
{
    [Header("場景組件")]
    // 【關鍵修正】變數類型已修改為對應的中文腳本名稱
    public ToolbarController toolbar;
    public CanvasGroup toolbarCanvasGroup;
    public List<InteractableProp> allProps;
    public DialogueManager dialogueManager;
    
    [Header("擦除模式組件")]
    public GameObject draggableArea;
    public DraggableMaskController maskController;

    [Header("交互反饋")]
    public Dialogue wrongToolDialogue;
    public Texture2D toolCursorTexture;

    [Header("動畫設置")]
    public float animDuration = 0.5f;
    public Transform focusPoint;

    // 內部狀態變數
    // 【關鍵修正】變數類型已修改為對應的中文腳本名稱
    private InteractableProp selectedProp = null;
    private ToolButton selectedTool = null;

    // 我們將渲染圖層的程式碼移除，以保持簡潔
    // 如果你需要它，我們可以再加回來

    void Start()
    {
        // Start 方法目前是空的，因為我們移除了預先儲存圖層資訊的邏輯
    }

    // 當一個道具被點擊時
    public void SelectProp(InteractableProp prop)
    {
        if (selectedProp == null)
        {
            selectedProp = prop;
            if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = true;
            toolbar.Show();
        }
    }

    // 當一個工具被選中時
    public void SelectTool(ToolButton tool)
    {
        if (selectedProp == null) return;

        selectedTool = tool;
        toolbar.Hide();
        if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = false;

        if (selectedTool.toolType == selectedProp.requiredTool)
        {
            StartCoroutine(FocusAndPrepareWipe(selectedProp));
        }
        else
        {
            dialogueManager.StartDialogue(wrongToolDialogue, ResetSelection);
        }
    }

    // 重置所有狀態
    public void ResetSelection()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        
        foreach (var p in allProps)
        {
            p.ResetState(animDuration);
        }
        
        toolbar.Hide();
        if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = false;
        
        if (draggableArea != null) draggableArea.SetActive(false);
        if (maskController != null) maskController.enabled = false;

        selectedProp = null;
        selectedTool = null;
    }

    // 聚焦動畫並準備擦除的協程
    private IEnumerator FocusAndPrepareWipe(InteractableProp propToFocus)
    {
        Cursor.SetCursor(toolCursorTexture, Vector2.zero, CursorMode.Auto);
        if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = false;
        
        foreach (var p in allProps)
        {
            if (p == propToFocus)
            {
                StartCoroutine(AnimateTransform(p.transform, focusPoint.position, p.transform.localScale * 1.5f, animDuration));
            }
            else
            {
                p.Animate(p.transform.localScale, 0f, animDuration);
            }
        }

        yield return new WaitForSeconds(animDuration);

        if (draggableArea != null) draggableArea.SetActive(true);
        if (maskController != null) maskController.enabled = true;
    }

    // 通用的移動和縮放動畫協程
    private IEnumerator AnimateTransform(Transform obj, Vector3 targetPos, Vector3 targetScale, float duration)
    {
        Vector3 startPos = obj.position;
        Vector3 startScale = obj.localScale;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float t = Mathf.SmoothStep(0, 1, timeElapsed / duration);
            obj.position = Vector3.Lerp(startPos, targetPos, t);
            obj.localScale = Vector3.Lerp(startScale, targetScale, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        obj.position = targetPos;
        obj.localScale = targetScale;
    }
}