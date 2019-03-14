using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void LoadClassicGame()
    {
        SceneManager.LoadScene("ClassicAsteroids");
    }

    public void LoadNebulaGame()
    {
        SceneManager.LoadScene("NebulaAsteroids");
    }

    public void LoadClassicInstructions()
    {
        SceneManager.LoadScene("ClassicInstructions");
    }

    public void LoadClassicMainMenu()
    {
        SceneManager.LoadScene("ClassicMainMenu");
    }

    public void LoadNebulaMainMenu()
    {
        SceneManager.LoadScene("NebulaMainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
