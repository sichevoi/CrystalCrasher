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

	void Start () {
		_scoreText = FindObjectOfType<Text> ();
		_scoreText.text = "0";

		_levelManager = FindObjectOfType<LevelManager> ();

		_backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource> ();
		_sirene = GameObject.FindGameObjectWithTag("Sirene");
		DontDestroyOnLoad(_sirene);

		_lasersRenderer = transform.FindChild("Lasers").GetComponent<SpriteRenderer> ();
		_lasersRenderer.enabled = false;

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

		_lasersRenderer.sprite = GetLasersSprite(type);
		_lasersRenderer.enabled = true;

		_levelManager.LoadDeathWithDelay(GAME_RESTART_TIMEOUT);
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
