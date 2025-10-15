using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MonologuePlayer : MonoBehaviour
{
    [Header("UI组件")]
    public TextMeshProUGUI monologueText;

    [Header("文本内容和速度")]
    // 注意：这个数组的内容将由触发器动态传入
    public static string[] sentencesToPlay;
    public float typingSpeed = 0.05f;
    public float sentenceDelay = 1.5f;

    void Start()
    {
        // 检查是否有要播放的句子
        if (sentencesToPlay == null || sentencesToPlay.Length == 0)
        {
            Debug.LogError("没有要播放的句子！将直接返回。");
            ReturnToPreviousScene();
            return;
        }

        StartCoroutine(PlayMonologue());
    }

    IEnumerator PlayMonologue()
    {
        monologueText.text = "";
        yield return new WaitForSeconds(1f);

        foreach (string sentence in sentencesToPlay)
        {
            yield return StartCoroutine(TypeSentence(sentence));
            yield return new WaitForSeconds(sentenceDelay);
        }

        // 播放完毕，返回之前的场景
        ReturnToPreviousScene();
    }

    IEnumerator TypeSentence(string sentence)
    {
        monologueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            monologueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void ReturnToPreviousScene()
    {
        // 检查“记事本”里是否记录了要返回的场景
        if (!string.IsNullOrEmpty(GameManager.sceneToReturnTo))
        {
            SceneManager.LoadScene(GameManager.sceneToReturnTo);
        }
        else
        {
            // 作为安全保障，如果记录丢失，则返回主菜单
            Debug.LogError("要返回的场景名丢失！将返回主菜单。");
            SceneManager.LoadScene("MainMenu"); 
        }
        // 在返回之前，更新全局任务状态
        GameManager.KeyItemQuestStatus = QuestState.MonologueDone;

        if (!string.IsNullOrEmpty(GameManager.sceneToReturnTo))
        {
            SceneManager.LoadScene(GameManager.sceneToReturnTo);
        }
    }
}