// CompleteGameEventAction.cs
using UnityEngine;

public class CompleteGameEventAction : MonoBehaviour
{
    [Header("要完成的事件")]
    public GameEvent eventToComplete;

    public void Execute()
    {
        if (eventToComplete != GameEvent.None)
        {
            GameManager.CompleteEvent(eventToComplete);
        }
    }
}