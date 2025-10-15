using UnityEngine;
using UnityEngine.SceneManagement; // 切换场景必须引入

public class NPCSimpleTransitionController : MonoBehaviour
{
    [Header("NPC的对话内容")]
    public Dialogue dialogue;

    [Header("跳转设置")]
    // 对话结束后要跳转到哪个场景
    public string sceneToLoad; 
    // 玩家在下一个场景的哪个位置出现
    public Transform targetSpawnPoint;

    [Header("交互提示")]
    public GameObject interactionPrompt;

    // 内部状态变量
    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // 如果玩家在范围内，并且对话框没显示，并且按下了E键
        if (isPlayerInRange && !dialogueManager.dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            // 隐藏交互提示，因为马上要开始对话了
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
            
            // 开始对话，并将“跳转场景”这个动作作为对话结束后的回调
            dialogueManager.StartDialogue(dialogue, TransitionToNextScene);
        }
    }

    // 这个方法将在对话结束后被 DialogueManager 调用
    private void TransitionToNextScene()
    {
        Debug.Log("对话结束，准备跳转到场景: " + sceneToLoad);

        // 1. 在“全局记事本”上记下玩家在下一个场景应该出现的位置
        if (targetSpawnPoint != null)
        {
            GameManager.nextPlayerPosition = targetSpawnPoint.position;
            GameManager.hasNextPosition = true;
        }
        else
        {
            GameManager.hasNextPosition = false;
        }

        // 2. 加载目标场景
        SceneManager.LoadScene(sceneToLoad);
    }
    
    // --- 以下是标准的触发检测，与之前的脚本完全一样 ---
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