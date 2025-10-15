using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestTrigger : MonoBehaviour
{
    [Header("1. 触发这个任务的前置条件")]
    // 在Inspector中选择一个事件作为触发此任务的前提
    public GameEvent requiredEvent = GameEvent.None; 

    [Header("2. 阶段一：独白内容")]
    [TextArea(3, 10)]
    public string[] monologueSentences;

    [Header("3. 阶段二：返回后的对话内容")]
    public Dialogue dialogueAfterMonologue;

    [Header("4. 任务完成后要标记的事件")]
    // (可选) 当整个事件链结束后，在GameManager中标记哪个事件已完成
    public GameEvent eventToCompleteOnFinish = GameEvent.None;

    [Header("场景与UI设置")]
    public string cutsceneSceneName = "MonologueScene";
    public GameObject interactionPrompt;

    // 内部变量
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 检查前置条件是否满足
            if (requiredEvent == GameEvent.None || GameManager.IsEventCompleted(requiredEvent))
            {
                // 检查任务是否已经完成了
                if (eventToCompleteOnFinish != GameEvent.None && GameManager.IsEventCompleted(eventToCompleteOnFinish))
                {
                    // (可选) 如果任务已经做完了，可以显示一句“这里已经没什么可做的了”
                    Debug.Log("这个任务已经完成了。");
                }
                else
                {
                    // 条件满足，且任务未完成，开始事件链
                    TriggerSequence();
                }
            }
            else
            {
                // 条件不满足，可以显示一个“未解锁”的提示
                // (这里的逻辑可以复用GenericCutsceneTrigger的lockedDialogue)
                Debug.Log("现在还不能触发这个事件。");
            }
        }
    }

    private void TriggerSequence()
    {
        // 1. 传递独白内容
        MonologuePlayer.sentencesToPlay = monologueSentences;
        // 2. 传递后续对话内容
        SceneEventManager.dialogueToPlayAfterMonologue = dialogueAfterMonologue;
        
        // 3. 记下返回信息
        GameManager.sceneToReturnTo = SceneManager.GetActiveScene().name;
        GameManager.nextPlayerPosition = FindObjectOfType<PlayerController>().transform.position;
        GameManager.hasNextPosition = true;

        // 4. 跳转到独白场景
        SceneManager.LoadScene(cutsceneSceneName);
    }
    
    // ... OnTriggerEnter2D 和 OnTriggerExit2D 保持不变 ...
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只有当进入的对象是“玩家”时才处理
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // 玩家在范围内
            // 显示交互提示（前提是提示框对象存在）
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 只有当离开的对象是“玩家”时才处理
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // 玩家离开范围
            // 隐藏交互提示
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
            
        }
    }
}