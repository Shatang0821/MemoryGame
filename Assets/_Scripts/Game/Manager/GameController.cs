using System.Collections.Generic;
using FrameWork.EventCenter;
using UnityEngine;

public class GameController
{
    private List<Card> _selectedCards;                          //選択したカード
    private int _matchedCardTotal;                              //マッチしたカード総数
    public GameController()
    {
        _selectedCards = new List<Card>();
    }
        
    // カード選択時の処理
    // ReSharper disable Unity.PerformanceAnalysis
    public void SelectCard(Card card,int total)
    {
        if (!_selectedCards.Contains(card) && _selectedCards.Count < 2)
        {
            card.ToggleCardFace(true);
            _selectedCards.Add(card);
        }

        // 2枚のカードが選択されたら自動的に一致判定を行う
        if (_selectedCards.Count == 2)
        {
            EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.CheckCards);
            CheckCard(total);
        }
    }
    
    private void CheckCard(int total)
    {
        if (_selectedCards[0].Id == _selectedCards[1].Id)
        {
            _selectedCards[0].SetCardMatched();
            _selectedCards[1].SetCardMatched();
            _matchedCardTotal += _selectedCards.Count;
            if (_matchedCardTotal == total)
            {
                EventCenter.TriggerEvent(EventKey.OnGameStateChange,GamePlayState.End);
                return;
            }
        }
        else
        {
            _selectedCards[0].ToggleCardFace(false);
            _selectedCards[1].ToggleCardFace(false);
        }
        _selectedCards.Clear();
        EventCenter.TriggerEvent(EventKey.OnGameStateChange,GamePlayState.SelectCards);  
    }
}