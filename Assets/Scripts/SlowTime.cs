using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlowTime : MonoBehaviour
{
    public Sprite btnPressed;
    private Image image;
    private int countSlowTime;
    public Text slowTimeRemaining;
    public SlowTimeObject slowTime;
    private void Start()
    {
        image = GetComponent<Image>();
        if (PlayerPrefs.GetString("First Tap On SlowTime") == "No")
        {
            countSlowTime = 3;
            //countSlowTime = PlayerPrefs.GetInt("Count SlowTime");
            slowTimeRemaining.text = countSlowTime.ToString();
        }
        else
        {
            countSlowTime = 3;
            slowTimeRemaining.text = countSlowTime.ToString();
            PlayerPrefs.SetString("First Tap On SlowTime", "No");
        }
    }
    public void SlowTimeSkill()
    {
        if (countSlowTime == 0)
            return;
        countSlowTime--;
        PlayerPrefs.SetInt("Count SlowTime", countSlowTime);
        slowTimeRemaining.text = PlayerPrefs.GetInt("Count SlowTime").ToString();
        slowTime.SlowMotion();
    }
    private void FixedUpdate()
    {
        if(slowTimeRemaining.IsDestroyed() == true)
            Time.timeScale = 1f;
        if(countSlowTime == 0)
            image.sprite = btnPressed;
    }
}
