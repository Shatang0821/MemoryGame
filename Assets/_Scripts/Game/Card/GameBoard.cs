using System.Collections.Generic;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;

public class GameBoard
{
    private GameObject _cardContainer;                          //カード親オブジェクト    
    private Card[,] _cards;                                     //盤面順番のカード二次元配列
    private List<Card> _selectedCards;                          //選択したカード
    private Deck _deck;                                         //カード配列クラスのインスタンス
    private Vector2 _initPos = new Vector2(-500, 330);      //カード生成時に使用する初期位置
    private const float X_OFFSET = 200;                         //カード間のx間隔
    private const float Y_OFFSET = 220;                         //カード間のy間隔
    private const int CARDS_PER_ROW = 6;                        //一行のカード数
    private int _totalCards;                                    //カード総数
    private int _remainCards;                                   //盤面に残っているカード
    public GameBoard(Deck deck,GameObject cardContainer)
    {
        _cards = new Card[4, 6];
        this._deck = deck;
        this._cardContainer = cardContainer;
        _selectedCards = new List<Card>();
        _totalCards = _deck.Cards.Count;
        _remainCards = _totalCards;
    }

    ~GameBoard()
    {
        DebugLogger.Log("GameBoard デストラクタ");
    }

    /// <summary>
    /// イベントの登録
    /// </summary>
    public void Subscribe()
    {
        EventCenter.Subscribe(StateKey.OnGameStatePrepare, PlacePrepareCard);  
    }

    /// <summary>
    /// イベントの解除
    /// </summary>
    public void Unsubscribe()
    {
        EventCenter.Unsubscribe(StateKey.OnGameStatePrepare, PlacePrepareCard);
    }
    
    public void OnEnable()
    {
        DebugLogger.Log("GameBoard OnEnable");
    }

    /// <summary>
    /// リセット処理
    /// </summary>
    public void OnDisable()
    { 
        DebugLogger.Log("GameBoard OnDisable");
        
        // ゲームボード上のカードの状態をリセット
        for (int row = 0; row < _cards.GetLength(0); row++)
        {
            for (int col = 0; col < _cards.GetLength(1); col++)
            {
                if (_cards[row, col] != null)
                {
                    // カードの表裏の状態や位置などをリセット
                    _cards[row, col].Reset(); // CardクラスにResetCardStateメソッドを定義する
                    _cards[row, col] = null;
                }
            }
        }

        _selectedCards.Clear();
        _remainCards = _totalCards;
    }

    #region カードを配る

    /// <summary>
    /// 準備段階のカードを配る
    /// シャッフル前と表向きのカードを配る
    /// </summary>
    private void PlacePrepareCard()
    {
        Debug.Log("PlaceCard");

        for (int i = 0; i < _totalCards; i++)
        {
            HandOutCards(_deck.Cards, i, true);
        }
    }

    /// <summary>
    /// ゲーム開始時のカードを配る
    /// シャッフル後と裏向きのカードを配る
    /// </summary>
    public void PlaceGameCard()
    {
        _deck.Shuffle();
        for (int i = 0; i < _totalCards; i++)
        {
            HandOutCards(_deck.RandomCards, i, false);
        }
    }

    #endregion
    
    
    /// <summary>
    /// カードを配って表裏画像を指定する
    /// </summary>
    /// <param name="i">インデックス計算用</param>
    /// <param name="isFront">表面どうか</param>
    private void HandOutCards(List<Card> cards,int i,bool isFront)
    {
        // 現在の行内でのカードのインデックスを計算
        int cardIndexInRow = i % CARDS_PER_ROW;

        // 現在の行を計算
        int currentRow = i / CARDS_PER_ROW;

        // カードのX軸とY軸の位置を計算
        float posX = _initPos.x + cardIndexInRow * X_OFFSET;
        float posY = _initPos.y - currentRow * Y_OFFSET; // 上方向に進むためには減算する

        // カードの新しい位置を設定
        Vector3 cardPosition = new Vector3(posX, posY, 0);
        //DebugLogger.Log($"Cards[{currentRow},{cardIndexInRow}]" + cardPosition + _deck.Cards[i].Id);
        cards[i].CardPrefab.transform.localPosition = cardPosition;

        cards[i].CardPrefab.SetActive(true);

        //シャッフル前のカード状態を一度保存して、シールを張るときに必要となります
        _cards[currentRow, cardIndexInRow] = cards[i];
        if (isFront)
        {
            cards[i].SetCardImageFront(isFront);
        }
        else
        {
            cards[i].SetCardImageFront(isFront);
        }
    }

