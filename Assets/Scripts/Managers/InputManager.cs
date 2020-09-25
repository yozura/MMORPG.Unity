using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _isPressed = false;

    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _isPressed = true;
            }
            else
            {
                if(_isPressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.Click);
                    _isPressed = false;
                }
            }
        }
    }
}
