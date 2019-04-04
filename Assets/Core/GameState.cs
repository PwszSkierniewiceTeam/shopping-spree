using System.Collections.Generic;
using System.Linq;
using Scenes.Menu.Scripts;
using Shared.Prefabs.PlayerCharacter;

namespace Core
{
    public class GameState
    {
        public readonly Dictionary<int, CharacterSelect> characterSelects = new Dictionary<int, CharacterSelect>();
        private static GameState _instance;
        private static readonly object Padlock = new object();
        private readonly Dictionary<int, PlayerCharacter> _playersCharacters = new Dictionary<int, PlayerCharacter>();

        private GameState()
        {
        }

        public void AddPlayerCharacter(PlayerCharacter playerCharacter)
        {
            _playersCharacters.Add(playerCharacter.Id, playerCharacter);
        }

        public List<PlayerCharacter> GetAllPlayersCharacters()
        {
            return _playersCharacters.Values.ToList();
        }

        public bool RemovePlayerCharacterById(int id)
        {
            return _playersCharacters.Remove(id);
        }

        public PlayerCharacter GetPlayerCharacterById(int id)
        {
            return _playersCharacters[id];
        }

        public bool RemovePlayerCharacterByGamepadNumber(int gamepadNumber)
        {
            foreach (KeyValuePair<int, PlayerCharacter> keyValuePair in _playersCharacters)
            {
                if (keyValuePair.Value.GamepadInput.gamepadNumber == gamepadNumber)
                {
                    return RemovePlayerCharacterById(keyValuePair.Value.Id);
                }
            }

            return false;
        }

        public void ClearPlayersCharacters()
        {
            _playersCharacters.Clear();
        }

        public static GameState Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameState();
                    }

                    return _instance;
                }
            }
        }
    }
}