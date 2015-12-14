using UnityEngine;
using System.Collections;

public class LaserScript : LaserBase {

	public Color redLaser = Color.red;
	public Color greenLaser = Color.green;
	public Color blueLaser = Color.blue;

	private AudioSource _audio;

	private CrystalController.Type _type;

	protected bool _hit = false;

	private bool _setColor = false;

	private GameController _gameController;

	protected override void Start () {
		base.Start();

		SetLaserColor(redLaser);

		_audio = GetComponent<AudioSource> ();

		_gameController = FindObjectOfType<GameController> ();
	}

	void Update () {
		if (_gameController.IsGameOver() == false && Input.GetButtonDown("Fire1")) {
			CheckColor();

			if (_audio.isPlaying) {
				_audio.Stop();
			}

			_audio.Play();
			_hit = false;

			StartFire();
		}
	}

	public void SetGunType(CrystalController.Type type) {
		if (_type != type) {
			_type = type;
			_setColor = true;
		}
	}

	public CrystalController.Type GetGunType() {
		return _type;
	}

	protected override void OnHit(Collider2D collider) {
		GameObject go = collider.gameObject;
		CrystalController cc = go != null ? go.GetComponent<CrystalController> () : null;
		if (cc != null) {
			cc.Hit(_type);
			_hit = true;
		}
	}

	protected override bool ShouldFire() {
		return Input.GetButton("Fire1") && _hit == false;
	}

	private void CheckColor() {
		if (_setColor) {
			Color newColor = redLaser;

			switch(_type) {
				case CrystalController.Type.GREEN:
					newColor = greenLaser;
					break;
				case CrystalController.Type.BLUE:
					newColor = blueLaser;
					break;
			}

			SetLaserColor(newColor);
		}
	}
}
