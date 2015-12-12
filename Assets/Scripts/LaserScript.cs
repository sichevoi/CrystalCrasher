using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	private float laserLength = 1f;

	private LineRenderer _line;
	private bool _hit = false;
	private CrystalController.Type _type;

	void Start () {
		_line = GetComponent<LineRenderer> ();
		_line.enabled = false;
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			StopCoroutine("FireLaser");
			_hit = false;
			StartCoroutine("FireLaser");
		}
	}

	public void SetGunType(CrystalController.Type type) {
		_type = type;
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
}
