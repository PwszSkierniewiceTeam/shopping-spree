using System;
using System.Collections.Generic;
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
        protected Dictionary<int, GameObject> playerCharacterGameObjects = new Dictionary<int, GameObject>();

        protected void Start()
        {
            players = GameState.Instance.GetAllPlayers();
        }

        protected void SpawnPlayersCharacters(Vector2[] positions)
        {
            for (int i = 0; i < players.Length; i++)
            {
                SpawnPlayerCharacter(players[i], positions[i]);
            }
        }

        protected void SpawnPlayerCharacter(Player player, Vector2 position)
        {
            int playerId = player.Id;

            if (playerCharacterGameObjects.ContainsKey(playerId))
            {
                playerCharacterGameObjects[playerId].SetActive(false);
            }

            playerCharacterGameObjects.Add(playerId,
                Instantiate(playerCharacterPrefab, position, Quaternion.identity, transform));
            PlayerCharacter playerCharacter = playerCharacterGameObjects[playerId].GetComponent<PlayerCharacter>();
            playerCharacter.ActivateSkin(player.activeSkinIndex);
            player.isDead = false;
            player.isReady = false;
            player.playerCharacter = playerCharacter;
        }
    }
}