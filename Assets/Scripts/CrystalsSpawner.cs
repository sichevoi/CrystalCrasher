using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalsSpawner : MonoBehaviour {

	public GameObject[] crystals;
	public Vector2 velocity = new Vector2(-5, 0);
	public float spawnPeriodMin = 1f;
	public float spawnPeriodMax = 2f;

	private float _elapsedTime;
	private float _nextSpawnPeriod;

	private Transform _originTransform;

	private GameController _gameController;

	private CrystalController.Type[] types = new CrystalController.Type[] { CrystalController.Type.RED, CrystalController.Type.BLUE, CrystalController.Type.GREEN };

	private bool _isRunning = true;

	private static int POOL_THRESHOLD = 20;
	private static float POOL_PROBABILITY_BELOW_THRESHOLD = 0.05f;
	private IList<GameObject> objectsPool = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		_originTransform = GameObject.FindGameObjectWithTag("Respawn").transform;

		ScheduleSpawn();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (_isRunning == false || _gameController.IsGameOver()) {
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
		}
	}

	public void SpawnResume() {
		if (_isRunning == false) {
			_isRunning = true;
			ScheduleSpawn();
		}
	}

	public void ReturnToPool(GameObject crystal) {
		crystal.SetActive(false);
		objectsPool.Add(crystal);
	}

	public void OnMisHit(GameObject gameObject, CrystalController.Type type) {
		_gameController.OnPlayerDeath(type);
	}

	private GameObject TryGetFromPool() {
		GameObject fromPool = null;
		bool getFromPool = false;

		if (objectsPool.Count < POOL_THRESHOLD && objectsPool.Count > 0) {
			float rnd = Random.Range(0f, 1f);
			getFromPool = rnd < POOL_PROBABILITY_BELOW_THRESHOLD;
		} else {
			getFromPool = objectsPool.Count > 0;
		}

		if (getFromPool) {
			int index = Random.Range(0, objectsPool.Count);
			fromPool = objectsPool[index];
			objectsPool.RemoveAt(index);
		}

		return fromPool;
	}

	private void ScheduleSpawn() {
		_nextSpawnPeriod = Random.Range(GetSpawnPeriodMin(), GetSpawnPeriodMax());
		_elapsedTime = 0f;
	}

	private float GetSpawnPeriodMin() {
		float period = spawnPeriodMin * _gameController.GetDifficultyModifier();
		Debug.Log("Spawn period is " + period);
		return period;
	}

	private float GetSpawnPeriodMax() {
		return spawnPeriodMax * _gameController.GetDifficultyModifier();
	}

	private void SpawnCrystal() {
		GameObject newCrystal = TryGetFromPool();

		if (newCrystal == null) {
			GameObject next = crystals[GetIntRandom(3)];
			newCrystal = Instantiate<GameObject>(next);
			newCrystal.transform.SetParent(transform);

			CrystalController crystalController = newCrystal.GetComponent<CrystalController> ();
			crystalController.SetCrystalType(types[GetIntRandom(3)]);
		} else {
			newCrystal.SetActive(true);

			CrystalController crystalController = newCrystal.GetComponent<CrystalController> ();
			crystalController.SetCrystalType(types[GetIntRandom(3)]);
			crystalController.Reset();
		}


		newCrystal.transform.position = _originTransform.position;

		Rigidbody2D rigidBody = newCrystal.GetComponent<Rigidbody2D> ();
		rigidBody.velocity = velocity;
	}

	private int GetIntRandom(int maxExclusive) {
		float rnd = Random.Range(0f, (float) (maxExclusive - 1));
		return (int) Mathf.Round(rnd);
	}
}
