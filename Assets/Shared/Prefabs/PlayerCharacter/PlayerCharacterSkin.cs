using System;
using UniRx;
using UnityEngine;

namespace Shared.Prefabs.PlayerCharacter
{
    public class PlayerCharacterSkin : MonoBehaviour
    {
        [NonSerialized] public readonly Subject<Collision2D> onCollisionEnter2DSub = new Subject<Collision2D>();

        [NonSerialized] public Animator animator;

        [NonSerialized] public Rigidbody2D rb2D;

        // Start is called before the first frame update
        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            onCollisionEnter2DSub.OnNext(other);
        }
    }
}