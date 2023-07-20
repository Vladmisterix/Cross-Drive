using UnityEngine;

public class MovementCar : MonoBehaviour
{
    public GameObject canvasFirst, secondCar, canvasSecond;
    private bool isFirst;
    private CarController controller;
    private void Start()
    {
        controller = GetComponent<CarController>();
    }
    private void Update()
    {
        if(transform.position.x < 8f && !isFirst)
        {
            isFirst = true;
            controller.speed = 0f;
            canvasFirst.SetActive(true);
            canvasSecond.SetActive(false);
        }
    }
    private void OnMouseDown()
    {
        if (!isFirst || transform.position.x > 9f)
            return;
        controller.speed = 15f;
        canvasFirst.SetActive(false);
        canvasSecond.SetActive(true);
        secondCar.GetComponent<CarController>().speed = 12f;
    }
}
