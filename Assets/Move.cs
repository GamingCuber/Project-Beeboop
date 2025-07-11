using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public int distance_travel = 6;
    public float starting_pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starting_pos = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= starting_pos + distance_travel)
        {


            Invoke(nameof(MovePosition), 1);
        }
        else
        {


            Invoke(nameof(MoveLeft), 1);

        }

    }



    void MovePosition()
    {



        transform.position = transform.position + new Vector3(0, .1f);


    }

    void MoveLeft()
    {

        transform.position = transform.position + new Vector3(0, -.1f);



    }



}
