using UnityEngine;
using System;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    public GameObject player;
    public float minSpeed;
    public float maxSpeed;
    public float yOffset;
    public float xDirOffset; //how far forward the camera is toward the direction the players looking

    [Tooltip("max dist btwn target camera position and actual camera position for camera to speed up")]
    public float maxDistance;
    [Tooltip("min dist btwn target camera position and actual camera position for camera to speed up")]
    public float minDistance;
    public float catchUpMult;

    private Rigidbody2D rb;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        rb = player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float speed = minSpeed;

        float dir = 1;

        if (PlayerDataManager.Instance.getData().playerDirection == "left")
        {
            dir = -1;
        }

        float playerVelo = Mathf.Abs(rb.linearVelocity.x);

        float defaultMaxPlayerSpeed = 30;
        float maxPlayerSpeed = PlayerDataManager.Instance != null ? 
            PlayerDataManager.Instance.getData().maxHorizontalSpeed : defaultMaxPlayerSpeed;
        
        speed = Mathf.Lerp(minSpeed, maxSpeed, playerVelo / maxPlayerSpeed);

        Vector3 targetPos = player.transform.position + Vector3.right * dir * xDirOffset;

        float dist = Vector2.Distance(targetPos, transform.position);

        if (dist > minDistance)
        {
            speed *= Mathf.Lerp(1, catchUpMult, dist/maxDistance);
        }
        

        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos + new Vector3(0f, yOffset, 0f), speed * Time.deltaTime);
        newPos.z = -10;

        this.transform.position = newPos;
    }

    public void teleportCameraToPlayer() => transform.position = player.transform.position;
}
