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

    private bool isActive = false;
    private bool finishedTyping = false;
    public float charactersPerSecond = 12;

    public override void Execute()
    {
        popUpText.gameObject.SetActive(true);
        popUpText.text = "";
        anim.Play("dialogue_box_appear");
        isActive = true;
        StartCoroutine(TypeTextUncapped(dialogue));

    }

    IEnumerator TypeTextUncapped(string line)
    {
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                popUpText.text = textBuffer;
                print(textBuffer);
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        finishedTyping = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isActive && finishedTyping)
        {
            anim.Play("dialogue_box_disappear");
            popUpText.gameObject.SetActive(false);
            cutsceneHandler.PlayNextElement();
        }
        else if (Input.GetButtonDown("Interact") && isActive && !finishedTyping)
        {
            StopAllCoroutines();
            finishedTyping = true;
            popUpText.text = dialogue;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
