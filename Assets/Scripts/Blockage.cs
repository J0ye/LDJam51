using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockage : MonoBehaviour
{

    protected static List<Vector3> spots = new List<Vector3>();
    // Start is called before the first frame update
    void Awake()
    {
        spots.Add(transform.position);
    }

    public void RemoveBlockage()
    {
        spots.Remove(transform.position);
        Destroy(gameObject);
    }

    public static bool Blocked(Vector3 position)
    {
        foreach (Vector3 sp in spots)
        {
            if (sp == position)
            {
                return true;
            }
        }

        return false;
    }
}
