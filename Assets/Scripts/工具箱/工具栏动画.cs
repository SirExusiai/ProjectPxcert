// ToolbarController.cs
using UnityEngine;
using System.Collections;

public class 工具栏动画 : MonoBehaviour
{
    public float moveDuration = 0.5f;
    public float hiddenYPosition = -150f;
    public float shownYPosition = 50f;

    private RectTransform rectTransform;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(shownYPosition));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(hiddenYPosition));
    }

    private IEnumerator MoveToPosition(float targetY)
    {
        float startY = rectTransform.anchoredPosition.y;
        float timeElapsed = 0f;
        while (timeElapsed < moveDuration)
        {
            float newY = Mathf.SmoothStep(startY, targetY, timeElapsed / moveDuration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetY);
    }
}