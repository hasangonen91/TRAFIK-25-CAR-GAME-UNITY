
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_Records : ScriptableObject {

	#region singleton
	private static RCC_Records instance;
	public static RCC_Records Instance{	get{if(instance == null) instance = Resources.Load("RCC Assets/RCC_Records") as RCC_Records; return instance;}}
	#endregion

	public List<RCC_Recorder.Recorded> records = new List<RCC_Recorder.Recorded>();

}
