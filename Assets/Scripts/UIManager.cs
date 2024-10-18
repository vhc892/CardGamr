using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    public static UIManager Instance;
    public GameObject loadCardPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLevelText(int levelIndex)
    {
        levelText.text = "Level " + (levelIndex + 1).ToString();
    }
}
