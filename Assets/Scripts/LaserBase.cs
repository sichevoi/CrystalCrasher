using UnityEngine;
using System.Collections;

public abstract class LaserBase : MonoBehaviour {

	protected LineRenderer _line;

	// Use this for initialization
	protected virtual void Start () {
		_line = GetComponent<LineRenderer> ();
		_line.enabled = false;
	}

	protected abstract void OnHit(Collider2D collider);

	protected abstract bool ShouldFire();

	protected Ray2D FireOnce() {
		Ray2D ray = new Ray2D(transform.position, transform.forward);
		_line.SetPosition(0, ray.origin);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
		if (hit.collider != null) {
			_line.SetPosition(1, hit.point);
			OnHit(hit.collider);

		} else {
			_line.SetPosition(1, ray.GetPoint(100));
		}

		return ray;
	}

	protected void StartFire() {
		StopCoroutine("FireLaser");
		StartCoroutine("FireLaser");
	}

	IEnumerator FireLaser() {
		_line.enabled = true;

		while(ShouldFire()) {
			FireOnce();
			yield return null;
		}

		_line.enabled = false;
	}
}
