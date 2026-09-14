using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardInterpolationDemo : MonoBehaviour
{
    public Transform poseA;
    public Transform poseB;

    [Range(0f, 1f)]
    public float u = 0.5f;

    void Update()
    {
        // Interpolate position
        Vector3 position =
            Vector3.Lerp(
                poseA.position,
                poseB.position,
                u
            );

        // Interpolate rotation
        Quaternion rotation =
            Quaternion.Slerp(
                poseA.rotation,
                poseB.rotation,
                u
            );

        // Apply the interpolated pose
        transform.position = position;
        transform.rotation = rotation;
    }
}