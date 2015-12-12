using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAtack : MonoBehaviour {

	public GameObject projectileRed;
	public GameObject projectileGreen;
	public GameObject projectileBlue;

	private LaserScript _laser;
	private Image _colorImage;

	// Use this for initialization
	void Start () {
		_laser = transform.FindChild("Gun/GunTip").GetComponent<LaserScript> ();
		_colorImage = FindObjectOfType<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			CrystalController.Type nextType = CrystalController.GetNext(_laser.GetGunType());
			_laser.SetGunType(nextType);
			_colorImage.color = CrystalController.GetColor(nextType);
		}
	}
}
