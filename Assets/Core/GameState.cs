using System.Collections.Generic;
using System.Linq;
using Scenes.Menu.Scripts;

namespace Core
{
    public class GameState
    {
        public Player lastWinner;
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

        /**
         * Debug purposes only
         */
        public void AddPlayers(int number)
        {
            for (int i = 1; i <= number; i++)
            {
                _players.Add(i, new Player(new GamepadInput(i)));
            }
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

        public void Clear()
        {
            _players.Clear();
            characterSelects.Clear();
            lastWinner = null;
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

        public Player GetPlayerWithHighestScore()
        {
            Player[] players = GetAllPlayers();
            Player player = players[0];

            for (int i = 1; i < players.Length; i++)
            {
                if (player.globalScore < players[i].globalScore)
                {
                    player = players[i];
                }
            }

            return player;
        }
    }
}