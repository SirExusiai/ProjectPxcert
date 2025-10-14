using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 公开变量，可以在 Unity 编辑器中调整移动速度
    public float moveSpeed = 5f;

    // 私有变量，用于存储 Rigidbody2D 组件的引用
    private Rigidbody2D rb;
    private float moveInput;

    void Awake()
    {
        // 检查是否是从主菜单点击“继续游戏”过来的
        if (MainMenuManager.shouldLoadGame)
        {
            LoadPlayerState();
            // 重置标志，以免每次加载场景都读档
            MainMenuManager.shouldLoadGame = false;
        }
    }
    
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // --- 临时添加：按 S 键保存游戏 ---
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerState();
            Debug.Log("Game Saved!");
        }
    }
    
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    // 保存玩家状态的方法
    public void SavePlayerState()
    {
        // 调用存档系统的静态方法来保存当前玩家数据
        SaveSystem.SaveGame(this);
    }

    // 加载玩家状态的方法
    public void LoadPlayerState()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            // 从加载的数据中恢复位置
            Vector3 position;
            position.x = data.playerPosition[0];
            position.y = data.playerPosition[1];
            position.z = data.playerPosition[2];
            transform.position = position;
            GameManager.currentDay = data.day;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 获取附加到同一个 GameObject 上的 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();
    }
    
}