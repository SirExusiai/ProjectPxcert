using UnityEngine;

[System.Serializable]
public class Sentence
{
    [Header("说话者")]
    public Speaker speaker; // 引用我们上面创建的 Speaker 资产

    [Header("对话内容")]
    [TextArea(3, 10)]
    public string text;
}