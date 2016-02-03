using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerVariableEditor : EditorWindow {

    private float _speed;
    private float _gravity;
    bool DisableInput = false;

    [MenuItem("Custom Tools/Player Tools/ Show Editor")]
    private static void showEditor()
    {
        EditorWindow.GetWindow<PlayerVariableEditor>(false, "Player Variables");
    }

    [MenuItem("Custom Tools/Player Tools/ Show Editor", true)]
    private static bool showEditorValidator()
    {
        if (Selection.activeGameObject.GetComponent<com.ootii.Actors.ActorDriver>())
            return true;
        else
            return false;
    }

    void OnSelectionChange()
    {
        if (Selection.activeGameObject.tag == "Player")
        {
            _speed = Selection.activeGameObject.GetComponent<com.ootii.Actors.ActorDriver>().MovementSpeed;

            Vector3 _gVal = Selection.activeGameObject.GetComponent<com.ootii.Actors.ActorController>().Gravity;
            _gravity = _gVal.y;
        }

        Repaint();
    }

    void OnGUI()
    {
        GUILayout.Label("Player Variables");
        EditorGUILayout.Space();
        _speed = EditorGUILayout.FloatField("Player Speed: ", _speed);
        _gravity = EditorGUILayout.FloatField("Gravity: ", _gravity);
        EditorGUILayout.Space();

        if(GUILayout.Button("SET"))
        {
            com.ootii.Actors.ActorDriver _driver = Selection.activeGameObject.GetComponent<com.ootii.Actors.ActorDriver>();
            _driver.MovementSpeed = _speed;

            com.ootii.Actors.ActorController _controller = Selection.activeGameObject.GetComponent<com.ootii.Actors.ActorController>();
            _controller.Gravity = new Vector3(0f, _gravity,0f);
        }
    }

}
