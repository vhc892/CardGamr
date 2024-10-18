﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dreamteck.Splines;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
    public Transform playerTransform;
    public SplineFollower splineFollower;
    public float followSpeed;
    public float swerveSpeed;
    public float swerveRange;
    public float rotationSpeed = 5f;
    public float maxRotation = 30f;

    public bool isMoving;
    private float lastPositionX;
    private float moveFactorX;

    public GraphicRaycaster canvas2Raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    public TextMeshProUGUI pressToStartText;
    private void Start()
    {
        splineFollower.follow = false;
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (IsPointerOverCanvas2())
        {
            return;
        }

        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pressToStartText.gameObject.SetActive(false);
                StartMoving();
                isMoving = true;
                lastPositionX = Input.mousePosition.x;
                Debug.Log("Click1");
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPositionX = Input.mousePosition.x;
                Debug.Log(isMoving);
            }
            if (Input.GetMouseButton(0))
            {
                moveFactorX = Input.mousePosition.x - lastPositionX;
                SwerveMovement();
                UpdateRotation();
                lastPositionX = Input.mousePosition.x;
            }
            if (Input.GetMouseButtonUp(0))
            {
                moveFactorX = 0f;
            }
        }
    }

    private bool IsPointerOverCanvas2()
    {
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        canvas2Raycaster.Raycast(pointerEventData, results);

        return results.Count > 0;
    }

    private void SwerveMovement()
    {
        var playerPos = playerTransform.localPosition;
        var swerveValue = (moveFactorX * swerveSpeed) * Time.deltaTime;
        var targetPosX = playerPos.x + swerveValue;
        targetPosX = Mathf.Clamp(targetPosX, -swerveRange, swerveRange);
        var newPos = new Vector3(targetPosX, playerPos.y, playerPos.z);
        playerTransform.localPosition = newPos;
    }

    private void UpdateRotation()
    {
        float normalizedX = playerTransform.localPosition.x / swerveRange;
        float targetZRotation = -normalizedX * maxRotation;

        playerTransform.localRotation = Quaternion.Euler(0, 0, targetZRotation);
    }

    private void StartMoving()
    {
        Debug.Log("Starting movement...");
        splineFollower.follow = true;
        splineFollower.followSpeed = followSpeed;
    }
}