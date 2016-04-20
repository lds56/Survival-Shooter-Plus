using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {

	Canvas canvas;
	bool isPaused;

	void Start()
	{
		canvas = GetComponent<Canvas>();
		isPaused = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			canvas.enabled = !canvas.enabled;
			Pause();
			SetCursor();
		}
	}

	public void Pause()
	{
		isPaused = !isPaused;
		Time.timeScale = isPaused ? 0 : 1;
	}

	public void SetCursor() {
		if (Camera.main.name == "PlayerCamera") {
			if (isPaused) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}

	public void Quit()
	{
#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
	}
}
