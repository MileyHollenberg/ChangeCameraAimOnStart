using System.Collections.Generic;
using UnityEditor;

internal static class CameraAimSettings
{
    public const string USE_PLAYER_OBJECT = "UsePlayerObjectForCameraAim";

    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
        SettingsProvider provider = new SettingsProvider("Project/CameraAim", SettingsScope.Project)
        {
            label = "Camera Aim on Start",
            guiHandler = searchContext =>
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Move object tagged as Player:");
                EditorPrefs.SetBool(USE_PLAYER_OBJECT, EditorGUILayout.Toggle(EditorPrefs.GetBool(USE_PLAYER_OBJECT)));
                EditorGUILayout.EndHorizontal();
            },
            keywords = new HashSet<string>(new[] {"camera", "aim", "start"})
        };

        return provider;
    }
}
