using UnityEngine;

public class TimerGear : MonoBehaviour
{
    public RectTransform rect;

    public float minSpeed;

    public float maxSpeed;

    private float speed;

    public float dir; //-1 or 1 please

    void Update()
    {
        rect.Rotate(0, 0, speed * Time.deltaTime * dir);
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}
