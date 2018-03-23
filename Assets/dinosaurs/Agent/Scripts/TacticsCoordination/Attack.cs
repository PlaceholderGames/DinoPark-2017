using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    public int weight;
    public int power;
    public int targetHealth;
    public GameObject target;
    BoxCollider BoxCollider;


    public virtual IEnumerator Execute()
    {
        // your attack behaviour here
        if (target.tag == "Anky")
        {
            targetHealth -= power;
        }
        yield break;
    }
}
