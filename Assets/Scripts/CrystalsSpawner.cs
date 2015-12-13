using UnityEngine;
using System.Collections;

public class CrystalsSpawner : MonoBehaviour {

	public GameObject[] crystals;
	public Vector2 velocity = new Vector2(-5, 0);
	public float spawnPeriodMin = 1f;
	public float spawnPeriodMax = 2f;

	private float _elapsedTime;
	private float _nextSpawnPeriod;

	private Transform originTransform;

	private CrystalController.Type[] types = new CrystalController.Type[] { CrystalController.Type.RED, CrystalController.Type.BLUE, CrystalController.Type.GREEN };

	private bool _isRunning = true;

	// Use this for initialization
	void Start () {
		ScheduleSpawn();
		originTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (_isRunning == false) {
			return;
		}

		if (_elapsedTime > _nextSpawnPeriod) {
			SpawnCrystal();
			ScheduleSpawn();
		} else {
			_elapsedTime += Time.fixedDeltaTime;
		}
	}

	public void SpawnPause() {
		if (_isRunning) {
			_isRunning = false;
			_elapsedTime = 0;
		}
	}

	public void SpawnResume() {
		if (_isRunning == false) {
			_isRunning = true;
			ScheduleSpawn();
		}
	}

	private void ScheduleSpawn() {
		_nextSpawnPeriod = Random.Range(spawnPeriodMin, spawnPeriodMax);
		_elapsedTime = 0f;
	}

	private void SpawnCrystal() {
		GameObject next = crystals[Random.Range(0, crystals.Length)];
		GameObject newCrystal = Instantiate<GameObject>(next);
		newCrystal.transform.SetParent(transform);
		newCrystal.transform.position = originTransform.position;
		Rigidbody2D rigidBody = newCrystal.GetComponent<Rigidbody2D> ();
		rigidBody.velocity = velocity;
		CrystalController setup = newCrystal.GetComponent<CrystalController> ();
		setup.SetType(types[Random.Range(0, types.Length)]);
	}
}
