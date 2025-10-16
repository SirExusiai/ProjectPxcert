// FadeInOnStart.cs
using UnityEngine;
using System.Collections;

public class FadeInOnStart : MonoBehaviour
{
    public float fadeInDuration = 1.5f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // 初始时完全透明
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeInDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }
}