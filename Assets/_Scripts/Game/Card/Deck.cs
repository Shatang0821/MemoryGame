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
        for (int i = 0; i < ResLoader.Instance.SpringSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Spring, cardPrefab);
            //card.CardPrefab.SetActive(false);
            _cards.Add(card);
        }
        for (int i = 0; i < ResLoader.Instance.SummerSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Summer , cardPrefab);
            //card.CardPrefab.SetActive(false);
            _cards.Add(card);
        }
        for (int i = 0; i < ResLoader.Instance.AutumnSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Autumn , cardPrefab);
            //card.CardPrefab.SetActive(false);
            _cards.Add(card);
        }
        for (int i = 0; i < ResLoader.Instance.WinterSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num,(int)CardImage.Winter , cardPrefab);
            //card.CardPrefab.SetActive(false);
            _cards.Add(card);
        }
        Debug.Log(_cards.Count);
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