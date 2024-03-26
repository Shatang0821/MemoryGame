using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;

public enum SceneState
{
    Title,      //タイトル
    Gameplay,   //ゲーム中
    GameOver    //ゲーム終了
}

public enum GamePlayState
{
    Prepare,        //準備段階
    SelectCards,    //プレイヤーがカードを選択
    CheckCards,     //カードをチェックする状態
    ComparePoints,  //ポイント比較状態
    End             //終了
}
public class GameManager : PersistentUnitySingleton<GameManager>
{
    private SceneState _currentSceneState;
    private GamePlayState _currentGamePlayState;


    private readonly string TitleUI = "TitleUI";
    private readonly string GameUI = "GameUI";
    protected override void Awake()
    {
        base.Awake();
        SetSceneState(SceneState.Title);
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(StateKey.OnSceneStateChange,SetSceneState);
        EventCenter.Subscribe(StateKey.OnGameStateChange,SetGameplayState);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(StateKey.OnSceneStateChange,SetSceneState);
        EventCenter.Unsubscribe(StateKey.OnGameStateChange,SetGameplayState);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newState"></param>
    public void SetSceneState(object obj)
    {
        if (obj is SceneState newState)
        {
            _currentSceneState = newState;
            // ここでシーン状態に応じたUIの表示や非表示を制御
            OnSceneStateChange();
        }
        else
        {
            DebugLogger.LogWarning("引数の型が違う");
        }
        
    }
    
    public void SetGameplayState(object obj)
    {
        if (obj is GamePlayState newState)
        {
            _currentGamePlayState = newState;
            // ゲーム状態に応じたロジックをここで実行
            OnGamePlayStateChange();
        }
        else
        {
            DebugLogger.LogWarning("引数の型が違う");
        }
        
    }

    /// <summary>
    /// シーン状態変更時の処理
    /// </summary>
    private void OnSceneStateChange()
    {
        switch (_currentSceneState)
        {
            case SceneState.Title:
                EventCenter.TriggerEvent(UIEventKey.OnChangeUIPrefab,TitleUI);
                break;
            case SceneState.Gameplay:
                EventCenter.TriggerEvent(UIEventKey.OnChangeUIPrefab,GameUI);
                break;
            case SceneState.GameOver:
                break;
        }
    }
    

    /// <summary>
    /// ゲーム状態変更時の処理
    /// </summary>
    private void OnGamePlayStateChange()
    {
        switch (_currentGamePlayState)
        {
            case GamePlayState.Prepare:
                break;
            case GamePlayState.SelectCards:
                break;
            case GamePlayState.CheckCards:
                break;
            case GamePlayState.ComparePoints:
                break;
            case GamePlayState.End:
                break;
        }
    }
    
    
}