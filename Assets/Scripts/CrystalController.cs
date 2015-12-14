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
	public Transform gun;

	private Color[] colors = new Color[] { Color.red, Color.blue, Color.green };
	private Type _type = Type.RED;
	private Mode _mode = Mode.TEXT;

	private SpriteRenderer _spriteRenderer;
	private GameController _gameController;
	private CrystalsSpawner _spawner;
	private Collider2D _collider;
	private Rigidbody2D _rigidBody;

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

		_gameController = FindObjectOfType<GameController> ();
		_spawner = GetComponentInParent<CrystalsSpawner> ();
		_collider = GetComponent<PolygonCollider2D> ();
		_rigidBody = GetComponent<Rigidbody2D> ();
	}
	
	public void SetCrystalType(Type type) {
		_type = type;
	}

	public Type GetCrystalType() {
		return _type;
	}

	public void SetMode(Mode mode) {
		_mode = mode;
	}

	public void Reset() {
		_collider.enabled = true;
		StopCoroutine("SmoothMovement");
		transform.localScale = Vector3.one;
	}

	public void Hit(Type hitType) {
		if (hitType.Equals(_type)) {
			StartCoroutine (SmoothMovement (gun.position));
			_collider.enabled = false;
			_gameController.IncrementScore();
		} else {
			Debug.Log("Hit with a different type, my type is " + _type + " hit type is " + hitType);
			_spawner.OnMisHit(gameObject, _type);
		}
	}

	//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float remainingDistance = (transform.position - end).x;
        float moveTime = 0.02f;
        float inverseMoveTime = 1 / moveTime;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while(remainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(_rigidBody.position, end, inverseMoveTime * Time.deltaTime);
			float targetDiff = Time.deltaTime * 5;
			float currentScale = transform.localScale.x;
			float scaleDiff = targetDiff < currentScale ? targetDiff : currentScale;
			Vector3 newScale = new Vector3(scaleDiff, scaleDiff, 1);
            
            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			transform.localScale -= newScale;
            _rigidBody.MovePosition (newPostion);

            //Recalculate the remaining distance after moving.
			remainingDistance = (transform.position - end).x;
            
            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

		_spawner.ReturnToPool(gameObject);
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
