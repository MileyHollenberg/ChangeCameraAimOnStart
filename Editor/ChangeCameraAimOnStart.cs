using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace MegaTools.Editor.Toolbar
{
    internal static class CameraAimToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyleEnabled;
        public static readonly GUIStyle commandButtonStyleDisabled;

        private const int _buttonWidth = 150;

        static CameraAimToolbarStyles()
        {
            commandButtonStyleEnabled = new GUIStyle("Command")
            {
                fontSize = GUI.skin.label.fontSize,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = GUI.skin.label.normal.textColor,
                },
                fixedWidth = _buttonWidth,
            };

            commandButtonStyleDisabled = new GUIStyle("Command")
            {
                fontSize = GUI.skin.label.fontSize,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Normal,
                normal = new GUIStyleState
                {
                    textColor = GUI.skin.label.normal.textColor,
                },
                fixedWidth = _buttonWidth,
            };
        }
    }

    [InitializeOnLoad]
    public class ChangeCameraAimOnStart
    {
        private static Vector3 _previousPosition;
        private static Quaternion _previousRotation;

        private static bool IsEnabled { get; set; }

        static ChangeCameraAimOnStart()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
            IsEnabled = EditorPrefs.GetBool("FocusCamera_IsEnabled");
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange obj)
        {
            if (Camera.main == null)
            {
                Debug.LogError("No main camera found");
                return;
            }

            Transform camTransform = Camera.main.transform;

            if (IsEnabled && obj == PlayModeStateChange.ExitingEditMode)
            {
                Transform sceneCamTransform = SceneView.lastActiveSceneView.camera.transform;

                _previousPosition = camTransform.position;
                _previousRotation = camTransform.rotation;

                camTransform.position = sceneCamTransform.position;
                camTransform.rotation = sceneCamTransform.rotation;
            }
            else if (IsEnabled && obj == PlayModeStateChange.EnteredEditMode)
            {
                camTransform.position = _previousPosition;
                camTransform.rotation = _previousRotation;
            }
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            string focusText = IsEnabled ? "Start at scene position" : "Start at camera position";
            GUIStyle style = IsEnabled
                ? CameraAimToolbarStyles.commandButtonStyleEnabled
                : CameraAimToolbarStyles.commandButtonStyleDisabled;
            const string tooltip =
                "Start your project at the current scene view position or keep the camera object position";
            
            if (GUILayout.Button(new GUIContent(focusText, tooltip), style))
            {
                SetEnabled(!IsEnabled);
            }
        }

        private static void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
            EditorPrefs.SetBool("FocusCamera_IsEnabled", IsEnabled);
        }
    }
}
