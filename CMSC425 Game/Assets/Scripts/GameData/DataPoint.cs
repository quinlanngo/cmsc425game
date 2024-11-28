using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedGameData
{
    public string sceneName; // The name of the scene
    public float[] playerPosition;
    public float[] playerRotation;
    public List<TransformData> inventoryItemsData;
}


[Serializable]
public class TransformData
{
    public float[] position;
    public float[] rotation;
    public float[] scale;
    public string name;
}

// Extension methods for Vector3 and Quaternion serialization
public static class UnitySerializationExtensions
{
    public static float[] ToArray(this Vector3 vector)
    {
        return new float[] { vector.x, vector.y, vector.z };
    }

    public static Vector3 ToVector3(this float[] array)
    {
        return new Vector3(array[0], array[1], array[2]);
    }

    public static float[] ToArray(this Quaternion quaternion)
    {
        return new float[] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
    }

    public static Quaternion ToQuaternion(this float[] array)
    {
        return new Quaternion(array[0], array[1], array[2], array[3]);
    }
}
