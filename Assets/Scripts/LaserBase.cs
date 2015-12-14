using UnityEngine;
using System.Collections;

public abstract class LaserBase : MonoBehaviour {

	private static string COLOR_PROPERTY_NAME = "_TintColor";

	private LineRenderer _line;
	private Material _mat;

	// Use this for initialization
	protected virtual void Start () {
		_line = GetComponent<LineRenderer> ();
		_line.enabled = false;
		_mat = _line.material;
	}

	protected abstract void OnHit(Collider2D collider);

	protected abstract bool ShouldFire();

	public void SetLaserColor(Color color) {
		Color current = _mat.GetColor(COLOR_PROPERTY_NAME);
		if (current != color) {
			_mat.SetColor(COLOR_PROPERTY_NAME, color);
		}
	}

	protected Ray2D FireOnce() {

		Vector3 origin = GetLaserOrigin();
		Vector3 direction = GetLaserDirection();
		
		Ray2D ray = new Ray2D(origin, direction);
		_line.SetPosition(0, ray.origin);

		RaycastHit2D hit = Physics2D.Raycast(origin, direction);
		if (hit.collider != null) {
			_line.SetPosition(1, hit.point);
			OnHit(hit.collider);
		} else {
			_line.SetPosition(1, ray.GetPoint(100));
		}

		return ray;
	}

	protected virtual Vector3 GetLaserOrigin() {
		return transform.position;
	}

	protected virtual Vector3 GetLaserDirection() {
		return transform.forward;
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
