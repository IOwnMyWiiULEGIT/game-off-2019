﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] Levels LevelData;
    [SerializeField] VoidGameEvent GamePauseEvent;


    //singleton stuff
    public static GameManager instance;
    private bool gamePaused = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if(GameManager.instance != null && GameManager.instance!= this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }

    public void OnResume(){
        gamePaused = false;
    }


    public void ResetLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        ReadPauseInput();
    }

    private void ReadPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
           PauseGame();
        }
        
    }

    public void PauseGame(){
        gamePaused = true;
        GamePauseEvent.Raise();
    }


    public void LoadNextLevel(){
        level nextLevel = LevelData.getNextLevel(SceneManager.GetActiveScene().name);
        if(nextLevel!=null){
            LevelData.currentLevel = nextLevel;
            SceneManager.LoadScene(nextLevel.name);
        }
        else{
            //handle this
            Debug.Log("this is the last level");
        }
    }
    public void LoadPrevLevel(){
        level PrevLevel = LevelData.getPrevLevel(SceneManager.GetActiveScene().name);
        if(PrevLevel!=null){
            SceneManager.LoadScene(PrevLevel.name);
        }
        else{
           //handle this
           Debug.Log("this is the first level");
        }
    }

    public void LoadMainMenu(){
        level MainMenuLevel = LevelData.startLevel;
        if(MainMenuLevel!=null){
            SceneManager.LoadScene(MainMenuLevel.name);
        }
        else{
            Debug.LogError("error: startlevel not found in levels data");
        }
    }

    
}
