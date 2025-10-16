using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target; 

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

    void Start()
    {
        // --- 边界计算 (这部分代码已存在) ---
        float camHalfHeight = GetComponent<Camera>().orthographicSize;
        float camHalfWidth = camHalfHeight * GetComponent<Camera>().aspect;
        minX = leftBoundary.position.x + camHalfWidth;
        maxX = rightBoundary.position.x - camHalfWidth;

        // --- 新增的“瞬间归位”逻辑 ---
        // 在游戏开始的第一帧，立即将摄像机设置到正确的位置
        if (target != null)
        {
            // 1. 计算出理想的初始位置
            Vector3 initialPosition = target.position + offset;

            // 2. 同样要确保初始位置也遵守边界限制
            initialPosition.x = Mathf.Clamp(initialPosition.x, minX, maxX);
            
            // 3. 直接赋值，而不是平滑移动。这会瞬间完成，玩家看不到过程。
            transform.position = initialPosition;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // --- 以下的平滑跟随逻辑保持不变 ---
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}