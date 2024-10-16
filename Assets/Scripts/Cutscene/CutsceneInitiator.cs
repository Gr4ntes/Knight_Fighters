using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInitiator : MonoBehaviour
{
    private CutsceneHandler cutsceneHandler;

    private void Start()
    {
        cutsceneHandler = GetComponent<CutsceneHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CutsceneHandler.dialogueStarted?.Invoke();
            cutsceneHandler.PlayNextElement();
        }
            
    }
}
