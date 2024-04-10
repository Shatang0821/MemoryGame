using System.Collections.Generic;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Photon.Pun;

public class GameController : Singleton<GameController>
{
    //プレイヤーインスタンス
    public Player Player1 { get; private set; } 
    public Player Player2 { get; private set; }
    private Player _currentPlayer;
    
    //カード関連
    private List<Card> _selectedCards;                          //選択したカード
    private int _matchedCardTotal;                              //マッチしたカード総数
    
    //盤面関連
    private GameBoard _gameBoard;                            
    private Deck _deck;                                         
    
    public int WinnerNum { get; private set; }
    //カード総数
    private int _cardTotal;
    public void Init(GameObject cardContainer)
    {
        _selectedCards = new List<Card>();
        //Deckクラスの初期化
        _deck = new Deck(cardContainer);
		
        //GameBoardクラスの初期化
        _gameBoard = new GameBoard(_deck,cardContainer);
        _gameBoard.Subscribe();

        _cardTotal = _deck.Cards.Count;
    }

    /// <summary>
    /// プレイヤー設定、オンラインとオフライン対戦
    /// </summary>
    public void InitializePlayers()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("OnLine");
            // ここでマスタークライアントかどうかに基づいて、Player1 と Player2 を初期化
            Player1 = new Player { IsMaster = true, IsMyTurn = true, PlayerNum = 1 };
            Player2 = new Player { IsMaster = false, IsMyTurn = false, PlayerNum = 2 };
        }
        else
        {
            Debug.Log("OffLine");
            // ここでマスタークライアントかどうかに基づいて、Player1 と Player2 を初期化
            Player1 = new Player { IsMaster = true, IsMyTurn = true, PlayerNum = 1 };
            Player2 = new Player { IsMaster = false, IsMyTurn = false, PlayerNum = 2 };
        }
        _currentPlayer = Player1;
        EventCenter.TriggerEvent(EventKey.SwitchTurn, _currentPlayer);
    }

    public void OnEnable()
    {
        _gameBoard?.OnEnable();
        _deck?.OnEnable();

        WinnerNum = -1;
    }

    public void OnDisable()
    {
        Player1 = null;
        Player2 = null;
        
        _matchedCardTotal = 0;
        _selectedCards.Clear();
        
        _gameBoard?.OnDisable();
        _deck?.OnDisable();
    }

    ~GameController()
    {
        _gameBoard.Unsubscribe();
    }
        
   
    
    public async UniTask　SelectCard(Vector3 mouseWorldPos)
    {
        var card = _gameBoard.SelecteCard(mouseWorldPos);
        if (card == null)
        {
            return;
        }
        if (GameManager.Instance.IsOnlineMode)
        {
            NetworkManager.Instance.SendClickedCard(card.SelfId,_currentPlayer);
        }
        else
        {
            SyncSelectedCard(card.SelfId);
        }
    }
    
     
    // カード選択時の処理
    public async void SyncSelectedCard(int selfId)
    {
        await ProcessCardSelection(selfId);
    }

    /// <summary>
    /// カードを選択後の一連処理
    /// </summary>
    /// <param name="selfId">カードID</param>
    private async UniTask ProcessCardSelection(int selfId)
    {
        //カード自身のIDを-1にすればインデックスがわかる
        var card = _deck.Cards[selfId - 1];
        if (!_selectedCards.Contains(card) && _selectedCards.Count < 2)
        {
            //カードクリック処理メソッド
            card.ToggleCardFace(true);
            //
            _selectedCards.Add(card);
        }

        // 2枚のカードが選択されたら自動的に一致判定を行う
        if (_selectedCards.Count == 2)
        {
            EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.CheckCards);
            await CheckCard();
        }
    }
    
    /// <summary>
    /// カードが一致の時のチェック
    /// </summary>
    private async UniTask CheckCard()
    {
        if (_selectedCards[0].Id == _selectedCards[1].Id)
        {
            await UniTask.Delay(450);
            _selectedCards[0].SetCardMatched();
            _selectedCards[1].SetCardMatched();
            _matchedCardTotal += _selectedCards.Count;
            _currentPlayer.MyPoint += 2;
            if (_matchedCardTotal == _cardTotal)
            {
                JudgeWinner();
                EventCenter.TriggerEvent(EventKey.OnSceneStateChange, SceneState.GameOver);
                EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.End);
                return;
            }
        }
        else
        {
            await UniTask.Delay(500);
            _selectedCards[0].ToggleCardFace(false);
            _selectedCards[1].ToggleCardFace(false);
            SwitchTurn();
        }
        _selectedCards.Clear();
        EventCenter.TriggerEvent(EventKey.OnGameStateChange,GamePlayState.SelectCards);  
    }

    /// <summary>
    /// ターンの切り替え
    /// </summary>
    private void SwitchTurn()
    {
        if (_currentPlayer == Player1)
        {
            _currentPlayer = Player2;
        }
        else
        {
            _currentPlayer = Player1;
        }
        EventCenter.TriggerEvent(EventKey.SwitchTurn, _currentPlayer);
    }

    private void JudgeWinner()
    {
        if (Player1.MyPoint > Player2.MyPoint)
        {
            WinnerNum = 1;
        }
        else if (Player1.MyPoint < Player2.MyPoint)
        {
            WinnerNum = 2;
        }
        else
        {
            WinnerNum = 0;
        }
    }
}