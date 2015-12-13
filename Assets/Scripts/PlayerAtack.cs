using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAtack : MonoBehaviour {

	public Sprite playerRed;
	public Sprite playerGreen;
	public Sprite playerBlue;

	private LaserScript _laser;
	private SpriteRenderer _body;

	private LevelManager _levelManager;

	// Use this for initialization
	void Start () {
		_laser = transform.FindChild("Gun/GunTip").GetComponent<LaserScript> ();
		_body = transform.FindChild("Gun/Body").GetComponent<SpriteRenderer> ();
		_levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			CrystalController.Type nextType = CrystalController.GetNext(_laser.GetGunType());
			UpdateType(nextType);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Triggered player collision");
		if (other.gameObject.GetComponent<CrystalController> () != null) {
			_levelManager.LoadDeath();
		}
	}

	private void UpdateType(CrystalController.Type type) {
		_laser.SetGunType(type);
		_body.sprite = GetSpriteForType(type);
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
