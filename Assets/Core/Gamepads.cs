namespace Core
{
    public sealed class Gamepads
    {
        private readonly GamepadInput[] _gamepadInputs =
        {
            new GamepadInput(1),
            new GamepadInput(2),
            new GamepadInput(3),
            new GamepadInput(4)
        };

        private static Gamepads instance = null;
        private static readonly object padlock = new object();

        private Gamepads()
        {
        }

        public static Gamepads Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Gamepads();
                    }

                    return instance;
                }
            }
        }

        public GamepadInput GetGamepadInput(int gamepadNumber)
        {
            return _gamepadInputs[gamepadNumber - 1];
        }

        /**
         * If players pushed the button it will return gamepad number or 0 if no button is down
         */
        public int IsUp(GamepadButton gamepadButton)
        {
            foreach (GamepadInput gamepadInput in _gamepadInputs)
            {
                if (gamepadInput.IsUp(gamepadButton))
                {
                    return gamepadInput.gamepadNumber;
                }
            }

            return 0;
        }

        public int IsDown(GamepadButton gamepadButton)
        {
            foreach (GamepadInput gamepadInput in _gamepadInputs)
            {
                if (gamepadInput.IsDown(gamepadButton))
                {
                    return gamepadInput.gamepadNumber;
                }
            }

            return 0;
        }
    }
}