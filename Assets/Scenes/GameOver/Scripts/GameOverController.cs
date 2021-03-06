﻿using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.GameOver.Scripts
{
    public class GameOverController : BaseGameController
    {
        private Player _player;

        // Start is called before the first frame update
        private new void Start()
        {
            Physics2D.gravity = Vector2.zero;

            _player = GameState.Instance.GetPlayerWithHighestScore();

            SpawnPlayerCharacter(_player, new Vector2(0, 0));
        }

        // Update is called once per frame
        private void Update()
        {
            if (_player.GamepadInput.IsUp(GamepadButton.ButtonA))
            {
                SceneManager.LoadScene((int) AvailableScene.Menu);
            }
        }
    }
}