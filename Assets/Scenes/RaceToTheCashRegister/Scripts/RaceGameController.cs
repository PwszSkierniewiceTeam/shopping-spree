using Core;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.RaceToTheCashRegister.Scripts
{
    public class RaceGameController : BaseGameController
    {
        public static RaceGameController instance;

        public GameObject instruction;
        public GameObject light;
        public int winsToWinLevel = 2;

        private int moveSpeed = 2;
        private bool x;
        public Rigidbody2D rb2D;
        private Animator _lightAnimator;
        private AudioSource _audioSource;
        public bool _roundStarted;

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
                GameState.Instance.AddPlayers(4);
            }

            light.SetActive(false);
        }

        private new void Start()
        {
            base.Start();
            SpawnPlayersCharacters(new[]
                {
                    new Vector2(-7f, 0.75f),
                    new Vector2(-7f, -0.75f),
                    new Vector2(-7f, -3.75f),
                    new Vector2(-7f, -2.25f)
                }
            );

            _lightAnimator = light.GetComponent<Animator>();
            Countdown countdownInstance = countdown.GetComponent<Countdown>();
            countdownInstance.StartCountdown().Subscribe((c) =>
            {
                instruction.SetActive(false);
                _audioSource.Play();
                _roundStarted = true;
                light.SetActive(true);
            });
        }

        private void FixedUpdate()
        {
            CheckChangeLight();
            
            foreach (Player player in players)
            {
                rb2D = player.playerCharacter.rb2D;
                PlayerCharacterController pController = player.characterController;

                if (!pController.moving && pController.moved && x  && player.GamepadInput.IsDown(GamepadButton.ButtonX))
                {
                    if (light.GetComponent<CheckLight>()._isOpen)
                    {
                        pController.firstX = rb2D.position.x;
                        rb2D.AddForce(Vector2.right * 150);
                        pController.moving = true;
                        pController.goRight = true;
                        pController.moved = false;
                    }

                    if (light.GetComponent<CheckLight>()._isClosed && !CheckBack(player))
                    {
                        pController.firstX = rb2D.position.x;
                        rb2D.AddForce(Vector2.left * 180);
                        pController.moving = true;
                        pController.moved = false;
                    }
                }

                if (pController.moving)
                {
                    if (rb2D.position.x > pController.firstX + moveSpeed && pController.goRight)
                    {
                        rb2D.velocity = Vector2.zero;
                        pController.moving = false;
                        pController.goRight = false;
                    }

                    if (CheckBack(player) && !pController.goRight || rb2D.position.x < pController.firstX - 1.5 * moveSpeed && !pController.goRight)
                    {
                        rb2D.velocity = Vector2.zero;
                        pController.moving = false;
                    }

                    CheckWin();
                }
            }
        }

        private bool CheckBack(Player player)
        {
            if (player.playerCharacter.rb2D.position.x <= -8)
                return true;
            else
                return false;
        }
        private void CheckChangeLight()
        {
            if (light.GetComponent<CheckLight>()._isClosed || light.GetComponent<CheckLight>()._isOpen)
                x = true;
            else
            {
                x = false;
                foreach (Player player in players)
                {
                    player.characterController.moved = true;
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

        private void ClearPlayerMoving()
        {
            foreach (Player player in players)
            {
                player.characterController.moving = false;
            }
        }

        private void CheckWin()
        {
            foreach (Player player in players)
            {
                rb2D = player.playerCharacter.rb2D;
                if (rb2D.position.x > 6)
                {
                    gameOver = true;
                    player.levelScore += 1;


                    if (player.levelScore == winsToWinLevel)
                    {
                        player.globalScore += 1;
                        GameState.Instance.lastWinner = player;
                        ClearLevelScores();
                        SceneManager.LoadScene((int) AvailableScene.ScoreBoard);
                        ClearPlayerMoving();
                    }
                    else
                    {
                        ClearPlayerMoving();
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
            }
        }
    }
}