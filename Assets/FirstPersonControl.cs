using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControl : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public float rotateFactor = 4.0f;
    public float pitchFactor = 3.0f;

    private Transform eyeMount;
    private CharacterController characterController;

	// Use this for initialization
	void Start () {

        characterController = GetComponent<CharacterController>();

        eyeMount = transform.Find("EyeMount");
        if (eyeMount == null)
        {
            Debug.LogError("Player GameObject error: No EyeMount child.");
        }
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection += -transform.right;
        if (Input.GetKey(KeyCode.S)) moveDirection += -transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        characterController.SimpleMove(moveDirection.normalized * moveSpeed);
        transform.Rotate(Vector3.up, rotateFactor * (Input.GetAxis("Mouse X") * Time.deltaTime));
        if (eyeMount != null) eyeMount.Rotate(Vector3.right, rotateFactor * (Input.GetAxis("Mouse Y") * Time.deltaTime));

    }
}
