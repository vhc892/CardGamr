using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void WinGame()
    {
        particle1.Play();
        particle2.Play();
    }
}
