using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float moveVertical { get; private set; }
    public float moveHorizontal { get; private set; }
    public Vector3 mousePosition { get; private set; }
    
    public bool numberOnePushDown {  get; private set; }
    public bool numberTwoPushDown { get; private set; }
    public bool leftMouseButtonPushed { get; private set; }
    public bool leftShiftButtonPushed { get; private set; }


    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        mousePosition = Input.mousePosition;
        numberOnePushDown = Input.GetKeyDown(KeyCode.Alpha1);
        numberTwoPushDown = Input.GetKeyDown(KeyCode.Alpha2);
        leftMouseButtonPushed = Input.GetMouseButton(0);
        leftShiftButtonPushed = Input.GetKey(KeyCode.LeftShift);
    }
}
