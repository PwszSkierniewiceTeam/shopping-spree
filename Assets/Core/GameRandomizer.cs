using System;
using System.Collections.Generic;

namespace Core
{
    public class GameRandomizer
    {
        private static GameRandomizer _instance;
        private static readonly object Padlock = new object();

        private List<int> _availableGames;
        private List<int> _playedGames = new List<int>();

        private GameRandomizer()
        {
            _availableGames = new List<int>
            {
                (int) AvailableScene.FlappyRun,
                (int) AvailableScene.RaceToTheCashRegister
            };
        }

        public static GameRandomizer Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameRandomizer();
                    }

                    return _instance;
                }
            }
        }

        public int GetNextSceneBuildIndex()
        {
            Random rnd = new Random();

            int r = rnd.Next(_availableGames.Count);
            int buildIndex = _availableGames[r];

            _playedGames.Add(r);
            _availableGames.Remove(r);

            if (_availableGames.Count == 0)
            {
                _availableGames = new List<int>(_playedGames);
                _playedGames = new List<int>();
            }

            return buildIndex;
        }
    }
}