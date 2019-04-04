﻿using Core;
using UnityEngine;

namespace Scenes.Menu.Scripts
{
    public class CharacterSelect : MonoBehaviour
    {
        public int slotNumber;
        private GamepadInput _gamepadInput;
        private bool _changed;
        private int _charactersCount = 0;
        private int _currentCharacter = 0;
        private GameObject[] _charactersList;
        private int[] _slotsWithGamepadNumbers;
        private bool _characterSelected;
        private GameObject _notSelectedBg;
        private GameObject _selectedBg;

        public void SetGamepadInput(GamepadInput gamepadInput)
        {
            _gamepadInput = gamepadInput;

            if (_charactersList[0])
            {
                _charactersList[0].SetActive(true);
            }

            transform.GetChild(1).gameObject.SetActive(false);
        }

        public Character SelectCurrentCharacter()
        {
            _selectedBg.SetActive(true);
            _characterSelected = true;
            return new Character {GameObject = _charactersList[_currentCharacter]};
        }

        public void ReselectCharacter()
        {
            _characterSelected = false;
            _selectedBg.SetActive(false);
        }

        public bool isCharacterSelected()
        {
            return _characterSelected;
        }

        private void Awake()
        {
            GameState.Instance.characterSelects.Add(slotNumber, this);
        }

        // Start is called before the first frame update
        void Start()
        {
            int childCount = transform.childCount;
            _selectedBg = transform.GetChild(childCount - 1).gameObject;
            _notSelectedBg = transform.GetChild(childCount - 2).gameObject;
            _selectedBg.SetActive(false);
            
            Transform characters = transform.GetChild(0);

            _charactersCount = characters.childCount;
            _charactersList = new GameObject[characters.childCount];

            for (int i = 0; i < characters.childCount; i++)
            {
                _charactersList[i] = characters.GetChild(i).gameObject;
            }

            foreach (GameObject go in _charactersList)
            {
                go.SetActive(false);
            }
        }

        private void ChangeCharacter(bool next)
        {
            _charactersList[_currentCharacter].SetActive(false);

            if (next)
            {
                _currentCharacter = _currentCharacter < _charactersCount - 1 ? _currentCharacter + 1 : 0;
            }
            else
            {
                _currentCharacter = _currentCharacter > 0 ? _currentCharacter - 1 : _charactersCount - 1;
            }

            _charactersList[_currentCharacter].SetActive(true);

            _changed = true;
        }

        // Update is called once per frame
        void Update()
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
                    ChangeCharacter(true);
                }
                else if (!_changed && axis < -0.5f)
                {
                    ChangeCharacter(false);
                }
            }
        }
    }
}