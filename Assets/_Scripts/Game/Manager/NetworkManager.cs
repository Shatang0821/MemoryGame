using FrameWork.EventCenter;
using FrameWork.Utils;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Logger = FrameWork.Utils.Logger;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // 唯一のインスタンスを保持する静的変数。
    public static NetworkManager Instance { get; private set; }

    private PhotonView _photonView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        _photonView = GetComponent<PhotonView>();
        
        
        EventCenter.AddListener(EventKey.OnStartOnLine, StartOnlinePlay);
        EventCenter.AddListener(EventKey.OnLeaveOnline,LeaveOnlinePlay);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventKey.OnStartOnLine, StartOnlinePlay);
        EventCenter.RemoveListener(EventKey.OnLeaveOnline,LeaveOnlinePlay);
    }

    private void StartOnlinePlay()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 接続を切る
    /// </summary>
    private void LeaveOnlinePlay()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        GameManager.Instance.IsOnlineMode = true;
        // 部屋に参加した際の追加のロジックをここに記述
        CheckPlayersInRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // プレイヤーが部屋に入った際の追加のロジックをここに記述
        CheckPlayersInRoom();
    }

    void CheckPlayersInRoom()
    {
        // 部屋のプレイヤー数が2人に達した場合の処理
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            EventCenter.TriggerEvent(EventKey.ShowStartButton);
        }
    }

    public override void OnLeftRoom()
    {
        // 部屋から正常に出た場合に呼び出される
        GameManager.Instance.IsOnlineMode = false;
    }


    public void OnStartButton()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            _photonView.RPC("EnterGameScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void EnterGameScene()
    {
        //GameController.Instance.InitializePlayers();
        EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.Gameplay);
        EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.Prepare);
    }


    public void SendStart()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            _photonView.RPC("EnterSelectScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void EnterSelectScene()
    {
        EventCenter.TriggerEvent(EventKey.OnStartSelect);
    }

    public void SendRestart()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            _photonView.RPC("Restart", RpcTarget.All);
        }
    }

    [PunRPC]
    private void Restart()
    {
        EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.Gameplay);
        EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.Prepare);
    }


    public void SendShuffledCard(int[] shuffledCards)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("ShareShuffledDeck", RpcTarget.Others, shuffledCards);
        }
    }

    [PunRPC]
    private void ShareShuffledDeck(int[] shuffledSelfIds)
    {
        // シャッフルされたSelfIdリストを受け取り、デッキを更新
        EventCenter.TriggerEvent(EventKey.SetShuffledCard, shuffledSelfIds);
    }


    public void SendClickedCard(int cardSelfId, Player player)
    {
        if (PhotonNetwork.IsMasterClient && player.IsMaster)
        {
            _photonView.RPC("ShareClickedCard", RpcTarget.All, cardSelfId);
        }
        else if (!PhotonNetwork.IsMasterClient && !player.IsMaster)
        {
            _photonView.RPC("ShareClickedCard", RpcTarget.All, cardSelfId);
        }
        else
        {
            return;
        }
    }

    [PunRPC]
    private void ShareClickedCard(int cardSelfId)
    {
        GameController.Instance.SyncSelectedCard(cardSelfId);
    }
}