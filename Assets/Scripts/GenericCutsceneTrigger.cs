using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericCutsceneTrigger : MonoBehaviour
{
    [Header("前置条件")]
    // 在Inspector中选择一个事件作为触发此过场的前提
    public GameEvent requiredEvent = GameEvent.None; 

    [Header("独白内容 (解锁后)")]
    [TextArea(3, 10)]
    public string[] sentences;

    [Header("未解锁时的对话")]
    public Dialogue lockedDialogue;

    [Header("场景设置")]
    public string cutsceneSceneName = "MonologueScene";

    [Header("交互提示")]
    public GameObject interactionPrompt;

    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // --- 核心修改：使用新的事件系统进行检查 ---
            // 如果前置条件为None，或者该事件已经完成
            if (requiredEvent == GameEvent.None || GameManager.IsEventCompleted(requiredEvent))
            {
                TriggerCutscene();
            }
            else
            {
                // 条件不满足，显示“未解锁”的提示对话
                if (dialogueManager != null && lockedDialogue.sentences.Length > 0)
                {
                    if(interactionPrompt != null) interactionPrompt.SetActive(false);
                    dialogueManager.StartDialogue(lockedDialogue, null);
                }
            }
        }
    }
    private void TriggerCutscene()
    {
        // 1. 把独白内容传递给“独白播放器”
        MonologuePlayer.sentencesToPlay = sentences;

        // 2. 在“全局记事本”上记下当前场景的名字，以便返回
        GameManager.sceneToReturnTo = SceneManager.GetActiveScene().name;

        // 3. 记下玩家的当前位置和“需要移动”的信号，以便返回时定位
        GameManager.nextPlayerPosition = FindObjectOfType<PlayerController>().transform.position;
        GameManager.hasNextPosition = true;

        // 4. 跳转到独白场景
        SceneManager.LoadScene(cutsceneSceneName);
    }

    // 当玩家进入触发区域时调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    // 当玩家离开触发区域时调用
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
    // ... TriggerCutscene(), OnTriggerEnter2D(), OnTriggerExit2D() 方法与之前的脚本完全一样 ...
}