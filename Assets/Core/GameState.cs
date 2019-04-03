using System.Collections.Generic;
using System.Linq;
using Scenes.Menu.Scripts;

namespace Core
{
    public class GameState
    {
        public Dictionary<int, CharacterSelect> characterSelects = new Dictionary<int, CharacterSelect>();
        private static GameState instance = null;
        private static readonly object padlock = new object();
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        private GameState()
        {
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player.id, player);
        }

        public List<Player> GetAllPlayers()
        {
            return _players.Values.ToList();
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
                    return RemovePlayerById(keyValuePair.Value.id);
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
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameState();
                    }

                    return instance;
                }
            }
        }
    }
}