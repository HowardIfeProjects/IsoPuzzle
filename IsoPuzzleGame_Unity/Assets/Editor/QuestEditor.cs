using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelManager))]
public class QuestEditor : EditorWindow
{

    enum displayFieldType { DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields }
    displayFieldType DisplayFieldType;

    LevelManager t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;


    [MenuItem("Custom Tools/Mission Tools/ Show Editor")]
    private static void showEditor()
    {
        EditorWindow.GetWindow<QuestEditor>(false, "Mission Info");
    }

    [MenuItem("Custom Tools/Mission Tools/ Show Editor", true)]
    private static bool showEditorValidator()
    {
        if (Selection.activeGameObject.GetComponent<LevelManager>())
            return true;
        else
            return false;
    }

    void OnSelectionChange()
    {
        Repaint();
    }

    void OnGUI()
    {
    }
}
