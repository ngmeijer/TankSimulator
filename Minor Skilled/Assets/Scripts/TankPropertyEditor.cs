// using System;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(MovementManager)), ]
// public class TankPropertyEditor : Editor
// {
//     private SerializedProperty _mass;
//     private SerializedProperty _dampingRate;
//     private SerializedProperty _suspensionSpring;
//     private SerializedProperty _suspensionDamper;
//
//     public float ForwardExtrenumSlip = 1;
//     public float ForwardExtrenumValue = 1;
//     public float ForwardAsymptoteSlip = 1;
//     public float ForwardAsymptoteValue = 1;
//     public float ForwardStiffness = 20;
//
//     private void OnEnable()
//     {
//         _mass = serializedObject.FindProperty("_mass");
//         _dampingRate = serializedObject.FindProperty("_dampingRate");
//         _suspensionSpring = serializedObject.FindProperty("_suspensionSpring");
//         _suspensionDamper = serializedObject.FindProperty("_suspensionDamper");
//     }
//
//     
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();
//         
//         GUILayout.Space(20);
//         EditorGUILayout.PropertyField(_mass, new GUIContent("Mass"));
//         EditorGUILayout.PropertyField(_dampingRate, new GUIContent("Damping Rate"));
//         EditorGUILayout.PropertyField(_suspensionSpring, new GUIContent("Suspension Spring"));
//         EditorGUILayout.PropertyField(_suspensionDamper, new GUIContent("Suspension Damper"));
//         
//         GUILayout.Space(10);
//         // _valueContainer.ForwardExtrenumSlip = EditorGUILayout.FloatField("Extrenum slip:", _valueContainer.ForwardExtrenumSlip);
//         // _valueContainer.ForwardExtrenumValue = EditorGUILayout.FloatField("Extrenum value:", _valueContainer.ForwardExtrenumValue);
//         // _valueContainer.ForwardAsymptoteSlip = EditorGUILayout.FloatField("Asymptote slip:", _valueContainer.ForwardAsymptoteSlip);
//         // _valueContainer.ForwardAsymptoteValue = EditorGUILayout.FloatField("Asymptote value:", _valueContainer.ForwardAsymptoteValue);
//         // _valueContainer.ForwardStiffness = EditorGUILayout.FloatField("Stiffness:", _valueContainer.ForwardStiffness);
//         
//         serializedObject.ApplyModifiedProperties();
//     }
// }
//
// public class TankEditorValues
// {
//     public float Mass = 5f;
//     public float DampingRate  = 5f;
//
//     public float SuspensionSpring = 1;
//     public float SuspensionDamper = 50;
//
//     public float ForwardExtrenumSlip = 1;
//     public float ForwardExtrenumValue = 1;
//     public float ForwardAsymptoteSlip = 1;
//     public float ForwardAsymptoteValue = 1;
//     public float ForwardStiffness = 20;
// }