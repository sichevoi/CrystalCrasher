﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAtack : MonoBehaviour {

	public Sprite playerRed;
	public Sprite playerGreen;
	public Sprite playerBlue;

	private LaserScript _laser;
	private SpriteRenderer _body;
	private AudioSource _gunChangeSound;

	private GameController _gameController;

	// Use this for initialization
	void Start () {
		_laser = transform.FindChild("Gun/GunTip").GetComponent<LaserScript> ();
		_body = transform.FindChild("Gun/Body").GetComponent<SpriteRenderer> ();
		_gameController = FindObjectOfType<GameController> ();
		_gunChangeSound = transform.FindChild("Gun/ChangeSound").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_gameController.IsGameOver() == false && Input.GetButtonDown("Jump")) {
			CrystalController.Type nextType = CrystalController.GetNext(_laser.GetGunType());
			UpdateType(nextType);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<CrystalController> () != null) {
			CrystalController.Type type = other.gameObject.GetComponent<CrystalController> ().GetCrystalType();
			_gameController.OnPlayerDeath(type);
		}
	}

	private void UpdateType(CrystalController.Type type) {
		_laser.SetGunType(type);
		_body.sprite = GetSpriteForType(type);
		_gunChangeSound.Play();
	}

	private Sprite GetSpriteForType(CrystalController.Type type) {
		switch(type) {
			case CrystalController.Type.RED:
				return playerRed;
			case CrystalController.Type.GREEN:
				return playerGreen;
			case CrystalController.Type.BLUE:
			default:
				return playerBlue;
		}
	}
}
