using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
	public Rigidbody rb;

    public void FixedUpdate()
    {
		if (!variableJoystick)
			return;
        Vector3 direction = gameObject.transform.forward * variableJoystick.Vertical + gameObject.transform.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

	public void SpawnJoyStick()
	{

	}
}