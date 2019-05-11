using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    [SerializeField] AudioClip buttonPressSFX;
    [SerializeField] [Range(0, 1)] float buttonPressVolume = 0.75f;

    public void LoadMainMenu()
    {
        LoadScene(0);

    }
    public void LoadPlayground()
    {
        ResetScore();
        LoadScene("Playground");
    }
    public void LoadGameOver(float delayInSeconds)
    {
        LoadScene("GameOver", delayInSeconds);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ResetScore()
    {
        GameSession.Instance.ResetGame();
    }
    void LoadScene(string sceneName)
    {
        StartCoroutine(WaitUntilButtonSoundEnd(sceneName));
    }
    void LoadScene(int sceneIndex)
    {
        StartCoroutine(WaitUntilButtonSoundEnd(sceneIndex));
    }
    void LoadScene(string sceneName, float delayInSeconds)
    {
        StartCoroutine(WaitUntilButtonSoundEnd(sceneName, delayInSeconds));
    }
    private IEnumerator WaitUntilButtonSoundEnd(string sceneName)
    {
        if (buttonPressSFX != null)
        {
            AudioSource.PlayClipAtPoint(buttonPressSFX, Camera.main.transform.position, buttonPressVolume);
            yield return new WaitForSeconds(buttonPressSFX.length);
        }
        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator WaitUntilButtonSoundEnd(string sceneName, float delayInSeconds)
    {
        if (buttonPressSFX != null)
        {
            AudioSource.PlayClipAtPoint(buttonPressSFX, Camera.main.transform.position, buttonPressVolume);
            yield return new WaitForSeconds(delayInSeconds);
        }
        else
        {
            yield return new WaitForSeconds(delayInSeconds);
        }
        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator WaitUntilButtonSoundEnd(int sceneIndex)
    {
        if (buttonPressSFX != null)
        {
            AudioSource.PlayClipAtPoint(buttonPressSFX, Camera.main.transform.position, buttonPressVolume);
            yield return new WaitForSeconds(buttonPressSFX.length);
        }
        SceneManager.LoadScene(sceneIndex);
    }
}
