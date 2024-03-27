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
	public override void Awake() {

		base.Awake();
		this.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		AddButtonListener(_restartButton, Restart);
		AddButtonListener(_titleButton, GotoTitle);
	}

	private void OnDisable()
	{
		RemoveButtonListener(_restartButton);
		RemoveButtonListener(_titleButton);
	}

	private void Restart()
	{
		EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.Gameplay);
		EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.Prepare);
	}

	private void GotoTitle()
	{
		EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.Title);
	}

}
