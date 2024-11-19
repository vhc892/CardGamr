using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dreamteck.Splines;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
    public Transform playerTransform;
    public Transform movementTarget;
    public SplineFollower splineFollower;
    public float followSpeed;
    public float swerveSpeed;
    public float swerveRange;
    public float rotationSpeed = 5f;
    public float maxRotation = 30f;

    public bool isMoving;
    private float lastPositionX;
    private float moveFactorX;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;
    private bool isJumping = false;
    private float jumpStartTime;

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
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        if (isJumping)
        {
            PerformJump();
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
        movementTarget.localPosition = newPos;
    }

    private void UpdateRotation()
    {
        float normalizedX = playerTransform.localPosition.x / swerveRange;
        float targetZRotation = -normalizedX * maxRotation;

        playerTransform.localRotation = Quaternion.Euler(0, 0, targetZRotation);
        movementTarget.localRotation = Quaternion.Euler(0, 0, targetZRotation);
    }

    private void StartMoving()
    {
        Debug.Log("Starting movement...");
        splineFollower.follow = true;
        splineFollower.followSpeed = followSpeed;
    }
    private void StartJump()
    {
        isJumping = true;
        jumpStartTime = Time.time;
    }
    private void PerformJump()
    {
        float elapsedTime = Time.time - jumpStartTime;
        if (elapsedTime < jumpDuration)
        {
            float jumpProgress = elapsedTime / jumpDuration;
            float jumpY = Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;
            playerTransform.localPosition = new Vector3(playerTransform.localPosition.x, jumpY + 2.5f, playerTransform.localPosition.z);
        }
        else
        {
            isJumping = false;
            playerTransform.localPosition = new Vector3(playerTransform.localPosition.x, 2.5f, playerTransform.localPosition.z);
        }
    }
}
