using System;
using UnityEngine;

namespace Shared.Prefabs.PlayerCharacter
{
    public class PlayerCharacter : MonoBehaviour
    {
        public AudioClip audioFartClip;
        public AudioSource audioFartSource;
        
        [NonSerialized] public int playerIndex;

        private GameObject[] _availableSkins;
        public int CurrentSkinIndex { get; private set; }
        public PlayerCharacterSkin CurrentSkin { get; private set; }

        public GameObject CurrentSkinGameObject
        {
            get { return _availableSkins[CurrentSkinIndex]; }
        }

        public void ActivateSkin(int index)
        {
            if (CurrentSkinIndex >= 0)
            {
                CurrentSkinGameObject.SetActive(false);
            }

            CurrentSkinIndex = index;
            CurrentSkinGameObject.SetActive(true);
            CurrentSkin = CurrentSkinGameObject.GetComponent<PlayerCharacterSkin>();
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
            CurrentSkinIndex = 0;
            CurrentSkinGameObject.SetActive(true);
        }

        private void InitializeAudio()
        {
            audioFartSource.clip = audioFartClip;
            
        }

        private void Awake()
        {
            InitializeSkins();
            InitializeAudio();
        }

        public void SelectNextSkin()
        {
            ActivateSkin(CurrentSkinIndex < _availableSkins.Length - 1 ? CurrentSkinIndex + 1 : 0);
        }

        public void SelectPreviousSkin()
        {
            ActivateSkin(CurrentSkinIndex > 0 ? CurrentSkinIndex - 1 : _availableSkins.Length - 1);
        }

        public void AudioFart()
        {
            audioFartSource.Play();
        }
    }
}