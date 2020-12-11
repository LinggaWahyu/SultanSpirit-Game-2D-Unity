using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour {

	public void setPopUp(GameObject popUp, bool active) 
	{
		popUp.SetActive(active);
	}
}
