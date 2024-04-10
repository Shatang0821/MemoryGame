using FrameWork.EventCenter;
using Photon.Realtime;

public class Player
{
    public bool IsMaster { get; set; }
    public bool IsMyTurn { get; set; }
    
    public int PlayerNum { get; set; }

    private int _myPoint;

    public int MyPoint
    {
        get { return _myPoint; }
        set
        {
            _myPoint = value;
            EventCenter.TriggerEvent(EventKey.OnChangePoint,PlayerNum,_myPoint);
            //イベントをトリガー
        }
    }
}