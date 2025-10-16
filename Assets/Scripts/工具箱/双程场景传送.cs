// LoadSceneAndSetSpawnAction.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAndSetSpawnAction : MonoBehaviour
{
    [Header("要跳转的目标场景")]
    public string sceneToLoad;

    [Header("玩家在'下一个场景'的哪个位置出现？")]
    public Transform targetSpawnPoint;

    public void Execute()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("要加载的场景名称为空！请在 Inspector 中设置。");
            return;
        }

        // 在“全局记事本”上记下玩家在下一个场景应该出现的位置
        if (targetSpawnPoint != null)
        {
            GameManager.nextPlayerPosition = targetSpawnPoint.position;
            GameManager.hasNextPosition = true;
        }
        else
        {
            // 如果不指定，就确保信号是关闭的，让玩家出现在目标场景的默认位置
            GameManager.hasNextPosition = false;
        }
        
        Debug.Log("正在加载场景: " + sceneToLoad + "，并已设置好生成点。");
        SceneManager.LoadScene(sceneToLoad);
    }
}