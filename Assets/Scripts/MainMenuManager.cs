using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO; // 引入IO命名空间来检查文件

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName;
    
    // 在 Unity 编辑器中，将你的“继续游戏”按钮拖拽到这里
    public GameObject continueButton;

    // 添加一个静态变量，用于在场景切换时传递信息
    public static bool shouldLoadGame = false;

    void Start()
    {
        // 构造存档文件的完整路径
        string path = Path.Combine(Application.persistentDataPath, "player.sav");

        // 在菜单开始时检查存档文件是否存在
        if (File.Exists(path))
        {
            // 如果存在，显示“继续游戏”按钮
            continueButton.SetActive(true);
        }
        else
        {
            // 如果不存在，隐藏“继续游戏”按钮
            continueButton.SetActive(false);
        }
    }

    public void StartGame()
    {
        // 开始新游戏时，确保加载标志为 false
        shouldLoadGame = false;
        SceneManager.LoadScene(gameSceneName);
    }

    // 这个方法将关联到“继续游戏”按钮的 OnClick 事件
    public void ContinueGame()
    {
        // 设置加载标志为 true
        shouldLoadGame = true;
        // 加载游戏场景
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}