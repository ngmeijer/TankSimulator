using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Clamp(Vector3 targetVec, Vector3 min, Vector3 max)
    {
        float x = Mathf.Clamp(targetVec.x, min.x, max.x);
        float y = Mathf.Clamp(targetVec.y, min.y, max.y);
        float z = Mathf.Clamp(targetVec.z, min.z, max.z);

        return new Vector3(x,y,z);
    }
}
