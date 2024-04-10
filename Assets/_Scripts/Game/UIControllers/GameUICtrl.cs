using UnityEngine;
using FrameWork.Utils;
using FrameWork.EventCenter;
using UnityEngine.UI;

public class GameUICtrl : UICtrl
{
    private string _inPlay = "InPlay";
    private string _inPrepare = "InPrepare";
    private string _endButton = "EndButton";
    private string _startButton = "InPrepare/StartButton";

    private string _player1CardContainer = "InPlay/Player1CardContainer";
    private string _player2CardContainer = "InPlay/Player2CardContainer";

    private Outline _player1OutLine;
    private Outline _player2OutLine;
    
    private Camera _mainCamera;

    public override void Awake()
    {
        base.Awake();
        GameController.Instance.Init(View["CardContainer"],this);
        
        _mainCamera = Camera.main;

        _player1OutLine = View[_player1CardContainer].GetComponent<Outline>();
        _player2OutLine = View[_player2CardContainer].GetComponent<Outline>();
        
        this.gameObject.SetActive(false);
        EventCenter.AddListener(EventKey.OnStartSelect, OnGameStartSelect);
    }

    private void OnEnable()
    {
        GameController.Instance.InitializePlayers();
        AddButtonListener(_endButton, OnEndButton);
        AddButtonListener(_startButton, OnStartButton);

        SetViewActive(_inPrepare, true);

        GameController.Instance.OnEnable();
    }

    private void OnDisable()
    {
        RemoveButtonListener(_endButton);
        RemoveButtonListener(_startButton);

        SetViewActive(_inPlay, false);
        SetViewActive(_inPrepare, false);

        GameController.Instance.OnDisable();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventKey.OnStartSelect, OnGameStartSelect);
    }

    private void Update()
    {
        //修正必要
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.CurrentGamePlayState == GamePlayState.SelectCards)
        {
        	// スクリーン座標をワールド座標に変換
        	Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, _mainCamera.nearClipPlane));
        	GameController.Instance.SelectCard(mouseWorldPos);
        }
    }

    private void OnEndButton()
    {
        EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.GameOver);
        EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.End);
    }

    private void OnStartButton()
    {
        if (GameManager.Instance.IsOnlineMode)
        {
            NetworkManager.Instance.OnSelectStartButton();
        }
        else
        {
            EventCenter.TriggerEvent(EventKey.OnStartSelect);
        }
        
    }

    /// <summary>
    /// 対戦開始　カード選択画面に移る
    /// </summary>
    void OnGameStartSelect()
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

    public void ChangeContainerOutLineColor(Player player)
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
}