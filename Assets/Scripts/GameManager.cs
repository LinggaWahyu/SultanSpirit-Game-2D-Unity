using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	[SerializeField]
	private GameObject swordPrefab;

	[SerializeField]
	private Text swordTxt;

	private int collectedSwords;

	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}

	public GameObject SwordPrefab
	{
		get
		{
			return swordPrefab;
		}
	}

	public int CollectedSwords
	{
		get 
		{
			return collectedSwords;
		}

		set 
		{
			swordTxt.text = "x " + value.ToString();
			this.collectedSwords = value;
		}
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
