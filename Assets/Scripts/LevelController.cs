using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

    [SerializeField] float delayBeforeScreenLoad = 1f;
    public static LevelController Instance = null;
    [SerializeField] int currentSceneIndex = 0;
    [SerializeField] string[] allScenes;

    private bool isPaused = false;

    void Awake()
    {
        //PlayerPrefs.DeleteKey("Health");
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        isPaused = false;
        allScenes = GetAllScenes();
        currentSceneIndex = 0;
    }

    public void LoadStartMenu()
    {
        LevelController.Instance.LoadStartMenuByInstance();
    }

    public void LoadFirstLevel()
    {
        //ScoreHandler.instance.ResetScores();
        LevelController.Instance.LoadFirstLevelByInstance();
    }

    public void LoadFirstLevelWithoutScoreReset()
    {
        LevelController.Instance.LoadFirstLevelByInstance();
    }

    private void LoadStartMenuByInstance()
    {
        currentSceneIndex = 0;
        //ShieldCountHandler.Instance.ResetShields();
        if (allScenes == null && allScenes.Length == 0)
        {
            allScenes = GetAllScenes();
        }

        SceneManager.LoadScene(allScenes[0]);
    }

    private void LoadFirstLevelByInstance()
    {
        currentSceneIndex = 1;
        print("Loading first level " + currentSceneIndex);
        SceneManager.LoadScene(allScenes[currentSceneIndex]);
    }

    public void LoadNextLevel()
    {
        try
        {
            currentSceneIndex++;

            //if (currentSceneIndex >= allScenes.Length - 2)
            //{
            //    currentSceneIndex = allScenes.Length - 2;
            //}
            
            Debug.Log("Loading Scene : " + allScenes[currentSceneIndex]);
            //StartCoroutine(DelayBeforeLoadingScene(allScenes[currentSceneIndex]));
            SceneManager.LoadScene(allScenes[currentSceneIndex]);
        }
        catch (Exception ex)
        {
            currentSceneIndex--;
            throw ex;
        }
        finally
        {
        }
    }

    public void LoadGameOverScene()
    {
        currentSceneIndex = allScenes.Length - 1;
        StartCoroutine(DelayBeforeLoadingScene(allScenes[currentSceneIndex]));
    }

    public void QuitGame()
    {
        //ScoreHandler.instance.ResetScores();
        Application.Quit();
    }

    IEnumerator DelayBeforeLoadingScene(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeScreenLoad);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    private string[] GetAllScenes()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }

        return scenes;
    }

    public string GetCurrentLevel()
    {
        return allScenes[currentSceneIndex];
    }

    public void TogglePause()
    {
        var playButton = GameObject.FindGameObjectWithTag("PlayButton");
        var pauseButton = GameObject.FindGameObjectWithTag("PauseButton");

        if (isPaused)
        {
            isPaused = false;
            if (playButton != null && pauseButton != null)
            {
                playButton.GetComponent<Image>().enabled = false;
                pauseButton.GetComponent<Image>().enabled = true;
            }
            Time.timeScale = 1.0f;
        }
        else
        {
            isPaused = true;
            if (playButton != null && pauseButton != null)
            {
                pauseButton.GetComponent<Image>().enabled = false;
                playButton.GetComponent<Image>().enabled = true;
            }
            Time.timeScale = 0f;
        }
    }
}
