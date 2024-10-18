using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public RawImage targetRawImage;
    private Texture normalTexture;
    private Texture rareTexture;
    private Texture srareTexture;
    private Texture epicTexture;
    public static PlayerScore Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        targetRawImage.texture = normalTexture;
    }

    public void SetTextures(Texture normal, Texture rare, Texture srare, Texture epic)
    {
        normalTexture = normal;
        rareTexture = rare;
        srareTexture = srare;
        epicTexture = epic;
        UpdateScoreAndImage();
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreAndImage();
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        UpdateScoreAndImage();
    }

    public void UpdateScoreAndImage()
    {
        if (score >= 10 && score < 20)
        {
            targetRawImage.texture = rareTexture;
        }
        else if (score >= 20 && score < 30)
        {
            targetRawImage.texture = srareTexture;
        }
        else if (score >= 30)
        {
            targetRawImage.texture = epicTexture;
        }
        else if (score < 10)
        {
            targetRawImage.texture = normalTexture;
        }
    }
    public int GetRarity()
    {
        if (score >= 30)
        {
            return 3;  // Epic
        }
        else if (score >= 20)
        {
            return 2;  // Srare
        }
        else if (score >= 10)
        {
            return 1;  // Rare
        }
        else
        {
            return 0;  // Normal
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamond"))
        {
            AddScore(5);
            Destroy(other.gameObject);
        }
    }
}
