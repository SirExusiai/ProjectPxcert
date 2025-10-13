using UnityEngine;

// [System.Serializable] 是一个重要的属性（Attribute）
// 它告诉 Unity 这个类的实例可以被序列化，从而能够被保存到文件中。
[System.Serializable]
public class GameData
{
    public float[] playerPosition; // 用一个浮点数数组来存储 Vector3

    // 构造函数，当创建 GameData 实例时调用
    public GameData(Vector3 position)
    {
        playerPosition = new float[3];
        playerPosition[0] = position.x;
        playerPosition[1] = position.y;
        playerPosition[2] = position.z;
    }
}