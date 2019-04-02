using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameController : MonoBehaviour
{
    private GamepadInput ci1 = new GamepadInput();

    private GamepadInput ci2 = new GamepadInput();

    // Start is called before the first frame update
    void Start()
    {
        ci1.InitController(1);
        ci2.InitController(2);
    }

    // Update is called once per frame
    void Update()
    {
        var x1 = ci1.GetJoystickAxis(GamepadJoystick.LeftJoystickVertical);
        var x3 = ci1.GetJoystickAxis(GamepadJoystick.LeftJoystickHorizontal);
        var x2 = ci1.GetJoystickAxis(GamepadJoystick.RightJoystickVertical);
        var x4 = ci1.GetJoystickAxis(GamepadJoystick.RightJoystickHorizontal);


        if (x1 != 0)
        {
            Debug.Log("Left vertical: " + x1);
        }
        
        if (x2 != 0)
        {
            Debug.Log("Right vertical: " + x2);
        }
        
        if (x3 != 0)
        {
            Debug.Log("Left horizontal: " + x3);
        }
        
        if (x4 != 0)
        {
            Debug.Log("Right horizontal: " + x4);
        }
    }
}