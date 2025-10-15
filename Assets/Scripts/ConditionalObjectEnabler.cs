using UnityEngine;

public class ConditionalObjectEnabler : MonoBehaviour
{
    void Start()
    {
        // 检查那个特定对话是否已经完成
        if (GameManager.IsEventCompleted(GameEvent.CompletedMirrorNPCDialogue))
        {
            // 如果完成了，就隐藏自己
            gameObject.SetActive(false);
        }
        else
        {
            // 如果没完成，就确保自己是显示的
            gameObject.SetActive(true);
        }
    }
}