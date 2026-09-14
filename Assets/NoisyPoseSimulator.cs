using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoisyPoseSimulator : MonoBehaviour
{
    public Transform poseA;
    public Transform poseB;

    public float positionNoise = 0.1f;
    public float rotationNoise = 3f;

    private Vector3 originalPositionA;
    private Vector3 originalPositionB;

    private Quaternion originalRotationA;
    private Quaternion originalRotationB;

    void Start()
    {
        originalPositionA = poseA.position;
        originalPositionB = poseB.position;

        originalRotationA = poseA.rotation;
        originalRotationB = poseB.rotation;
    }

    void Update()
    {
        poseA.position = originalPositionA +
            Random.insideUnitSphere * positionNoise;

        poseB.position = originalPositionB +
            Random.insideUnitSphere * positionNoise;

        poseA.rotation =
            originalRotationA *
            Quaternion.Euler(
                Random.Range(-rotationNoise, rotationNoise),
                Random.Range(-rotationNoise, rotationNoise),
                Random.Range(-rotationNoise, rotationNoise)
            );

        poseB.rotation =
            originalRotationB *
            Quaternion.Euler(
                Random.Range(-rotationNoise, rotationNoise),
                Random.Range(-rotationNoise, rotationNoise),
                Random.Range(-rotationNoise, rotationNoise)
            );
    }
}
