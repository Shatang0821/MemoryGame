using System;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Card
{
    public int SelfId { get; private set; }
    public int Id { get; private set; }
    public bool IsMatched { get; private set; }
    public bool IsFaceUp { get; private set; }

    public event Action<bool> OnFaceStateChange;
    public event Action<Transform> OnCardMatched;
    /// <summary>
    /// カードクラスの作成
    /// </summary>
    /// <param name="id">絵札数字</param>
    /// <param name="selfId">カード自身の番号</param>
    public Card(int id,int selfId)
    {
        this.Id = id;
        this.SelfId = selfId;
        this.IsMatched = false;
        this.IsFaceUp = false;
    }

    ~Card()
    {
        DOTween.KillAll();
    }

    /// <summary>
    /// リセット処理
    /// </summary>
    public void Reset()
    {
        IsMatched = false;
        IsFaceUp = false;
    }
    
    /// <summary>
    /// カードの表面または裏面を表示します。
    /// </summary>
    public void ToggleCardFace()
    {
        IsFaceUp = !IsFaceUp;
        OnFaceStateChange?.Invoke(IsFaceUp);
    }
    
    /// <summary>
    /// カードが一致した処理
    /// </summary>
    public void SetCardMatched(Transform cardContainer)
    {
        IsMatched = true;
        OnCardMatched?.Invoke(cardContainer);
    }
}