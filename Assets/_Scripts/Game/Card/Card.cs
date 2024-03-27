using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public int Id { get; private set; }
    //public Sprite Sprite { get; private set; }
    public GameObject CardPrefab { get; private set; }
    public Image Image { get; private set; }
    public Sprite FrontSprite { get; private set; }
    public Sprite BackSprite { get; private set; }
    public bool IsMatched { get; private set; }
    public bool IsFaceUp { get; private set; }
    
    public Card(int id,Sprite sprite,GameObject gameObject)
    {
        this.Id = id;
        //this.Sprite = image;
        this.CardPrefab = gameObject;
        this.Image = gameObject.GetComponent<Image>();
        FrontSprite = sprite;
        //デフォルトは裏面画像を使用する
        BackSprite = Image.sprite;
    }

    public void Reset()
    {
        SetCardImageFront(false);
        CardPrefab.SetActive(false);
    }

    public void SetCardImageFront(bool b)
    {
        if (b)
        {
            Image.sprite = FrontSprite;
        }
        else
        {
            Image.sprite = BackSprite;
        }
    }
}