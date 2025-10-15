using UnityEngine;
using System; // 使用 Action 回调需要这个命名空间 (已存在，但再次强调)

public class BirdManController : MonoBehaviour
{
     [Header("NPC的对话内容")]
    public Dialogue dialogue;

    [Header("状态变更")]
    public Sprite standingSprite; // (新增) NPC站立时的原始贴图
    public Sprite sittingSprite;
    public Transform standingPosition; // (新增) NPC站立时的原始位置
    public Transform sittingPosition;

    [Header("交互提示")]
    public GameObject interactionPrompt;

    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        // --- 核心修改：在游戏开始时检查全局状态 ---
        // 检查“记事本”里是否已经记录了对话已完成
        if (GameManager.IsEventCompleted(GameEvent.CompletedBirdMan))
        {
            // 如果是，则直接将NPC设置为“已完成”的状态
            SetToCompletedState();
        }
        else
        {
            // 否则，设置为“未完成”的初始状态
            SetToInitialState();
        }
    }

    void Update()
    {
        // 如果玩家在范围内，并且对话框没显示，并且按下了E键
        if (isPlayerInRange && !dialogueManager.dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            // 开始对话，并把 OnDialogueFinished 作为回调
            dialogueManager.StartDialogue(dialogue, OnDialogueFinished);
        }
    }

    // 对话结束时被调用
    public void OnDialogueFinished()
    {
        Debug.Log("对话结束，升起信号旗并改变NPC状态。");

        // 1. 升起全局信号旗
        GameManager.CompleteEvent(GameEvent.CompletedBirdMan);

        // 2. 将NPC设置为“已完成”的状态
        SetToCompletedState();
    }

    // --- 新增的两个状态设置函数 ---
    private void SetToInitialState()
    {
        if (standingSprite != null) spriteRenderer.sprite = standingSprite;
        if (standingPosition != null) transform.position = standingPosition.position;
        // 确保互动组件是开启的
        this.enabled = true;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = true;
    }
    
    private void SetToCompletedState()
    {
        if (sittingSprite != null) spriteRenderer.sprite = sittingSprite;
        if (sittingPosition != null) transform.position = sittingPosition.position;
        // 禁用互动，因为任务已完成
        this.enabled = false;
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;
    }

    // --- 【关键】这两个方法是检测玩家进入/离开触发区域的核心 ---
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
            // 如果对话框还在显示，也应该强制关闭它
            if (dialogueManager.dialoguePanel.activeSelf)
            {
                dialogueManager.DisplayNextSentence(); // 调用一次 DisplayNextSentence 来强制结束对话
            }
        }
    }
}