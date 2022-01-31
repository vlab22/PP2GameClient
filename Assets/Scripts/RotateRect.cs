using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRect : MonoBehaviour
{
    public RectTransform target;
    public Vector3 axis = Vector3.forward;
    public float speed = 10;
    public Space space = Space.Self;


    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        target.Rotate(axis, speed * Time.deltaTime, space);
    }
}
