using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public AudioClip crash;
    public AudioClip[] accelerates;
    public float speed = 15f, force = 50f;
    private Rigidbody carRb;
    public bool rightTurn, leftTurn, moveFromUp;
    private float originRotationY, rotateMultRight = 8f, rotateMultLeft = 6f;
    private Camera mainCam;
    public LayerMask carsLayer;
    private bool isMovingFast, carCrashed;
    [NonSerialized] public bool carPassed;
    [NonSerialized] public static bool isLose;
    [NonSerialized] public static int countCars;
    public GameObject turnLeftSignal, turnRightSignal, explosion, exhaust;
    
    private void Start()
    {
        mainCam = Camera.main;
        carRb = GetComponent<Rigidbody>();
        originRotationY = transform.eulerAngles.y;

        if (rightTurn)
            StartCoroutine(TurnSignals(turnRightSignal));
        else if(leftTurn)
            StartCoroutine(TurnSignals(turnLeftSignal));
    }
    IEnumerator TurnSignals(GameObject turnSignal)
    {
        while (!carPassed)
        {
            turnSignal.SetActive(!turnSignal.activeSelf);
            yield return new WaitForSeconds(0.25f);
        }
    }
    private void FixedUpdate()
    {
        carRb.MovePosition(transform.position - transform.forward * speed * Time.fixedDeltaTime);
    }
    private void Update()
    {
# if UNITY_EDITOR
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
#else
        if (Input.touchCount == 0)
            return;
        Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);
#endif

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, carsLayer))
        {
            string carName = hit.transform.gameObject.name;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !isMovingFast && gameObject.name == carName)
            {
#else       
            if (Input.GetTouch(0).phase == TouchPhase.Began && !isMovingFast && gameObject.name == carName)
            {
#endif
                if(transform.rotation == Quaternion.Euler(0,270,0) || transform.rotation == Quaternion.Euler(0, 90, 0))
                {
                    GameObject vfxExhaustRotation = Instantiate(exhaust, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0, 90, 0)) as GameObject;
                    Destroy(vfxExhaustRotation, 2f);
                }
                else
                {
                    GameObject vfxExhaust = Instantiate(exhaust, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    Destroy(vfxExhaust, 2f);
                }
                speed *= 2;
                isMovingFast = true;
                if (PlayerPrefs.GetString("Music") != "No")
                {
                    GetComponent<AudioSource>().clip = accelerates[UnityEngine.Random.Range(0, accelerates.Length)];
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") && !carCrashed)
        {
            carCrashed = true;
            isLose = true;
            speed = 0f;
            collision.gameObject.GetComponent<CarController>().speed = 0f;

            GameObject vfxExplosion = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(vfxExplosion, 5f);

            if (isMovingFast)
                force *= 1.5f;
            carRb.AddRelativeForce(Vector3.back * force);
            if (PlayerPrefs.GetString("Music") != "No")
            {
                GetComponent<AudioSource>().clip = crash;
                GetComponent<AudioSource>().Play();
            }
        }
            
    }
    private void OnTriggerStay(Collider other)
    {
        if (carCrashed)
            return;
        if (other.transform.CompareTag("TurnBlock Right") && rightTurn)
            RotateCar(rotateMultRight);
        else if (other.transform.CompareTag("TurnBlock Left") && leftTurn)
            RotateCar(rotateMultLeft, -1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Car") && other.GetComponent<CarController>().carPassed)
            other.GetComponent<CarController>().speed = speed + 5f;
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (carCrashed)
            return;
        if (other.transform.gameObject.CompareTag("Trigger pass"))
        {
            if (carPassed)
                return;
            carPassed = true;
            Collider[] colliders = GetComponents<BoxCollider>();
            foreach (Collider col in colliders)
                col.enabled = true;
            countCars++;
        }

        if (other.transform.CompareTag("TurnBlock Right") && rightTurn)
            carRb.rotation = Quaternion.Euler(0, originRotationY + 90, 0);
        else if (other.transform.CompareTag("TurnBlock Left") && leftTurn)
            carRb.rotation = Quaternion.Euler(0, originRotationY - 90, 0);
        else if (other.transform.CompareTag("Delete Trigger"))
            Destroy(gameObject);
    }

    private void RotateCar(float speedRotate, int dir = 1)
    {
        if (carCrashed)
            return;
        if (dir == -1 && transform.localRotation.eulerAngles.y < originRotationY - 90f)
            return;
        if (dir == -1 && moveFromUp && transform.localRotation.eulerAngles.y > 250f && transform.localRotation.eulerAngles.y < 270f)
            return;
        float rotateSpeed = speed * speedRotate * dir;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotateSpeed, 0) * Time.fixedDeltaTime);
        carRb.MoveRotation(carRb.rotation * deltaRotation);
    }
}
