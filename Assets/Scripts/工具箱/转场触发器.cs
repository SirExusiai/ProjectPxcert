// TransitionTrigger.cs
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    [Header("要跳轉到的目標場景")]
    public string sceneToLoad;

    // 當滑鼠點擊這個物件時被呼叫
    void OnMouseDown()
    {
        // 呼叫「總導演」開始轉場
        if (SceneFader.instance != null)
        {
            SceneFader.instance.FadeToScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("場景中找不到 SceneFader 實例！");
        }
    }
}