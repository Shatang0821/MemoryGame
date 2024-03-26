using System;
using UnityEngine;

namespace FrameWork.Utils
{
    // ジェネリックシングルトンクラス。任意の型Tの唯一のインスタンスを保持します。
    public class Singleton<T> where T : new()
    {
        // 唯一のインスタンスを保持する静的変数。
        private static T _instance;

        // スレッドセーフなシングルトン実装のためのミューテックス。
        private static object mutex = new object();

        // インスタンスへのアクセスを提供するプロパティ。
        public static T Instance
        {
            get
            {
                // インスタンスがまだ作成されていない場合
                if (_instance == null)
                {
                    lock (mutex) // スレッドセーフにするためのロック
                    {
                        // ダブルチェックロッキング
                        if (_instance == null)
                        {
                            _instance = new T(); // 新しいインスタンスを作成
                        }
                    }
                }

                return _instance;
            }
        }
    }

    public class PersistentUnitySingleton<T> : MonoBehaviour where T : Component
    {
        // 唯一のインスタンスを保持する静的変数。
        private static T _instance = null;

        // インスタンスへのアクセスを提供するプロパティ。
        public static T Instance
        {
            get
            {
                // インスタンスがまだ存在しない場合
                if (_instance == null)
                {
                    // 既存のインスタンスを検索
                    _instance = FindObjectOfType(typeof(T)) as T;

                    // インスタンスが見つからなかった場合、新しく作成
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(); // 新しいGameObjectを作成
                        _instance = (T)obj.AddComponent(typeof(T)); // コンポーネントを追加
                        obj.hideFlags = HideFlags.DontSave; // シーン保存時に破棄されないように設定
                        obj.name = typeof(T).Name; // GameObjectにクラス名を設定
                    }
                }

                return _instance;
            }
        }

        // MonoBehaviourのAwakeメソッドをオーバーライド
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject); // シーンロード時に破棄されないように設定

            // インスタンスが未設定の場合、自身をインスタンスとして設定
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this) // インスタンスが既に存在し、自身がそれではない場合
            {
                GameObject.Destroy(this.gameObject); // 重複するGameObjectを破棄
            }
        }
    }

    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        // 唯一のインスタンスを保持する静的変数。
        private static T _instance = null;
        // インスタンスへのアクセスを提供するプロパティ。
        public static T Instance
        {
            get
            {
                // インスタンスがまだ存在しない場合
                if (_instance == null)
                {
                    // 既存のインスタンスを検索
                    _instance = FindObjectOfType(typeof(T)) as T;

                    // インスタンスが見つからなかった場合、新しく作成
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(); // 新しいGameObjectを作成
                        _instance = (T)obj.AddComponent(typeof(T)); // コンポーネントを追加
                        obj.name = typeof(T).Name; // GameObjectにクラス名を設定
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // インスタンスが未設定の場合、自身をインスタンスとして設定
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this) // インスタンスが既に存在し、自身がそれではない場合
            {
                GameObject.Destroy(this.gameObject); // 重複するGameObjectを破棄
            }
        }
    }
}