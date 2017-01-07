using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetController : MonoBehaviour {

	public float moveSpeed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// 移動
		{
			Vector3 moveDir = Vector3.zero;
			if (Input.GetKey (KeyCode.UpArrow)) {
				moveDir.y += 1.0f;
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				moveDir.y += -1.0f;
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				moveDir.z += -1.0f;
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				moveDir.z += 1.0f;
			}

			Debug.Log (moveDir);

			this.transform.Translate (moveDir * moveSpeed * Time.deltaTime);
		}
	}
}
