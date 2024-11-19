using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Image rewardOverlay;
    private int levelCompletedCount = 0;
    private float overlayHeight; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        overlayHeight = rewardOverlay.rectTransform.sizeDelta.y;
    }

    public void LevelCompleted()
    {
        levelCompletedCount++;
        UpdateRewardOverlay();
        if (levelCompletedCount >= 3)
        {
            UnlockRewardImage();
        }
    }

    private void UpdateRewardOverlay()
    {
        float newHeight = overlayHeight * (1 - (levelCompletedCount / 3.0f));
        rewardOverlay.rectTransform.sizeDelta = new Vector2(rewardOverlay.rectTransform.sizeDelta.x, newHeight);
    }

    private void UnlockRewardImage()
    {
        rewardOverlay.gameObject.SetActive(false);
    }
}
