using UnityEngine;
using UnityEngine.SceneManagement; // !! 切换场景必须引入这个命名空间

public class SceneTransition : MonoBehaviour
{
    [Header("场景切换设置")]
    // 在Unity编辑器中，手动输入要加载的场景的文件名
    public string sceneToLoad;

    [Header("交互提示")]
    // 将你的交互提示UI面板（比如 "[E] 进入"）拖拽到这里
    public GameObject interactionPrompt;

    // 内部状态变量
    private bool isPlayerInRange = false;

    void Start()
    {
        // 确保游戏开始时提示是隐藏的
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // 如果玩家在范围内并且按下了 E 键
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 确保场景名称不为空
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.Log("正在加载场景: " + sceneToLoad);
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("要加载的场景名称为空！请在Inspector中设置。");
            }
        }
    }

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