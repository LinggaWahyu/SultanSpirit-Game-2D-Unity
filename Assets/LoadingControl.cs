using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour {

	public Transform LoadingBar;
	public string MainMenuScene;

	[SerializeField]
	private float currentAmount;
	[SerializeField]
	private float speed;
	
	// Update is called once per frame
	void Update () {
		if (currentAmount < 100)
		{
			currentAmount += speed * Time.deltaTime;
			Debug.Log((int)currentAmount);
		} 
		else
		{
			SceneManager.LoadScene(MainMenuScene);
		}

		LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
}
