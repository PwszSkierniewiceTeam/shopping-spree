using Core;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class FishFightGameController : BaseGameController
{
    public static FishFightGameController instance;
    private AudioSource _audioSource;
    public int winsToWinLevel = 2;
    private bool _roundStarted;
    public GameObject instruction;
    public GameObject throwable;

    [SerializeField]
    private int secondsToRespawn = 4;
    [SerializeField]
    private GameObject blood;

    private List<Vector2> _initialSpawnPoints = new List<Vector2>
    {
        new Vector2(-8, 3.6f),
        new Vector2(8f, 3.6f),
        new Vector2(-8f, -2.5f),
        new Vector2(8f, -2.5f)
    };
    private Vector2[] _spawnPoints;

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
        _spawnPoints = _initialSpawnPoints.Take(players.Length).ToArray();
        SpawnPlayersCharacters(_spawnPoints);
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
                player.characterController.CanMove = true;
                player.characterController.CanJump = true;
                player.characterController.CanCrouch = true;
                player.characterController.CanThrowStuff = true;
                player.characterController.ThrowableObject = throwable;
                player.characterController.CanThrowMoreThanOneThing = false;
                player.characterController.IsThrowPowerFromButtonHold = true;
                player.characterController.ResetStatus();
            }
        });
    }


    private void WatchForCollisions()
    {
        foreach (Player player in players)
        {
            Player p = player;
            p.playerCharacter.onTriggerEnter2DSub.Subscribe(async (Collider2D collision) =>
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Killz") && !player.characterController.Throwables.Contains(collision.gameObject))
                {
                    await PlayerDiedAsync(p, collision);
                }

            });
        }
    }

    private async Task PlayerDiedAsync(Player player, Collider2D collision)
    {
        Instantiate(blood, player.playerCharacter.gameObject.transform.position, Quaternion.identity);

        player.characterController.AllowCharacterControll = false;
        player.playerCharacter.circleCollider2D.enabled = false;
        player.playerCharacter.boxCollider2D.enabled = false;
        player.playerCharacter.rb2D.AddForce(new Vector2(0, -100));

        await ((Func<int, Task>)(async t => await Task.Delay(TimeSpan.FromSeconds(t))))(secondsToRespawn);

        player.characterController.ResetStatus();
        player.playerCharacter.circleCollider2D.enabled = true;
        player.playerCharacter.boxCollider2D.enabled = true;
        player.playerCharacter.rb2D.velocity = Vector3.zero;
        player.characterController.gameObject.transform.position = _spawnPoints.ElementAt(Array.IndexOf(players, player));
        player.characterController.AllowCharacterControll = true;
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
