using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

public class SampleWindow : EditorWindow
{
    private int intervalTime = 60;
    private int selected, selectedToolbar;

    private float width = 600f;
    private float height = 400f;
    private float angle;

    private bool on;

    private readonly float animFloatValue = 0.0001f;

    private const string AUTO_SAVE_INTERVAL_TIME = "Autosave interval time (sec)";
    private const string SIZE_WIDTH_KEY = "ExampleWindow size width";
    private const string SIZE_HEIGHT_KEY = "ExampleWindow size height";

    private AnimFloat animFloat;

    [MenuItem("Window/SampleWindow")]
    private static void Open()
    {
        GetWindow<SampleWindow>();

        //var sampleWindow = CreateInstance<SampleWindow>();
        //sampleWindow.Show();
        //sampleWindow.ShowUtility(); //항상 포커싱, CreateInstance 필수
    }

    //재컴파일, 재생 모드 등의 상황에서 호출
    [InitializeOnLoadMethod]
    private static void OnLoaded()
    {
        //Debug.Log("Loaded");

        //바이너리 형식 저장
        //EditorUserSettings.SetConfigValue("token", "euijong");
    }

    private void OnEnable()
    {
        intervalTime = EditorPrefs.GetInt(AUTO_SAVE_INTERVAL_TIME, 60);
        width = EditorPrefs.GetFloat(SIZE_WIDTH_KEY, 600f);
        height = EditorPrefs.GetFloat(SIZE_HEIGHT_KEY, 400f);
        position = new Rect(position.x, position.y, width, height);

        animFloat = new AnimFloat(animFloatValue);
    }

    private void OnDisable()
    {
        EditorPrefs.SetFloat(SIZE_WIDTH_KEY, width);
        EditorPrefs.SetFloat(SIZE_HEIGHT_KEY, height);
    }

    private void OnGUI()
    {
        //ValueUpdateDisplay();

        //DisabledGroupDisplay();

        //FadeGroupDisplay();

        //ObjectFieldDisplay();

        //IndentLevelDisplay();

        //KnobDisplay();

        //ScopeDisplay();

        ToggleDisplay();
    }

    private void GroupDisplay()
    {
        EditorGUILayout.Vector3IntField("Vector3", Vector3Int.zero);
        GUILayout.Button("Button");
    }

    private void ValueUpdateDisplay()
    {
        EditorGUI.BeginChangeCheck();

        intervalTime = EditorGUILayout.IntSlider("간격 (초)", intervalTime, 1, 3600);
        width = EditorGUILayout.Slider("너비", width, 200f, 1920f);
        height = EditorGUILayout.Slider("높이", height, 200f, 1080f);

        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetInt(AUTO_SAVE_INTERVAL_TIME, intervalTime);
            EditorPrefs.SetFloat(SIZE_WIDTH_KEY, width);
            EditorPrefs.SetFloat(SIZE_HEIGHT_KEY, height);
            position = new Rect(position.x, position.y, width, height);
        }
    }

    private void DisabledGroupDisplay()
    {
        EditorGUI.BeginDisabledGroup(true);
        GroupDisplay();
        EditorGUI.EndDisabledGroup();
    }

    private void FadeGroupDisplay()
    {
        on = (animFloat.value >= 1f);

        if (GUILayout.Button(on ? "Close" : "Open", GUILayout.Width(64)))
        {
            animFloat.target = on ? animFloatValue : 1f;
            animFloat.speed = 0.5f;

            var env = new UnityEvent();
            env.AddListener(() => Repaint());
            animFloat.valueChanged = env;
        }

        EditorGUILayout.BeginFadeGroup(animFloat.value);
        GroupDisplay();
        EditorGUILayout.EndFadeGroup();
    }

    private void ObjectFieldDisplay()
    {
        EditorGUILayout.ObjectField(null, typeof(GameObject), false);
        EditorGUILayout.ObjectField(null, typeof(Material), false);
        EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
        EditorGUILayout.ObjectField(null, typeof(GameObject), false, new[] { GUILayout.Width(64), GUILayout.Height(64) });
    }

    private void IndentLevelDisplay()
    {
        EditorGUILayout.LabelField("Parent");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Child");
        EditorGUILayout.LabelField("Child");
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("Parent");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Child");
    }

    private void KnobDisplay()
    {
        angle = EditorGUILayout.Knob(Vector2.one * 64, angle, 0, 360, "º", Color.gray, Color.blue, true);
    }

    private void ScopeDisplay()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Button("Button1");
            GUILayout.Button("Button2");
        }
    }

    private void ToggleDisplay()
    {
        on = GUILayout.Toggle(on, on ? "on" : "off", "button");

        selected = GUILayout.Toolbar(selected, new string[] { "1", "2", "3" });

        selectedToolbar = GUILayout.SelectionGrid(selectedToolbar, new string[] { "1", "2", "3", "4", "5", "6" }, 2);
    }
}
