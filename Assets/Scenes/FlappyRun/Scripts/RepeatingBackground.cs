using UnityEngine;

namespace Scenes.FlappyRun.Scripts
{
    public class RepeatingBackground : MonoBehaviour
    {
        private BoxCollider2D _groundCollider;

        private float _groundHorizontalLength;

        // Start is called before the first frame update
        void Start()
        {
            _groundCollider = GetComponent<BoxCollider2D>();
            _groundHorizontalLength = _groundCollider.size.x;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.x < -_groundHorizontalLength)
            {
                RepositionBackground();
            }
        }

        private void RepositionBackground()
        {
            Vector2 groundOffset = new Vector2(_groundHorizontalLength * 2f, 0);
            var transform1 = transform;
            transform1.position = (Vector2) transform1.position + groundOffset;
        }
    }
}