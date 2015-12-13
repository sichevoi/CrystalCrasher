using UnityEngine;
using System.Collections;

public class CrystalController : MonoBehaviour {

	public enum Type {
		RED,
		GREEN,
		BLUE
	}

	public enum Mode {
		TEXT,
		COLOR
	}

	public Sprite[] sprites;

	private Color[] colors = new Color[] { Color.red, Color.blue, Color.green };
	private Type _type = Type.RED;
	private SpriteRenderer _spriteRenderer;

	private ScoresManager _scoreManager;

	private Mode _mode = Mode.COLOR;

	public static Type GetNext(Type type) {
		switch(type) {
			case Type.RED:
				return Type.GREEN;
			case Type.GREEN:
				return Type.BLUE;
			case Type.BLUE:
			default:
				return Type.RED;
		}
	}

	public static Color GetColor(Type type) {
		switch(type) {
		case Type.RED:
				return Color.red;
			case Type.GREEN:
				return Color.green;
			case Type.BLUE:
			default:
				return Color.blue;
		}
	}

	// Use this for initialization
	void Start () {

		_spriteRenderer = GetComponent<SpriteRenderer> ();
		if (_spriteRenderer == null) {
			gameObject.AddComponent<SpriteRenderer> ();
		}
		_spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

		ApplyType(_type);

		_scoreManager = GetComponentInParent<ScoresManager> ();
	}
	
	public void SetType(Type type) {
		_type = type;
	}

	public void SetMode(Mode mode) {
		_mode = mode;
	}

	public void Hit(Type hitType) {
		if (hitType.Equals(_type)) {
			enabled = false;
			_scoreManager.IncrementScore();
			Destroy(gameObject);
		} else {
			Debug.Log("Hit with a different type, my type is " + _type + " hit type is " + hitType);
		}
	}

	private void ApplyType(Type type) {
		GameObject textRed = transform.Find("TextRed").gameObject;
		GameObject textBlue = transform.Find("TextBlue").gameObject;
		GameObject textGreen = transform.Find("TextGreen").gameObject;

		switch(_mode) {
			case Mode.TEXT:
				ApplyTypeModeText(type, textRed, textBlue, textGreen);
				break;
			case Mode.COLOR:
				ApplyTypeModeColor(type, textRed, textBlue, textGreen);
				break;
		}
	}

	private void ApplyTypeModeText(Type type, GameObject textRed, GameObject textBlue, GameObject textGreen) {
		GameObject activeText = null;

		switch(type) {
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

		if (activeText != null) {
			TextMesh textMesh = activeText.GetComponent<TextMesh> ();
			textMesh.color = colors[Random.Range(0, colors.Length)];
		}
	}

	private void ApplyTypeModeColor(Type type, GameObject textRed, GameObject textBlue, GameObject textGreen) {
		GameObject activeText = null;

		switch(Random.Range(0, 3)) {
			case 0:
				activeText = textRed;

				textRed.SetActive(true);
				textBlue.SetActive(false);
				textGreen.SetActive(false);

				break;
			case 1:
				activeText = textBlue;

				textRed.SetActive(false);
				textBlue.SetActive(true);
				textGreen.SetActive(false);

				break;
			case 2:
				activeText = textGreen;

				textRed.SetActive(false);
				textBlue.SetActive(false);
				textGreen.SetActive(true);
				break;	
		}

		if (activeText != null) {
			TextMesh textMesh = activeText.GetComponent<TextMesh> ();
			textMesh.color = CrystalController.GetColor(type);
		}
	}
}
