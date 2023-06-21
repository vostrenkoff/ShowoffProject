using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultVector : MonoBehaviour
{
    public Vector2 DefaultVector2;
    public bool choosen = false;

    private void Start()
    {
        choosen = false;
        DefaultVector2 = transform.position;
    }
}
