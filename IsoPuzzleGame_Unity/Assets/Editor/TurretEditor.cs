using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (TurretBehaviour) )]
public class TurretEditor : Editor{

    void OnSceneGUI()
    {
        TurretBehaviour _Behaviour = (TurretBehaviour)target;
        Handles.color = Color.cyan;
        Handles.DrawWireArc(_Behaviour.transform.position, Vector3.up, Vector3.forward, 360, _Behaviour.m_SightRange);

        if (_Behaviour.m_FieldOfView <= 0)
            return;

        Vector3 startPos = _Behaviour.DirFromAngle(-_Behaviour.m_FieldOfView / 2, false);
        Vector3 endPos = _Behaviour.DirFromAngle(_Behaviour.m_FieldOfView / 2, false);

        Handles.DrawLine(_Behaviour.transform.position, _Behaviour.transform.position + startPos * _Behaviour.m_SightRange);
        Handles.DrawLine(_Behaviour.transform.position, _Behaviour.transform.position + endPos * _Behaviour.m_SightRange);
    }
    
}
