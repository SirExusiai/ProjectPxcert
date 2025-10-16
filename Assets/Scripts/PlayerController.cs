using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 单例模式声明
    public static PlayerController instance;

    // 公开变量，可以在 Unity 编辑器中调整移动速度
    public float moveSpeed = 5f;

    // 私有变量，用于存储 Rigidbody2D 组件的引用
    private Rigidbody2D rb;
    private float moveInput;

    private Animator anim;
    private bool isFacingRight = true;

    void Awake()
    {
        // --- 新增代码: Singleton 模式 ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 在加载新场景时不销毁此对象
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 如果已有实例存在，则销毁这个重复的
            return;
        }
        // --- 新增代码结束 ---
        
        // 检查是否是从主菜单点击“继续游戏”过来的
        if (MainMenuManager.shouldLoadGame)
        {
            LoadPlayerState();
            // 重置标志，以免每次加载场景都读档
            MainMenuManager.shouldLoadGame = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 获取附加到同一个 GameObject 上的 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // 获取 Animator 组件
    }
    
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // 根据是否有移动输入来设置 IsWalking 参数
        if (moveInput != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        // 翻转角色朝向
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
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
}