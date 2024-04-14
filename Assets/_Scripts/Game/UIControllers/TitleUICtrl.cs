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
using UnityEngine.InputSystem.Switch;

/// <summary>
/// タイトルUIパネル
/// </summary>
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
        
        SetOnLineMenuDisable();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventKey.ShowStartButton,ShowStartButton);
    }
    
    /// <summary>
    /// オンラインモードから退出
    /// </summary>
    private void OnLeaveButton()
    {
        EventCenter.TriggerEvent(EventKey.OnLeaveOnline);
        SetOnLineMenuDisable();
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
    /// マルチプレイ内のゲームスタート
    /// </summary>
    private void OnLineStartGame()
    {
        //ゲーム状態を切り替わるだけ
        NetworkManager.Instance.OnStartButton(); 
    }

    /// <summary>
    /// マルチプレイボタン
    /// </summary>
    private void OnStartOnlineButton()
    {
        EventCenter.TriggerEvent(EventKey.OnStartOnLine);
        View["Room"].SetActive(true);
    }
    
    /// <summary>
    /// マルチプレイパネル内のスタートボタンの表示
    /// </summary>
    private void ShowStartButton()
    {
        View["Room/Start"].SetActive(true);
    }
    
    /// <summary>
    /// ルームパネルとそのボタンを非表示にする
    /// </summary>
    private void SetOnLineMenuDisable()
    {
        View["Room"].SetActive(false);
        View["Room/Start"].SetActive(false);
    }

   
}