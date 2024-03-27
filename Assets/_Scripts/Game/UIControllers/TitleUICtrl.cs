using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using FrameWork.EventCenter;
using Unity.VisualScripting;


public class TitleUICtrl : UICtrl
{
    private readonly string _startButton = "StartButton";
    public override void Awake()
    {
        base.Awake();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AddButtonListener(_startButton, OnStartButton);
    }

    private void OnDisable()
    {
        RemoveButtonListener(_startButton);
    }

    /// <summary>
    /// スタートボタンクリックしたら
    /// シーン状態をGameplay
    /// ゲーム状態をPrepare
    /// </summary>
    private void OnStartButton()
    {
        EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.Gameplay);
        EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.Prepare);
    }
    
}