using UnityEngine;

public class CameraController : MonoBehaviour
{
    // [Header("跟随目标")]  <--- 我们不再需要这个公开变量了
    // public Transform target; 
    private Transform target; // 将 target 变成一个私有变量，脚本会自动寻找它

    [Header("移动平滑度")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    [Header("摄像机偏移量")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("地图边界")]
    public Transform leftBoundary;
    public Transform rightBoundary;

    private float minX;
    private float maxX;
    private Camera cam; // 把 Camera 组件缓存起来，提高性能

    void Start()
    {
        // === 优化的核心部分：自动寻找唯一的玩家实例 ===
        // 1. 检查 PlayerController 的单例是否存在
        if (PlayerController.instance != null)
        {
            // 2. 如果存在，就将它的 transform 设为我们的跟随目标
            target = PlayerController.instance.transform;
            Debug.Log("CameraController 成功找到玩家！");
        }
        else
        {
            // 3. 如果由于某种原因没找到，打印一个错误方便调试
            Debug.LogError("CameraController 无法在场景中找到 PlayerController 的实例！");
        }
        
        // --- 缓存 Camera 组件 ---
        cam = GetComponent<Camera>();

        // --- 边界计算 (这部分代码保持不变) ---
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;
        minX = leftBoundary.position.x + camHalfWidth;
        maxX = rightBoundary.position.x - camHalfWidth;

        // --- 瞬间归位逻辑 (现在更加健壮了) ---
        if (target != null)
        {
            Vector3 initialPosition = target.position + offset;
            initialPosition.x = Mathf.Clamp(initialPosition.x, minX, maxX);
            transform.position = initialPosition;
        }
    }

    void LateUpdate()
    {
        // 检查目标是否存在，如果不存在（比如玩家正在被销毁），就什么都不做
        if (target == null)
        {
            return;
        }

        // --- 平滑跟随逻辑 (保持不变) ---
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}