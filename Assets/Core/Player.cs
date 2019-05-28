using Shared.Prefabs.PlayerCharacter;
using UniRx;

namespace Core
{
    public class Player
    {
        public bool isDead
        {
            get { return _isDead; }
            set
            {
                _isDead = value;
                if (playerCharacter != null)
                {
                    if (_isDead)
                    {
                        playerCharacter.collider2D.enabled = false;
                        playerCharacter.gameObject.SetActive(false);
                    }
                    else
                    {
                        playerCharacter.collider2D.enabled = true;
                    }
                }
            }
        }

        public bool isReady = false;
        public int activeSkinIndex = 0;
        public int levelScore = 0;
        public int globalScore = 0;
        public PlayerCharacter playerCharacter;
        public PlayerCharacterController characterController;
        public bool moving, goRight; 
        public float firstX, curentX;

        private bool _isDead;
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