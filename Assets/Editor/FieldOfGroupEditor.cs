using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfGroup))]
public class FieldOfGroupEditor : Editor
{

    // Use this for initialization
    void OnSceneGUI()
    {
        FieldOfGroup fog = (FieldOfGroup)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fog.transform.position, Vector3.up, Vector3.forward, 360, fog.viewRadius);
        Vector3 viewAngleA = fog.DirFromAngle(-fog.viewAngle / 2, false);
        Vector3 viewAngleB = fog.DirFromAngle(fog.viewAngle / 2, false);

        Handles.DrawLine(fog.transform.position, fog.transform.position + viewAngleA * fog.viewRadius);
        Handles.DrawLine(fog.transform.position, fog.transform.position + viewAngleB * fog.viewRadius);

        Handles.color = Color.red;
        Vector3 stereoAngleA = fog.DirFromAngle(-fog.stereoAngle / 2, false);
        Vector3 stereoAngleB = fog.DirFromAngle(fog.stereoAngle / 2, false);

        Handles.DrawLine(fog.transform.position, fog.transform.position + stereoAngleA * fog.viewRadius);
        Handles.DrawLine(fog.transform.position, fog.transform.position + stereoAngleB * fog.viewRadius);

        Handles.color = Color.green;

        foreach (Transform visibleTarget in fog.visibleTargets)
        {
            Handles.DrawLine(fog.transform.position, visibleTarget.position);
        }
    }
}
