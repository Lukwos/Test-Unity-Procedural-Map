using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speed = 20f;
    Rigidbody playerRigidbody;

    public float movementValue;
    public float turnValue;

	// Use this for initialization
	void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
	}

    void Update()
    {
        movementValue = Input.GetAxis("Vertical");
        turnValue = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Vector3 movement = transform.forward * movementValue * speed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + movement);

        float turn = turnValue * 4 * speed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        playerRigidbody.MoveRotation(playerRigidbody.rotation * turnRotation);
    }
}
