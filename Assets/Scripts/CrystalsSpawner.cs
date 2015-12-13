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
	private LevelManager _levelManager;

	private GameObject _securityLaser;
	private AudioSource _backgroundMusic;
	private AudioSource _sirene;

	private CrystalController.Type[] types = new CrystalController.Type[] { CrystalController.Type.RED, CrystalController.Type.BLUE, CrystalController.Type.GREEN };

	private bool _isRunning = true;

	private static int POOL_THRESHOLD = 20;
	private static float POOL_PROBABILITY_BELOW_THRESHOLD = 0.2f;
	private IList<GameObject> objectsPool = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		ScheduleSpawn();
		_originTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
		_levelManager = FindObjectOfType<LevelManager> ();

		_securityLaser = transform.FindChild("SecurityLaser").gameObject;
		_securityLaser.SetActive(false);
		_sirene = GetComponent<AudioSource> ();
		_backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource> ();
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

	public void OnMisHit(GameObject gameObject) {
		_securityLaser.transform.position = transform.position;
		_securityLaser.SetActive(true);
		_sirene.Play();
		_backgroundMusic.Stop();
		_levelManager.LoadDeathWithDelay(5);
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
		_nextSpawnPeriod = Random.Range(spawnPeriodMin, spawnPeriodMax);
		_elapsedTime = 0f;
	}

	private void SpawnCrystal() {
		GameObject newCrystal = TryGetFromPool();

		if (newCrystal == null) {
			GameObject next = crystals[Random.Range(0, crystals.Length)];
			newCrystal = Instantiate<GameObject>(next);
			newCrystal.transform.SetParent(transform);

			CrystalController crystalController = newCrystal.GetComponent<CrystalController> ();
			crystalController.SetType(types[Random.Range(0, types.Length)]);
		} else {
			newCrystal.SetActive(true);
			CrystalController crystalController = newCrystal.GetComponent<CrystalController> ();
			crystalController.Reset();
		}

		newCrystal.transform.position = _originTransform.position;

		Rigidbody2D rigidBody = newCrystal.GetComponent<Rigidbody2D> ();
		rigidBody.velocity = velocity;
	}
}
