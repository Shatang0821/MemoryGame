using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // 唯一のインスタンスを保持する静的変数。
    public static NetworkManager Instance { get; private set; }

    private PhotonView _photonView;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as NetworkManager;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        _photonView = GetComponent<PhotonView>();
    }
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("JoinRoom");
        // 部屋に参加した際の追加のロジックをここに記述
        CheckPlayersInRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Create room");
        PhotonNetwork.CreateRoom(null, new RoomOptions(){MaxPlayers = 2}, TypedLobby.Default);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("新しいプレイヤーが参加しました: {0}", newPlayer.NickName);

        // プレイヤーが部屋に入った際の追加のロジックをここに記述
        CheckPlayersInRoom();
    }
    
    void CheckPlayersInRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        // 部屋のプレイヤー数が2人に達した場合の処理
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("部屋のプレイヤー数が2人になりました。ゲームを開始します。");
            EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.Gameplay);
            EventCenter.TriggerEvent(StateKey.OnGameStateChange, GamePlayState.Prepare);
        }
    }
    
    
}
