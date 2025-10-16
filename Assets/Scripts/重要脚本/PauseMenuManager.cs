using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    // 在Unity编辑器中，将你的ConfirmReturnPanel拖拽到这里
    public GameObject confirmReturnPanel;

    // 一个变量，用于查找玩家
    private PlayerController playerController;

    void Start()
    {
        // 游戏开始时，确保弹窗是关闭的
        confirmReturnPanel.SetActive(false);
        // 找到玩家对象的引用，以便之后调用存档方法
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // 监听ESC键
        // 如果确认弹窗没有打开，按下ESC就打开它
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (confirmReturnPanel.activeSelf)
            {
                // 如果弹窗已经打开，按ESC则取消返回（相当于点击取消按钮）
                CancelReturn();
            }
            else
            {
                OpenConfirmation();
            }
        }
    }

    // --- 以下是供UI按钮调用的公共方法 ---

    // 这个方法给左上角的“返回”按钮使用
    public void OpenConfirmation()
    {
        // 显示弹窗
        confirmReturnPanel.SetActive(true);
        // 暂停游戏！Time.timeScale = 0 会让所有基于时间的物理和动画停止
        Time.timeScale = 0f;
    }

    // 这个方法给弹窗里的“确认”按钮使用
    public void ConfirmReturn()
    {
        // 1. 存档
        if (playerController != null)
        {
            playerController.SavePlayerState();
            Debug.Log("游戏已保存！");
        }

        // 2. 恢复游戏时间（非常重要！）
        // 如果不恢复，当你回到主菜单或重新开始游戏时，整个游戏都会是暂停状态
        Time.timeScale = 1f;

        // 3. 返回主菜单
        SceneManager.LoadScene("MainMenu"); // !! 确保 "MainMenu" 是你的主菜单场景的确切文件名
    }

    // 这个方法给弹窗里的“取消”按钮使用
    public void CancelReturn()
    {
        // 隐藏弹窗
        confirmReturnPanel.SetActive(false);
        // 恢复游戏！
        Time.timeScale = 1f;
    }
}