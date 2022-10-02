using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public GameObject projectilePrefab;
    protected float actiavteAt = 1f;

    protected GameObject projectile;
    protected float step = 1f;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance().OnAction += Action;
    }

    protected void Action()
    {
        if(step >= actiavteAt)
        {
            Fire();
        }
        step++;
    }

    protected void Fire()
    {
        GameManager.Instance().OnAction -= Action;
        projectile = Instantiate(projectilePrefab, GetAlteredPlayerPosition(), Quaternion.identity);
        projectile.transform.DOMove(transform.GetChild(0).position, 0.2f);
        projectile.transform.up = transform.position - GetAlteredPlayerPosition();
        Destroy(gameObject);
        Destroy(projectile, 1f);
    }

    protected Vector3 GetAlteredPlayerPosition()
    {
        Vector3 ret = new Vector3(MouseController.playerPosition.x + 0.5f, MouseController.playerPosition.y + 0.5f, 0f);
        return ret;
    }
}
