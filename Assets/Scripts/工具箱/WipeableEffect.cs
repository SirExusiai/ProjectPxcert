using UnityEngine;

// 【檢查點 #2】確保這裡繼承了 MonoBehaviour
public class WipeableEffect : MonoBehaviour 
{
    [Header("組件引用")]
    public SpriteRenderer originalSprite;
    public SpriteRenderer changedSprite;
    public SpriteMask spriteMask;

    private Collider2D objectCollider;

    void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
        // 初始時禁用，由總監來激活
        Debug.Log(gameObject.name + " 上的 WipeableEffect 腳本已初始化。");
        this.enabled = false;
    }

    
    // 我們用 OnMouseDown 來測試最基礎的點擊是否能被接收
    void OnMouseDown()
    {
        Debug.Log("滑鼠點擊已被 WipeableEffect 偵測到！");
    }
    // 這個方法只會在 enabled 為 true 時被 Unity 自動呼叫
    void OnMouseDrag()
    {
        Debug.Log("滑鼠拖曳中...");
        // 將滑鼠的螢幕座標轉換為世界座標
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // 確保z軸為0

        // 限制遮罩的移動範圍在物體自身的邊界內
        if (objectCollider != null)
        {
            Bounds bounds = objectCollider.bounds;
            float clampedX = Mathf.Clamp(mouseWorldPos.x, bounds.min.x, bounds.max.x);
            float clampedY = Mathf.Clamp(mouseWorldPos.y, bounds.min.y, bounds.max.y);

            // 移動遮罩
            if (spriteMask != null)
            {
                spriteMask.transform.position = new Vector3(clampedX, clampedY, 0);
                Debug.Log("正在移動 SpriteMask 到位置: " + spriteMask.transform.position);
            }
        }
    }
}