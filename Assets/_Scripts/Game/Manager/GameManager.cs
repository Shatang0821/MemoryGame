using System;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum SceneState
{
    Join,
    Title, //タイトル
    Gameplay, //ゲーム中
    GameOver //ゲーム終了
}

public enum GamePlayState
{
    Prepare, //準備段階
    SelectCards, //プレイヤーがカードを選択
    CheckCards, //カードをチェックする状態
    End //終了
}

public class GameManager : PersistentUnitySingleton<GameManager>
{
    [SerializeField] private SceneState _currentSceneState;         //UIPanelのこと
    [SerializeField] private GamePlayState _currentGamePlayState;   //ゲーム進行状態

    public bool IsOnlineMode { get; set; } = false;
    public GamePlayState CurrentGamePlayState => _currentGamePlayState;

    private const string _titleUI = "TitleUI";
    private const string _gameUI = "GameUI";
    private const string _endUI = "EndUI";

    private void Start()
    {
        SetSceneState(SceneState.Title);
    }

    private void OnEnable()
    {
        EventCenter.AddListener<SceneState>(EventKey.OnSceneStateChange, SetSceneState);
        EventCenter.AddListener<GamePlayState>(EventKey.OnGameStateChange, SetGameplayState);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<SceneState>(EventKey.OnSceneStateChange, SetSceneState);
        EventCenter.RemoveListener<GamePlayState>(EventKey.OnGameStateChange, SetGameplayState);
    }

    /// <summary>
    /// シーン状態(UI状態)の設定
    /// </summary>
    /// <param name="newState"></param>
    public void SetSceneState(SceneState newState)
    {
        _currentSceneState = newState;
        // ここでシーン状態に応じたUIの表示や非表示を制御
        OnSceneStateChange();
    }

    /// <summary>
    /// ゲーム状態の設定
    /// </summary>
    /// <param name="obj"></param>
    public void SetGameplayState(GamePlayState newState)
    {
        _currentGamePlayState = newState;
        // ゲーム状態に応じたロジックをここで実行
        OnGamePlayStateChange();
    }

    /// <summary>
    /// シーン状態変更時の処理
    /// </summary>
    private void OnSceneStateChange()
    {
        switch (_currentSceneState)
        {
            case SceneState.Join:
                break;
            case SceneState.Title:
                EventCenter.TriggerEvent(EventKey.OnChangeUIPrefab, _titleUI);
                break;
            case SceneState.Gameplay:
                //UIの表示
                EventCenter.TriggerEvent(EventKey.OnChangeUIPrefab, _gameUI);
                break;
            case SceneState.GameOver:
                EventCenter.TriggerEvent(EventKey.OnChangeUIPrefab, _endUI);
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
                //Debug.Log("PrepareInManager");
                EventCenter.TriggerEvent(EventKey.OnGameStatePrepare);
                break;
            case GamePlayState.SelectCards:
                //Debug.Log("SelectCardsInManager");
                EventCenter.TriggerEvent(EventKey.OnGameStateSelectCards);
                break;
            case GamePlayState.CheckCards:
                //Debug.Log("CheckCardsInManager");
                EventCenter.TriggerEvent(EventKey.OnGameStateCheckCards);
                break;
            case GamePlayState.End:
                //Debug.Log("EndInManager");
                EventCenter.TriggerEvent(EventKey.OnGameStateEnd);
                break;
        }
    }
}