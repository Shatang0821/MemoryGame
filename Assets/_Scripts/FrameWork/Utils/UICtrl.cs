using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrameWork.Utils
{
    public class UICtrl : MonoBehaviour
    {
        private Dictionary<string, GameObject> _view = new Dictionary<string, GameObject>();

        protected Dictionary<string, GameObject> View => _view;

        public virtual void Awake()
        {
            LoadAllObjectsToView(this.gameObject, "");
        }

        /// <summary>
        /// UIの子オブジェクトをすべてを取得する
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        private void LoadAllObjectsToView(GameObject root, string path)
        {
        
            foreach (Transform transform in root.transform)
            {
                var gameObject = transform.gameObject;
                if(this._view.ContainsKey(path + gameObject.name))
                {
                    continue;
                }
                this._view.Add(path + gameObject.name,gameObject);
                this.LoadAllObjectsToView(gameObject,path + gameObject.name + "/");
            }
        }

        /// <summary>
        /// ボタンにクリック処理イベントを登録
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="onClick"></param>
        protected void AddButtonListener(string viewName, UnityAction onClick)
        {
            Button bt = this._view[viewName].GetComponent<Button>();
            if (bt == null)
            {
                Debug.LogWarning(this.name + "Try add button listener but failed");
                return;
            }
        
            bt.onClick.AddListener(onClick);
        }

        /// <summary>
        /// ボタンにイベントの登録を解除する
        /// </summary>
        /// <param name="viewName"></param>
        protected void RemoveButtonListener(string viewName)
        {
            Button bt = this._view[viewName].GetComponent<Button>();
            if (bt == null)
            {
                Debug.LogWarning(this.name + "Try Remove button listener but failed");
                return;
            }
            
            bt.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// オブジェクトのアクティブ状態設定
        /// </summary>
        /// <param name="viewName">オブジェクトの名前</param>
        /// <param name="b">真偽</param>
        protected void SetViewActive(string viewName,bool b)
        {
            _view[viewName].SetActive(b);
        }
        
        
    }
}

