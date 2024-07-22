using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum CardImageKind
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class CardView
{
    public GameObject CardPrefab { get; private set; }  //カードオブジェクト
    public Image Image { get; private set; }            //イメージコンポーネント
    public Sprite FrontSprite { get; private set; }     //表面画像
    public Sprite BackSprite { get; private set; }      //裏面画像

    private RectTransform _rectTransform;
    
    public CardView(GameObject gameObject,CardImageKind cardImageKind,int id)
    {
        this.CardPrefab = gameObject;
        this.Image = gameObject.GetComponent<Image>();
        this._rectTransform = gameObject.GetComponent<RectTransform>();
        
        InitFrontSprite((int)cardImageKind,id);
        
        //デフォルトは裏面画像を使用する
        BackSprite = Image.sprite;
    }

    /// <summary>
    /// 絵札イメージの初期化
    /// </summary>
    /// <param name="cardKind">カードの種類</param>
    /// <param name="id">カード番号</param>
    private void InitFrontSprite(int cardKind,int id)
    {
        //カード画像の設定
        switch (cardKind)
        {
            case (int)CardImageKind.Spring:
                FrontSprite = ResLoader.Instance.SpringSprites[id];
                break;
            case (int)CardImageKind.Summer:
                FrontSprite = ResLoader.Instance.SummerSprites[id];
                break;
            case (int)CardImageKind.Autumn:
                FrontSprite = ResLoader.Instance.AutumnSprites[id];
                break;
            case (int)CardImageKind.Winter:
                FrontSprite = ResLoader.Instance.WinterSprites[id];
                break;
        }
    }
    
    public void Reset()
    {
        ToggleCardFace(false);
        CardPrefab.SetActive(false);
    }
    
    /// <summary>
    /// カードの表面または裏面を表示します。
    /// </summary>
    /// <param name="showFace">trueの場合、カードの表面を表示。falseの場合、裏面を表示。</param>
    public void ToggleCardFace(bool showFace)
    {
        RotateCard(showFace);
    }
    
    /// <summary>
    /// カード回転処理
    /// </summary>
    /// <param name="showFace"></param>
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

                _rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f);
            });
    }

    /// <summary>
    /// カードを移動する
    /// </summary>
    /// <param name="parent"></param>
    public void MoveCardTo(Transform parent)
    {
        CardPrefab.transform.SetParent(parent);
        var position = parent.transform.position;
        Vector3 pos = new Vector3(position.x + 90, position.y + 100, 0);
        //Vector3 pos = new Vector3(parent.transform.position.x, parent.transform.position.y, 0);
        _rectTransform.DOAnchorPos(pos,1f);
    }
}