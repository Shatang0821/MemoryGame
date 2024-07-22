using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEditor;
using UnityEngine;
using Logger = FrameWork.Utils.Logger;

/// <summary>
/// カード順番保持するクラス
/// </summary>
public class Deck
{
    public List<Card> Cards { get; private set; }                   //データ
    public List<CardView> CardViews { get; private set; }           //ビュー

    public List<int> RandomCardsSelfIdList { get; private set; }    //シャッフルしたカードのSelfIdリスト

    public Dictionary<int, Card> CardTable { get; private set; }
    public Dictionary<int, CardView> CardViewTable { get; private set; }


    public Deck(GameObject cardContainer)
    {
        //カードリスト初期化
        Cards = new List<Card>();
        CardTable = new Dictionary<int, Card>();

        CardViews = new List<CardView>();
        CardViewTable = new Dictionary<int, CardView>();

        // SelfIdのリストを初期化
        RandomCardsSelfIdList = new List<int>();

        InitializeDeck(cardContainer);

        ResLoader.Instance.ClearSprites();

        EventCenter.AddListener<int[]>(EventKey.SetShuffledCard, SetShuffledDeck);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="cardContainer"></param>
    private void InitializeDeck(GameObject cardContainer)
    {
        foreach (CardImageKind kind in Enum.GetValues(typeof(CardImageKind)))
        {
            InitializeCardKind(kind, cardContainer);
        }
    }

    /// <summary>
    /// カードのデータ初期化
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="cardContainer"></param>
    private void InitializeCardKind(CardImageKind kind, GameObject cardContainer)
    {
        for (int i = 0; i < ResLoader.Instance.GetSpriteCountByKind(kind); i++)
        {
            int num = (i % 6) + 1;
            var cardPrefab = GameObject.Instantiate(ResLoader.Instance.CardPrefab, cardContainer.transform);
            var cardSelfId = num + (int)kind * 6;
            var cardView = new CardView(cardPrefab, kind, i);
            var card = new Card(num, cardSelfId);

            card.OnFaceStateChange += cardView.ToggleCardFace;
            card.OnCardMatched += cardView.MoveCardTo;

            CardTable.Add(cardSelfId, card);
            Cards.Add(card);

            CardViewTable.Add(cardSelfId, cardView);
            CardViews.Add(cardView);
        }
    }

    ~Deck()
    {
        EventCenter.RemoveListener<int[]>(EventKey.SetShuffledCard, SetShuffledDeck);
    }

    public void OnEnable()
    {
        Shuffle();
    }
    
    public void OnDisable()
    {
        //カードデータのリセット
        foreach (var card in Cards)
        {
            card.Reset();
        }
        RandomCardsSelfIdList.Clear();
    }

    /// <summary>
    /// 同期用メソッド、受け取ったカードIDを
    /// </summary>
    /// <param name="shuffledSelfIds"></param>
    public void SetShuffledDeck(int[] shuffledSelfIds)
    {
        RandomCardsSelfIdList.Clear();
        RandomCardsSelfIdList.AddRange(shuffledSelfIds.ToList());
        // 必要に応じて、_cardsリストの順番も更新するロジックをここに追加
    }

    /// <summary>
    /// カードをシャッフル
    /// </summary>
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = Cards.Count;
        RandomCardsSelfIdList.Clear();

        // シャッフル後のカードのSelfIdをリストに追加
        foreach (var card in Cards)
        {
            RandomCardsSelfIdList.Add(card.SelfId);
        }

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            // カードをシャッフル
            (RandomCardsSelfIdList[k], RandomCardsSelfIdList[n]) = (RandomCardsSelfIdList[n], RandomCardsSelfIdList[k]);
        }

        if (GameManager.Instance.IsOnlineMode)
        {
            NetworkManager.Instance.SendShuffledCard(RandomCardsSelfIdList.ToArray());
        }
    }
}