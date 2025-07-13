using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class scrip : MonoBehaviour
{
    public int distance_travel = 6;
    public float starting_pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starting_pos = transform.position.x;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= starting_pos + distance_travel)
        {


            Invoke(nameof(MovePosition), .3f);
        }
        else
        {


            Invoke(nameof(MoveLeft), .3f);

        }

    }



    void MovePosition()
    {



        transform.position = transform.position + new Vector3(.1f, 0);


    }

    void MoveLeft()
    {

        transform.position = transform.position + new Vector3(-.1f, 0);



    }



}
