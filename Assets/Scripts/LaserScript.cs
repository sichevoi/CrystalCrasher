using UnityEngine;
using System.Collections;

public class LaserScript : LaserBase {

	public Color redLaser = Color.red;
	public Color greenLaser = Color.green;
	public Color blueLaser = Color.blue;

	private static string COLOR_PROPERTY_NAME = "_TintColor";

	private Material _mat;
	private AudioSource _audio;

	private CrystalController.Type _type;

	protected bool _hit = false;

	private bool _setColor = false;

	protected override void Start () {
		base.Start();

		_mat = _line.material;
		_mat.SetColor(COLOR_PROPERTY_NAME, redLaser);

		_audio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
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
			Color current = _mat.GetColor(COLOR_PROPERTY_NAME);
			Color newColor = redLaser;

			switch(_type) {
				case CrystalController.Type.GREEN:
					newColor = greenLaser;
					break;
				case CrystalController.Type.BLUE:
					newColor = blueLaser;
					break;
			}

			if (current != newColor) {
				_mat.SetColor(COLOR_PROPERTY_NAME, newColor);
			}
		}
	}
}
