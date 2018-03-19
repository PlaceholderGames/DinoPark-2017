using UnityEngine;
using System.Collections;
public class Drink : AgentBehaviour
{

    public void drink()
    {
        MyAnky myAnky = new MyAnky();
        while (myAnky.health != 100)
        {
            myAnky.health += 1;
        }
    }

}
