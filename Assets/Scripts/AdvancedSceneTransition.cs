using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AdvancedSceneTransition : MonoBehaviour
{
    [Header("规则列表 (从上到下依次检查)")]
    public List<TransitionRule> rules = new List<TransitionRule>();

    [Header("当所有规则都不满足时的反馈")]
    public Dialogue blockedDialogue;

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
            CheckRulesAndTransition();
        }
    }

    private void CheckRulesAndTransition()
    {
        foreach (var rule in rules)
        {
            if (AreAllConditionsMet(rule.requiredConditions))
            {
                TransitionToScene(rule.sceneToLoad, rule.targetSpawnPoint);
                return;
            }
        }

        if (dialogueManager != null && blockedDialogue.sentences.Length > 0)
        {
            if(interactionPrompt != null) interactionPrompt.SetActive(false);
            dialogueManager.StartDialogue(blockedDialogue, null);
        }
    }

    private bool AreAllConditionsMet(List<Condition> conditions)
    {
        foreach (var condition in conditions)
        {
            bool eventCompleted = GameManager.IsEventCompleted(condition.eventToCheck);
            
            if (condition.checkType == ConditionType.MustBeCompleted && !eventCompleted)
            {
                return false;
            }
            if (condition.checkType == ConditionType.MustNotBeCompleted && eventCompleted)
            {
                return false;
            }
        }
        return true;
    }
    
    private void TransitionToScene(string sceneName, Transform spawnPoint)
    {
        if (spawnPoint != null)
        {
            GameManager.nextPlayerPosition = spawnPoint.position;
            GameManager.hasNextPosition = true;
        }
        else
        {
            GameManager.hasNextPosition = false;
        }
        SceneManager.LoadScene(sceneName);
    }
    
    // --- 以下是完整的触发检测方法 ---

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

            // --- 核心修正：在访问 dialogueManager 之前，先检查它是否为 null ---
            if (dialogueManager != null && dialogueManager.dialoguePanel.activeSelf)
            {
                dialogueManager.DisplayNextSentence(); // 强制关闭对话框
            }
        }
    }
}