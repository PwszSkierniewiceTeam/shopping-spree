using Shared.Prefabs.PlayerCharacter;
using UnityEngine;

namespace Core
{
    public class Player
    {
        public int activeSkinIndex = 0;
        private static int _id = 1;
        private readonly int _playerId;
        private readonly GamepadInput _gamepadInput;
        public PlayerCharacter playerCharacter;
        private GameObject playerCharacterGameObject;

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