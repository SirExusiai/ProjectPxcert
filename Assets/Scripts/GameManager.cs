using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    // --- 检查这两行是否存在并且完全正确 ---
    public static Vector3 nextPlayerPosition;
    public static bool hasNextPosition = false; // <-- 编译器找不到的就是这一行！

    public static int currentDay = 1;
    private static bool hasBeenCreated = false;
    public static HashSet<GameEvent> completedEvents = new HashSet<GameEvent>();
    // --- 新增：用于过场返回的变量 ---
    public static string sceneToReturnTo; // 记录要返回的场景名
    // --- 新增：用于追踪特定任务是否激活的旗帜 ---
    public static QuestState KeyItemQuestStatus = QuestState.NotStarted;
    
    public static void CompleteEvent(GameEvent eventToComplete)
    {
        if (!completedEvents.Contains(eventToComplete))
        {
            completedEvents.Add(eventToComplete);
            Debug.Log("游戏事件完成: " + eventToComplete.ToString());
        }
    }

    // 检查某个事件是否已经完成
    public static bool IsEventCompleted(GameEvent eventToCheck)
    {
        return completedEvents.Contains(eventToCheck);
    }


    void Awake()
    {
        if (hasBeenCreated)
        {
            Destroy(gameObject);
        }
        else
        {
            hasBeenCreated = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}