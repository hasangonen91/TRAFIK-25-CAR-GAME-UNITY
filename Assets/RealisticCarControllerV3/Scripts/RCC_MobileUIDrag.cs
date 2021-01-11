
#pragma warning disable 0414

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Mobile UI Drag used for orbiting RCC Camera.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Drag")]
public class RCC_MobileUIDrag : MonoBehaviour, IDragHandler, IEndDragHandler{

	private bool isPressing = false;

	public void OnDrag(PointerEventData data){

		if (RCC_Settings.Instance.controllerType != RCC_Settings.ControllerType.Mobile)
			return;

		isPressing = true;

		RCC_SceneManager.Instance.activePlayerCamera.OnDrag (data);

	}

	public void OnEndDrag(PointerEventData data){

		if (RCC_Settings.Instance.controllerType != RCC_Settings.ControllerType.Mobile)
			return;

		isPressing = false;

	}

}
