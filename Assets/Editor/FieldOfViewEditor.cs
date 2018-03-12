using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

	// Use this for initialization
	void OnSceneGUI() {
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
		Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle/2, false);
		Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle/2, false);
		
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
		
		Handles.color = Color.red;
		Vector3 stereoAngleA = fow.DirFromAngle(-fow.stereoAngle/2, false);
		Vector3 stereoAngleB = fow.DirFromAngle(fow.stereoAngle/2, false);
		
		Handles.DrawLine(fow.transform.position, fow.transform.position + stereoAngleA * fow.viewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + stereoAngleB * fow.viewRadius);

        Handles.color = Color.green;

        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
	}
}
