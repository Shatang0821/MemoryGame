using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public enum CardImage
{
    Spring,
    Summer,
    Autumn,
    Winter
}
public class Card
{
    public int Id { get; private set; }
    
    public int CardImageNum { get; private set; }
    //public Sprite Sprite { get; private set; }
    public GameObject CardPrefab { get; private set; }
    public Image Image { get; private set; }
    public Sprite FrontSprite { get; private set; }
    public Sprite BackSprite { get; private set; }
    public bool IsMatched { get; private set; }
    public bool IsFaceUp { get; private set; }
    
    /// <summary>
    /// カードクラスの作成
    /// </summary>
    /// <param name="id">カード数字</param>
    /// <param name="cardImage">カード絵札に対応する数字</param>
    /// <param name="gameObject">カードオブジェクト</param>
    public Card(int id,int cardImage,GameObject gameObject)
    {
        this.Id = id;
        //this.Sprite = image;
        this.CardPrefab = gameObject;
        this.Image = gameObject.GetComponent<Image>();
        this.CardImageNum = cardImage;

        SetFrontSprite(cardImage);
        //デフォルトは裏面画像を使用する
        BackSprite = Image.sprite;
    }

    public void SetFrontSprite(int cardImage)
    {
        switch (cardImage)
        {
            case (int)CardImage.Spring:
                FrontSprite = ResLoader.Instance.SpringSprites[Id-1];
                break;
            case (int)CardImage.Summer:
                FrontSprite = ResLoader.Instance.SummerSprites[Id-1];
                break;
            case (int)CardImage.Autumn:
                FrontSprite = ResLoader.Instance.AutumnSprites[Id-1];
                break;
            case (int)CardImage.Winter:
                FrontSprite = ResLoader.Instance.WinterSprites[Id-1];
                break;
        }
    }

    public void Reset()
    {
        SetCardImageFront(false);
        CardPrefab.SetActive(false);
        IsMatched = false;
        IsFaceUp = false;
    }

    public void SetCardImageFront(bool b)
    {
        if (b)
        {
            Image.sprite = FrontSprite;
            IsFaceUp = true;
        }
        else
        {
            Image.sprite = BackSprite;
            IsFaceUp = false;
        }
    }

    public void SetCardMatched()
    {
        IsMatched = true;
        //仮処理
        CardPrefab.SetActive(false);
    }
}