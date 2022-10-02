using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileTargetController : MonoBehaviour, IPointerClickHandler
{
    public Tilemap map;
    public GameObject highlightPrefab;
    public float range = 2f;

    public static TileTargetController activeInstance;

    protected Camera cam;
    protected GameObject highlight;
    protected Collider2D mapCollider;
    protected RaycastHit2D rayHit;
    protected Vector3 targetSpace;
    protected Vector3 mousepos;

    protected virtual void Awake()
    {
        cam = Camera.main;
        mapCollider = map.GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (activeInstance == this)
        {
            mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SetTarget();
            }
        }
    }

    protected virtual void SetTarget()
    {
        if (!GameManager.Instance().pause && activeInstance == this && mapCollider.OverlapPoint(mousepos))
        {
            Vector3Int gridPosition = map.WorldToCell(mousepos);
            float dis = Vector3.Distance(MouseController.playerPosition, gridPosition);
            if (dis <= range)
            {
                targetSpace = gridPosition;
                if (highlight == null)
                {
                    highlight = Instantiate(highlightPrefab, null);
                    highlight.transform.position = targetSpace;
                }
                else
                {
                    highlight.transform.position = targetSpace;
                }
            }
            else
            {
                print("target needs to be within range");
            }
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        activeInstance = this;
    }
}
