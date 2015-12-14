using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static string MENU_SCENE_NAME = "StartMenu";
	public static string GAME_SCENE_NAME = "Game";
	public static string DEATH_SCENE_NAME = "TheDeath";

	public void LoadStartMenu() {
		SceneManager.LoadScene(MENU_SCENE_NAME);
	}
	
	public void LoadStartLevel() {
		SceneManager.LoadScene(GAME_SCENE_NAME);
	}

	public void LoadDeathWithDelay(long delay) {
		Invoke("LoadDeath", delay);
	}

	public void LoadDeath() {
		Debug.Log("Need to load death");
		SceneManager.LoadScene(DEATH_SCENE_NAME);
	}
	
	public void RestartLevel() {
		SceneManager.LoadScene(GAME_SCENE_NAME);
	}
	
	public void QuitRequest(string name) {
		Debug.Log("Button clicked " + name);
		Application.Quit();
	}
}
