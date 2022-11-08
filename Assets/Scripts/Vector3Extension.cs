using UnityEngine;

public static class Vector3Extension{
    public static Vector3 Copy(this Vector3 toCopy){
        return new Vector3(toCopy.x, toCopy.y, toCopy.z);
    }
}