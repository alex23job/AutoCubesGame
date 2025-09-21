using UnityEngine;

public class Wheel : MonoBehaviour
{
    private float speedRotate;
    private bool isRotate = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TurnForward();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.z += Time.deltaTime * speedRotate * 3;
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    public void TurnForward()
    {
        speedRotate = -50f;
        isRotate = true;
    }

    public void TurnBackward()
    {
        speedRotate = 50f;
        isRotate = true;
    }

    public void TurnStop()
    {
        isRotate = false;
        speedRotate = 0;
    }
}
