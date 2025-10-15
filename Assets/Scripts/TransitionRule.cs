// TransitionRule.cs
using UnityEngine;
using System.Collections.Generic; // 使用列表需要这个

[System.Serializable]
public class TransitionRule
{
    [Header("规则名称 (方便识别)")]
    public string ruleName;
    
    [Header("需要满足的条件列表 (AND关系)")]
    public List<Condition> requiredConditions = new List<Condition>();
    
    [Header("满足条件后跳转的目标")]
    public string sceneToLoad;
    public Transform targetSpawnPoint;
}