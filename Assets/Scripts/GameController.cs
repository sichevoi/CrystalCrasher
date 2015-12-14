using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Sprite lasersBlue;
	public Sprite lasersRed;
	public Sprite lasersGreen;

	private Text _scoreText;
	private long _score = 0;

	private AudioSource _backgroundMusic;
	private GameObject _sirene;
	private SpriteRenderer _lasersRenderer;

	private LevelManager _levelManager;

	private static int GAME_RESTART_TIMEOUT = 3;
	private static float DIFFICULTY_FACTOR = 0.8f;

	private bool _gameOver = false;

	private float _difficiltyModifier = 1;

	void Start () {
		_scoreText = FindObjectOfType<Text> ();
		_scoreText.text = "0";

		_levelManager = FindObjectOfType<LevelManager> ();

		_backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource> ();

		_sirene = GameObject.FindGameObjectWithTag("Sirene");
		_sirene.GetComponent<AudioSource> ().Stop();

		DontDestroyOnLoad(_sirene);

		_lasersRenderer = transform.FindChild("Lasers").GetComponent<SpriteRenderer> ();
		_lasersRenderer.enabled = false;

		Toolbox.Instance.currentScore = 0;
	}

	public bool IsGameOver() {
		return _gameOver;
	}

	public void IncrementScore() {

		if (_gameOver) {
			return;
		}

		_score += 1;
		_scoreText.text = _score.ToString();

		if (_score > 0 && _score % 10 == 0) {
			long numberOfTens = _score / 10;
			float difficulty = 1f;
			for (int i = 0; i < numberOfTens; ++i) {
				difficulty *= DIFFICULTY_FACTOR;
			}
			_difficiltyModifier = difficulty;
		}

		Toolbox.Instance.currentScore = _score;
	}

	public void OnPlayerDeath(CrystalController.Type type) {
		_gameOver = true;

		_sirene.GetComponent<AudioSource> ().Play();
		_backgroundMusic.Stop();

		_lasersRenderer.sprite = GetLasersSprite(type);
		_lasersRenderer.enabled = true;

		_levelManager.LoadDeathWithDelay(GAME_RESTART_TIMEOUT);
	}

	public float GetDifficultyModifier() {
		return _difficiltyModifier;
	}

	private Sprite GetLasersSprite(CrystalController.Type type) {
		switch(type) {
			case CrystalController.Type.BLUE:
				return lasersBlue;
			case CrystalController.Type.GREEN:
				return lasersGreen;
			case CrystalController.Type.RED:
			default:
				return lasersRed;
		}
	}
}
