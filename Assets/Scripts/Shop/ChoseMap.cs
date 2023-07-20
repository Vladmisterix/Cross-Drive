using UnityEngine;

public class ChoseMap : MonoBehaviour
{

    public AudioClip btnClick;
    public void ChooseNewMap(int numberMap)
    {
        if (PlayerPrefs.GetString("Music") != "No")
        {
            GetComponent<AudioSource>().clip = btnClick;
            GetComponent<AudioSource>().Play();
        }
            

        PlayerPrefs.SetInt("NowMap", numberMap);
        GetComponent<CheckMaps>().WhichMapSelection();
    }
}
