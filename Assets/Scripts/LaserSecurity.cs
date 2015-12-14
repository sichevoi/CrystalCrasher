using UnityEngine;
using System.Collections;

public class LaserSecurity : LaserBase {

	public Transform playerPosition;

	private bool _enabled;
	private bool _triggered;

	private Vector3 _direction;
	private Vector3 _origin;

	protected override void Start () {
		base.Start ();
		_origin = transform.FindChild("Origin").position;
		_direction = playerPosition.position - _origin;
	}

	// Update is called once per frame
	void Update () {
		if (_triggered) {
			_triggered = false;
			StartFire();
		}
	}

	public void SecurityEnable(bool enable) {
		_triggered = enable && enable != _enabled;
		_enabled = enable;
	}

	protected override bool ShouldFire () {
		return _enabled;
	}

	protected override void OnHit (Collider2D collider) {
		Debug.Log("Hit object " + collider.gameObject);
	}

	protected override Vector3 GetLaserDirection () {
		return _direction;
	}

	protected override Vector3 GetLaserOrigin () {
		return _origin;
	}
}
