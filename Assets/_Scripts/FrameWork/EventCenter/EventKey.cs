using Unity.IO.LowLevel.Unsafe;

namespace FrameWork.EventCenter
{
    public enum EventKey
    {
        OnChangeUIPrefab,       //UIプレハブの切り替え
        OnStartSelect,
        OnSceneStateChange,
        OnGameStateChange,
        
        OnGameStatePrepare,
        OnGameStateSelectCards,
        OnGameStateCheckCards,
        OnGameStateEnd,
        OnStartOnLine,
        OnLeaveOnline,
        
        ShowCardsInBoard,       //対戦用のかードを配る
        ShowStartButton,        
        
        SetShuffledCard,
    }
    
    

    // public static class StateKey
    // {
    //     public static readonly EventKey OnSceneStateChange = new("OnSceneStateChange");
    //     public static readonly EventKey OnGameStateChange = new("OnGameStateChange");
    //
    //     public static readonly EventKey OnGameStatePrepare = new("OnGameStatePrepare");
    //     public static readonly EventKey OnGameStateSelectCards = new("OnGameStateSelectCards");
    //     public static readonly EventKey OnGameStateCheckCards = new("OnGameStateCheckCards");
    //     public static readonly EventKey OnGameStateEnd = new("OnGameStateEnd");
    // }
}
