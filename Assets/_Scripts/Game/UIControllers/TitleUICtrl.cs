using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;
using FrameWork.EventCenter;


public class TitleUICtrl : UICtrl
{
    private readonly string _startButton = "StartButton";
    public override void Awake()
    {
        base.Awake();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AddButtonListener(_startButton,
            () => EventCenter.TriggerEvent(StateKey.OnSceneStateChange, SceneState.Gameplay));
    }

    private void OnDisable()
    {
        RemoveButtonListener(_startButton);
    }
}