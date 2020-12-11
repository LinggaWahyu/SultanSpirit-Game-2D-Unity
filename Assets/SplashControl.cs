using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashControl : MonoBehaviour {

	public Image splashImage;
	public string loadLevel;

	IEnumerator Start () {
		splashImage.canvasRenderer.SetAlpha(0.0f);

		FadeIn();
		yield return new WaitForSeconds(3f);

		FadeOut();
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(loadLevel);
	}
	
	void FadeIn() {
		splashImage.CrossFadeAlpha(1.0f, 1.5f, false);
	}

	void FadeOut() {
		splashImage.CrossFadeAlpha(0.0f, 2.5f, false);
	}
}
