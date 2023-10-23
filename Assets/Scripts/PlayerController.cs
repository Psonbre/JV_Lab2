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
	[SerializeField] private float maxSustainedJumpTime = 1;
    private Rigidbody2D rb;
    private float horizontalInput;
	private bool grounded = false;
	private AudioSource audioSource;
	private bool dying = false;
	private bool winning = false;
	private float sustainedJumpTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		audioSource = FindFirstObjectByType<Camera>().GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

		if (Input.GetButton("Jump") && sustainedJumpTimer > 0 && !dying && !winning)
		{
			sustainedJumpTimer -= Time.deltaTime;
			
			if (grounded) 
			{
				rb.velocityY = jumpForce;
				grounded = false;
				audioSource.PlayOneShot(SoundManager.Instance.PlayerJump);
			}

			if (rb.velocityY > 0) rb.velocityY = jumpForce;

		}
	}

	private void FixedUpdate()
	{
		if (!dying)
		{
			//Acceleration
			if (!winning) rb.velocityX += horizontalInput * acceleration;

			//Deceleration
			rb.velocityX -= Mathf.Min(deceleration, Mathf.Abs(rb.velocityX)) * Mathf.Sign(rb.velocityX);

			//Max Speed
			rb.velocityX = Mathf.Clamp(rb.velocityX, -maxSpeed, maxSpeed);
		}
		else
		{
			rb.velocity = Vector2.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Floor")) 
		{
			StopCoroutine("RevokeGrounded");
			grounded = true;
			sustainedJumpTimer = maxSustainedJumpTime;
		}

		if (collision.CompareTag("Monster") && !dying && !winning)
		{
			StartCoroutine(KillPlayer(0.5f));
		}

		if (collision.CompareTag("Exit") && !dying && !winning)
		{
			winning = true;
			audioSource.PlayOneShot(SoundManager.Instance.ExitReached);
			GameManager.Instance.StartNextlevel(2);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Floor") && gameObject.activeInHierarchy)
		{
			StopCoroutine("RevokeGrounded");
			StartCoroutine("RevokeGrounded");
		}
	}

	IEnumerator RevokeGrounded()
	{
		yield return new WaitForSeconds(0.15f);
		grounded = false;
	}

	IEnumerator KillPlayer(float time)
	{
		audioSource.PlayOneShot(SoundManager.Instance.PlayerKilled);
		dying = true;
		float timer = 0;
		while(transform.localScale.magnitude > 0)
		{
			timer += Time.deltaTime;
			transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer/time);
			yield return null;
		}
		GameManager.Instance.PlayerDie();
		GameManager.Instance.RestartLevel(1);
	}
}
