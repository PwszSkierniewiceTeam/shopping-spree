using Core;
using UnityEngine;

namespace Scenes.ScoreBoard.Scripts
{
    public class ScoreBoardController : BaseGameController
    {
        public int wonRoundsToWin = 3;
        public GameObject scorePrefab;

        private ScoreComponent _scoreComponent;

        private readonly Vector2[] _playerPositions =
        {
            new Vector2(-7.5f, 0),
            new Vector2(-2.5f, 0),
            new Vector2(2.5f, 0),
            new Vector2(7.5f, 0)
        };

        private readonly Vector2[] _scorePositions =
        {
            new Vector2(-7.5f, 1f),
            new Vector2(-2.5f, 1f),
            new Vector2(2.5f, 1f),
            new Vector2(7.5f, 1f)
        };

        private void Awake()
        {
            // Debug purposes
            if (GameState.Instance.GetAllPlayers().Length == 0)
            {
                GameState.Instance.AddPlayers(4);
            }
        }


        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            Physics2D.gravity = Vector2.zero;

            SpawnPlayersCharacters(_playerPositions);

            _scoreComponent = scorePrefab.GetComponent<ScoreComponent>();

            for (int i = 0; i < players.Length; i++)
            {
                _scoreComponent.ShowScore(players[i].globalScore, wonRoundsToWin, _scorePositions[i], transform);
            }
        }
    }
}