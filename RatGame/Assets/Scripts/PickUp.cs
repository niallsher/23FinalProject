using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : Interactable {

	public GameObject PickFxPrefab;
	public GameObject health;
	public UnityEvent onPickup;

	void Start() {
		health = GameObject.Find("Player");
	}

	// Update is called once per frame
	new void Update () {
		base.Update ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		OnTriggerEnter2DFunc (col);
	}

	void OnTriggerStay2D(Collider2D col) {
		OnTriggerStay2DFunc (col);
	}

	void OnTriggerExit2D(Collider2D col) {
		OnTriggerExit2DFunc (col);
	}

	protected override void OnPlayerTrigger (Player player)
	{
		base.OnPlayerTrigger (player);

		if (onPickup != null) onPickup.Invoke();

		health.GetComponent<Health>().TakeHeal(1);
		PlayerPrefs.SetFloat("Health", (int)PlayerPrefs.GetFloat("Health") + 1);
		UIHeartsHealthBar.instance.SetHearts ((int)PlayerPrefs.GetFloat("Health"));



		// Instantiate the pickup fx
		if (PickFxPrefab != null) {
			Instantiate (PickFxPrefab, transform.position, Quaternion.identity);
		}

		// Camera Shake
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.05f, 1f);
		}

		// Destroy the gameobject
		Destroy (gameObject);
	}

}
