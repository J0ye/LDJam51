using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Player
{
    public float speed = 2f;

    protected Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(GetInputAxis());
    }

    protected void Move(Vector3 target)
    {
        target = target * speed * Time.deltaTime;
        rb.velocity = target;
    }
}
