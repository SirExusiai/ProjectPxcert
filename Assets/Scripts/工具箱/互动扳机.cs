// PlayerInteractor.cs
using UnityEngine;
using UnityEngine.Events; // 使用 UnityEvent 必须引入这个命名空间

public class PlayerInteractor : MonoBehaviour
{
    [Header("交互提示UI")]
    public GameObject interactionPrompt;

    [Header("信号输出")]
    // 这是核心！一个可以在Inspector窗口中配置的事件。
    public UnityEvent OnInteract;

    private bool isPlayerInRange = false;

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    void Update()
    {
        // 如果玩家在范围内并且按下了E键
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 发出“我被互动了！”的信号
            OnInteract.Invoke();
        }
    }

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