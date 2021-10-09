using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    public Vector3 y = Vector3.zero;

    public void FixedUpdate()
    {
		if (!variableJoystick)
			return;
		transform.Rotate(y + new Vector3(0, variableJoystick.Horizontal, 0), Space.Self);
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
    }
}