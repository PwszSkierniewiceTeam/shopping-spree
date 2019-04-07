using Shared.Prefabs.PlayerCharacter;

namespace Core
{
    public class Player
    {
        public bool isDead = false;
        public int activeSkinIndex = 0;
        public int levelScore = 0;
        public int globalScore = 0;
        public PlayerCharacter playerCharacter;

        private static int _id = 1;
        private readonly int _playerId;
        private readonly GamepadInput _gamepadInput;

        public int Id
        {
            get { return _playerId; }
        }

        public GamepadInput GamepadInput
        {
            get { return _gamepadInput; }
        }

        public Player(GamepadInput gamepadInput)
        {
            _playerId = _id;
            _id++;
            _gamepadInput = gamepadInput;
        }
    }
}