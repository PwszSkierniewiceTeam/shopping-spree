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
        private readonly Dictionary<int, GameObject> _playerCharacterGameObjects = new Dictionary<int, GameObject>();

        private void Awake()
        {
            // Debug purposes
            if (GameState.Instance.GetAllPlayers().Length == 0)
            {
                GameState.Instance.AddPlayers(4);
            }
        }

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

            if (_playerCharacterGameObjects.ContainsKey(playerId))
            {
                _playerCharacterGameObjects[playerId].SetActive(false);
            }

            _playerCharacterGameObjects.Add(playerId,
                Instantiate(playerCharacterPrefab, position, Quaternion.identity, transform));
            PlayerCharacter playerCharacter = _playerCharacterGameObjects[playerId].GetComponent<PlayerCharacter>();
            PlayerCharacterController playerCharacterController = _playerCharacterGameObjects[playerId].GetComponent<PlayerCharacterController>();
            playerCharacter.ActivateSkin(player.activeSkinIndex);
            player.isDead = false;
            player.isReady = false;
            player.playerCharacter = playerCharacter;
            player.characterController = playerCharacterController;
            player.characterController.SetInputSource(player.GamepadInput);
        }
    }
}