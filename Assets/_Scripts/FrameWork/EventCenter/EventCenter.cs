using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrameWork.EventCenter
{
    /// <summary>
    /// イベント処理中心
    /// </summary>
    public class EventCenter
    {
        //デフォルトイベント
        private static Dictionary<EventKey, Action> eventNoParamDictionary = new Dictionary<EventKey, Action>();

        //引数持ちイベント
        private static Dictionary<EventKey, Action<object>> eventWithParamDictionary =
            new Dictionary<EventKey, Action<object>>();

        // 戻り値のあるイベント
        private static Dictionary<EventKey, Func<object, object>> eventWithReturnDictionary =
            new Dictionary<EventKey, Func<object, object>>();

        /// <summary>
        /// イベントをサブスクライブ
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="listener">イベント</param>
        public static void Subscribe(EventKey eventKey, Action listener)
        {
            if (eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                // 既に同じリスナーが登録されている場合は追加しない
                if (thisEvent != null && !thisEvent.GetInvocationList().Contains(listener))
                {
                    eventNoParamDictionary[eventKey] += listener;
                }
            }
            else
            {
                eventNoParamDictionary.Add(eventKey, listener);
            }
        }

        /// <summary>
        /// 引数持ちイベントをサブスクライブ
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="listener">イベント</param>
        public static void Subscribe(EventKey eventKey, Action<object> listener)
        {
            if (eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                thisEvent += listener;
                eventWithParamDictionary[eventKey] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                eventWithParamDictionary.Add(eventKey, thisEvent);
            }
        }

        /// <summary>
        /// 戻り値のあるイベントをサブスクライブ
        /// 一つのリスナーだけ有効
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="listener">イベント</param>
        public static void Subscribe(EventKey eventKey, Func<object, object> listener)
        {
            if (!eventWithReturnDictionary.ContainsKey(eventKey))
            {
                eventWithReturnDictionary[eventKey] = listener;
            }
            else
            {
                Debug.LogWarning(eventKey.ToString() + "が複数のイベントをサブスクライブしている");
                // 既存のリスナーと新しいリスナーを連結する処理（必要に応じて実装）
                // 例: 最後に追加されたリスナーのみが有効になる
                eventWithReturnDictionary[eventKey] = listener;
            }
        }

        /// <summary>
        /// サブスクライブを解除
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="listener">イベント</param>
        public static void Unsubscribe(EventKey eventKey, Action listener)
        {
            if (eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                thisEvent -= listener;
                eventNoParamDictionary[eventKey] = thisEvent;
            }
        }

        /// <summary>
        /// 引数持ちサブスクライブを解除
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="listener">イベント</param>
        public static void Unsubscribe(EventKey eventKey, Action<object> listener)
        {
            if (eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                thisEvent -= listener;
                eventWithParamDictionary[eventKey] = thisEvent;
            }
        }

        /// <summary>
        /// 戻り値のあるイベントを解除
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="listener">イベント</param>
        public static void Unsubscribe(EventKey eventKey, Func<object, object> listener)
        {
            if (eventWithReturnDictionary.ContainsKey(eventKey))
            {
                eventWithReturnDictionary.Remove(eventKey);
            }
        }

        /// <summary>
        /// イベントを実行する
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        public static void TriggerEvent(EventKey eventKey)
        {
            if (eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                thisEvent?.Invoke();
            }
        }

        /// <summary>
        /// 引数持ちイベントを実行
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="eventParam">引数</param>
        public static void TriggerEvent(EventKey eventKey, object eventParam)
        {
            if (eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                thisEvent?.Invoke(eventParam);
            }
        }

        // 戻り値のあるイベントをトリガー
        public static object TriggerEventHasValue(EventKey eventKey, object eventParam)
        {
            if (eventWithReturnDictionary.TryGetValue(eventKey, out var thisEvent))
            {
                return thisEvent?.Invoke(eventParam);
            }
            return null;
        }
    }
}


