using System.Collections;
using System.Collections.Generic;
using Core;
using Shared.Prefabs.PlayerCharacter;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playerCharacterPrefab;
    private Player[] _players;
    private GameObject[] _playerCharacterGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        _players = GameState.Instance.GetAllPlayers();
        _playerCharacterGameObjects = new GameObject[_players.Length];
        SpawnPlayersCharacters();
    }

    void SpawnPlayersCharacters()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            _playerCharacterGameObjects[i] = Instantiate(playerCharacterPrefab, transform);
            _players[i].playerCharacter = _playerCharacterGameObjects[i].GetComponent<PlayerCharacter>();
            _players[i].playerCharacter.ActivateSkin(_players[i].activeSkinIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}