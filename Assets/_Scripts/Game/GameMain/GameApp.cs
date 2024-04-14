using System.Collections;
using FrameWork.Factories;
using FrameWork.Manager;
using FrameWork.Utils;
using UnityEngine;

public class GameApp : UnitySingleton<GameApp>
{
    public void InitGame()
    {
      this.EnterMainScene();
    }
    /// <summary>
    /// ゲームシーンに入る
    /// </summary>
    private void EnterMainScene()
    {
        //マネージャーの生成
        InitMgr();
        //UIの初期化
        InitUI();
    }
    
    /// <summary>
    /// 必要なマネージャーのオブジェクトを生成
    /// </summary>
    private void InitMgr()
    {
        ManagerFactory.Instance.CreateManager<ResLoader>();
        ManagerFactory.Instance.CreateManager<GameManager>();
    }
    
    /// <summary>
    /// 必要なUIプレハブの生成
    /// </summary>
    public void InitUI()
    {
        UIManager.Instance.ShowUI("TitleUI");
        UIManager.Instance.ShowUI("GameUI");
        UIManager.Instance.ShowUI("EndUI");
    }

    
    


}
