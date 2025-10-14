using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

public class UIManager : MonoBehaviour
{
    // 在 Unity 编辑器中，将你的 DialogPanel 拖拽到这里
    public GameObject dialogPanel;
    
    // 将你的 DialogText 文本组件拖拽到这里
    public TextMeshProUGUI dialogText;

    // --- 新增 ---
    [Header("游戏状态UI")]
    public TextMeshProUGUI dayCounterText;
    void Start()
    {
        // 场景加载时，立即更新一次UI
        UpdateDayUI();
    }

    public void UpdateDayUI()
    {
        if (dayCounterText != null)
        {
            dayCounterText.text = "第 " + GameManager.currentDay + " 天";
        }
    }
    // 一个公开的方法，用于显示对话框
    // 参数 message 是要显示的对话内容
    public void ShowDialog(string message)
    {
        // 更新文本内容
        dialogText.text = message;
        // 激活（显示）整个面板
        dialogPanel.SetActive(true);
    }

    // 一个公开的方法，用于隐藏对话框
    public void HideDialog()
    {
        // 禁用（隐藏）整个面板
        dialogPanel.SetActive(false);
    }
}