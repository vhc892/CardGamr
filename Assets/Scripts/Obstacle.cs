using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int scoreChange = -10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScore playerScore = other.GetComponent<PlayerScore>();

            if (playerScore != null)
            {
                if (scoreChange < 0)
                    playerScore.SubtractScore(Mathf.Abs(scoreChange));
                else
                    playerScore.AddScore(scoreChange);
            }
        }
    }
}
