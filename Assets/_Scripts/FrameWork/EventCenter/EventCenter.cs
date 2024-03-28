using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.EventCenter
{
    /// <summary>
    /// イベント処理中心
    /// </summary>
    public class EventCenter
    {
        //デフォルトイベント
        private static Dictionary<EventKey, Delegate> m_EventDictionary = new Dictionary<EventKey, Delegate>();

        // 戻り値のあるイベント
        // private static Dictionary<EventType, Delegate> eventWithReturnDictionary =
        //     new Dictionary<CallBack, Delegate>();

#region Add

        /// <summary>
        /// イベント登録チェック
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="callBack"></param>
        /// <exception cref="Exception"></exception>
        private static void OnListenerAdding(EventKey eventKey, Delegate callBack)
        {
            //イベント登録処理
            if (!m_EventDictionary.ContainsKey(eventKey))
            {
                m_EventDictionary.Add(eventKey, null);
            }

            Delegate d = m_EventDictionary[eventKey];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("イベント{0}に異なるデリケートを追加しようとする,現在のイベントに対応するデリケートは{1},登録したいデリケートは{2}",
                    eventKey, d.GetType(), callBack.GetType()));
            }
        }

        /// <summary>
        /// 引数なしデリケートを登録
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        public static void AddListener(EventKey eventKey, CallBack callBack)
        {
            OnListenerAdding(eventKey, callBack);

            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }

        /// <summary>
        /// 引数一つ持ちデリケートの登録
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T">引数</typeparam>
        public static void AddListener<T>(EventKey eventKey, CallBack<T> callBack)
        {
            OnListenerAdding(eventKey, callBack);

            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }

        /// <summary>
        /// 戻り値一つ持ちデリケートの登録
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="TResult"></typeparam>
        public static void AddListener<TResult>(EventKey eventKey, Func<TResult> callBack)
        {
            OnListenerAdding(eventKey, callBack);
            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }
        
        /// <summary>
        /// 引数１つ戻り値１つデリケートの登録
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T">引数</typeparam>
        /// <typeparam name="TResult">戻り値</typeparam>
        public static void AddListener<T, TResult>(EventKey eventKey, Func<T, TResult> callBack)
        {
            OnListenerAdding(eventKey, callBack);
            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }

#endregion

#region Remove

        private static void OnListenerRemoving(EventKey eventKey, Delegate callBack)
        {
            //イベント登録処理
            if (m_EventDictionary.ContainsKey(eventKey))
            {
                Delegate d = m_EventDictionary[eventKey];
                if (d == null)
                {
                    throw new Exception(string.Format("デリケートを解除失敗,イベント{0}には対応するデリケート{1}は存在していない", eventKey,
                        callBack.GetType()));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format(
                        "デリケートを解除失敗,イベント{0}に違う型のデリケートを解除しようとする,現在のイベントに対応するデリケートは{1},解除したいデリケートは{2}", eventKey,
                        d.GetType(), callBack.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("デリケートを解除失敗,イベントキー{0}は存在していない", eventKey));
            }
        }

        private static void OnListenerRemoved(EventKey eventKey)
        {
            if (m_EventDictionary[eventKey] == null)
            {
                m_EventDictionary.Remove(eventKey);
            }
        }

        /// <summary>
        /// デリケートを解除
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        public static void RemoveListener(EventKey eventKey, CallBack callBack)
        {
            OnListenerRemoving(eventKey, callBack);

            m_EventDictionary[eventKey] = (CallBack)m_EventDictionary[eventKey] - callBack;

            OnListenerRemoved(eventKey);
        }

        /// <summary>
        /// 引数１つ持ちデリケートを解除
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T">引数</typeparam>
        public static void RemoveListener<T>(EventKey eventKey, CallBack<T> callBack)
        {
            OnListenerRemoving(eventKey, callBack);

            m_EventDictionary[eventKey] = (CallBack<T>)m_EventDictionary[eventKey] - callBack;

            OnListenerRemoved(eventKey);
        }

        /// <summary>
        /// 戻り値一つある
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="TResult"></typeparam>
        public static void RemoveListener<TResult>(EventKey eventKey, Func<TResult> callBack)
        {
            OnListenerRemoving(eventKey, callBack);
            
            m_EventDictionary[eventKey] = (Func<TResult>)m_EventDictionary[eventKey] - callBack;
            
            OnListenerRemoved(eventKey);
        }
        
        /// <summary>
        /// 戻り値１引数１デリケートを解除
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static void RemoveListener<T, TResult>(EventKey eventKey, Func<T, TResult> callBack)
        {
            OnListenerRemoving(eventKey, callBack);
            
            m_EventDictionary[eventKey] = (Func<T, TResult>)m_EventDictionary[eventKey] - callBack;
            
            OnListenerRemoved(eventKey);
        }

#endregion

#region Trigger

        /// <summary>
        /// イベントを実行する
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        public static void TriggerEvent(EventKey eventKey)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is CallBack callBack)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("イベントを実行失敗:イベント{0}は違う型のデリケートを持っている", eventKey));
                }
            }
        }

        /// <summary>
        /// 引数一つのイベントを実行
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="arg">引数</param>
        /// <typeparam name="T">型</typeparam>
        /// <exception cref="Exception"></exception>
        public static void TriggerEvent<T>(EventKey eventKey, T arg)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is CallBack<T> callBack)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("イベントを実行失敗:イベント{0}は違う型のデリケートを持っている", eventKey));
                }
            }
        }

        /// <summary>
        /// 戻り値一つあるデリケートの登録
        /// </summary>
        /// <param name="eventKey"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static TResult TriggerEvent<TResult>(EventKey eventKey)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is Func<TResult> callBack)
                {
                    return callBack();
                }
                else
                {
                    throw new Exception($"イベントを実行失敗:イベント{eventKey}は違う型のデリケートを持っている");
                }
            }
            return default;
        }

        // 戻り値のあるイベントをトリガー
        public static TResult TriggerEvent<T, TResult>(EventKey eventKey, T arg)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is Func<T, TResult> callBack)
                {
                    return callBack(arg);
                }
                else
                {
                    throw new Exception($"イベントを実行失敗:イベント{eventKey}は違う型のデリケートを持っている");
                }
            }
            return default;
        }
        
        
#endregion
    }
}