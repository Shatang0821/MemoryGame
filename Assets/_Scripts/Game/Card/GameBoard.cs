using System.Collections.Generic;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;

/// <summary>
/// カード配置情報保持クラス
/// </summary>
public class GameBoard
{
    private GameObject _cardContainer; //カード親オブジェクト    
    private Card[,] _cards; //盤面順番のカード二次元配列

    private Deck _deck; //カード配列クラスのインスタンス
    private Vector2 _initPos = new Vector2(-500, 330); //カード生成時に使用する初期位置

    private const float X_OFFSET = 200; //カード間のx間隔
    private const float Y_OFFSET = 220; //カード間のy間隔
    private const int CARDS_PER_ROW = 6; //一行のカード数
    private int _totalCards; //カード総数

    public GameBoard(Deck deck, GameObject cardContainer)
    {
        //_cards = new Card[4, 6];

        this._deck = deck;
        this._cardContainer = cardContainer;
        _totalCards = _deck.Cards.Count;
    }

    /// <summary>
    /// イベントの登録
    /// </summary>
    public void Subscribe()
    {
        EventCenter.AddListener(EventKey.OnGameStatePrepare, PlacePrepareCard);
        EventCenter.AddListener(EventKey.ShowCardsInBoard, PlaceGameCard);
    }

    /// <summary>
    /// イベントの解除
    /// </summary>
    public void Unsubscribe()
    {
        EventCenter.RemoveListener(EventKey.OnGameStatePrepare, PlacePrepareCard);
        EventCenter.RemoveListener(EventKey.ShowCardsInBoard, PlaceGameCard);
    }

    public void OnEnable()
    {
        _cards = new Card[4, 6];
    }

    /// <summary>
    /// リセット処理
    /// </summary>
    public void OnDisable()
    {
        _cards = null;
    }

    #region カードを配る

    /// <summary>
    /// 準備段階のカードを配る
    /// シャッフル前と表向きのカードを配る
    /// </summary>
    private void PlacePrepareCard()
    {
        // 準備段階でカードを配る
        for (int i = 0; i < _deck.Cards.Count; i++)
        {
            // カードを盤面に配置
            PlaceCardOnBoard(_deck.Cards[i], _deck.CardViews[i], i, true); // 第三引数はカードを表向きにするかどうか
        }
    }

    /// <summary>
    /// ゲーム開始時のカードを配る
    /// シャッフル後と裏向きのカードを配る
    /// </summary>
    public void PlaceGameCard()
    {
        // シャッフルされたカードのIDを基にカードを配る
        for (int i = 0; i < _deck.RandomCardsSelfIdList.Count; i++)
        {
            // シャッフルされたSelfIdリストからカードIDを取得
            int cardSelfId = _deck.RandomCardsSelfIdList[i];

            // カードIDに基づいてカードオブジェクトを取得
            if (_deck.CardTable.TryGetValue(cardSelfId, out Card card) &&
                _deck.CardViewTable.TryGetValue(cardSelfId, out CardView cardView))
            {
                PlaceCardOnBoard(card, cardView, i, false);
            }
        }
    }

    #endregion
    
    /// <summary>
    /// カードを配る
    /// </summary>
    /// <param name="card">カードデータ</param>
    /// <param name="cardView">カードビュー</param>
    /// <param name="index">位置計算するための添え字</param>
    /// <param name="isFront">カードの表裏フラグ</param>
    private void PlaceCardOnBoard(Card card, CardView cardView, int index, bool isFront)
    {
        cardView.CardPrefab.transform.SetParent(_cardContainer.transform);
        // カードの位置を計算（盤面上での行と列）
        int row = index / CARDS_PER_ROW;
        int col = index % CARDS_PER_ROW;
        float posX = _initPos.x + col * X_OFFSET;
        float posY = _initPos.y - row * Y_OFFSET;
        Vector3 cardPosition = new Vector3(posX, posY, 0);

        // カードオブジェクトの位置を更新し、アクティブにする
        cardView.CardPrefab.transform.localPosition = cardPosition;
        cardView.CardPrefab.SetActive(true);
        // カードの表裏を設定
        card.ToggleCardFace();

        // カード配列にカードを保存
        _cards[row, col] = card;
    }
    

    /// <summary>
    /// カードをめくる(仮)
    /// </summary>
    /// <param name="pos"></param>
    public Card SelecteCard(Vector3 pos)
    {
        var card = JudgeCard(pos);
        if (card == null || card.IsFaceUp == true)
        {
            return null;
        }

        return card;
    }


    #region SelecteCard

    /// <summary>
    /// クリック位置を判定してカードを返す
    /// </summary>
    /// <param name="pos">クリック位置</param>
    /// <returns></returns>
    private Card JudgeCard(Vector3 pos)
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