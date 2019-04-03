using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Scenes.Menu.Scripts
{
    public class PlayerSlot
    {
        public GamepadInput GamepadInput { get; set; }
        public CharacterSelect CharacterSelect { get; set; }
    }

    public class PlayerSlots : MonoBehaviour
    {
        private List<PlayerSlot> _slots = new List<PlayerSlot>();

        // Start is called before the first frame update
        void Start()
        {
        }

        private void SelectSlot(GamepadInput gamepadInput)
        {
            CharacterSelect characterSelect = GameState.Instance.characterSelects[_slots.Count + 1];
            characterSelect.SetGamepadInput(gamepadInput);
            _slots.Add(new PlayerSlot {GamepadInput = gamepadInput, CharacterSelect = characterSelect});
        }

        private void WaitForSlotSelect()
        {
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
                        Character character = playerSlot.CharacterSelect.SelectCurrentCharacter();
                        Player player = new Player {Character = character, GamepadInput = playerSlot.GamepadInput};
                        GameState.Instance.AddPlayer(player);
                    }
                }
                else
                {
                    SelectSlot(Gamepads.Instance.GetGamepadInput(gamepadNumber));
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_slots.Count < 4)
            {
                WaitForSlotSelect();
            }
        }
    }
}