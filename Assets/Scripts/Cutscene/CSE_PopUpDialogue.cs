using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CSE_PopUpDialogue : CutsceneElementBase
{
    [SerializeField] private TMP_Text popUpText;
    [SerializeField] private GameObject dialogueBox;
    [TextArea] [SerializeField] private string dialogue;
    [SerializeField] Animator anim;

    private bool isActive;

    public override void Execute()
    {
        popUpText.gameObject.SetActive(true);
        anim.Play("dialogue_box_appear");
        isActive = true;
        popUpText.text = dialogue;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isActive)
        {
            anim.Play("dialogue_box_disappear");
            popUpText.gameObject.SetActive(false);
            cutsceneHandler.PlayNextElement();
        }
    }
}
