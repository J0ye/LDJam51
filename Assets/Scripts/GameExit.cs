using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GameExit : MonoBehaviour
{
    public UnityEvent OnExit = new UnityEvent();

    float waitUntil = 3f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            OnExit.Invoke();
            Invoke("Finish", waitUntil);
            GameManager.Instance().finishedLevel = true;
            Vector3 pos = transform.position;
            collision.transform.position = pos;
            Vector3 up = new Vector3(pos.x + 0.3f, pos.y + 0.7f, 0f);
            collision.gameObject.transform.DOMove(up, waitUntil);
            transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0.9f, waitUntil);
        }
    }

    protected void Finish()
    {
        GameManager.Instance().FinishLevel();
    }
}
