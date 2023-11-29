using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SampleScriptableWizard : ScriptableWizard
{
    [SerializeField] private string gameObjectName;

    private Dictionary<string, GameObject> gameObjectDict;

    [MenuItem("Window/SampleScriptableWizard")]
    private static void Open()
    {
        DisplayWizard<SampleScriptableWizard>("Sample Wizard", "Create", "Find");
    }

    private void OnEnable()
    {
        if (gameObjectDict == null)
            gameObjectDict = new Dictionary<string, GameObject>();
    }

    private void OnWizardCreate()
    {
        gameObjectDict[gameObjectName] = new GameObject(gameObjectName);
    }

    private void OnWizardOtherButton()
    {
        var gameObject = gameObjectDict[gameObjectName];

        if (gameObject == null)
        {
            Debug.LogError("게임 오브젝트를 찾을 수 없습니다.");
        }
    }
}
