using UnityEngine;
using System.Collections;

public class CrystalsSpawner : MonoBehaviour {

	public GameObject[] crystals;
	public Vector2 velocity = new Vector2(-5, 0);
	public float spawnPeriodMin = 1f;
	public float spawnPeriodMax = 2f;

	private float _elapsedTime;
	private float _nextSpawnPeriod;

	// Use this for initialization
	void Start () {
		ScheduleSpawn();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_elapsedTime > _nextSpawnPeriod) {
			SpawnCrystal();
			ScheduleSpawn();
		} else {
			_elapsedTime += Time.fixedDeltaTime;
		}
	}

	private void ScheduleSpawn() {
		_nextSpawnPeriod = Random.Range(spawnPeriodMin, spawnPeriodMax);
		_elapsedTime = 0f;
	}

	private void SpawnCrystal() {
		GameObject next = crystals[Random.Range(0, crystals.Length)];
		GameObject newCrystal = Instantiate<GameObject>(next);
		newCrystal.transform.parent = transform;
		newCrystal.transform.position = transform.position;
		Rigidbody2D rigidBody = newCrystal.GetComponent<Rigidbody2D> ();
		rigidBody.velocity = velocity;
	}
}
