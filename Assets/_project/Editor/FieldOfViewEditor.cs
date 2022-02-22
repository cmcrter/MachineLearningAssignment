using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CharacterSenses))]
public class FieldOfViewEditor : Editor
{
	void OnSceneGUI() 
	{
		CharacterSenses fow = (CharacterSenses)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
		Vector3 viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle (fow.viewAngle / 2, false);

		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		Handles.color = Color.green;
		foreach (Transform visibleTarget in fow.visibleTargets)
		{
			Handles.DrawLine (fow.transform.position, visibleTarget.position, 0.5f);
		}
	}
}