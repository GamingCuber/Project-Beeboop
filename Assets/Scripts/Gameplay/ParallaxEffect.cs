using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public GameObject player;

    public float parallaxAmount;

    private Vector3 startPos; 

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float distance = player.transform.position.x * parallaxAmount;

        transform.position = new Vector3(startPos.x + distance, startPos.y, startPos.z);
    }
}
