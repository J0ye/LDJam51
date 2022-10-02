using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public Tilemap map;
    public float attackRange = 1f;
    public float moveRange = 2f;
    public int health = 2;
    public bool idle = false;
    [Header("Graphics")]
    public GameObject effectPrefab;

    public UnityEvent OnDeath = new UnityEvent();

    protected Vector3 targetPosition;
    protected Vector3 alteredPosition;

    protected static Dictionary<Enemy, Vector3> occupiedSpaces = new Dictionary<Enemy, Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance().OnAction += Action;
        GameManager.Instance().enemies.Add(this);
    }

    public void Damage()
    {
        print("Damaged");
        health--;
        Vector3 altered = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, 0f);
        Instantiate(effectPrefab, altered, Quaternion.identity);
        if(health <= 0)
        {
            Death();
        }
    }

    public void Damage(int value)
    {
        print("Damaged");
        health -= value;
        if (health <= 0)
        {
            Death();
        }
    }

    protected virtual void Action()
    {
        if(!idle)
        {
            alteredPosition = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, 0f);
            if (Vector3.Distance(transform.position, MouseController.playerPosition) <= attackRange)
            {
                GameManager.Instance().DamagePlayer();
                Vector3 altered = new Vector3(MouseController.playerPosition.x + 0.5f, MouseController.playerPosition.y + 0.5f, 0f);
                Vector3 thisAltered = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, 0f);
                Vector3 dir = MouseController.playerPosition - transform.position;
                transform.DOPunchPosition(dir, 0.3f, 1, 1, false);
            }
            else
            {
                Move();
            }
        }
    }

    protected virtual void Move()
    {
        Vector3 targetPosition = GetDirection();
        transform.DOMove(targetPosition, 1f);
    }

    protected void Death()
    {
        GameManager.Instance().OnAction -= Action;
        GameManager.Instance().enemies.Remove(this);
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    protected virtual Vector3Int GetDirection()
    {
        Vector3 dir = MouseController.playerPosition - transform.position;
        dir = Vector3.ClampMagnitude(dir, moveRange + 0.2f);
        Vector3Int gridPosition = map.WorldToCell(dir + alteredPosition);
        if(gridPosition == MouseController.playerPosition)
        {
            gridPosition = map.WorldToCell(MouseController.ClosestContactPoint(transform.position));
        }
        RemoveOccupation(this);
        occupiedSpaces.Add(this, targetPosition);
        gridPosition = IsOccupied(gridPosition);
        targetPosition = gridPosition;
        return gridPosition;
    }

    protected Vector3Int IsOccupied(Vector3Int targetGridPosition)
    {
        Vector3Int ret = targetGridPosition;
        foreach (KeyValuePair<Enemy, Vector3> pair in occupiedSpaces)
        {
            if(pair.Value == targetGridPosition)
            {
                Vector3 dir = targetGridPosition - transform.position;
                dir = Vector3.ClampMagnitude(dir, (moveRange/2) + 0.2f);
                ret = map.WorldToCell(dir + alteredPosition);

                return ret;
            }
        }

        return ret;
    }

    protected static void RemoveOccupation(Enemy target)
    {
        bool doRemove = false;
        foreach(KeyValuePair<Enemy, Vector3> pair in occupiedSpaces)
        {
            if(pair.Key == target)
            {
                doRemove = true; // Dont remove while iterating through a dictionary
            }
        }

        if(doRemove) occupiedSpaces.Remove(target);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(targetPosition, new Vector3(0.3f, 0.3f, 0.3f));
        Vector3 posA = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, 0f);


        Vector3 altered = new Vector3(MouseController.playerPosition.x + 0.5f, MouseController.playerPosition.y + 0.5f, 0f);
        Vector3 dir = MouseController.playerPosition - transform.position;
        Gizmos.DrawLine(posA, transform.position + dir);
    }
}
