using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : ScriptableObject
{
    public PlayerInputActions _inputActions;
    
    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        _inputActions.Player.Click.performed += PressAction;
    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Click.performed -= PressAction;
    }
    
    private void PressAction(InputAction.CallbackContext context)
    {
        var pointer = Pointer.current;
        if (pointer == null)
            return;

        // クリック位置を取得
        var position = pointer.position.ReadValue();
        // 取得座標をログ出力
        EventCenter.TriggerEvent<Vector3>(EventKey.OnPress,position);
    }
}
