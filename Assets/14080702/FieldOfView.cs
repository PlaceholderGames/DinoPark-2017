using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public float viewRadius; // Range of vision. Anky = 100, Rapty = 200
	[Range(0,360)]
    public float viewAngle; // Field of Vision. Anky = 300, Rapty = 240
	[Range(0,180)]
	public float stereoAngle; // Not doing anything with this yet. Stereo FoV Anky = 30, Rapty = 60
    // Note: These values above are subject to change on game balancing

    public LayerMask targetMask;
    //public LayerMask obstacleMask; // Not using raytracing to determine visibility right now

    [HideInInspector] // If you want to see the list of visible Dinos in the Inspector view, comment this out
    public List<Transform> visibleTargets = new List<Transform>(); // this is the list of visible dinosaurs

    [HideInInspector] // If you want to see the list of visible Dinos in the Inspector view, comment this out
    public List<Transform> stereoVisibleTargets = new List<Transform>(); // this is the list of visible dinosaurs in stereo

	private void Start()
	{
        StartCoroutine("FindTargetsWithDelay", 0.2f);
	}
	IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear ();
	stereoVisibleTargets.Clear ();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < stereoAngle / 2) //Can be seen in stereo
            {
                //float distanceToTarget = Vector3.Distance(transform.position, target.position); // Only if we see it do we know how far away it is, due to stereo vision
                //Debug.Log("Relative distance: " + distanceToTarget); // This will be used to determine states, such as attack, alert, etc

                //float orientationOfTarget = target.eulerAngles.y; // This gives the relative angle of the target. 0 is directly facing, 180 is facing away
                // Note, if the animal has you on its left side, the value will be in the high 300s and will switch to low values if it crosses to having you
                // on the right side. You might want to subtract 180 to allow 0 to be facing away, and very low and very high values are facing towards you
                //Debug.Log("Relative angle: " + orientationOfTarget);
                stereoVisibleTargets.Add(target);
            }
            else if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                //float directionToTarget = Vector3.Angle(transform.forward, dirToTarget); // We need the direction of the object only for checking with raytracing
                visibleTargets.Add(target); // For now, if it is in range and angle of eyesight we can see it
            }
         
        }
    }
	
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}
