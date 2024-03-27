using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.Utils;
using UnityEngine;
public class Deck
{
    private List<Card> _cards;
    public List<Card> Cards
    {
        get => _cards;
    }
    public Deck(GameObject cardContainer)
    {
        _cards = new List<Card>();
        for (int i = 0; i < ResLoader.Instance.CardSprites.Count; i++)
        {
            var num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var card = new Card(num, ResLoader.Instance.CardSprites[i], cardPrefab);
            //card.CardPrefab.SetActive(false);
            Cards.Add(card);
        }
    }

    

    public void Shuffle()
    {
        _cards =  _cards.OrderBy(a => Guid.NewGuid()).ToList();
    }
}