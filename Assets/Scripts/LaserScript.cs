using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	public Color redLaser = Color.red;
	public Color greenLaser = Color.green;
	public Color blueLaser = Color.blue;

	private static string COLOR_PROPERTY_NAME = "_TintColor";

	private LineRenderer _line;
	private Material _mat;
	private AudioSource _audio;

	private bool _hit = false;
	private CrystalController.Type _type;

	private bool _setColor = false;

	void Start () {
		_line = GetComponent<LineRenderer> ();
		_line.enabled = false;

		_mat = _line.material;
		_mat.SetColor(COLOR_PROPERTY_NAME, redLaser);

		_audio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			CheckColor();

			StopCoroutine("FireLaser");

			if (_audio.isPlaying) {
				_audio.Stop();
			}

			_audio.Play();
			_hit = false;
			StartCoroutine("FireLaser");
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

	IEnumerator FireLaser() {
		_line.enabled = true;

		while(Input.GetButton("Fire1") && _hit == false) {
			FireOnce();
			yield return null;
		}

		_line.enabled = false;
	}

	private Ray2D FireOnce() {
		Ray2D ray = new Ray2D(transform.position, transform.forward);
		_line.SetPosition(0, ray.origin);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
		if (hit != null && hit.collider != null) {
			_line.SetPosition(1, hit.point);

			GameObject go = hit.collider.gameObject;
			CrystalController cc = go != null ? go.GetComponent<CrystalController> () : null;
			if (cc != null) {
				cc.Hit(_type);
				_hit = true;
			}
		} else {
			_line.SetPosition(1, ray.GetPoint(100));
		}

		return ray;
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
