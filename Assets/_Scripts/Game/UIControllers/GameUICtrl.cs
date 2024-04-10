using UnityEngine;
using FrameWork.Utils;
using FrameWork.EventCenter;
using UnityEngine.UI;

public class GameUICtrl : UICtrl
{
    #region オブジェクト名

    private string _inPlay = "InPlay";
    private string _inPrepare = "InPrepare";
    private string _endButton = "EndButton";
    private string _startButton = "InPrepare/StartButton";

    private string _player1CardContainer = "InPlay/Player1CardContainer";
    private string _player2CardContainer = "InPlay/Player2CardContainer";

    private string _player1Point = "InPlay/Player1CardContainer/Point";
    private string _player2Point = "InPlay/Player2CardContainer/Point";

    #endregion
    
    //UIコンポーネント
    private Outline _player1OutLine;
    private Outline _player2OutLine;

    private Text _player1PointText;
    private Text _player2PointText;

    //カメラ
    private Camera _mainCamera;

    public override void Awake()
    {
        base.Awake();
        GameController.Instance.Init(View["CardContainer"]);

        _mainCamera = Camera.main;

        ComponentInitialize();

        this.gameObject.SetActive(false);
        
        EventSubscribe();
    }

    /// <summary>
    /// イベントの登録
    /// </summary>
    private void EventSubscribe()
    {
        EventCenter.AddListener(EventKey.OnStartSelect, OnGameStartSelect);
        EventCenter.AddListener<Player>(EventKey.SwitchTurn, ChangeContainerOutLineColor);
        EventCenter.AddListener<int, int>(EventKey.OnChangePoint, ChangePointText);
    }

    /// <summary>
    /// コンポーネントの初期化
    /// </summary>
    private void ComponentInitialize()
    {
        _player1OutLine = View[_player1CardContainer].GetComponent<Outline>();
        _player2OutLine = View[_player2CardContainer].GetComponent<Outline>();

        _player1PointText = View[_player1Point].GetComponent<Text>();
        _player2PointText = View[_player2Point].GetComponent<Text>();
    }

    private void OnEnable()
    {
        AddButtonListener(_endButton, OnEndButton);
        AddButtonListener(_startButton, OnStartButton);
        SetViewActive(_inPrepare, true);
                        
        GameController.Instance.InitializePlayers();
        GameController.Instance.OnEnable();
    }

    private void OnDisable()
    {
        RemoveButtonListener(_endButton);
        RemoveButtonListener(_startButton);

        SetViewActive(_inPlay, false);
        SetViewActive(_inPrepare, false);

        _player1PointText.text = 0.ToString();
        _player2PointText.text = 0.ToString();
        
        
        GameController.Instance.OnDisable();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventKey.OnStartSelect, OnGameStartSelect);
        EventCenter.RemoveListener<Player>(EventKey.SwitchTurn, ChangeContainerOutLineColor);
        EventCenter.RemoveListener<int, int>(EventKey.OnChangePoint, ChangePointText);
    }

    private void Update()
    {
        //修正必要
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.CurrentGamePlayState == GamePlayState.SelectCards)
        {
            // スクリーン座標をワールド座標に変換
            Vector3 mouseWorldPos =
                _mainCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, _mainCamera.nearClipPlane));
            GameController.Instance.SelectCard(mouseWorldPos);
        }
    }

    /// <summary>
    /// 終了ボタン
    /// </summary>
    private void OnEndButton()
    {
        EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.GameOver);
        EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.End);
    }

    private void OnStartButton()
    {
        if (GameManager.Instance.IsOnlineMode)
        {
            NetworkManager.Instance.SendStart();
        }
        else
        {
            EventCenter.TriggerEvent(EventKey.OnStartSelect);
        }
    }

    /// <summary>
    /// 対戦開始　カード選択画面に移る
    /// </summary>
    private void OnGameStartSelect()
    {
        EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.SelectCards);
        ShowSelectGameUI();
    }

    /// <summary>
    /// カード選択画面
    /// </summary>
    private void ShowSelectGameUI()
    {
        View["InPrepare"].SetActive(false);
        View["InPlay"].SetActive(true);

        //カードを配る
        EventCenter.TriggerEvent(EventKey.ShowCardsInBoard);
    }

    /// <summary>
    /// ターンの切り替えに応じてUIの変化処理
    /// </summary>
    /// <param name="player">どのプレイヤー</param>
    private void ChangeContainerOutLineColor(Player player)
    {
        switch (player.PlayerNum)
        {
            case 1:
                _player1OutLine.effectColor = Color.red;
                _player2OutLine.effectColor = Color.black;
                break;
            case 2:
                _player1OutLine.effectColor = Color.black;
                _player2OutLine.effectColor = Color.red;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ポイントUI変更する
    /// </summary>
    /// <param name="playerNum"></param>
    /// <param name="point"></param>
    private void ChangePointText(int playerNum, int point)
    {
        switch (playerNum)
        {
            case 1:
                _player1PointText.text = point.ToString();
                break;
            case 2:
                _player2PointText.text = point.ToString();
                break;
        }
    }
}