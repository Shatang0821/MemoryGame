using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.Utils;
using UnityEngine;
public class Deck
{
    private List<Card> _cards;
    private List<int> _randomCardsSelfId;
    private Dictionary<int, Card> _cardTable;
    
    public List<Card> Cards
    {
        get => _cards;
    }

    public List<int> RandomCards
    {
        get => _randomCardsSelfId;
    }

    public Dictionary<int, Card> CardTable
    {
        get => _cardTable;
    }
    public Deck(GameObject cardContainer)
    {
        //カードリスト初期化
        _cards = new List<Card>();
        _cardTable = new Dictionary<int, Card>();
        //カード絵札ごとに生成していく
        for (int i = 0; i < ResLoader.Instance.SpringSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Spring, cardPrefab);
            
            _cardTable.Add(card.SelfId,card);
            _cards.Add(card);
        }
        for (int i = 0; i < ResLoader.Instance.SummerSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Summer , cardPrefab);
            
            _cardTable.Add(card.SelfId,card);
            _cards.Add(card);
        }
        for (int i = 0; i < ResLoader.Instance.AutumnSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Autumn , cardPrefab);
            
            _cardTable.Add(card.SelfId,card);
            _cards.Add(card);
        }
        for (int i = 0; i < ResLoader.Instance.WinterSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Winter , cardPrefab);
            
            _cardTable.Add(card.SelfId,card);
            _cards.Add(card);
        }
        
    }

    ~Deck()
    {
        DebugLogger.Log("Deck デストラクタ");
    }
    
    public void OnEnable()
    {
        DebugLogger.Log("Deck");
    }

    public void OnDisable()
    {
        _randomCardsSelfId.Clear();
    }
    
    /// <summary>
    /// カードをシャッフル
    /// </summary>
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = _cards.Count;
        _randomCardsSelfId = new List<int>(); // SelfIdのリストを初期化

        // シャッフル後のカードのSelfIdをリストに追加
        foreach (var card in _cards)
        {
            _randomCardsSelfId.Add(card.SelfId);
        }
        
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            // カードをシャッフル
            (_randomCardsSelfId[k], _randomCardsSelfId[n]) = (_randomCardsSelfId[n], _randomCardsSelfId[k]);
        }

        for (int i = 0; i < _randomCardsSelfId.Count; i++)
        {
            DebugLogger.Log(_randomCardsSelfId[i]);
        }
    }
}