using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAwayFromPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.up = transform.position - MouseController.playerPosition;
    }
}
