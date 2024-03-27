using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;

public class GameBoard
{
    private GameObject _cardContainer;
    private Card[,] cards;
    private Deck _deck;

    private Vector2 _initPos = new Vector2(-500, 330);
    private float _xOffset = 200;
    private float _yOffset = 220;
    private int _cardsPerRow = 6;
    private int _totalCards;
    public GameBoard(GameObject gameObject)
    {
        _cardContainer = gameObject;
        cards = new Card[4, 6];
        _deck = new Deck(_cardContainer);
        _totalCards = _deck.Cards.Count;
    }

    public void Subscribe()
    {
        EventCenter.Subscribe(StateKey.OnGameStatePrepare, PlacePrepareCard);
    }

    public void Unsubscribe()
    {
        EventCenter.Unsubscribe(StateKey.OnGameStatePrepare, PlacePrepareCard);
    }

    private void PlacePrepareCard()
    {
        Debug.Log("PlaceCard");

        for (int i = 0; i < _totalCards; i++)
        {
            HandOutCards(i,true);
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Debug.Log(cards[i, j].Id);
            }
        }
    }

    

    public void PlaceGameCard()
    {
        _deck.Shuffle();
        for (int i = 0; i < _totalCards; i++)
        {
            HandOutCards(i,false);
        }
    }
    
    /// <summary>
    /// カードを配って表裏画像を指定する
    /// </summary>
    /// <param name="i">インデックス計算用</param>
    /// <param name="isFront">表面どうか</param>
    private void HandOutCards(int i,bool isFront)
    {
        // 現在の行内でのカードのインデックスを計算
        int cardIndexInRow = i % _cardsPerRow;

        // 現在の行を計算
        int currentRow = i / _cardsPerRow;

        // カードのX軸とY軸の位置を計算
        float posX = _initPos.x + cardIndexInRow * _xOffset;
        float posY = _initPos.y - currentRow * _yOffset; // 上方向に進むためには減算する

        // カードの新しい位置を設定
        Vector3 cardPosition = new Vector3(posX, posY, 0);
        //DebugLogger.Log($"Cards[{currentRow},{cardIndexInRow}]" + cardPosition + _deck.Cards[i].Id);
        _deck.Cards[i].CardPrefab.transform.localPosition = cardPosition;

        _deck.Cards[i].CardPrefab.SetActive(true);

        //シャフル前のカード状態を一度保存して、シールを張るときに必要となります
        cards[currentRow, cardIndexInRow] = _deck.Cards[i];
        if (isFront)
        {
            _deck.Cards[i].SetCardImageFront(isFront);
        }
        else
        {
            _deck.Cards[i].SetCardImageFront(isFront);
        }
    }

    public void FlipCard(Vector3 pos)
    {
        var card = SelecteCard(pos);
        if (card == null)
        {
            DebugLogger.Log("カード選択していないよ");
            return;
        }
        card.SetCardImageFront(true);
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
        int columnIndex = Mathf.FloorToInt((relativeX + halfCardWidth) / _xOffset);
        int rowIndex = Mathf.FloorToInt((relativeY + halfCardHeight) / _yOffset);

        // 範囲チェック
        if (columnIndex >= 0 && columnIndex < _cardsPerRow && rowIndex >= 0 && rowIndex < cards.GetLength(0))
        {
            // クリックされた位置がカードの領域外であるかを確認
            if (IsCardRange(rowIndex, columnIndex, localPos))
            {
                return cards[rowIndex, columnIndex];
            }
            else
            {
                DebugLogger.Log($"Clicked on card at:  {columnIndex},{rowIndex}");
                return null;
            }

            // ここでカードの反転やその他の処理を実行
        }
        else
        {
            DebugLogger.Log("Clicked index out of bounds.");
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