using System;
using Shared.Prefabs.PlayerCharacter;
using UnityEngine;

namespace Core
{
    public class BaseGameController : MonoBehaviour
    {
        public GameObject playerCharacterPrefab;
        public GameObject countdown;

        [NonSerialized] public bool gameOver;

        protected Player[] players;
        protected GameObject[] playerCharacterGameObjects;

        protected void Start()
        {
            players = GameState.Instance.GetAllPlayers();
            playerCharacterGameObjects = new GameObject[players.Length];
        }

        protected void SpawnPlayersCharacters(Vector2[] positions)
        {
            for (int i = 0; i < players.Length; i++)
            {
                playerCharacterGameObjects[i] =
                    Instantiate(playerCharacterPrefab, positions[i], Quaternion.identity, transform);
                PlayerCharacter playerCharacter = playerCharacterGameObjects[i].GetComponent<PlayerCharacter>();
                playerCharacter.ActivateSkin(players[i].activeSkinIndex);
                playerCharacter.playerIndex = i;
                players[i].isDead = false;
                players[i].isReady = false;
                players[i].playerCharacter = playerCharacter;
            }
        }
    }
}