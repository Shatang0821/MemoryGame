using System.Collections.Generic;
using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.PoolSystem
{
    public class ObjectPool<T> where T : IPoolable, new()
{
    /// <summary>
    /// 使用可能
    /// </summary>
    private Queue<T> _availableInstances;

    /// <summary>
    /// 初期化時のサイズ
    /// </summary>
    public int Size { get; private set; }

    public int RuntimeSize => _availableInstances.Count;
    private int _maxSize = 0;

    public ObjectPool(int size)
    {
        DebugLogger.Log("Create Object");
        this.Size = size;
        Initialize();
    }

    ~ObjectPool()
    {
#if UNITY_EDITOR
        var size = Mathf.Max(RuntimeSize, _maxSize);
        if (Size < size)
        {
            Debug.LogWarning($"適切サイズは{size}です");
        }
#endif
        if (RuntimeSize <= 0)
        {
            DebugLogger.Log("ObjectPoolに問題が発生しました");
        }
    }


    private void Initialize()
    {
        _availableInstances = new Queue<T>();
        Debug.Log(Size);
        for (var i = 0; i < Size; i++)
        {
            var copy = Copy();
            copy.IsActive = false;
            _availableInstances.Enqueue(copy);
        }
    }

    private T Copy()
    {
        return new T();
    }

    /// <summary>
    /// オブジェクトプールからインスタンスを取得します。
    /// 使用後には <see cref="ReturnInstance"/> を呼び出してインスタンスを返却してください。
    /// </summary>
    /// <returns>利用可能なインスタンス。</returns>
    public T GetInstance()
    {
        var availableInstance = default(T);
        DebugLogger.Log("インスタンスを取り出す");
        if (_availableInstances.Count > 0)
        {
            availableInstance = _availableInstances.Dequeue();
            availableInstance.IsActive = true;
        }
        else
        {
            availableInstance = Copy();
            availableInstance.IsActive = true;
        }

        return availableInstance;
    }

    /// <summary>
    /// 使用が完了したインスタンスをプールに返却します。
    /// </summary>
    /// <param name="instance">返却するインスタンス。</param>
    public void ReturnInstance(T instance)
    {
        instance.Reset();
        // インスタンスの状態をリセットするロジックをここに追加
        _availableInstances.Enqueue(instance);
    }
}

[System.Serializable]
public class UnityObjectPool
{
    // プレハブへの参照を外部から取得するためのプロパティ
    public GameObject Prefab => prefab;

    // サイズを外部から取得するためのプロパティ
    public int Size => size;

    // 実行時のキューのサイズを取得するためのプロパティ
    public int RuntimeSize => _availableQueue.Count;

    [SerializeField] private GameObject prefab; // このプールに格納するゲームオブジェクトのプレハブ

    [SerializeField] private int size = 1; // プールの初期サイズ

    // ゲームオブジェクトを保持するためのキュー
    private Queue<GameObject> _availableQueue;

    // ゲームオブジェクトがインスタンス化されるときの親オブジェクト
    private Transform parent;

    #region 初期化関連

    /// <summary>
    /// キューの初期化し、指定された数のゲームオブジェクトをキューに追加する
    /// </summary>
    /// <param name="parent">親オブジェクトのTransform</param>
    public void Initialize(Transform parent)
    {
        //キューの初期化
        _availableQueue = new Queue<GameObject>();
        //親オブジェクトを生成してそれの下にオブジェクトを生成する
        this.parent = parent;

        //サイズ分のオブジェクトをキューに入れる
        for (var i = 0; i < size; i++)
        {
            _availableQueue.Enqueue(Copy());
        }
    }

    /// <summary>
    /// プレハブからゲームオブジェクトを作成し、非アクティブ状態にする
    /// </summary>
    private GameObject Copy()
    {
        //作成したオブジェクトをparentの子オブジェクトにする
        var copy = GameObject.Instantiate(prefab, parent);
        //初期非アクティブ化にする
        copy.SetActive(false);
        //作成したオブジェクトを返す
        return copy;
    }

    #endregion

    #region オブジェクトを生成

    /// <summary>
    /// 利用可能なオブジェクトをキューから取得する。
    /// もしキューが空の場合は新しいオブジェクトを生成する。
    /// </summary>
    private GameObject AvailableObject()
    {
        GameObject availbleObject = null;
        // キューが空でなく、先頭のオブジェクトが非アクティブな場合
        if (_availableQueue.Count > 0 && !_availableQueue.Peek().activeSelf)
        {
            //Dequeueはキューの先頭からオブジェクトを取り出すことができるため
            //先頭のオブジェクトが使っている時取り出さない
            availbleObject = _availableQueue.Dequeue();
        }
        else
        {
            //利用可能なオブジェクトがないから
            //新しいオブジェクトを作って、返す
            availbleObject = Copy();
        }

        // オブジェクトを再びキューに追加する
        //先頭から取り出したオブジェクトを末に追加する
        //循環させるため
        _availableQueue.Enqueue(availbleObject);

        return availbleObject;
    }

    #region オーバーロード

    /// <summary>
    /// 利用可能なゲームオブジェクトを取得してアクティブ化する
    /// </summary>
    public GameObject preparedObject()
    {
        //オブジェクトを生成する時
        GameObject preparedObject = AvailableObject();
        //アクティブ化する
        preparedObject.SetActive(true);
        return preparedObject;
    }

    /// <summary>
    /// 特定の位置を基に生成
    /// </summary>
    /// <param name="position">特定の位置</param>
    /// <returns></returns>
    public GameObject preparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    /// <summary>
    /// 特定の位置と回転を基に生成
    /// </summary>
    /// <param name="position">特定の位置</param>
    /// <param name="rotation">特定の回転</param>
    /// <returns></returns>
    public GameObject preparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    /// <summary>
    /// 特定の位置と回転と拡大を基に生成
    /// </summary>
    /// <param name="position">特定の位置</param>
    /// <param name="rotation">特定の回転</param>
    /// <param name="localScale">特定の拡大・縮小</param>
    /// <returns></returns>
    public GameObject preparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }

    #endregion

    #endregion
}
}
