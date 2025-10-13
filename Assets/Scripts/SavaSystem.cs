using UnityEngine;
using System.IO; // 用于文件操作
using System.Runtime.Serialization.Formatters.Binary; // 用于二进制序列化

public static class SaveSystem
{
    // 定义存档文件的路径
    private static string GetSavePath()
    {
        // Application.persistentDataPath 是 Unity 提供的标准路径，用于在所有平台（PC, Mac, Android, iOS）上存储持久化数据
        return Path.Combine(Application.persistentDataPath, "player.sav");
    }

    // 存档方法
    public static void SaveGame(PlayerController player)
    {
        // 创建一个二进制格式化程序
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetSavePath();

        // 创建文件流来写入文件
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            // 从玩家当前状态创建 GameData 实例
            GameData data = new GameData(player.transform.position);

            // 将数据序列化并写入文件
            formatter.Serialize(stream, data);
        }
    }

    // 读档方法
    public static GameData LoadGame()
    {
        string path = GetSavePath();
        // 检查存档文件是否存在
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                // 从文件中反序列化数据
                GameData data = formatter.Deserialize(stream) as GameData;
                return data;
            }
        }
        else
        {
            // 如果文件不存在，返回 null
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }
}