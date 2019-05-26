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
        private RaceGameController raceGameController;
        private Rigidbody2D rb2D;
        private GameController flappyController;
        private ScoreBoardController scoreBoardController;
        private Player player;


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
                //Debug.Log("SSSSSSSSSSSSSSSSSSSCCCCCCCCCCCCC");
                

              //   if (GameState.Instance.lastWinner != null && GameState.Instance.lastWinner.Id == player.Id )
              //      _animator.SetBool("Cheer", true);
              //  else
              //      _animator.SetBool("Lose", true);


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
        }
    }
}