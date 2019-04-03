namespace Core
{
    public class Player
    {
        private static int _id = 1;

        public int id;
        public int score = 0;
        public Character Character { get; set; }
        public GamepadInput GamepadInput { get; set; }

        private GamepadInput _gamepadInput;

        public Player()
        {
            id = _id;
            _id++;
        }
    }
}