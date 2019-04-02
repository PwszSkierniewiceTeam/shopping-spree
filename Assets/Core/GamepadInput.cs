using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamepadButton
{
    ButtonA,
    ButtonB,
    ButtonX,
    ButtonY,
    LBumper,
    RBumper
}

public enum GamepadJoystick
{
    LeftJoystickHorizontal,
    LeftJoystickVertical,
    RightJoystickHorizontal,
    RightJoystickVertical
}

public class GamepadInput
{
    public int gamepadNumber;

    private Dictionary<GamepadButton, string> buttons = new Dictionary<GamepadButton, string>();
    private Dictionary<GamepadJoystick, string> joysticks = new Dictionary<GamepadJoystick, string>();

    public bool IsUp(GamepadButton gamepadButton)
    {
        return Input.GetButtonUp(buttons[gamepadButton]);
    }

    public bool IsDown(GamepadButton gamepadButton)
    {
        return Input.GetButtonDown(buttons[gamepadButton]);
    }

    public bool IsPressed(GamepadButton gamepadButton)
    {
        return Input.GetButton(buttons[gamepadButton]);
    }

    public float GetJoystickAxis(GamepadJoystick gamepadJoystick)
    {
        return Input.GetAxis(joysticks[gamepadJoystick]);
    }

    public void InitController(int number)
    {
        gamepadNumber = number;
        buttons.Clear();
        joysticks.Clear();
        InitButtons(number);
        InitJoysticks(number);
    }

    private void InitButtons(int number)
    {
        foreach (GamepadButton jb in Enum.GetValues(typeof(GamepadButton)))
        {
            buttons.Add(jb, "J" + number + jb);
        }
    }

    private void InitJoysticks(int number)
    {
        foreach (GamepadJoystick gj in Enum.GetValues(typeof(GamepadJoystick)))
        {
            joysticks.Add(gj, "J" + number + gj);
//            Debug.Log("J" + number + gj);
        }
    }
}