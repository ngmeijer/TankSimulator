using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public static class Indenter
{
    private const string IndentationString = "    ";

    public static string GetIdent(int indentLevel)
    {
        string indentation = new string(' ', indentLevel * IndentationString.Length);
        return indentation;
    }
}

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