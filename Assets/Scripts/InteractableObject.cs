using UnityEngine;
using TMPro; // 如果你想动态改变提示文字，就需要这个

public class InteractableObject : MonoBehaviour
{
    [Header("对话设置")]
    [TextArea(3, 10)]
    public string dialogMessage;

    [Header("交互提示设置")]
    // 在Unity编辑器中，将你的InteractionPromptPanel拖拽到这里
    public GameObject interactionPrompt; 
    // (可选) 如果你想动态改变提示文字，就把PromptText也拖进来
    // public TextMeshProUGUI promptText; 
    // public string promptMessage = "[E] 进行互动";

    // 内部状态变量
    private bool isPlayerInRange = false;
    private bool isDialogOpen = false;

    // UIManager 的引用
    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        // 确保游戏开始时提示框是隐藏的
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // 只有当玩家在范围内时，才监听按键
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 如果对话框是打开的，就关闭它，并重新显示交互提示
            if (isDialogOpen)
            {
                uiManager.HideDialog();
                isDialogOpen = false;

                // 重新显示交互提示，因为玩家还在范围内
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(true);
                }
            }
            // 如果对话框是关闭的，就打开它，并隐藏交互提示
            else
            {
                // 先隐藏交互提示
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(false);
                }

                // 再显示主对话
                uiManager.ShowDialog(dialogMessage);
                isDialogOpen = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // 玩家进入范围，显示交互提示（前提是对话没打开）
            if (!isDialogOpen && interactionPrompt != null)
            {
                // (可选) 在这里可以用 promptText.text = promptMessage 来设置文字
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // 玩家离开，隐藏所有相关UI
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
            if (isDialogOpen)
            {
                uiManager.HideDialog();
                isDialogOpen = false;
            }
        }
    }
}