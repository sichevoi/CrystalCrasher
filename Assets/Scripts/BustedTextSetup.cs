using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BustedTextSetup : MonoBehaviour {

	public int[] scores = new int[] {10, 50, 100, 1000};
	public string[] texts = new string[] {
		"You stole {0} crystals. At least you'll be able to buy cigaretes in prison.",
		"But you managed to steal {0} crystals. This would be enough to get yourself a luxery cell!",
		"But you managed to steal {0} crystals. This would be enough to bribe the judge!",
		"But you managed to steal {0} crystals. This would be enough to bribe the whole state!"};

	// Use this for initialization
	void Start () {
		Text bustedText = GetComponent<Text> ();
		long score = Toolbox.Instance.currentScore;

		for (int i = 0; i < scores.Length; ++i) {
			if (score < scores[i]) {
				bustedText.text = string.Format(texts[i], score);
				break;
			}
		}
	}
}
