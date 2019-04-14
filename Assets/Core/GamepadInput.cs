using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum GamepadButton
    {
        ButtonA,
        ButtonB,
        ButtonX,
        ButtonY,
        LBumper,
        RBumper,
        ButtonStart
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

        private readonly Dictionary<GamepadButton, string> _buttons = new Dictionary<GamepadButton, string>();
        private readonly Dictionary<GamepadJoystick, string> _joysticks = new Dictionary<GamepadJoystick, string>();

        public bool IsUp(GamepadButton gamepadButton)
        {
            return Input.GetButtonUp(_buttons[gamepadButton]);
        }

        public bool IsDown(GamepadButton gamepadButton)
        {
            return Input.GetButtonDown(_buttons[gamepadButton]);
        }

        public bool IsPressed(GamepadButton gamepadButton)
        {
            return Input.GetButton(_buttons[gamepadButton]);
        }

        public float GetJoystickAxis(GamepadJoystick gamepadJoystick)
        {
            return Input.GetAxis(_joysticks[gamepadJoystick]);
        }

        public GamepadInput(int gamepadNumber)
        {
            InitController(gamepadNumber);
        }

        private void InitController(int number)
        {
            gamepadNumber = number;
            _buttons.Clear();
            _joysticks.Clear();
            InitButtons(number);
            InitJoysticks(number);
        }

        private void InitButtons(int number)
        {
            foreach (GamepadButton jb in Enum.GetValues(typeof(GamepadButton)))
            {
                _buttons.Add(jb, "J" + number + jb);
            }
        }

        private void InitJoysticks(int number)
        {
            foreach (GamepadJoystick gj in Enum.GetValues(typeof(GamepadJoystick)))
            {
                _joysticks.Add(gj, "J" + number + gj);
            }
        }
    }
}