    /// <summary>
    /// カードをめくる(仮)
    /// </summary>
    /// <param name="pos"></param>
    public void FlipCard(Vector3 pos)
    {
        var card = SelecteCard(pos);
        
        if (card == null || card.IsFaceUp == true)
        {
            return;
        }
        card.SetCardImageFront(true);
        _selectedCards.Add(card);
        if (_selectedCards.Count >= 2)
        {
            EventCenter.TriggerEvent(StateKey.OnGameStateChange,GamePlayState.CheckCards);
            CheckCard();
            _selectedCards.Clear();
        }
    }

    private void CheckCard()
    {
        if (_selectedCards[0].Id == _selectedCards[1].Id)
        {
            _selectedCards[0].SetCardMatched();
            _selectedCards[1].SetCardMatched();
            _remainCards -= 2;
            if (_remainCards <= 0)
            {
                EventCenter.TriggerEvent(StateKey.OnGameStateChange,GamePlayState.End);
                return;
            }
        }
        else
        {
            _selectedCards[0].SetCardImageFront(false);
            _selectedCards[1].SetCardImageFront(false);
        }
        EventCenter.TriggerEvent(StateKey.OnGameStateChange,GamePlayState.SelectCards);  
        
    }
    #region SelecteCard

    /// <summary>
    /// クリック位置を判定してカードを返す
    /// </summary>
    /// <param name="pos">クリック位置</param>
    /// <returns></returns>
    private Card SelecteCard(Vector3 pos)
    {
        // ローカル座標に変換
        Vector3 localPos = _cardContainer.transform.InverseTransformPoint(pos);

        // カードの幅と高さの半分
        float halfCardWidth = 85;
        float halfCardHeight = 100;
        // カードの中心からのオフセット
        Vector2 firstCardCenterOffset = new Vector2(-500, 330);
        // クリック位置がカードの中心からどのくらい離れているかを計算
        float relativeX = localPos.x - firstCardCenterOffset.x;
        float relativeY = firstCardCenterOffset.y - localPos.y;

        //DebugLogger.Log($"{relativeX},{relativeY}");
        // クリックされた位置からカードのインデックスを計算
        int columnIndex = Mathf.FloorToInt((relativeX + halfCardWidth) / X_OFFSET);
        int rowIndex = Mathf.FloorToInt((relativeY + halfCardHeight) / Y_OFFSET);

        // 範囲チェック
        if (columnIndex >= 0 && columnIndex < CARDS_PER_ROW && rowIndex >= 0 && rowIndex < _cards.GetLength(0))
        {
            // クリックされた位置がカードの領域外であるかを確認
            if (IsCardRange(rowIndex, columnIndex, localPos))
            {
                return _cards[rowIndex, columnIndex];
            }
            else
            {
                return null;
            }

            // ここでカードの反転やその他の処理を実行
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// カード選択チェック
    /// カードの範囲内にクリックしたかをチェック
    /// </summary>
    /// <param name="rowIndex">y</param>
    /// <param name="columnIndex">x</param>
    /// <param name="clickPos">クリック位置</param>
    /// <returns></returns>
    private bool IsCardRange(int rowIndex, int columnIndex, Vector3 clickPos)
    {
        var centerX = columnIndex * 200 - 500;
        var centerY = 330 - rowIndex * 220;

        if (clickPos.x <= (centerX + 85) &&
            clickPos.x >= (centerX - 85) &&
            clickPos.y <= (centerY + 100) &&
            clickPos.y >= (centerY - 100)
           )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}