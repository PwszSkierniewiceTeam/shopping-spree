using System.Collections.Generic;
using Core;
using Shared.Prefabs.PlayerCharacter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Menu.Scripts
{
    public class PlayerSlot
    {
        public GamepadInput GamepadInput { get; set; }
        public CharacterSelect CharacterSelect { get; set; }
    }

    public class PlayerSlots : MonoBehaviour
    {
        private readonly List<PlayerSlot> _slots = new List<PlayerSlot>();
        private bool _startingGame;

        private void SelectSlot(GamepadInput gamepadInput)
        {
            CharacterSelect characterSelect = GameState.Instance.characterSelects[_slots.Count + 1];
            characterSelect.SetGamepadInput(gamepadInput);
            _slots.Add(new PlayerSlot {GamepadInput = gamepadInput, CharacterSelect = characterSelect});
        }

        private void WatchSlots()
        {
            if (_startingGame)
            {
                return;
            }

            int gamepadNumber = Gamepads.Instance.IsDown(GamepadButton.ButtonA);

            if (gamepadNumber != 0)
            {
                PlayerSlot playerSlot = _slots.Find(ps => ps.GamepadInput.gamepadNumber == gamepadNumber);

                if (playerSlot != null)
                {
                    if (playerSlot.CharacterSelect.isCharacterSelected())
                    {
                        GameState.Instance.RemovePlayerByGamepadNumber(playerSlot.GamepadInput.gamepadNumber);
                        playerSlot.CharacterSelect.ReselectCharacter();
                    }
                    else
                    {
                        PlayerCharacter playerCharacter = playerSlot.CharacterSelect.GetPlayerCharacter();
                        Player player = new Player(playerSlot.GamepadInput)
                        {
                            activeSkinIndex = playerCharacter.CurrentSkinIndex
                        };
                        GameState.Instance.AddPlayer(player);
                    }
                }
                else
                {
                    SelectSlot(Gamepads.Instance.GetGamepadInput(gamepadNumber));
                }
            }
        }

        private void WatchGameStart()
        {
            int gamepadNumber = Gamepads.Instance.IsUp(GamepadButton.ButtonStart);

            if (gamepadNumber != 0)
            {
                if (_slots.Count > 0 && _slots.TrueForAll(ps => ps.CharacterSelect.isCharacterSelected()))
                {
                    _startingGame = true;
                    // Todo: add scene randomizer
                    SceneManager.LoadScene(6);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            WatchSlots();
            WatchGameStart();
        }
    }
}