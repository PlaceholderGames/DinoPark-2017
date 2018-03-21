using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfAlert))]
public class FieldOfAlertEditor : Editor {

    // Use this for initialization
    void OnSceneGUI()
    {
        FieldOfAlert foa = (FieldOfAlert)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(foa.transform.position, Vector3.up, Vector3.forward, 360, foa.viewRadius);
        Vector3 viewAngleA = foa.DirFromAngle(-foa.viewAngle / 2, false);
        Vector3 viewAngleB = foa.DirFromAngle(foa.viewAngle / 2, false);

        Handles.DrawLine(foa.transform.position, foa.transform.position + viewAngleA * foa.viewRadius);
        Handles.DrawLine(foa.transform.position, foa.transform.position + viewAngleB * foa.viewRadius);

        Handles.color = Color.red;
        Vector3 stereoAngleA = foa.DirFromAngle(-foa.stereoAngle / 2, false);
        Vector3 stereoAngleB = foa.DirFromAngle(foa.stereoAngle / 2, false);

        Handles.DrawLine(foa.transform.position, foa.transform.position + stereoAngleA * foa.viewRadius);
        Handles.DrawLine(foa.transform.position, foa.transform.position + stereoAngleB * foa.viewRadius);

        Handles.color = Color.green;

        foreach (Transform visibleTarget in foa.visibleTargets)
        {
            Handles.DrawLine(foa.transform.position, visibleTarget.position);
        }
    }
}
