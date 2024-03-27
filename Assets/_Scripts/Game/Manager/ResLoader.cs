using System.Collections;
using System.Collections.Generic;
using FrameWork.Manager;
using FrameWork.Utils;
using UnityEngine;

public class ResLoader : UnitySingleton<ResLoader>
{
    public List<Sprite> CardSprites { get; private set; }
    public Sprite BackSprite{ get; private set; }
    public GameObject CardPrefab{ get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Init();
        Debug.Log("ResLoader");
    }

    public void Init()
    {
        CardSprites = new List<Sprite>();
        CardPrefab = ResManager.Instance.GetAssetCache<GameObject>("Prefabs/Card");
        for (int i = 0; i < 6; i++)
        {
            CardSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Spring_" + (i+1)));
        }
        for (int i = 0; i < 6; i++)
        {
            CardSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Summer_" + (i+1)));
        }
        for (int i = 0; i < 6; i++)
        {
            CardSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Autumn_" + (i+1)));
        }
        for (int i = 0; i < 6; i++)
        {
            CardSprites.Add(ResManager.Instance.GetAssetCache<Sprite>("Sprite/Winter_" + (i+1)));
        }
        BackSprite = ResManager.Instance.GetAssetCache<Sprite>("Sprite/Back");
        Debug.Log(CardSprites.Count);
    }
}