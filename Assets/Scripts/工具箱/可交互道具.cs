// InteractableProp.cs (升級版)
using UnityEngine;
using System.Collections;

public class InteractableProp : MonoBehaviour
{
    [Header("樣式")]
    public Sprite defaultSprite;
    public Sprite changedSprite;

    [Header("解鎖條件")]
    public ToolType requiredTool;

    // --- 新增：用於儲存初始狀態的「記憶」 ---
    private Vector3 originalScale;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;

        // 在遊戲開始時，立刻記下自己當前的縮放值
        originalScale = transform.localScale;
    }

    // 當被滑鼠點擊時，由總監呼叫 (此方法不變)
    void OnMouseDown()
    {
        // 直接呼叫新場景總監的 OnPropClicked 方法，並把自己傳過去
        FindObjectOfType<SceneInteractionManager>().SelectProp(this);
    }

    // --- 新增：一個專門用於恢復初始狀態的方法 ---
    public void ResetState(float duration)
    {
        // 呼叫動畫協程，但目標是我們記住的 originalScale
        Animate(originalScale, 1f, duration);
    }

    // 動畫方法 (本身邏輯不變)
    public void Animate(Vector3 targetScale, float targetAlpha, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateCoroutine(targetScale, targetAlpha, duration));
    }

    private IEnumerator AnimateCoroutine(Vector3 targetScale, float targetAlpha, float duration)
    {
        Vector3 startScale = transform.localScale;
        Color startColor = spriteRenderer.color;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            float newAlpha = Mathf.Lerp(startColor.a, targetAlpha, t);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }

    public void ChangeStyle()
    {
        spriteRenderer.sprite = changedSprite;
    }
}