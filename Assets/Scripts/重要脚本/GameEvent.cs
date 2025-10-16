// GameEvents.cs

// 这个枚举将定义我们游戏中所有可以被追踪的关键事件
public enum GameEvent
{
    None, // 代表没有前置条件
    CompletedMirrorNPCDialogue, // 完成了镜子NPC的对话
    DayTwoStarted, // 第二天已开始
    CompletedBirdMan, // 与鸟人对话完成
    RepairedTheBridge,// 修好了桥
    
    与工作台交互,
    测试事件
    // ... 未来你可以根据需要在这里添加更多事件
}
public enum PostMonologueAction
{
    ReturnToPreviousScene,
    LoadNewScene
}