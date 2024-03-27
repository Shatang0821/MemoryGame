using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.Utils;
using UnityEngine;
public class Deck
{
    private List<Card> _cards;
    private List<Card> _randomCards;
    public List<Card> Cards
    {
        get => _cards;
    }

    public List<Card> RandomCards
    {
        get => _randomCards;
    }
    public Deck(GameObject cardContainer)
    {
        _cards = new List<Card>();
        _randomCards = new List<Card>();
        for (int i = 0; i < ResLoader.Instance.CardSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num, ResLoader.Instance.CardSprites[i], cardPrefab);
            //card.CardPrefab.SetActive(false);
            Cards.Add(card);
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
        _randomCards.Clear();
    }
    
    /// <summary>
    /// カードをシャッフル
    /// </summary>
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = _cards.Count;
        _randomCards = new List<Card>(_cards); // 新しいリストの作成で現在のカードの順番を保持
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = _randomCards[k];
            _randomCards[k] = _randomCards[n];
            _randomCards[n] = value;
        }
    }
}