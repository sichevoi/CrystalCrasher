using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoresManager : MonoBehaviour {

	private Text _scoreText;
	private long _score = 0;

	// Use this for initialization
	void Start () {
		_scoreText = FindObjectOfType<Text> ();
		_scoreText.text = "0";
	}

	public void IncrementScore() {
		_score += 1;
		_scoreText.text = _score.ToString();
	}	
}
