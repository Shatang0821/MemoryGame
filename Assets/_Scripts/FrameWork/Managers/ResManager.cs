using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Manager
{
    public class ResManager : PersistentUnitySingleton<ResManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public T GetAssetCache<T>(string name) where T : UnityEngine.Object
        {
            // string path = "Assets/AssetsPackage/" + name;
            // UnityEngine.Object target = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            // return target as T;
            UnityEngine.Object target = Resources.Load<T>(name);
            return target as T;
        }
    }
}
