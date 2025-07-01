using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float distanceScale; //lower means cam start going faster earlier, higher means faster later (as in player distance)
    public float minSpeed;
    public float maxSpeed;
    public float yOffset;

    void FixedUpdate()
    {
        float speed = minSpeed;

        float dis = Vector2.Distance(this.transform.position, player.transform.position) - yOffset; //distance btwn player and cam

        if (dis < distanceScale)
        {
            speed = Mathf.Lerp(minSpeed, maxSpeed, dis / distanceScale);
        }
        else
        {
            speed = maxSpeed;
        } 

        Vector3 newPos = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0f, yOffset, 0f), speed * Time.deltaTime);
        newPos.z = -10;
        this.transform.position = newPos;
    }
}
