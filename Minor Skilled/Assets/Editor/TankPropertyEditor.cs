using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using Tank_components;
using TankComponents;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class TankPropertyEditor : EditorWindow
{
    private const int DEFAULT_SPACING = 20;
    private const int NO_TANK_SELECTED = -1;
    
    private bool _wheelsGroupEnabled;
    private bool _forwFricFoldout;
    private bool _sideFricFoldout;
    private bool _suspSpringFoldout;
    
    private string _tankName;
    private AnimationCurve _motorTorque;
    private AnimationCurve _gearRatios;
    private int _maxGears;
    private float _maxHealth;
    private float _maxArmor;
    private float _wheelMass;
    private float _wheelRadius;
    private float _wheelDampingRate;
    private float _wheelSuspDist;
    private float _wheelForceAppDist;
    private Vector3 _wheelCenter;
    private float _suspSpring;
    private float _suspDamper;
    private float _suspTargetPos;

    private float _forwFricExtrenSlip;
    private float _forwFricExtrenVal;
    private float _forwFricAsymSlip;
    private float _forwFricAsymVal;
    private float _forwFricStiff;
    
    private float _sideFricExtrenSlip;
    private float _sideFricExtrenVal;
    private float _sideFricAsymSlip;
    private float _sideFricAsymVal;
    private float _sideFricStiff;
    
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

    private WheelCollider _tankWheelProperties;
    private Vector2 _scrollPos;

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
        
        _scrollPos =
            EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
        GUILayout.Label("Tank editor", _header1Style);
        DrawListOfTanks();

        GUILayout.Label("Select a tank to edit properties (cannot save values in playmode!)", _cursiveStyle);
        GUILayout.Space(DEFAULT_SPACING);
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

        List<WheelCollider> leftColliders = _selectedTank.GetLeftWheelColliders();
        List<WheelCollider> rightColliders = _selectedTank.GetRightWheelColliders();

        foreach (var wheel in leftColliders)
        {
            AssignValuesToWheel(wheel);
        }
        
        foreach (var wheel in rightColliders)
        {
            AssignValuesToWheel(wheel);
        }
    }

    private void AssignValuesToWheel(WheelCollider wheel)
    {
        wheel.mass = _wheelMass;
        wheel.radius = _wheelRadius;
        wheel.wheelDampingRate = _wheelDampingRate;
        wheel.suspensionDistance = _wheelSuspDist;
        wheel.forceAppPointDistance = _wheelForceAppDist;
        wheel.center = _wheelCenter;

        JointSpring suspSpringValues = new JointSpring
        {
            spring = _suspSpring,
            damper = _suspDamper,
            targetPosition = _suspTargetPos
        };
        wheel.suspensionSpring = suspSpringValues;

        WheelFrictionCurve forwFricValues = new WheelFrictionCurve
        {
            extremumSlip = _forwFricExtrenSlip,
            extremumValue = _forwFricExtrenVal,
            asymptoteSlip = _forwFricAsymSlip,
            asymptoteValue = _forwFricAsymVal,
            stiffness = _forwFricStiff
        };
        wheel.forwardFriction = forwFricValues;

        WheelFrictionCurve sideFricValues = new WheelFrictionCurve
        {
            extremumSlip = _sideFricExtrenSlip,
            extremumValue = _sideFricExtrenVal,
            asymptoteSlip = _sideFricAsymSlip,
            asymptoteValue = _sideFricAsymVal,
            stiffness = _sideFricStiff
        };
        wheel.sidewaysFriction = sideFricValues;
    }

    private void SelectTank(int index)
    {
        GUI.FocusControl(null);    
        _currentlySelectedTankIndex = index;
        _selectedTank = _foundTanks[index];
        _newPropertyData = _selectedTank.Properties;

        if (_retrievedPropertyData == null) return;
        
        //Wheel
        if (_selectedTank.GetLeftWheelColliders().Count == 0) return;
        _tankWheelProperties = _selectedTank.GetLeftWheelColliders()[0];
        _wheelMass = _tankWheelProperties.mass;
        _wheelRadius = _tankWheelProperties.radius;
        _wheelDampingRate = _tankWheelProperties.wheelDampingRate;
        _wheelSuspDist = _tankWheelProperties.suspensionDistance;
        _wheelForceAppDist = _tankWheelProperties.forceAppPointDistance;
        _wheelCenter = _tankWheelProperties.center;

        _suspSpring = _tankWheelProperties.suspensionSpring.spring;
        _suspDamper = _tankWheelProperties.suspensionSpring.damper;
        _suspTargetPos = _tankWheelProperties.suspensionSpring.targetPosition;
        
        //Forw fric
        _forwFricExtrenSlip = _tankWheelProperties.forwardFriction.extremumSlip;
        _forwFricExtrenVal = _tankWheelProperties.forwardFriction.extremumValue;
        _forwFricAsymSlip = _tankWheelProperties.forwardFriction.asymptoteSlip;
        _forwFricAsymVal = _tankWheelProperties.forwardFriction.asymptoteValue;
        _forwFricStiff = _tankWheelProperties.forwardFriction.stiffness;
        
        //Side
        _sideFricExtrenSlip = _tankWheelProperties.sidewaysFriction.extremumSlip;
        _sideFricExtrenVal = _tankWheelProperties.sidewaysFriction.extremumValue;
        _sideFricAsymSlip = _tankWheelProperties.sidewaysFriction.asymptoteSlip;
        _sideFricAsymVal = _tankWheelProperties.sidewaysFriction.asymptoteValue;
        _sideFricStiff = _tankWheelProperties.sidewaysFriction.stiffness;
    }

    private void DrawCurrentlySelectedTank()
    {
        if (_currentlySelectedTankIndex != NO_TANK_SELECTED && _selectedTank != null)
        {
            GUILayout.Label("Currently selected tank:", _header2Style);
            
            //Tank name
            _tankName = _selectedTank.name;
            _tankName = EditorGUILayout.TextField("Scene name", _tankName);
            _selectedTank.name = _tankName;
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
            _newPropertyData.MotorTorque = EditorGUILayout.CurveField("Motor torque", _newPropertyData.MotorTorque, Color.green, new Rect(0,0, 80000, 40000));
            _newPropertyData.GearRatios = EditorGUILayout.CurveField("Gear ratios", _newPropertyData.GearRatios, Color.green, new Rect(-1,-5, _newPropertyData.MaxGears + 1, 25));
            _newPropertyData.MaxGears = EditorGUILayout.IntSlider("Max gears", _newPropertyData.MaxGears, 0, 10);
            _newPropertyData.TankMass = EditorGUILayout.Slider("Tank mass", _newPropertyData.TankMass, 1, 100000);
            _newPropertyData.ReloadTime = EditorGUILayout.Slider("Reload time", _newPropertyData.ReloadTime, 0.5f, 50f);

            //
            GUILayout.Space(DEFAULT_SPACING);
            DrawWheelColliderProperties();
        }
    }

    private void DrawWheelColliderProperties()
    {
            GUILayout.Label("Wheel collider properties:", _header2Style);
            _wheelMass = EditorGUILayout.FloatField("Mass", _wheelMass);
            _wheelRadius = EditorGUILayout.FloatField("Radius", _wheelRadius);
            _wheelDampingRate = EditorGUILayout.FloatField("Damping rate", _wheelDampingRate);
            _wheelSuspDist = EditorGUILayout.FloatField("Suspension distance", _wheelSuspDist);
            _wheelForceAppDist = EditorGUILayout.FloatField("Force app point distance", _wheelForceAppDist);
            _wheelCenter = EditorGUILayout.Vector3Field("Center", _wheelCenter);
            
            _suspSpringFoldout = EditorGUILayout.Foldout(_suspSpringFoldout, "Suspension spring");
            if (_suspSpringFoldout)
            {
                _suspSpring = EditorGUILayout.FloatField("Spring", _suspSpring);
                _suspDamper = EditorGUILayout.FloatField("Damper", _suspDamper);
                _suspTargetPos = EditorGUILayout.FloatField("Target position", _suspTargetPos);
            }
            
            _forwFricFoldout = EditorGUILayout.Foldout(_forwFricFoldout, "Forward friction");
            if (_forwFricFoldout)
            {
                _forwFricExtrenSlip = EditorGUILayout.FloatField("Extrenum slip", _forwFricExtrenSlip);
                _forwFricExtrenVal = EditorGUILayout.FloatField("Extrenum value", _forwFricExtrenVal);
                _forwFricAsymSlip = EditorGUILayout.FloatField("Asymptote slip", _forwFricAsymSlip);
                _forwFricAsymVal = EditorGUILayout.FloatField("Asymptote value", _forwFricAsymVal);
                _forwFricStiff = EditorGUILayout.FloatField("Stiffness", _forwFricStiff);
            }
            
            _sideFricFoldout = EditorGUILayout.Foldout(_sideFricFoldout, "Sideways friction");
            if (_sideFricFoldout)
            {
                _sideFricExtrenSlip = EditorGUILayout.FloatField("Extrenum slip", _sideFricExtrenSlip);
                _sideFricExtrenVal = EditorGUILayout.FloatField("Extrenum value", _sideFricExtrenVal);
                _sideFricAsymSlip = EditorGUILayout.FloatField("Asymptote slip", _sideFricAsymSlip);
                _sideFricAsymVal = EditorGUILayout.FloatField("Asymptote value", _sideFricAsymVal);
                _sideFricStiff = EditorGUILayout.FloatField("Stiffness", _sideFricStiff);
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