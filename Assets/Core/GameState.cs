using System.Collections.Generic;
using System.Linq;
using Scenes.Menu.Scripts;

namespace Core
{
    public class GameState
    {
        public readonly Dictionary<int, CharacterSelect> characterSelects = new Dictionary<int, CharacterSelect>();
        private static GameState _instance;
        private static readonly object Padlock = new object();
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        private GameState()
        {
        }

        public void AddPlayer(Player playerCharacter)
        {
            _players.Add(playerCharacter.Id, playerCharacter);
        }

        public Player[] GetAllPlayers()
        {
            return _players.Values.ToArray();
        }

        public bool RemovePlayerById(int id)
        {
            return _players.Remove(id);
        }

        public Player GetPlayerById(int id)
        {
            return _players[id];
        }

        public bool RemovePlayerByGamepadNumber(int gamepadNumber)
        {
            foreach (KeyValuePair<int, Player> keyValuePair in _players)
            {
                if (keyValuePair.Value.GamepadInput.gamepadNumber == gamepadNumber)
                {
                    return RemovePlayerById(keyValuePair.Value.Id);
                }
            }

            return false;
        }

        public void ClearPlayers()
        {
            _players.Clear();
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