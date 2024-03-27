using System.Collections.Generic;
using UnityEngine;
using System;
using FrameWork.Utils;

namespace FrameWork.Manager
{
    public class UIManager : PersistentUnitySingleton<UIManager>
    {
        public GameObject Canvas;
        
        private Dictionary<string, GameObject> UiPrefabs;   //Ctrlプレハブ
        private GameObject _currentUIPrefab;
        /// <summary>
        /// UIプレハブルート
        /// </summary>
        private readonly string UIPREFABROOT = "GUI/UIPrefabs/";
        protected override void Awake()
        {
            base.Awake();
            //Canvasを検索して
            this.Canvas = GameObject.Find("MainCanvas");
            //存在しない場合はエラー表示
            if (this.Canvas == null)
            {
                Debug.LogError("UI manager load Canvas failed!!!!");
            }

            UiPrefabs = new Dictionary<string, GameObject>();
        }

        private void OnEnable()
        {
            EventCenter.EventCenter.Subscribe(EventCenter.UIEventKey.OnChangeUIPrefab,ChangeUIPrefab);
        }

        private void OnDisable()
        {
            EventCenter.EventCenter.Unsubscribe(EventCenter.UIEventKey.OnChangeUIPrefab,ChangeUIPrefab);
        }

        /// <summary>
        /// UIオブジェクトの切り替え操作
        /// </summary>
        /// <param name="name">stringをキーとして使用する</param>
        private void ChangeUIPrefab(object obj)
        {
            if (obj is string uiName)
            {
                if (_currentUIPrefab != null)
                {
                    _currentUIPrefab.SetActive(false);
                }
                _currentUIPrefab = UiPrefabs[uiName];
                _currentUIPrefab.SetActive(true);
            }
            else
            {
                DebugLogger.LogWarning("型が違う");
            }
            
        }
    
        /// <summary>
        /// 指定UIを生成する
        /// </summary>
        /// <param name="name">UI名前</param>
        /// <returns></returns>
        public UICtrl ShowUI(string name,Transform parent =null)
        {
            // UIプレハブを取得する
            GameObject uiPrefab = ResManager.Instance.GetAssetCache<GameObject>(UIPREFABROOT + name);
            
            // UIプレハブを生成する
            GameObject uiView = GameObject.Instantiate(uiPrefab);
            uiView.name = name;
            UiPrefabs.Add(name,uiView);
            
            if (parent == null)
            {
                parent = this.Canvas.transform;
            }
            uiView.transform.SetParent(parent,false);
            
    
            Type type = Type.GetType(name + "Ctrl");
            UICtrl ctrl = (UICtrl)uiView.AddComponent(type);
    
            return ctrl;
        }
        
        /// <summary>
        /// 指定UIを削除
        /// </summary>
        /// <param name="name"></param>
        public void RemoveUI(string name)
        {
            Transform view = this.Canvas.transform.Find(name);
            if (view)
            {
                GameObject.Destroy(view.gameObject);
            }
        }
    
        /// <summary>
        /// すべてのUIを削除
        /// </summary>
        public void RemoveAll()
        {
            //すべてのUIをリストに入れる
            List<Transform> children = new List<Transform>();
            //すべてのUIをリストに入れる
            foreach (Transform VARIABLE in this.Canvas.transform)
            {
                children.Add(VARIABLE);
            }
            
            //リスト内のUIを削除
            for (int i = 0; i < children.Count; i++)
            {
                GameObject.Destroy(children[i].gameObject);
            }
        }
        
    }
}

