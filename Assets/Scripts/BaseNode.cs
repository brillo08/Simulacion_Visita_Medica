using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : Node
{
	[Output(dynamicPortList = true)] public DialogueOption[] dialogueLine;
	public string randomElementID;
	public float waitTime;

	public virtual string GetString()
	{
		return null;
	}

	public DialogueOption[] GetOptions()
	{
		var ret = new DialogueOption[dialogueLine.Length];

		for (var i = 0; i < ret.Length; i++)
			ret[i] = dialogueLine[i];

		return ret;
	}
}