using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("1. 要跳转到哪个场景？")]
    // 在Unity编辑器中，手动输入要加载的场景的文件名
    public string sceneToLoad;

    [Header("2. 玩家在'下一个场景'的哪个位置出现？")]
    // 将目标场景中的生成点拖拽到这里（如果没有特定位置，则留空）
    public Transform targetSpawnPoint;

    [Header("3. 交互提示UI")]
    // 将你的交互提示UI面板（比如 "[E] 进入"）拖拽到这里
    public GameObject interactionPrompt;

    // 内部状态变量
    private bool isPlayerInRange = false;

    void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 在加载新场景之前，设置好“全局记事本”
            if (targetSpawnPoint != null)
            {
                // 如果指定了目标生成点，就记录下它的位置和“需要移动”的信号
                GameManager.nextPlayerPosition = targetSpawnPoint.position;
                GameManager.hasNextPosition = true;
                Debug.Log("记录了下一个生成点: " + targetSpawnPoint.name + " at " + targetSpawnPoint.position);
            }
            else
            {
                // 如果没有指定，就确保信号是关闭的
                GameManager.hasNextPosition = false;
            }
            
            // 加载目标场景
            SceneManager.LoadScene(sceneToLoad);
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