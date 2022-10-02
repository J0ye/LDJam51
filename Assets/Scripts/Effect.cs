using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public int damage = 1;
    public float destroyAfter = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if(destroyAfter > 0)
        {
            Destroy(transform.parent.gameObject, destroyAfter);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            print(gameObject.name +  " hit: " + collision.gameObject.name + " with " + damage);
            for(int i = damage; i > 0; i--)
            {
                collision.gameObject.SendMessage("Damage", damage);
            }
        }
    }
}
