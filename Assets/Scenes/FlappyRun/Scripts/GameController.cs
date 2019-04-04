﻿using System.Collections;
using System.Collections.Generic;
using Core;
using Shared.Prefabs.PlayerCharacter;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool gameOver;
    public float scrollSpeed = -1.5f;
    public float upForce = 200f;
    public static GameController instance;
    public GameObject playerCharacterPrefab;
    private Player[] _players;
    private GameObject[] _playerCharacterGameObjects;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _players = GameState.Instance.GetAllPlayers();
        _playerCharacterGameObjects = new GameObject[_players.Length];
        SpawnPlayersCharacters();
    }

    void SpawnPlayersCharacters()
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
/*            playerCharacter.OnCollision += new PlayerCharacter.OnCollisionEventHandler(PlayerDied);*/
            
            _players[i].playerCharacter = playerCharacter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Player player in _players)
        {
            if (!player.isDead && player.GamepadInput.IsDown(GamepadButton.ButtonX))
            {
                Rigidbody2D rb2D = player.playerCharacter.rb2D;
                rb2D.velocity = Vector2.zero;
                rb2D.AddForce(new Vector2(0, upForce));
            }
        }
    }

    void PlayerDied(int playerIndex)
    {
        _players[playerIndex].isDead = true;
    }
}