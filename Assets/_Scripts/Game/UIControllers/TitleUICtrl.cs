using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using FrameWork.EventCenter;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;


public class TitleUICtrl : UICtrl
{
    private const string _startButton = "StartButton";
    private const string _startOnlineButton = "OnLineStart";
    public override void Awake()
    {
        base.Awake();
        EventCenter.AddListener(EventKey.ShowStartButton,ShowStartButton);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //ボタンにイベントを登録する
        AddButtonListener(_startButton, OnStartButton);
        AddButtonListener("Room/Start",OnLineStartGame);
        AddButtonListener(_startOnlineButton,OnStartOnlineButton);
        AddButtonListener("Room/Leave",OnLeaveButton);
    }

    private void OnDisable()
    {
        //ボタンにイベントを解除する
        RemoveButtonListener(_startButton);
        RemoveButtonListener(_startOnlineButton);
        RemoveButtonListener("Room/Start");
        RemoveButtonListener("Room/Leave");
        
        SetMenuDisable();
    }

    /// <summary>
    /// ルームパネルとそのボタンを非表示にする
    /// </summary>
    private void SetMenuDisable()
    {
        View["Room"].SetActive(false);
        View["Room/Start"].SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventKey.ShowStartButton,ShowStartButton);
    }

    private void OnLeaveButton()
    {
        EventCenter.TriggerEvent(EventKey.OnLeaveOnline);
        SetMenuDisable();
    }

    /// <summary>
    /// スタートボタンクリックしたら
    /// シーン状態をGameplay
    /// ゲーム状態をPrepare
    /// </summary>
    private void OnStartButton()
    {
            EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.Gameplay);
            EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.Prepare);

    }

    /// <summary>
    /// オンラインスタート
    /// </summary>
    private void OnLineStartGame()
    {
        NetworkManager.Instance.OnStartButton(); 
    }

    private void OnStartOnlineButton()
    {
        EventCenter.TriggerEvent(EventKey.OnStartOnLine);
        View["Room"].SetActive(true);
    }
    
    private void ShowStartButton()
    {
        View["Room/Start"].SetActive(true);
    }
    
    
}