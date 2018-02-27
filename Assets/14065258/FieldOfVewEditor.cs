using UnityEngine;
using System.Collections;
using UnityEditor;

//Pulls in varibles from other scripts and changes it to suit this script
[CustomEditor(typeof(fieldOfView))]
public class FieldOfVewEditor : Editor
{

    void OnSceneGUI()
    {
        
        //will draw the "total view" around the current object and the view it can see.
        fieldOfView fow = (fieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        //Draws a line from us to the targets position
        Handles.color = Color.red;
        foreach(Transform visibleTarget in fow.visibleTargets)
        {
            if (visibleTarget != null)
            {
                Handles.DrawLine(fow.transform.position, visibleTarget.position);
            }
        }
    }
}
