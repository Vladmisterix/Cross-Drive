using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanvasButtons : MonoBehaviour
{
    public Sprite btn, btnPressed, musicOn, musicOff;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();

        if(gameObject.name == "Music Button")
        {
            if (PlayerPrefs.GetString("Music") == "No")
                transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
    }
    public void MusicButton()
    {
        if(PlayerPrefs.GetString("Music") == "No")
        {
            // Turn On
            PlayerPrefs.SetString("Music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            // Turn Off
            PlayerPrefs.SetString("Music", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
        PlayButtonSound();
    }
    public void ShopScene()
    {
        StartCoroutine(LoadScene("Shop"));
        PlayButtonSound();
    }
    public void ExitShopScene()
    {
        StartCoroutine(LoadScene("Main"));
        PlayButtonSound();
    }
    public void PlayGame()
    {
        if(PlayerPrefs.GetString("First Game") == "No")
        {
            StartCoroutine(LoadScene("Game"));
        }
        else
            StartCoroutine(LoadScene("Study"));
        PlayButtonSound();
    }
    public void RestartGame()
    {
        StartCoroutine(LoadScene("Game"));
        PlayButtonSound();
    }
    public void SetPressedButton()
    {
        image.sprite = btnPressed;
        transform.GetChild(0).localPosition -= new Vector3(0, 5f, 0);
    }
    public void SetDefultButton()
    {
        image.sprite = btn;
        transform.GetChild(0).localPosition += new Vector3(0, 5f, 0);
    }
    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(name);
    }
    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("Music") != "No")
            GetComponent<AudioSource>().Play();
    }
}
