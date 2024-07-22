using Unity.IO.LowLevel.Unsafe;

namespace FrameWork.EventCenter
{
    public enum EventKey
    {
        OnChangeUIPrefab,       //UIプレハブの切り替え
        OnStartSelect,          //スタートボタン
        OnSceneStateChange,     //シーンの切り替え
        OnGameStateChange,      //ゲーム状態切り替え
        
        OnGameStatePrepare,     //ゲームの準備画面
        OnGameStateSelectCards, //カード選択画面
        OnGameStateCheckCards,  //カードチェック画面
        OnGameStateEnd,         //ゲーム終了画面
        OnStartOnLine,          //オンライン対戦開始
        OnLeaveOnline,          //オンライン対戦終了
        
        ShowCardsInBoard,       //対戦用のかードを配る
        ShowStartButton,        //スタートボタンの表示
        
        SetShuffledCard,        //シャッフルされたカードを同期する
        
        SwitchTurn,             //ターンの切り替え
        OnChangePoint,          //ポイントUIの更新
        
        OnPress,                //クリックイベント
    }
}
