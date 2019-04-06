using System.Linq;
using Core;
using Shared.Prefabs.PlayerCharacter;
using UniRx;
using UnityEngine;

namespace Scenes.FlappyRun.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        public GameObject countdown;
        public bool gameOver;
        public float scrollSpeed = -1.5f;
        public float upForce = 200f;
        public GameObject playerCharacterPrefab;

        private Player[] _players;
        private GameObject[] _playerCharacterGameObjects;
        private AudioSource _audioSource;
        private bool _roundStarted;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                _audioSource = GetComponent<AudioSource>();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            GameState.Instance.AddPlayers(3);
        }

        private void Start()
        {
            _players = GameState.Instance.GetAllPlayers();
            _playerCharacterGameObjects = new GameObject[_players.Length];
            SpawnPlayersCharacters();

            Physics2D.gravity = Vector2.zero;
            Countdown countdownInstance = countdown.GetComponent<Countdown>();
            countdownInstance.StartCountdown().Subscribe((c) =>
            {
                Debug.Log("Countdown finished");
                _audioSource.Play();
                Physics2D.gravity = new Vector2(0, -9.8f);
                _roundStarted = true;
            });
        }

        private void SpawnPlayersCharacters()
        {
            float begin = -8f;
            for (int i = 0; i < _players.Length; i++)
            {
                begin += 1;
                _playerCharacterGameObjects[i] = Instantiate(playerCharacterPrefab, new Vector2(begin, 0),
                    Quaternion.identity, transform);
                PlayerCharacter playerCharacter = _playerCharacterGameObjects[i].GetComponent<PlayerCharacter>();
                playerCharacter.ActivateSkin(_players[i].activeSkinIndex);
                playerCharacter.playerIndex = i;
                playerCharacter.onCollisionEnter2DSub.Subscribe((Collision2D other) =>
                {
                    PlayerDied(playerCharacter.playerIndex);
                });
                
                _players[i].playerCharacter = playerCharacter;
            }
        }

        private void Update()
        {
            foreach (Player player in _players)
            {
                if (_roundStarted && !player.isDead && player.GamepadInput.IsDown(GamepadButton.ButtonX))
                {
                    player.playerCharacter.AudioFart();
                    Rigidbody2D rb2D = player.playerCharacter.rb2D;
                    rb2D.velocity = Vector2.zero;
                    rb2D.AddForce(new Vector2(0, upForce));
                }
            }
        }

        private void CheckGameOver()
        {
            int deadPlayersCount = _players.Count(player => player.isDead);

            if (deadPlayersCount < _players.Length - 1)
            {
                gameOver = true;
                
            }
        }

        private void PlayerDied(int playerIndex)
        {
            _players[playerIndex].isDead = true;
            Debug.Log("Player " + playerIndex + " died.");
            CheckGameOver();
        }
    }
}