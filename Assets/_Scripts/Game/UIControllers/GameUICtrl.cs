using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using FrameWork.EventCenter;
using UnityEngine.EventSystems;


public class GameUICtrl : UICtrl
{
	private string _inPlay = "InPlay";
	private string _inPrepare = "InPrepare";
	private string _endButton = "EndButton";
	private string _startButton = "StartButton";
	private GameBoard _gameBoard;
	private Deck _deck;
	
	private Camera _mainCamera;
	
	public override void Awake() {

		base.Awake();
		_deck = new Deck(View["CardContainer"]);
		
		_gameBoard = new GameBoard(_deck,View["CardContainer"]);
		_gameBoard.Subscribe();
		
		_mainCamera = Camera.main;
		this.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		AddButtonListener(_endButton,OnEndButton);
		AddButtonListener(_startButton,OnEndButton);
		
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
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			// スクリーン座標をワールド座標に変換
			Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, _mainCamera.nearClipPlane));

			_gameBoard.FlipCard(mouseWorldPos);
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			_gameBoard.PlaceGameCard();
		}
	}

	private void OnEndButton()
	{
		EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.GameOver);
		EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.End);
	}

	private void OnStartButton()
	{
		EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.SelectCards);
		
	}
	
}
