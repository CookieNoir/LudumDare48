using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCameraAnimation : MonoBehaviour
{
    public FollowingCamera followingCamera;
    public Camera boatCamera;
    public Transform startTarget;
    public Transform nextTarget;
    public float timeStay = 3f;
    public float time = 5f;
    public float orthographicSizeMaxAddition = 3f;
    private IEnumerator cameraSmoothMovement;
    private float frequencyStay;
    private float frequency;

    private void Awake()
    {
        frequencyStay = 1f / timeStay;
        frequency = 1f / time;
        cameraSmoothMovement = CameraSmoothMovement();
    }

    public void StartSmoothMovement()
    {
        followingCamera.enabled = false;
        followingCamera.transform.position = startTarget.position;
        StopCoroutine(cameraSmoothMovement);
        cameraSmoothMovement = CameraSmoothMovement();
        StartCoroutine(cameraSmoothMovement);
    }

    private IEnumerator CameraSmoothMovement()
    {
        float baseSize = boatCamera.orthographicSize;
        float f = 1f;
        while (f > 0f)
        {
            yield return null;
            f -= Time.deltaTime * frequencyStay;
        }
        f = 0f;
        while (f < 1f)
        {
            yield return null;
            f += Time.deltaTime * frequency;
            followingCamera.transform.position = Vector3.Lerp(startTarget.position, nextTarget.position, Helpers.SmoothStep(f));
            boatCamera.orthographicSize = baseSize + Helpers.JumpFunction(f) * orthographicSizeMaxAddition;
        }
        boatCamera.orthographicSize = baseSize;
        followingCamera.enabled = true;
    }
}