using Core;
using Shared.Prefabs.PlayerCharacter;
using UnityEngine;

namespace Scenes.Menu.Scripts
{
    public class CharacterSelect : MonoBehaviour
    {
        public AudioClip audioClipChangeCharacter;
        public AudioClip audioClipSelectCharacter;
        public GameObject playerCharacterPrefab;
        private AudioSource _audioSource;
        private GameObject _playerCharacterGameObject;
        private PlayerCharacter _playerCharacter;
        private Player _player;

        public int slotNumber;
        private GamepadInput _gamepadInput;
        private bool _changed;
        private int[] _slotsWithGamepadNumbers;
        private bool _characterSelected;
        private GameObject _selectedBg;

        public void SetGamepadInput(GamepadInput gamepadInput)
        {
            _gamepadInput = gamepadInput;
            _playerCharacterGameObject = Instantiate(playerCharacterPrefab, transform);
            _playerCharacter = _playerCharacterGameObject.GetComponent<PlayerCharacter>();
            // Hide A button
            transform.GetChild(0).gameObject.SetActive(false);
            _audioSource.PlayOneShot(audioClipSelectCharacter, 1f);
        }

        public PlayerCharacter GetPlayerCharacter()
        {
            _audioSource.PlayOneShot(audioClipSelectCharacter, 1f);
            _selectedBg.SetActive(true);
            _characterSelected = true;
            return _playerCharacter;
        }

        public void ReselectCharacter()
        {
            _characterSelected = false;
            _selectedBg.SetActive(false);
            _audioSource.PlayOneShot(audioClipSelectCharacter, 1f);
        }

        public bool isCharacterSelected()
        {
            return _characterSelected;
        }

        private void Awake()
        {
            GameState.Instance.characterSelects.Add(slotNumber, this);
            _audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            int childCount = transform.childCount;
            _selectedBg = transform.GetChild(childCount - 1).gameObject;
            _selectedBg.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_characterSelected && _gamepadInput != null)
            {
                float axis = _gamepadInput.GetJoystickAxis(GamepadJoystick.LeftJoystickHorizontal);

                if (axis == 0)
                {
                    _changed = false;
                }
                else if (!_changed && axis > 0.5f)
                {
                    _playerCharacter.SelectNextSkin();
                    _audioSource.PlayOneShot(audioClipChangeCharacter, 0.25f);
                    _changed = true;
                }
                else if (!_changed && axis < -0.5f)
                {
                    _playerCharacter.SelectPreviousSkin();
                    _audioSource.PlayOneShot(audioClipChangeCharacter, 0.25f);
                    _changed = true;
                }
            }
        }
    }
}