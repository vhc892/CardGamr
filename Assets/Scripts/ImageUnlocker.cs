using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageUnlocker : MonoBehaviour
{
    public GameObject[] levelContainers; 
    public Button nextButton;
    public TextMeshProUGUI[] levelTexts;
    private RawImage[][] rawImages;
    private Image[][] lockImages;
    private bool[][] unlockedImages;

    private int currentSetIndex = 0;

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
            unlockedImages[i] = new bool[imagesPerLevel];

            Transform container = levelContainers[i % numberOfContainers].transform;
            for (int j = 0; j < imagesPerLevel; j++)
            {
                GameObject pic = container.GetChild(j).gameObject;
                rawImages[i][j] = pic.transform.Find("RawImage").GetComponent<RawImage>();
                lockImages[i][j] = pic.transform.Find("LockImage").GetComponent<Image>();
                unlockedImages[i][j] = false;
            }
        }

        nextButton.onClick.AddListener(ShowNextSet);
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

    // update image with level index
    private void UpdateImagesForContainer(GameObject container, int levelIndex)
    {
        ImageGroup group = GetImageGroup(levelIndex);  
        Transform containerTransform = container.transform;

        for (int i = 0; i < group.images.Length; i++)
        {
            // Gán texture
            rawImages[levelIndex][i].texture = group.images[i].texture;

            // update lock status
            lockImages[levelIndex][i].gameObject.SetActive(!unlockedImages[levelIndex][i]); 
        }
    }

    // get texture
    private ImageGroup GetImageGroup(int levelIndex)
    {
        return LevelManager.Instance.imageGroups[levelIndex];
    }


    // Reset status 
    private void ResetContainer(GameObject container)
    {
        Transform containerTransform = container.transform;
        for (int i = 0; i < containerTransform.childCount; i++)
        {
            GameObject pic = containerTransform.GetChild(i).gameObject;
            RawImage rawImage = pic.transform.Find("RawImage").GetComponent<RawImage>();
            Image lockImage = pic.transform.Find("LockImage").GetComponent<Image>();

            // Đặt lại texture và hiển thị lớp che phủ
            rawImage.texture = null;
            lockImage.gameObject.SetActive(true);
        }
    }

    public void UpdateUnlockedImages(ImageGroup group, int levelIndex, int unlockedCount)
    {
        for (int i = 0; i < group.images.Length; i++)
        {
            if (i <= unlockedCount)
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
    }
}
