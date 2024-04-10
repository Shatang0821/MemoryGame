namespace FrameWork.EventCenter
{
    public delegate void CallBack();
    public delegate void CallBack<T>(T arg);
    public delegate void CallBack<T1,T2>(T1 arg1,T2 arg2);
}
