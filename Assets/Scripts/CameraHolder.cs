using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform playerTranform;
    private Vector3 initialRotation;

    private void Awake()
    {
        initialRotation = transform.eulerAngles;
    }
    private void Update()
    {
        transform.position = new Vector3(playerTranform.position.x, playerTranform.position.y, playerTranform.position.z);
        transform.eulerAngles = new Vector3(playerTranform.eulerAngles.x + initialRotation.x, playerTranform.eulerAngles.y + initialRotation.y, 0);
    }
}
