﻿using Scenes.RaceToTheCashRegister.Scripts;
using Scenes.FlappyRun.Scripts;
using UnityEngine;
using Scenes.ScoreBoard.Scripts;
using Core;

namespace Shared.Prefabs.PlayerCharacter
{
    public class PlayerCharacterSkin : MonoBehaviour
    {
        public Animator _animator;
        
        private PlayerCharacter playerCharacter;
        private RaceGameController raceGameController;
        private Rigidbody2D rb2D;
        private GameController flappyController;
        private ScoreBoardController scoreBoardController;
        private Player player;
        private float timer { get; set; } = 1;


        private void Awake()
        {
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            raceGameController = FindObjectOfType<RaceGameController>();
            flappyController = FindObjectOfType<GameController>();
            scoreBoardController = FindObjectOfType<ScoreBoardController>();
            
            ClearBool();
        }

        private void Update()
        {
            if(_animator.GetBool("Dead"))
            {
                Debug.Log("umieram");
                ClearBool();
                _animator.SetBool("Dead", true);
                timer -= Time.deltaTime;
                if(timer <= 0)
                    playerCharacter.gameObject.SetActive(false);
            }
  

            if (raceGameController)
            {
                rb2D = playerCharacter.rb2D;
                SetFly(rb2D.velocity.x);
                

                //Debug.Log("RACE***RACE***RACE***RACE");
                
                _animator.SetFloat("Speed", rb2D.velocity.x);
            }


            if (flappyController)
            {
                //Debug.Log("FLLLAAAPPPPYY");
                rb2D = playerCharacter.rb2D;


                if (rb2D.velocity.y < 0)
                {
                    _animator.SetBool("Fart", false);
                    _animator.SetBool("Fly", true);
                }

                else if (rb2D.velocity.y > 0)
                {
                    _animator.SetBool("Fly", false);
                    _animator.SetBool("Fart", true);
                }
            }
                

            if(scoreBoardController)
            {
               if(_animator.GetBool("Cheer"))
                {
                    ClearBool();
                    _animator.SetBool("Cheer", true);
                }

                if (_animator.GetBool("Lose"))
                {
                    ClearBool();
                    _animator.SetBool("Lose", true);
                }
            }


        }

        private void SetFly(float i )
        {
            if (i < 0)
            {
                _animator.SetBool("Fly", true);
            }
            else
            {
                _animator.SetBool("Fly", false);
            }
        }

        private void ClearBool()
        {
            _animator.SetBool("Fart", false);
            _animator.SetBool("Fly", false);
            _animator.SetBool("Cheer", false);
            _animator.SetBool("Lose", false);
            _animator.SetBool("Dead", false);
        }
    }
}