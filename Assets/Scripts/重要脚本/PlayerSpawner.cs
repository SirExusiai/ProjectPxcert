using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        // 现在 Player 是跨场景存在的，所以我们直接找到它的实例
        GameObject player = PlayerController.instance.gameObject;

        if (player != null && GameManager.nextPlayerPosition != Vector3.zero)
        {
            player.transform.position = GameManager.nextPlayerPosition;
            GameManager.nextPlayerPosition = Vector3.zero;
        }
    }
}