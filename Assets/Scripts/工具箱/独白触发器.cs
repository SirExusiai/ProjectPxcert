using UnityEngine;
using UnityEngine.SceneManagement;

// 注意：我们不再需要 OnTriggerEnter2D 和 OnTriggerExit2D，
// 因为 PlayerInteractor 会处理交互范围和提示。
public class NarrativeEventTrigger : MonoBehaviour
{
    [Header("1. 独白内容")]
    [TextArea(3, 10)]
    public string[] monologueSentences;

    [Header("2. 独白结束后的行为")]
    public PostMonologueAction actionAfterMonologue = PostMonologueAction.ReturnToPreviousScene;

    [Header("3. (如果加载新场景) 目标场景信息")]
    public string newSceneToLoad;
    public Transform targetSpawnPointInNewScene;

    // 这个公共方法是新的“启动按钮”，可以被其他组件调用
    public void Execute()
    {
        // 1. 传递独白内容
        MonologuePlayer.sentencesToPlay = monologueSentences;

        // 2. 传递“指令牌”
        GameManager.actionAfterMonologue = actionAfterMonologue;

        // 3. 根据指令，准备相应的数据
        if (actionAfterMonologue == PostMonologueAction.ReturnToPreviousScene)
        {
            GameManager.sceneToReturnTo = SceneManager.GetActiveScene().name;
            GameManager.nextPlayerPosition = FindObjectOfType<PlayerController>().transform.position;
            GameManager.hasNextPosition = true;
        }
        else if (actionAfterMonologue == PostMonologueAction.LoadNewScene)
        {
            GameManager.newSceneAfterMonologue = newSceneToLoad;
            if (targetSpawnPointInNewScene != null)
            {
                GameManager.nextPlayerPosition = targetSpawnPointInNewScene.position;
                GameManager.hasNextPosition = true;
            }
            else
            {
                GameManager.hasNextPosition = false;
            }
        }

        // 4. 跳转到独白场景
        SceneManager.LoadScene("MonologueScene");
    }
}