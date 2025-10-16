// SceneInteractionManager.cs (再次確認的完整版本)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInteractionManager : MonoBehaviour
{
    [Header("場景組件")]
    public ToolbarController toolbar;
    public List<InteractableProp> allProps;
    public DialogueManager dialogueManager;

    [Header("交互反饋")]
    public Dialogue wrongToolDialogue;
    public Texture2D toolCursorTexture;

    [Header("動畫設置")]
    public float animDuration = 0.5f;
    public Transform focusPoint;

    private InteractableProp selectedProp = null;
    private ToolButton selectedTool = null;

    // SelectProp 應該只顯示工具欄
    public void SelectProp(InteractableProp prop)
    {
        if (selectedProp == null)
        {
            selectedProp = prop;
            toolbar.Show();
        }
    }

    // SelectTool 是決策者
    public void SelectTool(ToolButton tool)
    {
        if (selectedProp == null) return;

        selectedTool = tool;
        toolbar.Hide();

        if (selectedTool.toolType == selectedProp.requiredTool)
        {
            StartCoroutine(FocusAndPrepareWipe(selectedProp));
        }
        else
        {
            dialogueManager.StartDialogue(wrongToolDialogue, ResetSelection);
        }
    }

    // ResetSelection 呼叫道具自己的 ResetState
    public void ResetSelection()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        
        foreach (var p in allProps)
        {
            // 呼叫道具自己的恢復方法
            p.ResetState(animDuration);
        }
        
        toolbar.Hide();
        
        if (selectedProp != null)
        {
             WipeableEffect wipeEffect = selectedProp.GetComponent<WipeableEffect>();
             if (wipeEffect != null) wipeEffect.enabled = false;
        }

        selectedProp = null;
        selectedTool = null;
        Debug.Log("狀態已重置。");
    }

    // FocusAndPrepareWipe 協程 (這裡有個小優化)
    private IEnumerator FocusAndPrepareWipe(InteractableProp propToFocus)
    {
        Cursor.SetCursor(toolCursorTexture, Vector2.zero, CursorMode.Auto);

        foreach (var p in allProps)
        {
            if (p == propToFocus)
            {
                // 讓道具自己去播放移動和縮放的動畫
                // StartCoroutine(AnimateTransform(p.transform, focusPoint.position, Vector3.one * 1.5f, animDuration));
            }
            else
            {
                // 讓其他道具淡出
                p.Animate(p.transform.localScale, 0f, animDuration); // 淡出時保持現有大小
            }
        }

        // 我們可以讓被選中的道具單獨處理移動
        StartCoroutine(AnimateTransform(propToFocus.transform, focusPoint.position, propToFocus.transform.localScale * 1.5f, animDuration)); // 放大1.5倍

        yield return new WaitForSeconds(animDuration);

        WipeableEffect wipeEffect = propToFocus.GetComponent<WipeableEffect>();
        if (wipeEffect != null)
        {
            wipeEffect.enabled = true;
        }
    }

    // 這個協程可以保留，因為它處理了位置移動
    private IEnumerator AnimateTransform(Transform obj, Vector3 targetPos, Vector3 targetScale, float duration)
    {
        Vector3 startPos = obj.position;
        Vector3 startScale = obj.localScale;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            obj.position = Vector3.Lerp(startPos, targetPos, t);
            obj.localScale = Vector3.Lerp(startScale, targetScale, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        obj.position = targetPos;
        obj.localScale = targetScale;
    }
}