using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	private float laserLength = 1f;

	private LineRenderer line;
	private bool _hit = false;

	void Start () {
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			StopCoroutine("FireLaser");
			_hit = false;
			StartCoroutine("FireLaser");
		}
	}

	IEnumerator FireLaser() {
		line.enabled = true;

		while(Input.GetButton("Fire1") && _hit == false) {
			FireOnce();
			yield return null;
		}

		line.enabled = false;
	}

	private Ray2D FireOnce() {
		Ray2D ray = new Ray2D(transform.position, transform.forward);
		line.SetPosition(0, ray.origin);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
		if (hit != null && hit.collider != null) {
			line.SetPosition(1, hit.point);

			GameObject go = hit.collider.gameObject;
			CrystalController cc = go != null ? go.GetComponent<CrystalController> () : null;
			if (cc != null) {
				cc.Hit(CrystalController.Type.GREEN);
				_hit = true;
			}
		} else {
			line.SetPosition(1, ray.GetPoint(100));
		}

		return ray;
	}
}
