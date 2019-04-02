namespace Core
{
    public sealed class Gamepads
    {
        public GamepadInput g1 = new GamepadInput(1);
        public GamepadInput g2 = new GamepadInput(2);
        public GamepadInput g3 = new GamepadInput(3);
        public GamepadInput g4 = new GamepadInput(4);

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
    }
}