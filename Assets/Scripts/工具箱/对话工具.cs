// StartDialogueAction.cs
using UnityEngine;
using UnityEngine.Events;

public class StartDialogueAction : MonoBehaviour
{
    [Header("要播放的对话")]
    public Dialogue dialogue;
    
    [Header("信号输出")]
    public UnityEvent OnDialogueFinished; // 对话结束后发出的信号

    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void Execute()
    {
        if (dialogueManager != null)
        {
            // 开始对话，并将“发出完成信号”这个动作作为回调
            dialogueManager.StartDialogue(dialogue, () => {
                OnDialogueFinished.Invoke();
            });
        }
    }
}