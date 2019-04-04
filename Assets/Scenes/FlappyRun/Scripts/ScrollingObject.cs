using UnityEngine;

namespace Scenes.FlappyRun.Scripts
{
    public class ScrollingObject : MonoBehaviour
    {
        private Rigidbody2D _rb2D;
        // Start is called before the first frame update
        void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _rb2D.velocity = new Vector2(GameController.instance.scrollSpeed, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (GameController.instance.gameOver)
            {
                _rb2D.velocity = Vector2.zero;
            }
        }
    }
}
