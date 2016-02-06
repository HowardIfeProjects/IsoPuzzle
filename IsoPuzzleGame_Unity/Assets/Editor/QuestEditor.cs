using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelManager))]
public class QuestEditor : EditorWindow
{

    private int _levelIndex = 0;
    private int _MissionNumber = 0;
  
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
        EditorGUILayout.Space();
        GUILayout.Label("Level Number: " + _levelIndex);
        GUILayout.Label("Mission Number: " + _MissionNumber);
        EditorGUILayout.Space();

        //list stuff

        if (!Selection.activeGameObject.GetComponent<LevelManager>())
            return;

        int ListSize = Selection.activeGameObject.GetComponent<LevelManager>().li_LevelData[_levelIndex]._Objectives.Length;
        ListSize = EditorGUILayout.IntField("Objectives Count: ", ListSize);

        //end

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Button("Previous Level");
            GUILayout.Button("Next Level");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Button("Previous Mission");
            GUILayout.Button("Next Mission");
        }
    }
}
