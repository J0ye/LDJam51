using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MouseController : TileTargetController
{
    public static Vector3 playerPosition = Vector3.zero;
    public static List<Vector3> contactTiles = new List<Vector3>();
    public static float actionStack = 0f;
    public static bool activated = true;

    public UnityEvent OnMove = new UnityEvent();

    [Space]
    public bool drawGizmos = false;

    protected static MouseController instance;

    protected static GameObject playerHighlight;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        playerPosition = transform.position;
        SetContactPoints(playerPosition); 
        actionStack = 0f;

        playerHighlight = transform.GetChild(1).gameObject;
        playerHighlight.SetActive(false);

        instance = this;

        Activate();
    }

    private void Start()
    {
        GameManager.Instance().OnAction += Move;
        GameManager.Instance().OnAction += DecreaseActionStack;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(activeInstance != this && activated)
        {
            activated = false;
            playerHighlight.SetActive(false);
        }
    }

    protected void Move()
    {
        if(highlight != null)
        {
            float duration = 1f;

            transform.DOMove(targetSpace, duration);
            playerPosition = targetSpace;
            SetContactPoints(playerPosition);
            Destroy(highlight);
            OnMove.Invoke();
        }
    }

    protected void DecreaseActionStack()
    {
        if(actionStack >= 1f)
        {
            actionStack -= 1f;
        }
        Activate();
    }

    protected override void SetTarget()
    {
        base.SetTarget();
        if((Vector2)targetSpace == (Vector2)transform.position || Blockage.Blocked(targetSpace))
        {
            Destroy(highlight);
        }
    }

    protected void SetContactPoints(Vector3 targetCenter)
    {
        contactTiles.Clear();
        for(int x = -1; x < 2; x++)
        {
            for(int y = -1; y < 2; y++)
            {
                if((x != 0 || y != 0) && (Mathf.Abs(x) != 1 || Mathf.Abs(y) != 1))
                {
                    Vector3 nextTarget = new Vector3(targetCenter.x + x, targetCenter.y + y);
                    nextTarget = new Vector3(nextTarget.x, nextTarget.y);
                    contactTiles.Add(nextTarget);
                }
            }
        }
    }

    public static void Activate()
    {
        activeInstance = MouseController.instance;
        activated = true;
        playerHighlight.SetActive(true);
        print("Activated" + instance);
    }

    public static bool InContact(Vector3 target)
    {
        foreach(Vector3 pos in contactTiles)
        {
            if(pos == target)
            {
                return true;
            }
        }

        return false;
    }

    public static Vector3 ClosestContactPoint(Vector3 target)
    {
        Vector3 ret = contactTiles[0];
        float distance = Vector3.Distance(target, ret);
        float temp = Vector3.Distance(target, ret);
        foreach (Vector3 pos in contactTiles)
        {
            distance = Vector3.Distance(target, ret);
            temp = Vector3.Distance(target, pos);
            if (temp < distance)
            {
                ret = pos;
            }
        }

        return ret;
    }

    public static void IncreaseActionStack(float val)
    {
        print("Increase action stack to: " + actionStack);
        actionStack += val;
    }

    public static void Kill()
    {
        Destroy(instance.gameObject);
    }

    public void OnMouseDown()
    {
        Activate();
        print("Click");
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        print("Point click on player. oh oh");
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(drawGizmos)
        {
            print("Length: " + contactTiles.Count);
            foreach (Vector3 pos in contactTiles)
            {
                Gizmos.DrawCube(pos, new Vector3(0.6f, 0.6f, 0.6f));
            }
        }
    }
}
