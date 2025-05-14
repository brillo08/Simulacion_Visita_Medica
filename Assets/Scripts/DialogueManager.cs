using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public struct DialogueCollection
{
    public string dialogueID;
    public DialogueGraph dialogue;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject generalCanvas;
    [SerializeField] private OptionButtonContainer optionPrefab;
    [SerializeField] private Transform optionOrigin;
    [SerializeField] private TMP_Text speakerTitle;
    [SerializeField] private TMP_Text speakerDialogue;

    public DialogueCollection[] _dialogues;
    private string currentActiveDialogue;

    private Dictionary<string, DialogueCollection> dialogues = new();

    private DialogueNode currentNode;
    private List<OptionButtonContainer> createdOptionsContainers = new();

    void Start() 
    {
        foreach (var d in _dialogues)
            dialogues.Add(d.dialogueID, d);

        //currentNode = graph.nodes[0] as DialogueNode;
        //ShowDialogue();
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && currentActiveDialogue != "")
            ActivateDialogue();
    }

    public void ShowDialogue()
    {
        speakerTitle.text = currentNode.GetTitle();
        speakerDialogue.text = currentNode.GetDialogue();

        foreach (var o in currentNode.GetOptions())
        {
            var b = Instantiate(optionPrefab, optionOrigin).GetComponent<OptionButtonContainer>();
            createdOptionsContainers.Add(b);
            b.SetText(o.optionDialogue);
            b.GetComponent<Button>().onClick.AddListener( () => { NextNode(o.optionID); } );
        }
    }

    public void DeleteOptions()
    {
        foreach (var b in createdOptionsContainers)
            Destroy(b.gameObject);

        createdOptionsContainers.Clear();
    }

    public void HideDialogue()
    {
        speakerTitle.text = "";
        speakerDialogue.text = "";

        DeleteOptions();
    }

    public void ActivateDialogue()
    {
        if (currentActiveDialogue == "") return;

        ShowDialogue();
        generalCanvas.gameObject.SetActive(true);
    }

    public void SelectDialogue(string dialogueID)
    {
        if (dialogues.TryGetValue(dialogueID, out var d))
        {
            currentActiveDialogue = d.dialogueID;
            currentNode = d.dialogue.nodes[0] as DialogueNode;
        }
    }

    public void DisableDialogue()
    {
        generalCanvas.gameObject.SetActive(false);
    }

    public void DeselectDialogue()
    {
        currentActiveDialogue = "";
        currentNode = null;
    }

    public void NextNode(int optionID)
    {
        DeleteOptions();

        foreach (var p in currentNode.Ports)
        {
            if (p.fieldName != "dialogueLine") continue;

            var dialogueOptions = currentNode.GetOptions();

            foreach (var option in dialogueOptions)
            {
                if (option.optionID == optionID)
                {
                    currentNode = currentNode.GetOutputPort($"dialogueLine {optionID}").
                    Connection.node as DialogueNode;

                    break;
                }
            }

            break;
        }

        ShowDialogue();
    }
}
