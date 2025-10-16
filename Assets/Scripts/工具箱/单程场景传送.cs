// LoadSceneAction.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAction : MonoBehaviour
{
    [Header("要跳转的目标场景")]
    public string sceneToLoad;

    // 这个公共方法可以被任何 UnityEvent 调用
    public void Execute()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("要加载的场景名称为空！请在 Inspector 中设置。");
            return;
        }
        
        Debug.Log("正在加载场景: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}