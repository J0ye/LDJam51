using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Ability : TileTargetController, IPointerEnterHandler, IPointerExitHandler
{
    public float duration = 1f;
    [Header("Graphics")]
    public GameObject actionPrefab;
    public GameObject disclaimerPrefab;
    public float highlightHeight = 100f;
    public float tweenDuration = 0.5f;

    protected RectTransform rt;
    protected Tween activeTween;
    protected GameObject actionObject;
    protected float startHeight = -300f;
    protected bool hover = false;
    protected bool added = false;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<RectTransform>(out rt);
        startHeight = rt.anchoredPosition.y;
    }

    protected override void Update()
    {
        if(activeInstance != this && !hover)
        {
            ResetAnimation();
        }

        if(activeInstance != this && !MouseController.activated && added)
        {
            Destroy(highlight);
            GameManager.Instance().OnAction -= Action;
            added = false;
            print("removed action");
        }
        else
        {
            base.Update();
        }
    }

    public virtual void Action()
    {        
        actionObject = Instantiate(actionPrefab, null);
        actionObject.transform.position = targetSpace;
        Destroy(highlight);
        print("Action created: " + actionObject.name);
        MouseController.IncreaseActionStack(duration);
        GameManager.Instance().OnAction -= Action;
        added = false;
    }

    protected override void SetTarget()
    {
        if (!GameManager.Instance().pause && activeInstance == this)
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
                //MouseController.Activate();
            }
            else
            {
                print("target needs to be within range");
            }
        }
        if (!GameManager.Instance().pause && highlight != null && !added)
        {
            added = true;
            GameManager.Instance().OnAction += Action;
        }
    }

    #region Animations
    public override void OnPointerClick(PointerEventData eventData)
    {
        if(MouseController.actionStack <= 1f)
        {
            base.OnPointerClick(eventData);
        }
        else
        {
            GameObject temp = Instantiate(disclaimerPrefab, transform.parent);
            Destroy(temp, 1f);
            print("actionstack: " + MouseController.actionStack);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float targetHeight = startHeight + highlightHeight;
        activeTween = GetComponent<RectTransform>().DOAnchorPosY(targetHeight, tweenDuration);
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(activeInstance != this)
        {
            ResetAnimation();
        }
        hover = false;
    }

    protected void StopTween()
    {
        if(activeTween != null)
        {
            activeTween.Complete();
        }
    }

    protected void ResetAnimation()
    {
        activeTween = GetComponent<RectTransform>().DOAnchorPosY(startHeight, tweenDuration);
    }
    #endregion
}
