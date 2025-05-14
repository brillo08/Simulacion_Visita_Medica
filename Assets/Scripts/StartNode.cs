using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class StartNode : BaseNode {

	[Output] public int exit;

	public override string GetString (){
		return "Start";

	}

}