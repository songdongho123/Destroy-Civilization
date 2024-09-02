using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpin : MonoBehaviour
{
    public Transform bear;
    public float rotationSpeed = 100f;

    public void ClickRightButton(){
        RotateObject(-1);
    }

    public void ClickLeftButton(){
        RotateObject(1);
    }

    void RotateObject(float direction){
        bear.transform.Rotate(Vector3.up, direction * rotationSpeed);
    }
}
