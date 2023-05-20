using System;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts
{
    [Serializable]
    public class CustomKeyValue
    {
        [field: SerializeField] public string Name { set; get;}
        [field: SerializeField] public float Value { set; get;}
    }
}