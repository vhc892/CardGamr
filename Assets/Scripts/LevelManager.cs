using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dreamteck.Splines;

public class LevelManager : MonoBehaviour
{
    public GameObject[] level;
    public SplineFollower playerFollower;
    public ImageGroup[] imageGroups;
    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;
    public GameObject nextLevelButton;
    public GameObject loadCardPanel;
    public Movement movement;
    public static LevelManager Instance;
    public ImageUnlocker imageUnlocker;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadLevel(currentLevelIndex);
        imageUnlocker.Initialize(imageGroups.Length, 4);
    }

    public void WinLevel()  //ActiveNextLevelButton
    {
        nextLevelButton.SetActive(true);
        int unlockedCount = PlayerScore.Instance.GetRarity();
        imageUnlocker.UpdateUnlockedImages(imageGroups[currentLevelIndex], currentLevelIndex, unlockedCount);

        
        //UnlockImagesBasedOnTexture();
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
    /*private void UnlockImagesBasedOnTexture()
    {
        Texture currentTexture = PlayerScore.Instance.targetRawImage.texture;

        if (currentTexture == PlayerScore.Instance.normalTexture)
        {
            // Không mở khóa gì vì đang ở texture normal
        }
        else if (currentTexture == PlayerScore.Instance.rareTexture)
        {
            collectionGameObjects[0].GetComponent<Collection>().unlocked = true;
            collectionGameObjects[1].GetComponent<Collection>().unlocked = true;

        }
        else if (currentTexture == PlayerScore.Instance.srareTexture)
        {
            collectionGameObjects[0].GetComponent<Collection>().unlocked = true;
            collectionGameObjects[1].GetComponent<Collection>().unlocked = true;
            collectionGameObjects[2].GetComponent<Collection>().unlocked = true;
        }
        else if (currentTexture == PlayerScore.Instance.epicTexture)
        {
            collectionGameObjects[0].GetComponent<Collection>().unlocked = true;
            collectionGameObjects[1].GetComponent<Collection>().unlocked = true;
            collectionGameObjects[2].GetComponent<Collection>().unlocked = true;
            collectionGameObjects[3].GetComponent<Collection>().unlocked = true;
        }

        foreach (GameObject obj in collectionGameObjects)
        {
            obj.GetComponent<Collection>().UpdateImage();
        }
    }*/


}
