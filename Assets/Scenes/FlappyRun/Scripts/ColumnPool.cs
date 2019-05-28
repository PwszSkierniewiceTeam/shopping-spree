using UnityEngine;

namespace Scenes.FlappyRun.Scripts
{
    public class ColumnPool : MonoBehaviour
    {
        public int columnPoolSize = 5;
        public GameObject columnPrefab;
        public float spawnRate = 4f;
        public float firstSpawn = 1f;
        public float columnMin = -3f;
        public float columnMax = 3f;

        private GameObject[] _columns;
        private readonly Vector2 _objectPoolPosition = new Vector2(-15f, -25f);
        private float _timeSinceLastSpawned;
        private float spawnXPosition = 10f;
        private int _currentColumn;
        private bool _firstSpawned;

        // Start is called before the first frame update
        void Start()
        {
            _columns = new GameObject[columnPoolSize];
            for (int i = 0; i < columnPoolSize; i++)
            {
                _columns[i] = Instantiate(columnPrefab, _objectPoolPosition, Quaternion.identity);
            }
        }

        // Update is called once per frame
        void Update()
        {
            _timeSinceLastSpawned += Time.deltaTime;

            if (GameController.instance.gameOver == false
                && _timeSinceLastSpawned >= spawnRate
                || (_firstSpawned == false && _timeSinceLastSpawned >= firstSpawn))
            {
                _firstSpawned = true;
                _timeSinceLastSpawned = 0;
                float spawnYPosition = Random.Range(columnMin, columnMax);
                _columns[_currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);
                _currentColumn++;
                if (_currentColumn >= columnPoolSize)
                {
                    _currentColumn = 0;
                }
            }
        }
    }
}