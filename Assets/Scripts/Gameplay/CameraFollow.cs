using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float distanceScale; //lower means cam start going faster earlier, higher means faster later (as in player distance)
    public float minSpeed;
    public float maxSpeed;
    public float yOffset;
    public float xDirOffset; //how far forward the camera is toward the direction the players looking

    void FixedUpdate()
    {
        float speed = minSpeed;

        float dir = 1;

        if (PlayerDataManager.Instance.getData().playerDirection == "left")
        {
            dir = -1;
        }

        float dis = Vector2.Distance(this.transform.position + Vector3.down * yOffset - Vector3.right * dir * xDirOffset, player.transform.position); //distance btwn player and cam

        if (dis < distanceScale)
        {
            speed = Mathf.Lerp(minSpeed, maxSpeed, dis / distanceScale);
        }
        else
        {
            speed = maxSpeed;
        }

        Vector3 targetPos = player.transform.position + Vector3.right * dir * xDirOffset;

        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos + new Vector3(0f, yOffset, 0f), speed * Time.deltaTime);
        newPos.z = -10;
        this.transform.position = newPos;
    }
}
