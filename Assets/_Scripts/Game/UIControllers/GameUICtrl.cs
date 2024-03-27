using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class GameUICtrl : UICtrl
{
	private string _inPlay = "InPlay";
	private string _inPrepare = "InPrepare";

	private GameBoard _gameBoard;
	
	private Camera _mainCamera;
	public override void Awake() {

		base.Awake();
		_gameBoard = new GameBoard(View["CardContainer"]);
		_mainCamera = Camera.main;
		this.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		SetViewActive(_inPrepare,true);
		_gameBoard?.Subscribe();
	}

	private void OnDisable()
	{
		SetViewActive(_inPlay,false);
		SetViewActive(_inPrepare,false);
		
		_gameBoard?.Unsubscribe();
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

	public void Start()
	{
		
	}
}
