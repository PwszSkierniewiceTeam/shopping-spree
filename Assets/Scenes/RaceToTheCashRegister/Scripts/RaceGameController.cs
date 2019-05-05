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

        private int moveSpeed = 1;
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
                GameState.Instance.AddPlayers(2);
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
                    new Vector2(-7f, 2.25f),
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

        private void Update()
        {
            CheckChangeLight();

            foreach (Player player in players)
            {
               rb2D = player.playerCharacter.rb2D;

                if (player.GamepadInput.IsDown(GamepadButton.ButtonX) && !player.moving && x)
                {
                    Debug.Log("Start position = " + rb2D.position.x);
                    if (_lightAnimator.GetBool("IsOpen"))
                    {
                        player.firstX = rb2D.position.x;
                        rb2D.AddForce(Vector2.right * 100);
                        player.moving = true;
                        player.goRight = true;
                    }

                    if (_lightAnimator.GetBool("IsClosed"))
                    {
                        player.firstX = rb2D.position.x;
                        rb2D.AddForce(Vector2.left * 100);
                        player.moving = true;
                    }
                }

                player.curentX = rb2D.position.x;
                if(player.moving)
                {
                    Debug.Log("velocity = " + rb2D.velocity.x);
                    if (player.curentX > player.firstX + moveSpeed && player.goRight)
                    {
                        Debug.Log("Stop position = " + rb2D.position.x);

                        rb2D.velocity = Vector2.zero;
                        player.moving = false;
                        player.goRight = false;
                    }
                    if (player.curentX < player.firstX - moveSpeed && !player.goRight)
                    {
                        Debug.Log("Stop position = " + rb2D.position.x);

                        rb2D.velocity = Vector2.zero;
                        player.moving = false;
                    }
                    CheckWin();
                }
                
                
            }
        }
  
        private void CheckChangeLight()
        {
            if (_lightAnimator.GetBool("IsOpen") || _lightAnimator.GetBool("IsClosed"))
                x = true;
            else
                x = false;
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
                player.moving = false;
            }
        }

        private void CheckWin()
        {
            foreach (Player player in players)
            {
               rb2D = player.playerCharacter.rb2D;
                if (rb2D.position.x > 6 )
                {
                    Debug.Log("WINNNNNNNNNNNN");
                    gameOver = true;
                    player.levelScore += 1;
                    

                    if (player.levelScore == winsToWinLevel)
                    {
                        player.globalScore += 1;
                        GameState.Instance.lastWinner = player;
                        ClearLevelScores();
                        SceneManager.LoadScene((int)AvailableScene.ScoreBoard);
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