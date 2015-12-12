using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoresManager : MonoBehaviour {

	public string scoreText = "Score: ";

	private Text _scoreText;
	private long _score = 0;

	// Use this for initialization
	void Start () {
		_scoreText = FindObjectOfType<Text> ();
	}

	public void IncrementScore() {
		_score += 1;
		_scoreText.text = scoreText + _score;
	}	
}
