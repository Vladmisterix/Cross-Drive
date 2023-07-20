using UnityEngine.UI;
using UnityEngine;

public class BuyMapCoins : MonoBehaviour
{
    public AudioClip success, failed;
    public Text coinsCount;
    public GameObject coins1000, coins5000, money0_99, money1_99, cityButton, megapolisButton;
    public Animation coinsText;

    private void Start()
    {
        PlayerPrefs.DeleteKey("City");
    }

    public void BuyNewMap(int needCoins)
    {
        

        int coins = PlayerPrefs.GetInt("Coins");
        if(coins < needCoins)
        {
            if (PlayerPrefs.GetString("Music") != "No")
            {
                GetComponent<AudioSource>().clip = failed;
                GetComponent<AudioSource>().Play();
            }
            coinsText.Play();
        }
            
        else
        {
            // Buy map
            switch (needCoins)
            {
                case 1000:
                    PlayerPrefs.SetString("City", "Open");
                    PlayerPrefs.SetInt("NowMap", 2);
                    GetComponent<CheckMaps>().WhichMapSelection();
                    coins1000.SetActive(false);
                    money0_99.SetActive(false);
                    cityButton.SetActive(true);
                    break;
                case 5000:
                    PlayerPrefs.SetString("Megapolis", "Open");
                    PlayerPrefs.SetInt("NowMap", 3);
                    GetComponent<CheckMaps>().WhichMapSelection();
                    coins5000.SetActive(false);
                    money1_99.SetActive(false);
                    megapolisButton.SetActive(true);
                    break;
            }
            int nowCoins = coins - needCoins;
            coinsCount.text = nowCoins.ToString();
            PlayerPrefs.SetInt("Coins", nowCoins);

            if (PlayerPrefs.GetString("Music") != "No")
            {
                GetComponent<AudioSource>().clip = success;
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
