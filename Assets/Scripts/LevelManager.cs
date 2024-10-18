using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Dreamteck.Splines;

public class LevelManager : MonoBehaviour
{
    public GameObject[] level;
    public SplineFollower playerFollower;
    public ImageGroup[] imageGroups;
    private int currentLevelIndex=0;
    private GameObject currentLevelInstance;
    public GameObject nextLevelButton;
    public GameObject loadCardPanel;
    public Movement movement;
    public static LevelManager Instance;
    public ImageUnlocker imageUnlocker;

    private const string CurrentLevelKey = "CurrentLevel";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        currentLevelIndex = LoadCurrentLevel();
        LoadLevel(currentLevelIndex);
        imageUnlocker.Initialize(imageGroups.Length, 4);
        UIManager.Instance.UpdateLevelText(currentLevelIndex);
    }

    public void WinLevel()  //ActiveNextLevelButton
    {
        nextLevelButton.SetActive(true);
        int unlockedCount = PlayerScore.Instance.GetRarity();
        imageUnlocker.UpdateUnlockedImages(imageGroups[currentLevelIndex], currentLevelIndex, unlockedCount);

    }

    public void TurnOnCardPanel()
    {
        loadCardPanel.SetActive(true);
    }

    public void TurnOffCardPanel()
    {
        loadCardPanel.SetActive(false);
    }

    public void LoadNextLevel()
    {
        nextLevelButton.SetActive(false);

        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        currentLevelIndex++;
        if (currentLevelIndex < level.Length)
        {
            LoadLevel(currentLevelIndex);
            UIManager.Instance.UpdateLevelText(currentLevelIndex);
            SaveCurrentLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("Game Completed!");
        }
    }

    private void LoadLevel(int index)
    {
        ImageGroup group = imageGroups[index];
        PlayerScore.Instance.SetTextures(group.images[0].texture, group.images[1].texture, group.images[2].texture, group.images[3].texture);
        currentLevelInstance = Instantiate(level[index], Vector3.zero, Quaternion.identity);
        SplineComputer spline = currentLevelInstance.GetComponentInChildren<SplineComputer>();

        if (spline != null)
        {
            playerFollower.spline = spline;
            playerFollower.Restart(0.0);
            playerFollower.follow = false;
            movement.isMoving = false;
            movement.pressToStartText.gameObject.SetActive(true);
            PlayerScore.Instance.ResetScore();
            PlayerScore.Instance.UpdateScoreAndImage();
        }
        else
        {
            Debug.LogError("Spline not found in the loaded level!");
        }
    }

    // Hàm lưu level hiện tại vào PlayerPrefs
    private void SaveCurrentLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(CurrentLevelKey, levelIndex);
        PlayerPrefs.Save();
        Debug.Log("Current level saved: " + levelIndex);
    }

    // Hàm tải level hiện tại từ PlayerPrefs
    private int LoadCurrentLevel()
    {
        return PlayerPrefs.GetInt(CurrentLevelKey, 0);
    }
}
