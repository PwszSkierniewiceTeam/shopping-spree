using Core;
using UnityEngine;

namespace Shared.Prefabs.PlayerCharacter
{
    public class PlayerCharacter : MonoBehaviour
    {
        public int score = 0;

        private static int _id = 1;
        private readonly int _playerId;
        private GameObject[] _availableSkins;
        private int _currentSkinIndex;
        private GamepadInput _gamepadInput;

        public PlayerCharacter()
        {
            _playerId = _id;
            _id++;
        }

        public void Initialize(GamepadInput gamepadInput)
        {
            _gamepadInput = gamepadInput;
        }

        public int Id
        {
            get { return _playerId; }
        }

        public GamepadInput GamepadInput
        {
            get { return _gamepadInput; }
        }

        public GameObject CurrentSkin
        {
            get { return _availableSkins[_currentSkinIndex]; }
        }

        private void InitializeSkins()
        {
            Transform skins = transform.GetChild(0);
            int childCount = skins.childCount;
            _availableSkins = new GameObject[childCount];

            // fill skins array
            for (int i = 0; i < childCount; i++)
            {
                GameObject skin = skins.GetChild(i).gameObject;
                _availableSkins[i] = skin;
                skin.SetActive(false);
            }

            // select first skin by default
            _currentSkinIndex = 0;
            CurrentSkin.SetActive(true);
        }

        // Start is called before the first frame update
        private void Start()
        {
            InitializeSkins();
        }

        public void SelectNextSkin()
        {
            CurrentSkin.SetActive(false);
            _currentSkinIndex = _currentSkinIndex < _availableSkins.Length - 1 ? _currentSkinIndex + 1 : 0;
            CurrentSkin.SetActive(true);
        }

        public void SelectPreviousSkin()
        {
            CurrentSkin.SetActive(false);
            _currentSkinIndex = _currentSkinIndex > 0 ? _currentSkinIndex - 1 : _availableSkins.Length - 1;
            CurrentSkin.SetActive(true);
        }
    }
}