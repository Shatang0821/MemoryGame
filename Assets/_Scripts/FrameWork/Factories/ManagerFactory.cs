using FrameWork.Manager;
using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Factories
{
    public class ManagerFactory : Singleton<ManagerFactory>
    {
        /// <summary>
        /// マネージャーの生成
        /// </summary>
        /// <param name="parent">親の指定</param>
        /// <typeparam name="T">マネージャークラス</typeparam>
        /// <returns></returns>
        public T CreateManager<T>(Transform parent = null) where T : Component
        {
            GameObject gameObject = new GameObject(typeof(T).Name);
            if (parent != null)
            {
                gameObject.transform.parent = parent;
            }

            var component = gameObject.AddComponent<T>();
            if (component is IInitable initable)
            {
                initable.Init();
            }
            return component;
        }
    }
}