using UnityEngine;
using UnityEditor;
using com.ootii.Utilities.Debug;

[CanEditMultipleObjects]
[CustomEditor(typeof(SceneFlags))]
public class SceneFlagsEditor : Editor
{
    // Helps us keep track of when the list needs to be saved. This
    // is important since some changes happen in scene.
    private bool mIsDirty;

    // The actual class we're storing
    private SceneFlags mTarget;
    private SerializedObject mTargetSO;

    /// <summary>
    /// Called when the object is selected in the editor
    /// </summary>
    private void OnEnable()
    {
        // Grab the serialized objects
        mTarget = (SceneFlags)target;
        mTargetSO = new SerializedObject(target);
    }

    /// <summary>
    /// This function is called when the scriptable object goes out of scope.
    /// </summary>
    private void OnDisable()
    {
    }

    /// <summary>
    /// Called when the inspector needs to draw
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Pulls variables from runtime so we have the latest values.
        mTargetSO.Update();

        GUILayout.Space(5);

        bool lNewIsEnabled = GUILayout.Toggle(mTarget.IsEnabled, new GUIContent("Is Enabled", "Determines if adding flags is enabled"));
        if (lNewIsEnabled != mTarget.IsEnabled)
        {
            mTarget.IsEnabled = lNewIsEnabled;
        }

        if (GUILayout.Button(new GUIContent("Clear", "Clear all flags")))
        {
            SceneFlags.Flags.Clear();
        }

        GUILayout.Space(10);

        // If there is a change... update.
        if (mIsDirty)
        {
            // Flag the object as needing to be saved
            EditorUtility.SetDirty(mTarget);
            EditorApplication.MarkSceneDirty();

            // Pushes the values back to the runtime so it has the changes
            mTargetSO.ApplyModifiedProperties();

            // Clear out the dirty flag
            mIsDirty = false;
        }
    }    
}
