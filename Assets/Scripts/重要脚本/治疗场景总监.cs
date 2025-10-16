using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInteractionManager : MonoBehaviour
{
    [Header("場景組件")]
    public ToolbarController toolbar;
    public CanvasGroup toolbarCanvasGroup;
    public List<InteractableProp> allProps;
    public DialogueManager dialogueManager;
    
    [Header("擦除模式組件")]
    public GameObject draggableArea; // 【重要】將 DraggableArea 物件拖到這裡
    public DraggableMaskController maskController; // 【重要】將 DraggableArea 上的腳本拖到這裡

    [Header("交互反饋")]
    public Dialogue wrongToolDialogue;
    public Texture2D toolCursorTexture;

    [Header("動畫設置")]
    public float animDuration = 0.5f;
    public Transform focusPoint;

    // 內部狀態變數
    private InteractableProp selectedProp = null;
    private ToolButton selectedTool = null;

    // 用於儲存每個道具原始的渲染圖層設定
    private Dictionary<InteractableProp, int> originalOrdersInLayer = new Dictionary<InteractableProp, int>();
    private Dictionary<InteractableProp, string> originalSortingLayers = new Dictionary<InteractableProp, string>();

    void Start()
    {
        // 在遊戲開始時，遍歷所有道具並記下它們的原始圖層設定
        foreach (var prop in allProps)
        {
            var renderer = prop.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                originalOrdersInLayer[prop] = renderer.sortingOrder;
                originalSortingLayers[prop] = renderer.sortingLayerName;
            }
        }
    }

    // 當一個道具被點擊時 (由 InteractableProp 的 OnMouseDown 呼叫)
    public void SelectProp(InteractableProp prop)
    {
        // 只有在「閒置狀態」時才能選擇新道具
        if (selectedProp == null)
        {
            selectedProp = prop;
            // 顯示工具欄時，確保它可以被點擊
            if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = true;
            toolbar.Show();
        }
    }

    // 當一個工具被選中時 (由 ToolButton 的 OnPointerClick 呼叫)
    public void SelectTool(ToolButton tool)
    {
        if (selectedProp == null) return;

        selectedTool = tool;
        toolbar.Hide();

        // 工具被選中後，立刻讓工具欄不再阻擋滑鼠點擊
        if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = false;

        // 核心判斷：工具是否正確？
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

            // 恢復所有道具的原始渲染層級
            var renderer = p.GetComponent<SpriteRenderer>();
            if (renderer != null && originalSortingLayers.ContainsKey(p))
            {
                renderer.sortingLayerName = originalSortingLayers[p];
                renderer.sortingOrder = originalOrdersInLayer[p];
            }
        }
        
        toolbar.Hide();
        if (toolbarCanvasGroup != null) toolbarCanvasGroup.blocksRaycasts = false;
        
        // 重置時，禁用拖曳區域和其腳本
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
        
        var focusRenderer = propToFocus.GetComponent<SpriteRenderer>();
        if (focusRenderer != null)
        {
            focusRenderer.sortingLayerName = "FocusedObject";
        }

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

        // 啟用全新的拖曳系統
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