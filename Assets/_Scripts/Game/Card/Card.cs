using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CardImage
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class Card
{
    public int SelfId { get; private set; }
    public int Id { get; private set; }
    public int CardImageNum { get; private set; }

    public GameObject CardPrefab { get; private set; }
    public Image Image { get; private set; }
    public Sprite FrontSprite { get; private set; }
    public Sprite BackSprite { get; private set; }

    private RectTransform _rectTransform;
    public bool IsMatched { get; private set; }
    public bool IsFaceUp { get; private set; }
    
    /// <summary>
    /// カードクラスの作成
    /// </summary>
    /// <param name="id">カード数字</param>
    /// <param name="cardImage">カード絵札に対応する数字</param>
    /// <param name="gameObject">カードオブジェクト</param>
    public Card(int id, int cardImage, GameObject gameObject)
    {
        this.Id = id;
        //this.Sprite = image;
        this.CardPrefab = gameObject;
        this.Image = gameObject.GetComponent<Image>();
        this.CardImageNum = cardImage;
        this._rectTransform = gameObject.GetComponent<RectTransform>();
        InitFrontSprite(cardImage);
        //デフォルトは裏面画像を使用する
        BackSprite = Image.sprite;

        this.SelfId = id + CardImageNum * 6;
        Debug.Log(SelfId);
    }

    /// <summary>
    /// 絵札イメージの初期化
    /// </summary>
    /// <param name="cardImageIndex"></param>
    public void InitFrontSprite(int cardImageIndex)
    {
        //カード画像の設定
        switch (cardImageIndex)
        {
            case (int)CardImage.Spring:
                FrontSprite = ResLoader.Instance.SpringSprites[Id - 1];
                break;
            case (int)CardImage.Summer:
                FrontSprite = ResLoader.Instance.SummerSprites[Id - 1];
                break;
            case (int)CardImage.Autumn:
                FrontSprite = ResLoader.Instance.AutumnSprites[Id - 1];
                break;
            case (int)CardImage.Winter:
                FrontSprite = ResLoader.Instance.WinterSprites[Id - 1];
                break;
        }
    }

    /// <summary>
    /// リセット処理
    /// </summary>
    public void Reset()
    {
        ToggleCardFace(false);
        CardPrefab.SetActive(false);
        IsMatched = false;
        IsFaceUp = false;
    }

    /// <summary>
    /// カードの表面または裏面を表示します。
    /// </summary>
    /// <param name="showFace">trueの場合、カードの表面を表示。falseの場合、裏面を表示。</param>
    public void ToggleCardFace(bool showFace)
    {
        RotateCard(showFace);
    }

    private void RotateCard(bool showFace)
    {
        // Dotweenで回転処理を行う
        _rectTransform.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
            // 回転が完了したら
            .OnComplete(() =>
            {
                // 回転が90度に達した時点で、表裏を切り替える
                if (showFace)
                {
                    Image.sprite = FrontSprite; // 表面を表示
                }
                else
                {
                    Image.sprite = BackSprite; // 裏面を表示
                }
                _rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f)
                    // 回転が完了したら
                    .OnComplete(() => { IsFaceUp = showFace; });
            });
    }

    /// <summary>
    /// カードが一致した処理
    /// </summary>
    public void SetCardMatched()
    {
        IsMatched = true;
        //仮処理
        CardPrefab.SetActive(false);
    }
}