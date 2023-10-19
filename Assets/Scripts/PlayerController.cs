using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration = 10.0f;
	[SerializeField] private float deceleration = 10.0f;
	[SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float jumpForce = 10.0f;
    private Rigidbody2D rb;
    private float horizontalInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump") && IsGrounded())
		{
			rb.velocityY = jumpForce;
		}
	}

	private void FixedUpdate()
	{
		//Acceleration
		rb.velocityX += horizontalInput * acceleration;

		//Deceleration
		rb.velocityX -= Mathf.Min(deceleration, Mathf.Abs(rb.velocityX)) * Mathf.Sign(rb.velocityX);

		//Max Speed
		rb.velocityX = Mathf.Clamp(rb.velocityX, -maxSpeed, maxSpeed);
	}

	private bool IsGrounded()
	{
		return Physics2D.BoxCast(transform.position, new(0.9f, 0.9f), 0, Vector2.down, 0.1f);
	}
}
