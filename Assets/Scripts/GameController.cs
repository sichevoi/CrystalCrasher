using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	private Text _scoreText;
	private long _score = 0;

	private AudioSource _backgroundMusic;
	private GameObject _sirene;
	private Transform _secutiry;

	private LevelManager _levelManager;

	void Start () {
		_scoreText = FindObjectOfType<Text> ();
		_scoreText.text = "0";

		_levelManager = FindObjectOfType<LevelManager> ();

		_backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource> ();
		_sirene = GameObject.FindGameObjectWithTag("Sirene");
		DontDestroyOnLoad(_sirene);

		_secutiry = transform.FindChild("Security");

		Toolbox.Instance.currentScore = 0;
	}

	public void IncrementScore() {
		_score += 1;
		_scoreText.text = _score.ToString();

		Toolbox.Instance.currentScore = _score;
	}

	public void OnPlayerDeath(CrystalController.Type type) {
		_sirene.GetComponent<AudioSource> ().Play();
		_backgroundMusic.Stop();

		foreach (LaserSecurity laserSecurity in _secutiry.GetComponentsInChildren<LaserSecurity> ()) {
			laserSecurity.SetLaserColor(CrystalController.GetColor(type));
			laserSecurity.SecurityEnable(true);
		}

		_levelManager.LoadDeathWithDelay(5);
	}
}
