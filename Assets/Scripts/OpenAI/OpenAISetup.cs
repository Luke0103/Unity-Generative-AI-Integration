using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[FilePath("UserSettings/OpenAISettings.asset", FilePathAttribute.Location.ProjectFolder)]
public class OpenAISetup : ScriptableSingleton<OpenAISetup>
{
    public string apiKey = null;
    public int timeout = 0;

    public void Save() => Save(true);
    private void OnDisable() => Save();
}

public class OpenAISettingsProvider : SettingsProvider
{
    public OpenAISettingsProvider() : base ("Project/OpenAI Settings", SettingsScope.Project) { }

    public override void OnGUI(string context)
    {
        OpenAISetup settings = OpenAISetup.instance;

        string key = settings.apiKey;
        int timeout = settings.timeout;

        EditorGUI.BeginChangeCheck();

        key = EditorGUILayout.TextField("API Key", key);
        timeout = EditorGUILayout.IntField("Timeout", timeout);

        if (EditorGUI.EndChangeCheck())
        {
            settings.apiKey = key;
            settings.timeout = timeout;
            settings.Save();
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider() => new OpenAISettingsProvider();
}
