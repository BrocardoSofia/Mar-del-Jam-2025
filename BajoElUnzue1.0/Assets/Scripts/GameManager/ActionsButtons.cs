using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ActionsButtons : MonoBehaviour
{
    public AudioClip finalBueno;
    public void goHome()
    {
        SceneManager.LoadScene("0.Menu");
    }

    public void startGame()
    {
        SceneManager.LoadScene("0.Inicio");
    }

    public void iniciarNivel()
    {
        SceneManager.LoadScene("1.Level1");
    }

    public void iniciarCreditos()
    {
        MusicManager.Instance.PlayMusic(finalBueno);
        SceneManager.LoadScene("01.GameOver2");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("0.GameOver");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
