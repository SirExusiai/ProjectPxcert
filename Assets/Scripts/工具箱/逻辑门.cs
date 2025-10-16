// ConditionEvaluator.cs
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ConditionEvaluator : MonoBehaviour
{
    [Header("需要满足的条件列表")]
    public List<Condition> requiredConditions = new List<Condition>();

    [Header("信号输出")]
    public UnityEvent OnConditionsMet; // 条件满足时发出的信号
    public UnityEvent OnConditionsNotMet; // 条件不满足时发出的信号

    // 这是一个公共方法，可以被其他组件（比如PlayerInteractor）调用
    public void Evaluate()
    {
        if (AreAllConditionsMet())
        {
            // 发出“条件满足！”的信号
            OnConditionsMet.Invoke();
        }
        else
        {
            // 发出“条件不满足！”的信号
            OnConditionsNotMet.Invoke();
        }
    }

    private bool AreAllConditionsMet()
    {
        foreach (var condition in requiredConditions)
        {
            bool eventCompleted = GameManager.IsEventCompleted(condition.eventToCheck);
            if (condition.checkType == ConditionType.MustBeCompleted && !eventCompleted) return false;
            if (condition.checkType == ConditionType.MustNotBeCompleted && eventCompleted) return false;
        }
        return true;
    }
}