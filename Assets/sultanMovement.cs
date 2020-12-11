using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sultanMovement : MonoBehaviour {

	Rigidbody2D Rb;
	public float ms;
	public float jf;

	void Start () {
		Rb = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
		float horiz = Input.GetAxisRaw("Horizontal"); // a,d , kiri kanan
		Quaternion q = transform.rotation;

		Rb.velocity = new Vector2(ms * horiz, Rb.velocity.y);

		if (Input.GetKeyUp(KeyCode.UpArrow) && Mathf.Abs(Rb.velocity.y) < 0.001f) //space
		{
			Rb.AddForce(new Vector2(0, jf));
		}

		if (Input.GetKey(KeyCode.LeftArrow)) 
		{
			q.eulerAngles = new Vector3(q.eulerAngles.x, 180, 0);
			transform.rotation = q;
		} else if (Input.GetKey(KeyCode.RightArrow))
		{
			q.eulerAngles = new Vector3(q.eulerAngles.x, 0, 0);
			transform.rotation = q;
		}
	}
}
