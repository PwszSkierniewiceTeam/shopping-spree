using Core;
using UnityEngine;

namespace Scenes.ScoreBoard.Scripts
{
    public class ScoreBoardController : BaseGameController
    {
        public int wonRoundsToWin = 3;
        public GameObject scorePrefab;

        private ScoreComponent _scoreComponent;

        private Vector2[] _playerPositions;

        private void Awake()
        {
            // Debug purposes
            if (GameState.Instance.GetAllPlayers().Length == 0)
            {
                GameState.Instance.AddPlayers(4);
            }
        }

        private void CalcPlayerPositions()
        {
            _playerPositions = new Vector2[players.Length];
            float width = 20f;
            float spacing = width / (players.Length + 1);

            Vector2 pos = new Vector2(-10f, 0);
            for (int i = 0; i < players.Length; i++)
            {
                pos = new Vector2(pos.x + spacing, 0);
                _playerPositions[i] = pos;
            }
        }

        // Start is called before the first frame update
        private new void Start()
        {
            base.Start();
            CalcPlayerPositions();
            Physics2D.gravity = Vector2.zero;
            SpawnPlayersCharacters(_playerPositions);

            _scoreComponent = scorePrefab.GetComponent<ScoreComponent>();

            for (int i = 0; i < players.Length; i++)
            {
                _scoreComponent.ShowScore(players[i].globalScore, wonRoundsToWin,
                    _playerPositions[i] + new Vector2(0, 1f), transform);
                _scoreComponent.ShowPenalty(
                    GameState.Instance.lastWinner != null && GameState.Instance.lastWinner.Id == players[i].Id,
                    _playerPositions[i] + new Vector2(0, -1.5f), 1, 3, transform);
            }
        }
    }
}