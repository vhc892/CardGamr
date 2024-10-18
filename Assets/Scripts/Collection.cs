using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public bool unlocked;

    public Image lockedPanel;

    private void Update()
    {
        UpdateImage();
    }
    public void UpdateImage()
    {
        if (unlocked)
        {
            lockedPanel.gameObject.SetActive(false);
        }
    }
}
