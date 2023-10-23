using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gem : MonoBehaviour
{
	private AudioSource audioSource;
	private GameManager gameManager;

	private void Start()
	{
		audioSource = FindFirstObjectByType<Camera>().GetComponent<AudioSource>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		if (collision.CompareTag("Player"))
		{
			audioSource.PlayOneShot(SoundManager.Instance.GemCollected);
			GameManager.Instance.AddScore(50);
			gameObject.SetActive(false);
		}
	}
}
