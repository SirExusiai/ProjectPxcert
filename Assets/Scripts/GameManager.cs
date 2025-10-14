using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- 检查这两行是否存在并且完全正确 ---
    public static Vector3 nextPlayerPosition;
    public static bool hasNextPosition = false; // <-- 编译器找不到的就是这一行！

    public static int currentDay = 1;
    private static bool hasBeenCreated = false;

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