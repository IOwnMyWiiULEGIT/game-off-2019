﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Levels levels;
    public void HandlePlayButtonOnClickEvent()
    {
        if(levels.levels.Length>0){
            SceneManager.LoadScene(levels.levels[0].name);
        }
        else{
            Debug.LogError("there are no levels configured in levels data");
        }
        
    }

    /// <summary>
    /// Handles the on click event from the high score button
    /// </summary>
    public void HandleOptionsButtonOnClickEvent()
    {
        MenuManager.GoToMenu(MenuNamesEnum.OptionsMenu);
    }

    /// <summary>
    /// Handles the on click event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}
