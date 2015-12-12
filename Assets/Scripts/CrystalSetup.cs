using UnityEngine;
using System.Collections;

public class CrystalSetup : MonoBehaviour {

	public enum Type {
		RED,
		GREEN,
		BLUE
	}

	public Sprite[] sprites;

	private Color[] colors = new Color[] { Color.red, Color.blue, Color.green };
	private Type _type = Type.RED;

	// Use this for initialization
	void Start () {
		GameObject textRed = transform.Find("TextRed").gameObject;
		GameObject textBlue = transform.Find("TextBlue").gameObject;
		GameObject textGreen = transform.Find("TextGreen").gameObject;

		GameObject activeText = null;

		switch(_type) {
			case Type.RED:
				activeText = textRed;

				textRed.SetActive(true);
				textBlue.SetActive(false);
				textGreen.SetActive(false);

				break;
			case Type.BLUE:
				activeText = textBlue;

				textRed.SetActive(false);
				textBlue.SetActive(true);
				textGreen.SetActive(false);

				break;
			case Type.GREEN:
				activeText = textGreen;

				textRed.SetActive(false);
				textBlue.SetActive(false);
				textGreen.SetActive(true);
				break;	
		}

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		if (spriteRenderer == null) {
			gameObject.AddComponent<SpriteRenderer> ();
		}

		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

		if (activeText != null) {
			TextMesh textMesh = activeText.GetComponent<TextMesh> ();
			textMesh.color = colors[Random.Range(0, colors.Length)];
		}
	}
	
	public void SetType(Type type) {
		_type = type;
	}
}
