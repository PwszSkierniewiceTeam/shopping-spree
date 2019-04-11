using System.Linq;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.FlappyRun.Scripts
{
    public class GameController : BaseGameController
    {
        public static GameController instance;

        public GameObject instruction;
        
        public float scrollSpeed = -1.5f;
        public float upForce = 200f;
        public int winsToWinLevel = 2;

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

            // debug purposes
            if (GameState.Instance.GetAllPlayers().Length == 0)
            {
                GameState.Instance.AddPlayers(3);
            }
        }

        private new void Start()
        {
            base.Start();
            SpawnPlayersCharacters(new[]
                {
                    new Vector2(-6f, 0),
                    new Vector2(-5f, 0),
                    new Vector2(-4f, 0),
                    new Vector2(-3f, 0)
                }
            );
            WatchForCollisions();

            Physics2D.gravity = Vector2.zero;
            Countdown countdownInstance = countdown.GetComponent<Countdown>();
            countdownInstance.StartCountdown().Subscribe((c) =>
            {
                instruction.SetActive(false);
                _audioSource.Play();
                Physics2D.gravity = new Vector2(0, -9.8f);
                _roundStarted = true;
            });
        }

        private void WatchForCollisions()
        {
            foreach (Player player in players)
            {
                Player p = player;
                p.playerCharacter.onCollisionEnter2DSub.Subscribe((Collision2D other) => { PlayerDied(p); });
            }
        }

        private void Update()
        {
            foreach (Player player in players)
            {
                if (!gameOver && _roundStarted && !player.isDead && player.GamepadInput.IsDown(GamepadButton.ButtonX))
                {
                    player.playerCharacter.AudioFart();
                    Rigidbody2D rb2D = player.playerCharacter.rb2D;
                    rb2D.velocity = Vector2.zero;
                    rb2D.AddForce(new Vector2(0, upForce));
                }
            }
        }

        private void ClearLevelScores()
        {
            foreach (Player player in players)
            {
                player.levelScore = 0;
            }
        }

        private void CheckGameOver()
        {
            int deadPlayersCount = players.Count(player => player.isDead);

            if (deadPlayersCount == players.Length - 1)
            {
                gameOver = true;
                Player player = players.ToList().Find(p => !p.isDead);
                player.levelScore += 1;

                if (player.levelScore == winsToWinLevel)
                {
                    player.globalScore += 1;
                    GameState.Instance.lastWinner = player;
                    ClearLevelScores();
                    SceneManager.LoadScene((int) AvailableScene.ScoreBoard);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        private void PlayerDied(Player player)
        {
            player.isDead = true;
            CheckGameOver();
        }
    }
}