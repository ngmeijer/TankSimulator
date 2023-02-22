using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class TankPropertyEditor : EditorWindow
{
    private const int NO_TANK_SELECTED = -1;
    
    bool wheelsGroupEnabled;

    private string tankName;
    private float acceleration;
    private float singleTrackAcceleration;
    private float maxSpeed;
    private float rigidbodyMass;
    private float wheelMass;
    private float wheelRadius;
    private float wheelDampingRate;
    private float wheelSuspDist;
    private float wheelForceAppDist;
    private Vector3 wheelCenter;
    private bool suspSpringFoldout;
    private float suspSpring;
    private float suspDamper;
    private float suspTargetPos;

    private float forwFricExtrenSlip;
    private float forwFricExtrenVal;
    private float forwFricAsymSlip;
    private float forwFricAsymVal;
    private float forwFricStiff;
    
    private float sideFricExtrenSlip;
    private float sideFricExtrenVal;
    private float sideFricAsymSlip;
    private float sideFricAsymVal;
    private float sideFricStiff;
    
    private List<WheelCollider> leftTrackWheels = new List<WheelCollider>();
    private List<WheelCollider> rightTrackWheels = new List<WheelCollider>();

    private TankComponentManager _componentManager;

    private TankComponentManager[] _foundTanks = new TankComponentManager[] { };
    private int _currentlySelectedTankIndex = -1;
    private TankComponentManager _selectedTank;
    private TankProperties _retrievedPropertyData;
    private TankProperties _newPropertyData;

    private GUIStyle _header1Style;
    private GUIStyle _header2Style;
    private GUIStyle _cursiveStyle;
    private GUIStyle _buttonStyle;
    private bool forwFricFoldout;
    private bool sideFricFoldout;

    private WheelCollider tankWheelProps;
    private Vector2 scrollPos;

    [MenuItem("Custom Editors/Tank editor")]
    static void Init()
    {
        TankPropertyEditor window = (TankPropertyEditor)EditorWindow.GetWindow(typeof(TankPropertyEditor));
        window.Show();
    }

    private void OnEnable()
    {
        RefreshTanks();
    }

    private void RefreshTanks()
    {
        _foundTanks = FindObjectsOfType<TankComponentManager>();
    }
    

    private void OnGUI()
    {
        InitializeStyles();
        
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
        GUILayout.Label("Tank editor", _header1Style);
        DrawListOfTanks();

        GUILayout.Space(10);
        GUILayout.Label("Select a tank to edit properties (cannot edit tanks in playmode)", _cursiveStyle);
        DrawCurrentlySelectedTank();

        if (_currentlySelectedTankIndex != NO_TANK_SELECTED)
        {
            if (_retrievedPropertyData == null)
                _retrievedPropertyData = _selectedTank.Properties;
            if (GUILayout.Button("Confirm changes", _buttonStyle))
                ConfirmChanges();
        }

        GUILayout.EndScrollView();
    }

    private void DrawListOfTanks()
    {
        GUILayout.Label("Found tanks:", _header2Style);
        for (int index = 0; index < _foundTanks.Length; index++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select", _buttonStyle, GUILayout.MaxWidth(50))){
                SelectTank(index);
            }
            _foundTanks[index] = EditorGUILayout.ObjectField("Tank " + index, _foundTanks[index], 
                typeof(TankComponentManager), false) as TankComponentManager;
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove selection", _buttonStyle, GUILayout.MaxWidth(150)))
        {
            _currentlySelectedTankIndex = NO_TANK_SELECTED;
        }
        
        if (GUILayout.Button("Refresh tanks", _buttonStyle, GUILayout.MaxWidth(150)))
            RefreshTanks();
        
        GUILayout.EndHorizontal();
    }

    private void ConfirmChanges()
    {
        _selectedTank.Properties = _newPropertyData;

        foreach (var wheel in _selectedTank.LeftTrackWheelColliders)
        {
            AssignValuesToWheel(wheel);
        }
        
        foreach (var wheel in _selectedTank.RightTrackWheelColliders)
        {
            AssignValuesToWheel(wheel);
        }
    }

    private void AssignValuesToWheel(WheelCollider wheel)
    {
        wheel.mass = wheelMass;
        wheel.radius = wheelRadius;
        wheel.wheelDampingRate = wheelDampingRate;
        wheel.suspensionDistance = wheelSuspDist;
        wheel.forceAppPointDistance = wheelForceAppDist;
        wheel.center = wheelCenter;

        JointSpring suspSpringValues = new JointSpring
        {
            spring = suspSpring,
            damper = suspDamper,
            targetPosition = suspTargetPos
        };
        wheel.suspensionSpring = suspSpringValues;

        WheelFrictionCurve forwFricValues = new WheelFrictionCurve
        {
            extremumSlip = forwFricExtrenSlip,
            extremumValue = forwFricExtrenVal,
            asymptoteSlip = forwFricAsymSlip,
            asymptoteValue = forwFricAsymVal,
            stiffness = forwFricStiff
        };
        wheel.forwardFriction = forwFricValues;

        WheelFrictionCurve sideFricValues = new WheelFrictionCurve
        {
            extremumSlip = sideFricExtrenSlip,
            extremumValue = sideFricExtrenVal,
            asymptoteSlip = sideFricAsymSlip,
            asymptoteValue = sideFricAsymVal,
            stiffness = sideFricStiff
        };
        wheel.sidewaysFriction = sideFricValues;
    }

    private void SelectTank(int index)
    {
        GUI.FocusControl(null);    
        _currentlySelectedTankIndex = index;
        _selectedTank = _foundTanks[index];
        _newPropertyData = _selectedTank.Properties;
        
        acceleration = _retrievedPropertyData.Acceleration;
        singleTrackAcceleration = _retrievedPropertyData.SingleTrackSpeed;
        maxSpeed = _retrievedPropertyData.MaxSpeed;
        rigidbodyMass = _retrievedPropertyData.TankMass;
        
        //Wheel
        if (_selectedTank.LeftTrackWheelColliders.Count == 0) return;
        tankWheelProps = _selectedTank.LeftTrackWheelColliders[0];
        wheelMass = tankWheelProps.mass;
        wheelRadius = tankWheelProps.radius;
        wheelDampingRate = tankWheelProps.wheelDampingRate;
        wheelSuspDist = tankWheelProps.suspensionDistance;
        wheelForceAppDist = tankWheelProps.forceAppPointDistance;
        wheelCenter = tankWheelProps.center;

        suspSpring = tankWheelProps.suspensionSpring.spring;
        suspDamper = tankWheelProps.suspensionSpring.damper;
        suspTargetPos = tankWheelProps.suspensionSpring.targetPosition;
        
        //Forw fric
        forwFricExtrenSlip = tankWheelProps.forwardFriction.extremumSlip;
        forwFricExtrenVal = tankWheelProps.forwardFriction.extremumValue;
        forwFricAsymSlip = tankWheelProps.forwardFriction.asymptoteSlip;
        forwFricAsymVal = tankWheelProps.forwardFriction.asymptoteValue;
        forwFricStiff = tankWheelProps.forwardFriction.stiffness;
        
        //Side
        sideFricExtrenSlip = tankWheelProps.sidewaysFriction.extremumSlip;
        sideFricExtrenVal = tankWheelProps.sidewaysFriction.extremumValue;
        sideFricAsymSlip = tankWheelProps.sidewaysFriction.asymptoteSlip;
        sideFricAsymVal = tankWheelProps.sidewaysFriction.asymptoteValue;
        sideFricStiff = tankWheelProps.sidewaysFriction.stiffness;
    }

    private void DrawCurrentlySelectedTank()
    {
        if (_currentlySelectedTankIndex != NO_TANK_SELECTED && _selectedTank != null)
        {
            GUILayout.Label("Currently selected tank:", _header2Style);
            
            //Tank name
            tankName = _selectedTank.name;
            tankName = EditorGUILayout.TextField("Scene name", tankName);
            _selectedTank.name = tankName;
            _newPropertyData = _selectedTank.Properties;

            if (_retrievedPropertyData == null)
            {
                GUILayout.Label("Assign properties to tank:", _header2Style);
                _retrievedPropertyData = EditorGUILayout.ObjectField("Properties:",
                    null, typeof(TankProperties), false) as TankProperties;
                return;
            }

            _retrievedPropertyData = EditorGUILayout.ObjectField("Properties:",
                    _retrievedPropertyData, typeof(TankProperties), false) as TankProperties;

            //
            _newPropertyData.Acceleration = EditorGUILayout.Slider("Acceleration", _newPropertyData.Acceleration, 0, 10000);
            _newPropertyData.SingleTrackSpeed = EditorGUILayout.Slider("Single track acceleration", _newPropertyData.SingleTrackSpeed, 0, 10000);
            _newPropertyData.MaxSpeed = EditorGUILayout.Slider("Max speed", _newPropertyData.MaxSpeed, 0, 50);
            _newPropertyData.TankMass = EditorGUILayout.Slider("Tank mass", _newPropertyData.TankMass, 1, 10000);
            
            //
            GUILayout.Space(20);
            DrawWheelColliderProperties();
        }
    }

    private void DrawWheelColliderProperties()
    {
            GUILayout.Label("Wheel collider properties:", _header2Style);
            wheelMass = EditorGUILayout.FloatField("Mass", wheelMass);
            wheelRadius = EditorGUILayout.FloatField("Radius", wheelRadius);
            wheelDampingRate = EditorGUILayout.FloatField("Damping rate", wheelDampingRate);
            wheelSuspDist = EditorGUILayout.FloatField("Suspension distance", wheelSuspDist);
            wheelForceAppDist = EditorGUILayout.FloatField("Force app point distance", wheelForceAppDist);
            wheelCenter = EditorGUILayout.Vector3Field("Center", wheelCenter);
            
            suspSpringFoldout = EditorGUILayout.Foldout(suspSpringFoldout, "Suspension spring");
            if (suspSpringFoldout)
            {
                suspSpring = EditorGUILayout.FloatField("Spring", suspSpring);
                suspDamper = EditorGUILayout.FloatField("Damper", suspDamper);
                suspTargetPos = EditorGUILayout.FloatField("Target position", suspTargetPos);
            }
            
            forwFricFoldout = EditorGUILayout.Foldout(forwFricFoldout, "Forward friction");
            if (forwFricFoldout)
            {
                forwFricExtrenSlip = EditorGUILayout.FloatField("Extrenum slip", forwFricExtrenSlip);
                forwFricExtrenVal = EditorGUILayout.FloatField("Extrenum value", forwFricExtrenVal);
                forwFricAsymSlip = EditorGUILayout.FloatField("Asymptote slip", forwFricAsymSlip);
                forwFricAsymVal = EditorGUILayout.FloatField("Asymptote value", forwFricAsymVal);
                forwFricStiff = EditorGUILayout.FloatField("Stiffness", forwFricStiff);
            }
            
            sideFricFoldout = EditorGUILayout.Foldout(sideFricFoldout, "Sideways friction");
            if (sideFricFoldout)
            {
                sideFricExtrenSlip = EditorGUILayout.FloatField("Extrenum slip", sideFricExtrenSlip);
                sideFricExtrenVal = EditorGUILayout.FloatField("Extrenum value", sideFricExtrenVal);
                sideFricAsymSlip = EditorGUILayout.FloatField("Asymptote slip", sideFricAsymSlip);
                sideFricAsymVal = EditorGUILayout.FloatField("Asymptote value", sideFricAsymVal);
                sideFricStiff = EditorGUILayout.FloatField("Stiffness", sideFricStiff);
            }
    }

    private TankProperties CreateNewPropertyScriptableObject()
    {
        TankProperties example = ScriptableObject.CreateInstance<TankProperties>();
        // path has to start at "Assets"
        string path = "Assets/Scriptable Objects/filename.asset";
        AssetDatabase.CreateAsset(example, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return example;
    }

    private void InitializeStyles()
    {
        _header1Style = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 30
        };
        
        _header2Style = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 15
        };
        
        _cursiveStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Italic,
            fontSize = 12
        };
        
        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Normal,
            fontSize = 12
        };
    }
}