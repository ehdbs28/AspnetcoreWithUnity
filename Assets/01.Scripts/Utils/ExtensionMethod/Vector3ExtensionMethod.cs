using UnityEngine;

public static class Vector3ExtensionMethod
{
    public static System.Numerics.Vector3 ToSystemVector(this Vector3 vector)
    {
        return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
    }

    public static Vector3 ToUnityVector(this System.Numerics.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}