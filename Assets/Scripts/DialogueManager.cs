using System;
using System.Collections.Generic;
using KinematicCharacterController.Walkthrough.ClimbingLadders;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

//using XNodeEditor;

[Serializable]
public struct DialogueCollection
{
    public string dialogueID;
    public DialogueGraph dialogue;
    public UnityEvent OnDialogueStarted;
    public UnityEvent OnDialogueEnded;
}

[Serializable]
public class RandomElement
{
    public string elementID;
    public RandomElementMode randomElementMode;
    public bool isInteger;
    public Vector2Int rangeNumber;
    public float rangeValue;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private MyCharacterController player;
    [SerializeField] private GameObject generalCanvas;
    [SerializeField] private GameObject interactionHint;
    [SerializeField] private OptionButtonContainer optionPrefab;
    [SerializeField] private Transform optionOrigin;
    [SerializeField] private TMP_Text speakerTitle;
    [SerializeField] private TMP_Text speakerDialogue;

    public DialogueCollection[] _dialogues;
    public RandomElement[] _randElements;
    
    private string currentActiveDialogue;

    private int randomDecision;

    private Dictionary<string, DialogueCollection> dialogues = new();
    private Dictionary<string, RandomElement> randElements = new();

    private BaseNode currentNode;
    private List<OptionButtonContainer> createdOptionsContainers = new();

    void Start()
    {
        foreach (var d in _dialogues)
            dialogues.Add(d.dialogueID, d);
        
        foreach (var r in _randElements)
            randElements.Add(r.elementID, r);

        //currentNode = graph.nodes[0] as DialogueNode;
        //ShowDialogue();
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && currentActiveDialogue != "")
            ActivateDialogue();
    }

    public void DoSpecial(float explicitValue = -1)
    {
        var waitValue = explicitValue <= -1 ? currentNode.waitTime : explicitValue;

        StartCoroutine(SpecialRoutine(waitValue));
    }

    private IEnumerator SpecialRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentNode.randomElementID == string.Empty)
        {
            NextNode(0);
        }

        else
        {
            var randElement = randElements[currentNode.randomElementID];
            
            switch (randElement.randomElementMode)
            {
                case RandomElementMode.Range:

                    NextNode(Random.Range(randElement.rangeNumber.x, randElement.rangeNumber.y));
                    
                    break;
            }
        }
    }

    public void ShowDialogue()
    {
        interactionHint.SetActive(false);
        /*player.ToggleMovement(false);
        cameraWrapper.ToggleView(false);*/

        speakerTitle.text = ((DialogueNode)currentNode).GetTitle();
        speakerDialogue.text = ((DialogueNode)currentNode).GetDialogue();

        if (((DialogueNode)currentNode).waitTime <= 0f)
        {
            foreach (var o in ((DialogueNode)currentNode).GetOptions())
            {
                var b = Instantiate(optionPrefab, optionOrigin).GetComponent<OptionButtonContainer>();
                createdOptionsContainers.Add(b);
                b.SetText(o.optionDialogue);
                b.GetComponent<Button>().onClick.AddListener(() => { NextNode(o.optionID); });
            }
        }

        else
            DoSpecial();
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
        interactionHint.SetActive(false);
        speakerDialogue.text = "";
        dialogues[currentActiveDialogue].OnDialogueEnded?.Invoke();
        //Debug.Log(player.CanMove);

        DeleteOptions();
    }

    public void ActivateDialogue()
    {
        if (currentActiveDialogue == "") return;

        dialogues[currentActiveDialogue].OnDialogueStarted?.Invoke();
        ShowDialogue();
        player.ToggleMovement(false);
        generalCanvas.gameObject.SetActive(true);
    }

    public void SelectDialogue(string dialogueID)
    {
        if (dialogues.TryGetValue(dialogueID, out var d))
        {
            currentActiveDialogue = d.dialogueID;
            currentNode = d.dialogue.nodes[0] as DialogueNode;
            interactionHint.SetActive(true);
        }
    }

    public void DisableDialogue()
    {
        generalCanvas.gameObject.SetActive(false);
        player.ToggleMovement(true);
        dialogues[currentActiveDialogue].OnDialogueEnded?.Invoke();
        //player.ToggleMovement(true);
        /*cameraWrapper.ToggleView(true);
        Debug.Log(player.CanMove);*/
    }

    public void DeselectDialogue()
    {
        currentActiveDialogue = "";
        interactionHint.SetActive(false);
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
                    if (option.dialogueEnding)
                    {
                        DisableDialogue();
                        DeselectDialogue();
                        break;
                    }

                    var exitNode = currentNode.GetOutputPort($"dialogueLine {optionID}").Connection.node;

                    switch (exitNode)
                    {
                        case DialogueNode:

                            currentNode = exitNode as DialogueNode;
                            ShowDialogue();

                            break;

                        case SpecialNode:

                            Debug.Log("going to special");

                            currentNode = exitNode as SpecialNode;
                            DoSpecial();

                            break;
                    }

                    break;
                }
            }

            break;
        }
    }
}
