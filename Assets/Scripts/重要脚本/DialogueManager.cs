using UnityEngine;
using UnityEngine.UI; // 使用 Image 需要这个
using TMPro;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("UI组件")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    
    // --- 新增的头像UI引用 ---
    public Image playerAvatarImage;
    public Image npcAvatarImage;
    // (可选) public TextMeshProUGUI speakerNameText;

    private Queue<Sentence> sentences;
    private Action onDialogueEndCallback;

    void Start()
    {
        sentences = new Queue<Sentence>();
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, Action onDialogueEnd)
    {
        dialoguePanel.SetActive(true);
        onDialogueEndCallback = onDialogueEnd;

        sentences.Clear();

        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Sentence currentSentence = sentences.Dequeue();

        // --- 核心逻辑：根据说话者更新UI ---
        // 假设我们通过名字来判断是谁在说话
        if (currentSentence.speaker.speakerName.ToLower() == "player") // 你可以自己定义玩家的名字
        {
            // 是玩家在说话
            playerAvatarImage.gameObject.SetActive(true);

            npcAvatarImage.gameObject.SetActive(false);
            
            playerAvatarImage.sprite = currentSentence.speaker.speakerAvatar;
        }
        else
        {
            // 是NPC在说话
            playerAvatarImage.gameObject.SetActive(false);

            npcAvatarImage.gameObject.SetActive(true);

            npcAvatarImage.sprite = currentSentence.speaker.speakerAvatar;
        }

        // 更新文本内容
        dialogueText.text = currentSentence.text;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        onDialogueEndCallback?.Invoke();
    }
}