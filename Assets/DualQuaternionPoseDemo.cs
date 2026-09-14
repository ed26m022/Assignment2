using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DualQuaternionPoseDemo : MonoBehaviour
{
    public Transform poseA;
    public Transform poseB;

    [Range(0f, 1f)]
    public float u = 0.5f;
    void Start()
    {
        u = 0.5f;
    }
    void Update()
    {
        // Convert Pose A to dual quaternion
        Debug.Log("u = " + u);
        DualQuaternion dqA =
            DualQuaternion.FromPose(
                poseA.rotation,
                poseA.position);

        // Convert Pose B to dual quaternion
        DualQuaternion dqB =
            DualQuaternion.FromPose(
                poseB.rotation,
                poseB.position);

        // Interpolate
        DualQuaternion dq =
            DualQuaternion.Slerp(
                dqA,
                dqB,
                u);

        // Convert back to rotation + position
        dq.ToPose(
            out Quaternion rotation,
            out Vector3 position);

        // Apply to this GameObject
        transform.rotation = rotation;
        transform.position = position;
    }
}