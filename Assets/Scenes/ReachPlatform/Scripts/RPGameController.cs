using Core;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RPGameController : BaseGameController
{

    public static RPGameController instance;
    private AudioSource _audioSource;
    public int winsToWinLevel = 2;
    private bool _roundStarted;
    public GameObject instruction;
    public GameObject platforms;
    public GameObject platformsSpot;



    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("odpalilem sie");
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
    }
    private new void Start()
    {
        base.Start();
        SpawnPlayersCharacters(new[]
            {
                    new Vector2(-3,-4),
                    new Vector2(-39f, -49),
                    new Vector2(-5f, -4),
                    new Vector2(-6f, -4)
                }
        );
        WatchForCollisions();



        Physics2D.gravity = Vector2.zero;
        Countdown countdownInstance = countdown.GetComponent<Countdown>();
        countdownInstance.StartCountdown().Subscribe((c) =>
        {
            instruction.SetActive(false);
            _audioSource.Play();
            Physics2D.gravity = new Vector2(0, -6f);
            _roundStarted = true;
            foreach (var player in players)
            {
                //player.isDead = false;
                player.characterController.CanMove = true;
                player.characterController.CanJump = true;
                player.characterController.CanCrouch = true;
            }
            //platform.GetComponent<platform>().platforms_moving = true;
            platformsSpot.GetComponent<platformsSpot>().platforms_start = true;
            //platform.GetComponent<platform>().speed = 4f;
        });
    }



    private void WatchForCollisions()
    {
        foreach (Player player in players)
        {
            Player p = player;
            //p.playerCharacter.onCollisionEnter2DSub.Subscribe((Collision2D other) => PlayerDied(p, other));

            p.playerCharacter.onCollisionEnter2DSub.Subscribe((Collision2D collsion) => {
                if (collsion.collider.tag == "enemies" || collsion.collider.tag == "floor")
                {
                    Debug.Log(player + "zderzyl sie podlaga albo z poszczola");
                    PlayerDied(p);
                }

            });

        }
    }

    private void PlayerDied(Player player)//, Collision2D other)
    {
        player.isDead = true;
        CheckGameOver();
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
                SceneManager.LoadScene((int)AvailableScene.ScoreBoard);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}
