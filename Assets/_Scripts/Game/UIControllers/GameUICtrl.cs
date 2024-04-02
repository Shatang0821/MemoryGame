using UnityEngine;
using FrameWork.Utils;
using FrameWork.EventCenter;

public class GameUICtrl : UICtrl
{
    private string _inPlay = "InPlay";
    private string _inPrepare = "InPrepare";
    private string _endButton = "EndButton";
    private string _startButton = "InPrepare/StartButton";

    private Camera _mainCamera;

    public override void Awake()
    {
        base.Awake();
        GameController.Instance.Init(View["CardContainer"]);
        
        _mainCamera = Camera.main;
        this.gameObject.SetActive(false);
        EventCenter.AddListener(EventKey.OnStartSelect, OnGameStartSelect);
    }

    private void OnEnable()
    {
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
}