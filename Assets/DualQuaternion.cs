using UnityEngine;

[System.Serializable]
public struct DualQuaternion
{
    public Quaternion r;   // Real part
    public Quaternion d;   // Dual part

    public DualQuaternion(Quaternion real, Quaternion dual)
    {   
        r = real;
        d = dual;
    }

    private static Quaternion MultiplyScalar(Quaternion q, float s)
    {
        return new Quaternion(q.x * s, q.y * s, q.z * s, q.w * s);
    }

    private static Quaternion DivideScalar(Quaternion q, float s)
    {
        return new Quaternion(q.x / s, q.y / s, q.z / s, q.w / s);
    }

    // ---------------------------------------------------------
    // 1. FromPose
    // Pose (rotation + translation) -> Dual Quaternion
    // ---------------------------------------------------------

    public static DualQuaternion FromPose(Quaternion rotation,Vector3 translation)
    {
        // Make sure rotation is a unit quaternion
        rotation = rotation.normalized;

        // Translation represented as a pure quaternion
        Quaternion t =
            new Quaternion(
                translation.x,
                translation.y,
                translation.z,
                0f);

        // qd = 1/2 * t * qr
        Quaternion d = MultiplyScalar(t, 0.5f) * rotation;
        return new DualQuaternion(rotation, d);
    }


    // ---------------------------------------------------------
    // 2. ToPose
    // Dual Quaternion -> Pose (rotation + translation)
    // ---------------------------------------------------------

    public void ToPose(out Quaternion rotation,out Vector3 translation)
    {
        // Real part gives the rotation
        rotation = r.normalized;

        // Conjugate / inverse of unit quaternion
        Quaternion rConjugate = Quaternion.Inverse(rotation);

        // tq = 2 * qd * qr^-1
        Quaternion t = MultiplyScalar(d * rConjugate, 2f);

        // Extract vector part
        translation =
            new Vector3(
                t.x,
                t.y,
                t.z);
    }


    // ---------------------------------------------------------
    // 3. Multiply
    // Combine two rigid transformations
    // ---------------------------------------------------------

    public static DualQuaternion Multiply(DualQuaternion a,DualQuaternion b)
    {
        // qr3 = qr1 * qr2
        Quaternion real = a.r * b.r;

        // qd3 = qr1*qd2 + qd1*qr2
        Quaternion dualPart1 = a.r * b.d;
        Quaternion dualPart2 = a.d * b.r;
        Quaternion dual = new Quaternion(
            dualPart1.x + dualPart2.x,
            dualPart1.y + dualPart2.y,
            dualPart1.z + dualPart2.z,
            dualPart1.w + dualPart2.w);

        return new DualQuaternion(real, dual);
    }


    // ---------------------------------------------------------
    // 4. Slerp
    // Interpolate between two dual quaternion poses
    // ---------------------------------------------------------

    public static DualQuaternion Slerp(DualQuaternion a,DualQuaternion b,float u)
    {
        // // Keep u between 0 and 1
        // u = Mathf.Clamp01(u);

        // // Make sure we take the shortest rotation path
        // if (Quaternion.Dot(a.r, b.r) < 0f)
        // {
        //     b.r = new Quaternion(
        //         -b.r.x,
        //         -b.r.y,
        //         -b.r.z,
        //         -b.r.w);

        //     b.d = new Quaternion(
        //         -b.d.x,
        //         -b.d.y,
        //         -b.d.z,
        //         -b.d.w);
        // }

        // // Normalized linear interpolation of real part
        // Quaternion real = Quaternion.Lerp(a.r, b.r, u).normalized;

        // // Interpolate dual part consistently
        // Quaternion dual =  Quaternion.Lerp(a.d, b.d, u);

        // DualQuaternion result = new DualQuaternion(real, dual);

        // return result.Normalized();
        u = Mathf.Clamp01(u);

        // Extract poses
        a.ToPose(out Quaternion rotA, out Vector3 posA);
        b.ToPose(out Quaternion rotB, out Vector3 posB);

        // Interpolate rotation
        Quaternion rotation = Quaternion.Lerp(rotA, rotB, u).normalized;

        // Interpolate position
        Vector3 position = Vector3.Lerp(posA, posB, u);

        // Convert interpolated pose back to dual quaternion
        return FromPose(rotation, position);
    }


    // ---------------------------------------------------------
    // Helper: Normalize dual quaternion
    // ---------------------------------------------------------

    public DualQuaternion Normalized()
    {
        float magnitude = Mathf.Sqrt(
            r.x * r.x +
            r.y * r.y +
            r.z * r.z +
            r.w * r.w);

        if (magnitude < 1e-6f)
        {
            return new DualQuaternion(
                Quaternion.identity,
                new Quaternion(0f, 0f, 0f, 0f));
        }

        Quaternion real =
            DivideScalar(r, magnitude);

        float dot =
            Quaternion.Dot(real, d);

        Quaternion projectedDual = MultiplyScalar(real, dot);
        Quaternion dual =
            DivideScalar(
                new Quaternion(
                    d.x - projectedDual.x,
                    d.y - projectedDual.y,
                    d.z - projectedDual.z,
                    d.w - projectedDual.w),
                magnitude);

        return new DualQuaternion(real, dual);
    }
}