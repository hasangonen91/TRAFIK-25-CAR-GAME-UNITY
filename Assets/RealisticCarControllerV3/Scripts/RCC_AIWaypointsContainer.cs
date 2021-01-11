
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used for holding a list for waypoints, and drawing gizmos for all of them.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/RCC AI Waypoints Container")]
public class RCC_AIWaypointsContainer : MonoBehaviour {

	public List<Transform> waypoints = new List<Transform>();

	// Used for drawing gizmos on Editor.
	void OnDrawGizmos() {
		
		for(int i = 0; i < waypoints.Count; i ++){
			
			Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.3f);
			Gizmos.DrawSphere (waypoints[i].transform.position, 2);
			Gizmos.DrawWireSphere (waypoints[i].transform.position, 20f);
			
			if(i < waypoints.Count - 1){
				
				if(waypoints[i] && waypoints[i+1]){
					
					if (waypoints.Count > 0) {
						
						Gizmos.color = Color.green;

						if(i < waypoints.Count - 1)
							Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position); 
						if(i < waypoints.Count - 2)
							Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position); 
						
					}

				}

			}

		}
		
	}
	
}
