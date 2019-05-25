using Core;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishFightGameController : BaseGameController
{
    public static FishFightGameController instance;
    private AudioSource _audioSource;
    public int winsToWinLevel = 2;
    private bool _roundStarted;
    public GameObject instruction;

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
    }
    private new void Start()
    {
        base.Start();
        SpawnPlayersCharacters(new[]
            {
                    new Vector2(-8, 3),
                    new Vector2(-8f, -2.5f),
                    new Vector2(8f, -2.5f),
                    new Vector2(8f, 3)
                }
        );
        //WatchForCollisions();

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
                player.characterController.CanMove = true;
                player.characterController.CanJump = true;
                player.characterController.CanCrouch = true;
            }
        });
    }


    private void WatchForCollisions()
    {
        foreach (Player player in players)
        {
            Player p = player;
            p.playerCharacter.onCollisionEnter2DSub.Subscribe((Collision2D other) => PlayerDied(p, other));
        }
    }

    private void PlayerDied(Player player, Collision2D other)
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
