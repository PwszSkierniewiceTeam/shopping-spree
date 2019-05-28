using System;
using UniRx;
using UnityEngine;

namespace Shared.Prefabs.PlayerCharacter
{
    public class PlayerCharacter : MonoBehaviour
    {
        [NonSerialized] public readonly Subject<Collision2D> onCollisionEnter2DSub = new Subject<Collision2D>();
        [NonSerialized] public readonly Subject<Collider2D> onTriggerEnter2DSub = new Subject<Collider2D>();
        [NonSerialized] public readonly Subject<Collider2D> onTriggerStay2DSub = new Subject<Collider2D>();
        [NonSerialized] public readonly Subject<Collider2D> onTriggerExit2DSub = new Subject<Collider2D>();
        [NonSerialized] public Rigidbody2D rb2D;
        [NonSerialized] public PolygonCollider2D collider2D;

        public AudioClip fartSound;

        private AudioSource _audioSource;
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

        private void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExit2DSub.OnNext(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter2DSub.OnNext(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            onTriggerStay2DSub.OnNext(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            onCollisionEnter2DSub.OnNext(other);
        }

        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            collider2D = GetComponent<PolygonCollider2D>();
        }

        private void InitializeAudio()
        {
            _audioSource = GetComponent<AudioSource>();
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
            _audioSource.PlayOneShot(fartSound);
        }
    }
}