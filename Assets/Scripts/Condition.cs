// Condition.cs
using UnityEngine;

// 定义条件的检查方式
public enum ConditionType
{
    MustBeCompleted,      // 事件必须已完成
    MustNotBeCompleted    // 事件必须未完成
}

[System.Serializable]
public class Condition
{
    public GameEvent eventToCheck; // 要检查哪个事件
    public ConditionType checkType;    // 如何检查这个事件
}