using UnityEngine;
using System.Collections;

public class SceneEventManager : MonoBehaviour
{
    // --- 新增：一个静态变量，用来接收从上一个场景传来的对话数据 ---
    public static Dialogue dialogueToPlayAfterMonologue;

    [Header("场景组件引用")]
    public DialogueManager dialogueManager;
    public EventMover[] objectsToAnimate; // 把所有需要移动的物体链接到这里

    void Start()
    {
        // 场景加载时，立刻检查任务状态
        if (GameManager.KeyItemQuestStatus == QuestState.MonologueDone)
        {
            // 如果独白刚刚播完，就开始后续的事件序列
            StartCoroutine(PostMonologueSequence());
        }
    }

    private IEnumerator PostMonologueSequence()
    {
        // 1. 等待1秒
        yield return new WaitForSeconds(1f);

        // 2. 检查是否有要播放的对话
        if (dialogueToPlayAfterMonologue != null && dialogueManager != null)
        {
            // 3. 开始对话，并把“触发物体移动”作为对话结束后的回调
            dialogueManager.StartDialogue(dialogueToPlayAfterMonologue, TriggerObjectMovement);
        }
        else
        {
            Debug.LogError("要播放的对话为空或DialogueManager未设置！");
        }
    }

    // 这个方法将在对话结束后被调用
    private void TriggerObjectMovement()
    {
        Debug.Log("对话结束，开始移动物体！");

        // 4. 命令所有动画物件开始移动
        foreach (EventMover mover in objectsToAnimate)
        {
            mover.StartEvent();
        }

        // 5. 更新任务状态为“已完成”，防止重复
        GameManager.KeyItemQuestStatus = QuestState.DialogueDone;
        QuestTrigger sourceTrigger = FindObjectOfType<QuestTrigger>(); // 假设场景中只有一个
        if (sourceTrigger != null && sourceTrigger.eventToCompleteOnFinish != GameEvent.None)
        {
            GameManager.CompleteEvent(sourceTrigger.eventToCompleteOnFinish);
        }
    }
}