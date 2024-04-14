using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using FrameWork.EventCenter;


public class EndUICtrl : UICtrl
{
	private readonly string _restartButton = "RestartButton";
	private readonly string _titleButton = "TitleButton";
	
	private const string _winnerText = "Win";
	
	private Text _winnerTextComponent;
	public override void Awake() {

		base.Awake();
		_winnerTextComponent = View[_winnerText].GetComponent<Text>();
		this.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		ShowWinnerName();
		AddButtonListener(_restartButton, Restart);
		AddButtonListener(_titleButton, GotoTitle);
	}

	private void OnDisable()
	{
		RemoveButtonListener(_restartButton);
		RemoveButtonListener(_titleButton);
		
		_winnerTextComponent.text = "まだ勝負していない";
	}

	private void Restart()
	{
		if (GameManager.Instance.IsOnlineMode)
		{
			NetworkManager.Instance.SendRestart();
		}
		else
		{
			EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.Gameplay);
			EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.Prepare);
		}
	}

	/// <summary>
	/// タイトルに戻る
	/// </summary>
	private void GotoTitle()
	{
		if (GameManager.Instance.IsOnlineMode)
		{
			EventCenter.TriggerEvent(EventKey.OnLeaveOnline);
			EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.Title);
		}
		else
		{
			EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.Title);
		}
	}
	
	private void ShowWinnerName()
	{
		switch (GameController.Instance.WinnerNum)
		{
			case 0:
				_winnerTextComponent.text = "引き分け";
				break;
			case 1:
				_winnerTextComponent.text = "Player 1 の勝ち";
				break;
			case 2:
				_winnerTextComponent.text = "Player 2 の勝ち";
				break;
			default:
				_winnerTextComponent.text = "まだ勝負していない";
				break;
		}
	}

}
