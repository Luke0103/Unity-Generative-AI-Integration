using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu] //.asset으로 저장 가능하게 함
public class SampleScriptableObject : ScriptableObject
{
    [Range(0, 10), SerializeField] private int number = 0;
    [SerializeField] private bool toggle = false;
    [SerializeField] private string[] texts = new string[5];

    [MenuItem("ScriptableObject/Create SampleScriptableObject Instance")]
    private static void CreateObjectInstance()
    {
        var sampleAsset = CreateInstance<SampleScriptableObject>();
    }

    [MenuItem("ScriptableObject/Create SampleScriptableObject Asset")]
    private static void CreateObjectAsset()
    {
        var sampleAsset = CreateInstance<SampleScriptableObject>();
        AssetDatabase.CreateAsset(sampleAsset, "Assets/SampleAsset.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("ScriptableObject/Load SampleAsset")]
    private static void LoadSampleAsset()
    {
        var sampleAsset = AssetDatabase.LoadAssetAtPath<SampleScriptableObject>("Assets/SampleAsset.asset");
    }
}
