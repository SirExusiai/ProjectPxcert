using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
        // 在场景加载的最开始就执行
        player = GameObject.FindGameObjectWithTag("Player");

        // 检查“记事本”里是否有记录的位置
        // Vector3.zero 是默认值，如果记事本没被写过，我们就不要移动玩家
        if (GameManager.nextPlayerPosition != Vector3.zero)
        {
            // 找到了记录！立刻把玩家传送到那个位置
            player.transform.position = GameManager.nextPlayerPosition;
            
            // 清空记事本，以免下次无意中再次使用
            GameManager.nextPlayerPosition = Vector3.zero;
        }
    }
}