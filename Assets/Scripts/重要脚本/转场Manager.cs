// SceneFader.cs (修正後)
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    [Header("轉場設置")]
    public GameObject fadeCanvasPrefab;
    public float fadeDuration = 1.0f;

    private CanvasGroup fadeCanvasGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GameObject canvasObj = Instantiate(fadeCanvasPrefab, transform);
        fadeCanvasGroup = canvasObj.GetComponent<CanvasGroup>();

        // 【關鍵修正 #1】確保遊戲一開始，幕布就是不阻擋滑鼠的
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutIn(sceneName));
    }

    private IEnumerator FadeOutIn(string sceneName)
    {
        // --- 淡出 (Fade Out) ---
        // 【關鍵修正 #2】在開始淡出前，立刻讓幕布「能夠」阻擋滑鼠
        fadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = timer / fadeDuration;
            timer += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;

        // --- 加載新場景 ---
        yield return SceneManager.LoadSceneAsync(sceneName); // 使用異步加載更流暢

        // --- 淡入 (Fade In) ---
        timer = 0f;
        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = 1f - (timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;

        // 【關鍵修正 #3】在淡入完全結束後，立刻讓幕布「不再」阻擋滑鼠
        fadeCanvasGroup.blocksRaycasts = false;
    }
}