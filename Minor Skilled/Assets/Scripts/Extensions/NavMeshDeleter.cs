using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshDeleter
{
    [MenuItem("AI/NavMesh/Force Cleanup NavMesh")]
    public static void ForceCleanupNavMesh()
    {
        if (Application.isPlaying)
            return;
 
        NavMesh.RemoveAllNavMeshData();
    }
}