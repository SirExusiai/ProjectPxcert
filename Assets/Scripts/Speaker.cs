using UnityEngine;

// [CreateAssetMenu] 让我们可以在 Unity 编辑器里像创建材质球一样创建“说话者”资产
[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/New Speaker")]
public class Speaker : ScriptableObject 
{
    [Header("说话者名字")]
    public string speakerName;
    
    [Header("说话者头像")]
    public Sprite speakerAvatar;
}