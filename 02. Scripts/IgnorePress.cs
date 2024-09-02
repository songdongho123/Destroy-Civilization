using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePress : MonoBehaviour
{
    bool ig;
    // Start is called before the first frame update
    void Start()
    {
        ig = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ig && Input.GetKeyDown(KeyCode.E))
        {
            return;
        }
    }
}
