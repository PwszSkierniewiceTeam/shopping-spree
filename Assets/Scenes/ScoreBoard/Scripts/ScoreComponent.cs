using UnityEngine;

namespace Scenes.ScoreBoard.Scripts
{
    public class ScoreComponent : MonoBehaviour
    {
        public GameObject goldScorePrefab;
        public GameObject goldScoreTopPrefab;
        public GameObject silverScorePrefab;
        public GameObject silverScoreTopPrefab;
        public GameObject penaltyPrefab;
        public GameObject prizePrefab;

        private GameObject[] _scoreElements;

        public void ShowScore(int currentScore, int maxScore, Vector2 position, Transform trans)
        {
            _scoreElements = new GameObject[maxScore];
            Vector2 pos = position;

            for (int i = 1; i <= maxScore; i++)
            {
                pos = i == 1 ? pos : new Vector2(pos.x, pos.y + (i == maxScore ? 0.23f : 0.15f));
                _scoreElements[i - 1] =
                    Instantiate(GetScoreElement(i, currentScore, maxScore), pos, Quaternion.identity, trans);
            }
        }

        public void ShowPenalty(bool isWinner, Vector2 position, int minPenalty, int maxPenalty, Transform trans)
        {
            if (isWinner)
            {
                Instantiate(prizePrefab, position, Quaternion.identity, trans);
                return;
            }

            int penalty = Random.Range(minPenalty, maxPenalty);
            Vector2 pos = position;
            float width = penaltyPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            float gap = 0.15f;

            if (penalty > 1)
            {
                pos = new Vector2(pos.x - (width + gap) * (penalty - 1) / 2, pos.y);
            }

            for (int i = 1; i <= penalty; i++)
            {
                Instantiate(penaltyPrefab, pos, Quaternion.identity, trans);
                pos = new Vector2(pos.x + (width + gap), pos.y);
            }
        }

        private GameObject GetScoreElement(int currentElementIndex, int currentScore, int maxScore)
        {
            if (currentElementIndex == maxScore)
            {
                return currentScore == maxScore ? goldScoreTopPrefab : silverScoreTopPrefab;
            }

            return currentScore >= currentElementIndex ? goldScorePrefab : silverScorePrefab;
        }
    }
}