using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XNode;

public class DialogueNode : BaseNode {
	[Input] public int entry;
	//[Output] public int exit;
	public string speakerName;
	public string speakerDialogue;
	[Output(dynamicPortList = true)]public DialogueOption[] dialogueLine;

	public string GetTitle()
	{
		return speakerName;
	}

	public string GetDialogue()
	{
		return speakerDialogue;
	}

	public DialogueOption[] GetOptions()
	{
		var ret = new DialogueOption[dialogueLine.Length];

		for (var i = 0; i < ret.Length; i++)
			ret[i] = dialogueLine[i];

		return ret;
	}
	
}

[Serializable]
public class DialogueOption
{
    public int optionID;
    public string optionDialogue;
}
