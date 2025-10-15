using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("独白内容 (解锁后)")]
    [TextArea(3, 10)]
    public string[] sentences;

    [Header("未解锁时的对话")]
    // 当条件不满足时，会弹出这段对话作为提示
    public Dialogue lockedDialogue;

    [Header("场景设置")]
    // 要跳转到的独白/过场场景的名称
    public string cutsceneSceneName = "MonologueScene";

    [Header("交互提示")]
    public GameObject interactionPrompt;

    // 内部状态变量
    private bool isPlayerInRange = false;
    // 需要一个DialogueManager的引用来显示“未解锁”的提示
    private DialogueManager dialogueManager;

    void Start()
    {
        // 在游戏开始时，获取场景中唯一的DialogueManager实例
        dialogueManager = FindObjectOfType<DialogueManager>();
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // 只有当玩家在范围内，并且按下了E键时才检查
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 核心修改：在这里检查前置条件（是否已和NPC对话）
            if (GameManager.IsEventCompleted(GameEvent.CompletedMirrorNPCDialogue))
            {
                // 条件满足：触发独白过场
                TriggerCutscene();
            }
            else
            {
                // 条件不满足：显示“未解锁”的提示对话
                if (dialogueManager != null && lockedDialogue.sentences.Length > 0)
                {
                    // 隐藏交互提示，防止重叠
                    if(interactionPrompt != null) interactionPrompt.SetActive(false);
                    // 调用对话管理器来显示提示信息，对话结束后不执行任何操作 (null)
                    dialogueManager.StartDialogue(lockedDialogue, null);
                }
            }
        }
    }

    // 这个方法负责记录信息并跳转到过场场景
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
}