using System.Collections;
using System.Collections.Generic;
using FrameWork.Manager;
using FrameWork.Utils;
using UnityEngine;

public class GameLaunch : PersistentUnitySingleton<GameLaunch>
{
    protected override void Awake()
    {
        base.Awake();
        this.InitFramework();
        this.InitGameLogic();
    }

    /// <summary>
    /// アップデートチェック
    /// </summary>
    private void CheckHotUpdate()
    {
        //データ取得
        //ダウンロード情報
        //ローカルにダウンロード
    }
    
    /// <summary>
    /// フレームワークを初期化
    /// </summary>
    private void InitFramework()
    {
        this.gameObject.AddComponent<ResManager>();
        this.gameObject.AddComponent<UIManager>();
    }

    /// <summary>
    /// ゲームロジックに入る
    /// </summary>
    private void InitGameLogic()
    {
        this.gameObject.AddComponent<GameApp>();
        GameApp.Instance.InitGame();
    }
}
