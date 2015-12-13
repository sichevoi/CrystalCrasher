using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawRegulator : MonoBehaviour {

	private HashSet<Collider2D> _collided = new HashSet<Collider2D> ();
	private CrystalsSpawner _spawner;

	void Start () {
		_spawner = GetComponentInParent<CrystalsSpawner> ();
	}
	
	void FixedUpdate () {
		if (_collided.Count > 0) {
			_spawner.SpawnPause();
		} else {
			_spawner.SpawnResume();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		_collided.Add(other);
	}

	void OnTriggerStay2D(Collider2D other) {
		_collided.Add(other);
	}

	void OnTriggerExit2D(Collider2D other) {
		if (_collided.Contains(other)) {
			_collided.Remove(other);
		}
	}
}
