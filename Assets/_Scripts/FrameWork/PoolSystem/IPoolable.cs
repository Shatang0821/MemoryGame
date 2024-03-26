namespace FrameWork.PoolSystem
{
    public interface IPoolable
    {
        /// <summary>
        /// クラスが使用可能を示す
        /// </summary>
        bool IsActive { get; set; }
        /// <summary>
        /// リセット関数
        /// </summary>
        void Reset();
    }
}
