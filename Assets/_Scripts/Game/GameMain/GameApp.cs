using FrameWork.Factories;
using FrameWork.Manager;
using FrameWork.Utils;
using UnityEngine;

public class GameApp : UnitySingleton<GameApp>
{
    public void InitGame()
    {
      Debug.Log("Enter Game!");
      
      this.EnterMainScene();
    }
    /// <summary>
    /// ゲームシーンに入る
    /// </summary>
    private void EnterMainScene()
    {
        //UIの初期化
        UIManager.Instance.ShowUI("TitleUI");
        UIManager.Instance.ShowUI("GameUI");
        //マネージャーの生成
        InitMgr();
    }

    /// <summary>
    /// マネージャーのオブジェクトを生成
    /// </summary>
    private void InitMgr()
    {
        ManagerFactory.Instance.CreateManager<GameManager>();
    }


}
