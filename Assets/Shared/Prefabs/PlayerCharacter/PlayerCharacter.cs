using System;
using Core;
using UnityEngine;

namespace Shared.Prefabs.PlayerCharacter
{
    public class PlayerCharacter : MonoBehaviour
    {
        [NonSerialized] public Animator animator;
        [NonSerialized] public Rigidbody2D rb2D;

        private GameObject[] _availableSkins;

        public int CurrentSkinIndex { get; private set; }

        public GameObject CurrentSkin
        {
            get { return _availableSkins[CurrentSkinIndex]; }
        }

        public void ActivateSkin(int index)
        {
            if (CurrentSkinIndex >= 0)
            {
                CurrentSkin.SetActive(false);
            }

            CurrentSkinIndex = index;
            CurrentSkin.SetActive(true);

            rb2D = CurrentSkin.GetComponent<Rigidbody2D>();
            animator = CurrentSkin.GetComponent<Animator>();
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
            CurrentSkin.SetActive(true);
        }

        private void Awake()
        {
            InitializeSkins();
        }

        public void SelectNextSkin()
        {
            ActivateSkin(CurrentSkinIndex < _availableSkins.Length - 1 ? CurrentSkinIndex + 1 : 0);
        }

        public void SelectPreviousSkin()
        {
            ActivateSkin(CurrentSkinIndex > 0 ? CurrentSkinIndex - 1 : _availableSkins.Length - 1);
        }
    }
}