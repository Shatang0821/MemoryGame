using System.Collections;
using System.Collections.Generic;
using FrameWork.Manager;
using FrameWork.Utils;
using UnityEngine;

public class ResLoader : UnitySingleton<ResLoader>
{
    public List<Sprite> SpringSprites { get; private set; }
    public List<Sprite> SummerSprites { get; private set; }
    public List<Sprite> AutumnSprites { get; private set; }
    public List<Sprite> WinterSprites { get; private set; }
    public Sprite BackSprite{ get; private set; }
    public GameObject CardPrefab{ get; private set; }
    
    public int SpriteCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Init();
        Debug.Log("ResLoader");
    }

    /// <summary>
    /// 画像の初期ロード
    /// </summary>
    public void Init()
    {
        SpringSprites = new List<Sprite>();
        SummerSprites = new List<Sprite>();
        AutumnSprites = new List<Sprite>();
        WinterSprites = new List<Sprite>();
        
        CardPrefab = ResManager.Instance.GetAssetCache<GameObject>("Prefabs/Card");
        for (int i = 0; i < 6; i++)
        {
            SpringSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Spring_" + (i+1)));
            SpriteCount++;
        }
        for (int i = 0; i < 6; i++)
        {
            SummerSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Summer_" + (i+1)));
            SpriteCount++;
        }
        for (int i = 0; i < 6; i++)
        {
            AutumnSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Autumn_" + (i+1)));
            SpriteCount++;
        }
        for (int i = 0; i < 6; i++)
        {
            WinterSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Winter_" + (i+1)));
            SpriteCount++;
        }
        
        
        BackSprite = ResManager.Instance.GetAssetCache<Sprite>("Sprite/Back");
        Debug.Log(SpriteCount);
    }

    /// <summary>
    /// 種類ごとのスプライト数を返却
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public int GetSpriteCountByKind(CardImageKind kind)
    {
        switch ((int)kind)
        {
            case 0:
                return SpringSprites.Count;
                break;
            case 1:
                return SummerSprites.Count;
                break;
            case 2:
                return AutumnSprites.Count;
                break;
            case 3:
                return WinterSprites.Count;
                break;
            default:
                return 0;
            break;
        }
    }
    
    /// <summary>
    /// すべてのスプライトリストをクリアしてメモリを解放
    /// </summary>
    public void ClearSprites()
    {
        SpringSprites.Clear();
        SummerSprites.Clear();
        AutumnSprites.Clear();
        WinterSprites.Clear();
    }
}