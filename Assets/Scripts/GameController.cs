using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject[] maps;
    public float timeToSpawnFrom = 2f, timeToSpawnTo = 4.5f;
    public GameObject[] cars;
    public bool isMainScene;
    private int countCars = 0;
    private Coroutine bottomCars, rightCars, leftCars, upCars;
    private bool isLoseOnce;
    public GameObject canvasLosePanel;
    public Text nowScore, topScore, coinsCount;
    public GameObject horn;
    public AudioSource turnSignal;
    private void Start()
    {
        if(PlayerPrefs.GetInt("NowMap") == 2)
        {
            Destroy(maps[0]);
            maps[1].SetActive(true);
            Destroy(maps[2]);
        }
        else if(PlayerPrefs.GetInt("NowMap") == 3)
        {
            Destroy(maps[0]);
            Destroy(maps[1]);
            maps[2].SetActive(true);
            
        }
        else
        {
            maps[0].SetActive(true);
            Destroy(maps[1]);
            Destroy(maps[2]);
        }

        CarController.countCars = 0;
        CarController.isLose = false;
        if (isMainScene)
        {
            timeToSpawnFrom = 4f;
            timeToSpawnTo = 6f;
        }

        bottomCars = StartCoroutine(BottomCars());
        leftCars = StartCoroutine(LeftCars());
        rightCars = StartCoroutine(RightCars());
        upCars = StartCoroutine(UpCars());

        StartCoroutine(CreateHorn());
    }
    IEnumerator BottomCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-2.58f, 0, -42.1f), 180);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator LeftCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-97.7f, 0, -9.2f), 270);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator RightCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(41f, 0, -4.2f), 90);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator UpCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-7.72f, 0, 91.4f), 0, true);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    private void SpawnCar(Vector3 pos, float rotationY, bool isMoveFromUp = false)
    {
        GameObject newObj = Instantiate(cars[Random.Range(0, cars.Length)], pos, Quaternion.Euler(0, rotationY, 0)) as GameObject;
        newObj.name = "Car - " + ++countCars;

        int random = isMainScene ? 1: Random.Range(1, 6);
        if (isMainScene)
        {
            newObj.GetComponent<CarController>().speed = 10f;
        }
        switch (random)
        {
            case 1:
            case 2:
                // Move Right
                newObj.GetComponent<CarController>().rightTurn = true;
                if (PlayerPrefs.GetString("Music") != "No" && !turnSignal.isPlaying)
                {
                    turnSignal.Play();
                    Invoke("StopSound", 2f);
                }
                break;
            case 3:
            case 4:
                // Move Left
                newObj.GetComponent<CarController>().leftTurn = true;
                if (PlayerPrefs.GetString("Music") != "No" && !turnSignal.isPlaying)
                {
                    turnSignal.Play();
                    Invoke("StopSound", 2f);
                }
                if (isMoveFromUp)
                    newObj.GetComponent<CarController>().moveFromUp = true;
                break;
            case 5:
                // Move Forward
                break;
        } 
    }
    private void StopSound()
    {
        turnSignal.Stop();
    }
    private void Update()
    {
        if (CarController.isLose == true && !isLoseOnce)
        {
            StopCoroutine(bottomCars);
            StopCoroutine(leftCars);
            StopCoroutine(rightCars);
            StopCoroutine(upCars);
            nowScore.text = "<color=#F65757>Score: </color><color=black>" + CarController.countCars.ToString() + "</color> " ;
            if (PlayerPrefs.GetInt("Score") < CarController.countCars)
                PlayerPrefs.SetInt("Score", CarController.countCars);
            topScore.text = "<color=#F65757>Best:</color><color=orange> " + PlayerPrefs.GetInt("Score").ToString() + "</color> ";
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + CarController.countCars);
            coinsCount.text = PlayerPrefs.GetInt("Coins").ToString();
            canvasLosePanel.transform.GetChild(7).gameObject.SetActive(false);
            canvasLosePanel.transform.GetChild(8).gameObject.SetActive(false);
            canvasLosePanel.transform.GetChild(6).gameObject.SetActive(true);
            canvasLosePanel.transform.GetChild(5).gameObject.SetActive(true);
            canvasLosePanel.transform.GetChild(4).gameObject.SetActive(true);
            canvasLosePanel.transform.GetChild(3).gameObject.SetActive(true);
            canvasLosePanel.transform.GetChild(2).gameObject.SetActive(true);
            canvasLosePanel.transform.GetChild(1).gameObject.SetActive(true);
            canvasLosePanel.transform.GetChild(0).gameObject.SetActive(true);
            isLoseOnce = true;
        }
    }
    IEnumerator CreateHorn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            if (PlayerPrefs.GetString("Music") != "No")
                Instantiate(horn, Vector3.zero, Quaternion.identity);
        }
    }
}
