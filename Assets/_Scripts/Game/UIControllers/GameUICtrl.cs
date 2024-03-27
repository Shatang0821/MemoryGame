using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using FrameWork.EventCenter;
using Photon.Pun;
using UnityEngine.EventSystems;


public class GameUICtrl : UICtrl
{
	private string _inPlay = "InPlay";
	private string _inPrepare = "InPrepare";
	private string _endButton = "EndButton";
	private string _startButton = "InPrepare/StartButton";
	private GameBoard _gameBoard;
	private Deck _deck;
	
	private Camera _mainCamera;
	
	public override void Awake() {

		base.Awake();
		this.gameObject.AddComponent<PhotonView>();
		_deck = new Deck(View["CardContainer"]);
		
		_gameBoard = new GameBoard(_deck,View["CardContainer"]);
		_gameBoard.Subscribe();

		_mainCamera = Camera.main;
		this.gameObject.SetActive(false);
		EventCenter.Subscribe(UIEventKey.OnStartSelect,OnGameStartSelect);
	}

	private void OnEnable()
	{
		AddButtonListener(_endButton,OnEndButton);
		AddButtonListener(_startButton,OnStartButton);
		
		SetViewActive(_inPrepare,true);
		
		_gameBoard?.OnEnable();
		_deck?.OnEnable();
	}

	private void OnDisable()
	{
		RemoveButtonListener(_endButton);
		RemoveButtonListener(_startButton);
		
		SetViewActive(_inPlay,false);
		SetViewActive(_inPrepare,false);
		
		_gameBoard?.OnDisable();
		_deck?.OnDisable();
	}

	private void OnDestroy()
	{
		_gameBoard.Unsubscribe();
		EventCenter.Unsubscribe(UIEventKey.OnStartSelect,OnGameStartSelect);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && GameManager.Instance.CurrentGamePlayState == GamePlayState.SelectCards)
		{
			// スクリーン座標をワールド座標に変換
			Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, _mainCamera.nearClipPlane));

			_gameBoard.FlipCard(mouseWorldPos);
		}
	}

	private void OnEndButton()
	{
		EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.GameOver);
		EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.End);
	}
	
	private void OnStartButton()
	{
		NetworkManager.Instance.OnSelcteStartButton();
	}
	
	void OnGameStartSelect()
	{
		EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.SelectCards);
		ShowSelectGameUI();
	}

	private void ShowSelectGameUI()
	{
		View["InPrepare"].SetActive(false);
		View["InPlay"].SetActive(true);
    
		_gameBoard.PlaceGameCard();
	}
}
