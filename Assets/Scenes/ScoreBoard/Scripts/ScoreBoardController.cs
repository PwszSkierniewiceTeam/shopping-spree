using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.ScoreBoard.Scripts
{
    public class ScoreBoardController : BaseGameController
    {
        public int wonRoundsToWin = 3;
        public GameObject scorePrefab;
        public GameObject readyStatusPrefab;
        public GameObject notReadyStatusPrefab;

        private ScoreComponent _scoreComponent;
        private GameObject[] _statuses;
        private Vector2[] _playerPositions;
        private int _playersReady = 0;

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
            _statuses = new GameObject[players.Length];
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
                _statuses[i] = Instantiate(notReadyStatusPrefab, _playerPositions[i] + new Vector2(0, -2.5f),
                    Quaternion.identity, transform);
            }
        }

        private void Update()
        {
            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];

                if (player.GamepadInput.IsUp(GamepadButton.ButtonA) && !player.isReady)
                {
                    player.isReady = true;
                    _statuses[i].SetActive(false);
                    _statuses[i] = Instantiate(readyStatusPrefab, _statuses[i].transform.position,
                        Quaternion.identity, transform);
                    _playersReady += 1;

                    if (_playersReady == players.Length)
                    {
                        if (GameState.Instance.GetPlayerWithHighestScore().globalScore == wonRoundsToWin)
                        {
                            SceneManager.LoadScene((int) AvailableScene.GameOver);
                        }
                        else
                        {
                            SceneManager.LoadScene(GameRandomizer.Instance.GetNextSceneBuildIndex());
                        }
                    }
                }
            }
        }
    }
}