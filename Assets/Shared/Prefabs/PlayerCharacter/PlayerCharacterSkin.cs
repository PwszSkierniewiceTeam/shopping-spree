using Scenes.RaceToTheCashRegister.Scripts;
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
        private PlayerCharacterController playerCharacterController;
        private RaceGameController raceGameController;
        private Rigidbody2D rb2D;
        private GameController flappyController;
        private ScoreBoardController scoreBoardController;
        private FishFightGameController fishFightGameController;
        private avoidObstaclesGameController avoidObstaclesGameController;
        private RPGameController rPGameController;
        private Player player;
        private float timer { get; set; } = 1;


        private void Awake()
        {
            playerCharacterController = FindObjectOfType<PlayerCharacterController>();
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            raceGameController = FindObjectOfType<RaceGameController>();
            flappyController = FindObjectOfType<GameController>();
            scoreBoardController = FindObjectOfType<ScoreBoardController>();
            fishFightGameController = FindObjectOfType<FishFightGameController>();
            avoidObstaclesGameController = FindObjectOfType<avoidObstaclesGameController>();

            ClearParametr();
        }

        private void Update()
        {
            if(_animator.GetBool("Dead"))
            {
                SetAnimatorBool("Dead");
                timer -= Time.deltaTime;
                if(timer <= 0)
                    playerCharacter.gameObject.SetActive(false);
            }
            
            if(avoidObstaclesGameController)
            {
                rb2D = playerCharacter.rb2D;
                if (rb2D.velocity.y > 0.1f)
                {
                    SetAnimatorBool("Jump");
                }
                if(rb2D.velocity.y <= 0.1f)
                {
                    SetAnimatorBool("Fly");
                }
                if (playerCharacterController.isGrounded)
                {
                    ClearParametr();
                    _animator.SetFloat("Speed", 1f);
                }

            }
            if (fishFightGameController || rPGameController )
            {
                rb2D = playerCharacter.rb2D;
                if (rb2D.velocity.y > 1f)
                {
                    SetAnimatorBool("Jump");
                }
                if (rb2D.velocity.y <= -0.5f)
                {
                    SetAnimatorBool("Fly");
                }
                if (playerCharacterController.isGrounded)
                {
                    SetAnimatorBool("Idle");
                }
                if (playerCharacterController.isGrounded && (rb2D.velocity.x > 0.5 || rb2D.velocity.x < -0.5))
                {
                    ClearParametr();
                    _animator.SetFloat("Speed", 1f);
                }

            }

            if (raceGameController)
            {
                rb2D = playerCharacter.rb2D;
                SetFly(rb2D.velocity.x);
                
                _animator.SetFloat("Speed", rb2D.velocity.x);
            }


            if (flappyController)
            {
                rb2D = playerCharacter.rb2D;

                if (rb2D.velocity.y < 0)
                {
                    SetAnimatorBool("Fly");
                }

                else if (rb2D.velocity.y > 0)
                {
                    SetAnimatorBool("Fart");
                }
            }     

            if(scoreBoardController)
            {
               if(_animator.GetBool("Cheer"))
                {
                    SetAnimatorBool("Cheer");
                }

                if (_animator.GetBool("Lose"))
                {
                    SetAnimatorBool("Lose");
                }
            }

        }

        private void SetAnimatorBool(string s)
        {
            ClearParametr();
            _animator.SetBool(s, true);
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

        private void ClearParametr()
        {
            _animator.SetBool("Fart", false);
            _animator.SetBool("Fly", false);
            _animator.SetBool("Cheer", false);
            _animator.SetBool("Lose", false);
            _animator.SetBool("Dead", false);
            _animator.SetBool("Jump", false);
            _animator.SetFloat("Speed", 0);
        }
    }
}