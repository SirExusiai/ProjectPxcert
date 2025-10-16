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
            FinishMonologue();
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
        FinishMonologue();
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

    private void FinishMonologue()
    {
        // --- 核心修改：读取“指令牌”并执行 ---
        switch (GameManager.actionAfterMonologue)
        {
            case PostMonologueAction.ReturnToPreviousScene:
                if (!string.IsNullOrEmpty(GameManager.sceneToReturnTo))
                {
                    SceneManager.LoadScene(GameManager.sceneToReturnTo);
                }
                else
                {
                    Debug.LogError("要返回的场景名丢失！将返回主菜单。");
                    SceneManager.LoadScene("MainMenu");
                }
                break;

            case PostMonologueAction.LoadNewScene:
                if (!string.IsNullOrEmpty(GameManager.newSceneAfterMonologue))
                {
                    SceneManager.LoadScene(GameManager.newSceneAfterMonologue);
                }
                else
                {
                    Debug.LogError("要加载的新场景名丢失！将返回主菜单。");
                    SceneManager.LoadScene("MainMenu");

                }
                break;
            
            default:
                Debug.LogError("未知的独白后行为！将返回主菜单。");
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}