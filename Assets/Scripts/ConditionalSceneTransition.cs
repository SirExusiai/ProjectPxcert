using UnityEngine;
using UnityEngine.SceneManagement;

public class ConditionalSceneTransition : MonoBehaviour
{
    [Header("场景设置")]
    public string sceneForDay1; // 第一天跳转到街道
    public string sceneForDay2AndBeyond; // 第二天及以后跳转到广场

    [Header("玩家生成点")]
    public Transform targetSpawnPointForDay1;
    public Transform targetSpawnPointForDay2AndBeyond;

    [Header("交互提示")]
    public GameObject interactionPrompt;
    
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 根据当前天数决定加载哪个场景和使用哪个生成点
            if (GameManager.currentDay == 1)
            {
                TransitionToScene(sceneForDay1, targetSpawnPointForDay1);
            }
            else
            {
                TransitionToScene(sceneForDay2AndBeyond, targetSpawnPointForDay2AndBeyond);
            }
        }
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

    // ... OnTriggerEnter2D 和 OnTriggerExit2D 方法与普通门脚本一样 ...
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