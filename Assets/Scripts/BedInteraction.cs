using UnityEngine;
using System.Collections; // 使用协程需要这个命名空间

public class BedInteraction : MonoBehaviour
{
    [Header("交互提示")]
    public GameObject interactionPrompt; // 例如 "[E] 睡觉"

    private bool isPlayerInRange = false;
    private PlayerController player;
    private UIManager uiManager;

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        player = FindObjectOfType<PlayerController>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 当玩家互动时，开始新的一天
            StartNewDay();
        }
    }

    private void StartNewDay()
    {
        Debug.Log("新的一天开始了！");

        // 1. 增加天数
        GameManager.currentDay++;

        // 2. 保存游戏状态（位置和天数）
        if (player != null)
        {
            player.SavePlayerState(); // 我们需要确保 SavePlayerState 也会保存天数
        }
        
        // 3. 更新UI显示
        if (uiManager != null)
        {
            uiManager.UpdateDayUI();
            // (可选) 显示一个“第 X 天”的过场UI
            // StartCoroutine(ShowDayTransition());
        }

        // 4. (可选) 这里可以添加屏幕淡入淡出等效果
    }

    // (可选的过场效果)
    // IEnumerator ShowDayTransition() { ... }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}