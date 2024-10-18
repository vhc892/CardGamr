using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ImageUnlocker : MonoBehaviour
{
    public GameObject[] levelContainers;
    public Button nextButton;
    public Button unlockRandomButton;
    public TextMeshProUGUI[] levelTexts;
    private RawImage[][] rawImages;
    private Image[][] lockImages;
    private bool[][] unlockedImages;

    private int currentSetIndex = 0;
    private const string UnlockedImagesKeyPrefix = "UnlockedImages_Level_";

    public void Initialize(int totalLevels, int imagesPerLevel)
    {
        int numberOfContainers = levelContainers.Length;
        rawImages = new RawImage[totalLevels][];
        lockImages = new Image[totalLevels][];
        unlockedImages = new bool[totalLevels][];

        for (int i = 0; i < totalLevels; i++)
        {
            rawImages[i] = new RawImage[imagesPerLevel];
            lockImages[i] = new Image[imagesPerLevel];
            unlockedImages[i] = LoadUnlockedImages(i, imagesPerLevel);

            Transform container = levelContainers[i % numberOfContainers].transform;
            for (int j = 0; j < imagesPerLevel; j++)
            {
                GameObject pic = container.GetChild(j).gameObject;
                rawImages[i][j] = pic.transform.Find("RawImage").GetComponent<RawImage>();
                lockImages[i][j] = pic.transform.Find("LockImage").GetComponent<Image>();
            }
        }

        nextButton.onClick.AddListener(ShowNextSet);
        unlockRandomButton.onClick.AddListener(UnlockRandomImages);
        UpdateContainers();
    }

    private void ShowNextSet()
    {
        currentSetIndex++;
        if (currentSetIndex * levelContainers.Length >= rawImages.Length)
        {
            currentSetIndex = 0;
        }
        UpdateContainers();
    }

    private void UpdateContainers()
    {
        int startLevelIndex = currentSetIndex * levelContainers.Length;
        for (int i = 0; i < levelContainers.Length; i++)
        {
            int levelIndex = startLevelIndex + i;
            ResetContainer(levelContainers[i]);
            if (levelIndex < rawImages.Length)
            {
                UpdateImagesForContainer(levelContainers[i], levelIndex);
                levelTexts[i].text = "Level " + (levelIndex + 1).ToString();
                levelContainers[i].SetActive(true);
            }
            else
            {
                levelContainers[i].SetActive(false);
            }
        }
    }

    private void UpdateImagesForContainer(GameObject container, int levelIndex)
    {
        ImageGroup group = GetImageGroup(levelIndex);
        Transform containerTransform = container.transform;
        for (int i = 0; i < group.images.Length; i++)
        {
            rawImages[levelIndex][i].texture = group.images[i].texture;
            lockImages[levelIndex][i].gameObject.SetActive(!unlockedImages[levelIndex][i]);
        }
    }

    private ImageGroup GetImageGroup(int levelIndex)
    {
        return LevelManager.Instance.imageGroups[levelIndex];
    }

    private void ResetContainer(GameObject container)
    {
        Transform containerTransform = container.transform;
        for (int i = 0; i < containerTransform.childCount; i++)
        {
            GameObject pic = containerTransform.GetChild(i).gameObject;
            RawImage rawImage = pic.transform.Find("RawImage").GetComponent<RawImage>();
            Image lockImage = pic.transform.Find("LockImage").GetComponent<Image>();

            rawImage.texture = null;
            lockImage.gameObject.SetActive(true);
        }
    }

    public void UpdateUnlockedImages(ImageGroup group, int levelIndex, int unlockedCount)
    {
        for (int i = 0; i < group.images.Length; i++)
        {
            if (unlockedImages[levelIndex][i])
            {
                lockImages[levelIndex][i].gameObject.SetActive(false);
            }
            else if (i <= unlockedCount)
            {
                rawImages[levelIndex][i].texture = group.images[i].texture;
                lockImages[levelIndex][i].gameObject.SetActive(false);
                unlockedImages[levelIndex][i] = true;
            }
            else
            {
                lockImages[levelIndex][i].gameObject.SetActive(true);
            }
        }
        SaveUnlockedImages(levelIndex, unlockedImages[levelIndex]);
    }

    private void SaveUnlockedImages(int levelIndex, bool[] unlockedImages)
    {
        string key = UnlockedImagesKeyPrefix + levelIndex;
        string unlockedString = string.Join("", unlockedImages.Select(b => b ? "1" : "0"));
        PlayerPrefs.SetString(key, unlockedString);
        PlayerPrefs.Save();
    }

    private bool[] LoadUnlockedImages(int levelIndex, int imagesPerLevel)
    {
        string key = UnlockedImagesKeyPrefix + levelIndex;
        string unlockedString = PlayerPrefs.GetString(key, new string('0', imagesPerLevel));

        bool[] unlockedImages = new bool[imagesPerLevel];
        for (int i = 0; i < imagesPerLevel; i++)
        {
            unlockedImages[i] = unlockedString[i] == '1';
        }
        return unlockedImages;
    }

    private void UnlockRandomImages()
    {
        List<(int levelIndex, int imageIndex)> lockedImages = new List<(int, int)>();

        for (int levelIndex = 0; levelIndex < unlockedImages.Length; levelIndex++)
        {
            for (int imageIndex = 0; imageIndex < unlockedImages[levelIndex].Length; imageIndex++)
            {
                if (!unlockedImages[levelIndex][imageIndex])
                {
                    lockedImages.Add((levelIndex, imageIndex));
                }
            }
        }

        int unlockCount = Mathf.Min(3, lockedImages.Count);
        for (int i = 0; i < unlockCount; i++)
        {
            int randomIndex = Random.Range(0, lockedImages.Count);
            var (levelIndex, imageIndex) = lockedImages[randomIndex];

            unlockedImages[levelIndex][imageIndex] = true;
            rawImages[levelIndex][imageIndex].texture = GetImageGroup(levelIndex).images[imageIndex].texture;
            lockImages[levelIndex][imageIndex].gameObject.SetActive(false);

            SaveUnlockedImages(levelIndex, unlockedImages[levelIndex]);

            
            lockedImages.RemoveAt(randomIndex);
        }
        UpdateContainers();
    }
}
