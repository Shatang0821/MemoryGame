using System;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    End             //終了
}
public class GameManager : PersistentUnitySingleton<GameManager>
{
    [SerializeField]
    private SceneState _currentSceneState;
    [SerializeField]
    private GamePlayState _currentGamePlayState;

    private readonly string _titleUI = "TitleUI";
    private readonly string _gameUI = "GameUI";
    private readonly string _endUI = "EndUI";
    
    protected override void Awake()
    {
        base.Awake();
        DebugLogger.Log("GameManager");
    }

    private void Start()
    {
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
    /// シーン状態(UI状態)の設定
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
    
    /// <summary>
    /// ゲーム状態の設定
    /// </summary>
    /// <param name="obj"></param>
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
                EventCenter.TriggerEvent(UIEventKey.OnChangeUIPrefab,_titleUI);
                break;
            case SceneState.Gameplay:
                EventCenter.TriggerEvent(UIEventKey.OnChangeUIPrefab,_gameUI);
                break;
            case SceneState.GameOver:
                EventCenter.TriggerEvent(UIEventKey.OnChangeUIPrefab,_endUI);
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
                Debug.Log("PrepareInManager");
                EventCenter.TriggerEvent(StateKey.OnGameStatePrepare);
                break;
            case GamePlayState.SelectCards:
                Debug.Log("SelectCardsInManager");
                EventCenter.TriggerEvent(StateKey.OnGameStateSelectCards);
                break;
            case GamePlayState.CheckCards:
                Debug.Log("CheckCardsInManager");
                EventCenter.TriggerEvent(StateKey.OnGameStateCheckCards);
                break;
            case GamePlayState.End:
                Debug.Log("EndInManager");
                EventCenter.TriggerEvent(StateKey.OnGameStateEnd);
                break;
        }
    }
}