using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
    public GameObject goldScorePrefab;
    public GameObject goldScoreTopPrefab;
    public GameObject silverScorePrefab;
    public GameObject silverScoreTopPrefab;

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

    private GameObject GetScoreElement(int currentElementIndex, int currentScore, int maxScore)
    {
        if (currentElementIndex == maxScore)
        {
            return currentScore == maxScore ? goldScoreTopPrefab : silverScoreTopPrefab;
        }

        return currentScore >= currentElementIndex ? goldScorePrefab : silverScorePrefab;
    }
}