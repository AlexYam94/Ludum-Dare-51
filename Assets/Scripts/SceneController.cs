using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] int _gameOverSceneIndex;

    public void LoadNextScene()
    {
        StartCoroutine(LoadNexSceneCoroutine());
    }

    public void StartGame()
    {
        LoadNextScene();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(_gameOverSceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator LoadNexSceneCoroutine()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;


        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentIndex + 1);

        while (!asyncOperation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        //if (SceneManager.GetActiveScene().name.ToLower().CompareTo("gameover") != 0)
        //{
        //    PlayerPrefs.SetInt("lastSceneIndex", currentIndex + 1);
        //    SavingController.GetInstance().Save();
        //}
    }
}
