
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_Vehicles : ScriptableObject {

	public RCC_CarControllerV3[] vehicles;

	#region singleton
	private static RCC_Vehicles instance;
	public static RCC_Vehicles Instance{	get{if(instance == null) instance = Resources.Load("RCC Assets/RCC_Vehicles") as RCC_Vehicles; return instance;}}
	#endregion

}
