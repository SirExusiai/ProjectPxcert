using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PrologueManager : MonoBehaviour
{
    [Header("UI组件")]
    public TextMeshProUGUI prologueText; // 拖拽用于显示文字的Text组件

    [Header("文本内容和速度")]
    // 在Inspector中设置所有要播放的句子
    [TextArea(3, 10)] 
    public string[] sentences; 
    
    public float typingSpeed = 0.05f; // 打字机速度
    public float sentenceDelay = 1.5f; // 每句话显示完后的停留时间

    [Header("要加载的下一个场景")]
    public string nextSceneName; // 序章结束后要跳转的场景名

    void Start()
    {
        // 游戏一开始就启动协程来播放序章
        StartCoroutine(PlayPrologue());
    }

    // 主流程协程
    IEnumerator PlayPrologue()
    {
        // 确保开始时文本是空的
        prologueText.text = "";
        // 等待一小段时间，给玩家反应时间
        yield return new WaitForSeconds(1f);

        // 遍历我们设置的所有句子
        foreach (string sentence in sentences)
        {
            // 调用打字机效果的协程，并等待它完成
            yield return StartCoroutine(TypeSentence(sentence));
            
            // 当前句子显示完毕后，等待一段时间
            yield return new WaitForSeconds(sentenceDelay);
        }

        // 所有句子都播放完毕后，加载下一个场景
        Debug.Log("序章播放完毕，正在加载: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    // 打字机效果协程
    IEnumerator TypeSentence(string sentence)
    {
        prologueText.text = ""; // 清空上一句话
        // 将句子转换为字符数组，一个一个地显示出来
        foreach (char letter in sentence.ToCharArray())
        {
            prologueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}