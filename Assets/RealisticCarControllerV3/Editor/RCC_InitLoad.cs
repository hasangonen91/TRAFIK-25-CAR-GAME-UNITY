//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2019 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class RCC_InitLoad : MonoBehaviour {

	[InitializeOnLoad]
	public class InitOnLoad {

		static InitOnLoad(){

			SetEnabled("BCG_RCC", true);
			
			if(!EditorPrefs.HasKey("RCC" + "V3.3" + "Installed")){
				
				EditorPrefs.SetInt("RCC" + "V3.3" + "Installed", 1);
				EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing and using Realistic Car Controller. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started");

				Selection.activeObject = RCC_Settings.Instance;

			}

		}

		private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
		{

			BuildTargetGroup.Standalone,
			BuildTargetGroup.Android,
			BuildTargetGroup.iOS,
			BuildTargetGroup.WebGL,
			BuildTargetGroup.Facebook,
			BuildTargetGroup.XboxOne,
			BuildTargetGroup.PS4,
			BuildTargetGroup.tvOS,
			BuildTargetGroup.Switch,
			BuildTargetGroup.WSA

		};

		private static void SetEnabled(string defineName, bool enable)
		{
			//Debug.Log("setting "+defineName+" to "+enable);
			foreach (var group in buildTargetGroups)
			{
				var defines = GetDefinesList(group);
				if (enable)
				{
					if (defines.Contains(defineName))
					{
						return;
					}
					defines.Add(defineName);
				}
				else
				{
					if (!defines.Contains(defineName))
					{
						return;
					}
					while (defines.Contains(defineName))
					{
						defines.Remove(defineName);
					}
				}
				string definesString = string.Join(";", defines.ToArray());
				PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
			}
		}

		private static List<string> GetDefinesList(BuildTargetGroup group){
			
			return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));

		}

	}

}
