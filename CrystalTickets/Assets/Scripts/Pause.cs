﻿using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	public bool paused = false;
	public PlayerController playercontroller;
	
	void Update()
	{

	}
	
	void OnGUI()
	{
		if(paused)
		{
			GUILayout.Label("Game is paused!");
			if(GUILayout.Button("Resume")){

			paused = togglePause();
			}

			if(GUILayout.Button("Restart")){
				Application.LoadLevel("LevelOne");
				paused = togglePause();
			}

			if(GUILayout.Button("Back to main menu")){
				Application.LoadLevel("GameStartScene");
				paused = togglePause();
			}


		}
	}

	public void OnMouseDown(){
		paused = togglePause();
	}

	bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
			playercontroller.isPause = false;
			return(false);
		}
		else
		{
			Time.timeScale = 0f;
			playercontroller.isPause = true;
			return(true);    
		}
	}
}

