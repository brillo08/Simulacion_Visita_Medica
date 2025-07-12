using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XNode;

public class DialogueNode : BaseNode
{
	[Input] public int entry;
	//[Output] public int exit;
	public string speakerName;
	public string speakerDialogue;
	public float waitTime;
	

	public string GetTitle()
	{
		return speakerName;
	}

	public string GetDialogue()
	{
		return speakerDialogue;
	}
}

[Serializable]
public class DialogueOption
{
	public int optionID;
	public string optionDialogue;
	public bool dialogueEnding;
}
