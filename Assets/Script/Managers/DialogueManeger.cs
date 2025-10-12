using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManeger : MonoBehaviour, IService
{

    private GetherInput getherInput;


    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Image speakerIcon;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.03f;

    private DialogueObject currentDialogue;
    private int currentLineIndex;
    private bool isTyping;

    private Coroutine typingCoroutin;


    public void Init()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RegesterGetherInput(GetherInput getherInputInstance)
    {
        getherInput = getherInputInstance;
    }

 
    public void StartDialogue(DialogueObject dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueUI.SetActive(true);

        getherInput.DialogueActivated();
        Interact.isInteracting = true;
        ShowLine();// show start line
    }

    private void ShowLine()
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        speakerName.text = line.stringspeakerName;
        speakerIcon.sprite = line.speakerIcon;
        if(typingCoroutin!=null)
            StopCoroutine(typingCoroutin);
        typingCoroutin = StartCoroutine(TypingLine(line.dialogueText));
    }

    private void ShowNextLine()
    {
        currentLineIndex++;
        if(currentLineIndex>=currentDialogue.dialogueLines.Length)
        {
            // end dialogue
            EndDialogue();
        }
        else
        {
            ShowLine();
        }

    }
    public void ContinueDialogue()
    {
        if(isTyping)
        {
            // finish typing
            FinishTyping();

        }
        else
        {
            ShowNextLine();
        }
    }

    private void FinishTyping()
    {
        StopCoroutine(typingCoroutin);
        dialogueText.text = currentDialogue.dialogueLines[currentLineIndex].dialogueText;
        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        currentDialogue = null;
        getherInput.DialogueDeactivated();
        Interact.isInteracting = false;
    }

    private IEnumerator TypingLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping=false;
    }

}
