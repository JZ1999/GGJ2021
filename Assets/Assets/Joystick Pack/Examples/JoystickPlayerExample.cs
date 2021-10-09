﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
	public Rigidbody rb;
	[SerializeField]
	private GameObject joystick;
	[HideInInspector]
	public GameSetupController gameSetup;

    public void FixedUpdate()
    {
		if (!variableJoystick)
			return;

		if (variableJoystick.Vertical == 0 && variableJoystick.Vertical == 0)
			return;
        Vector3 direction = gameObject.transform.forward * variableJoystick.Vertical + gameObject.transform.right * variableJoystick.Horizontal;
		ApplyForce(direction);
		int viewID = GetComponent<PhotonView>().ViewID;
		gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "movement", JsonUtility.ToJson(direction), viewID.ToString());
    }

	public void ApplyForce(Vector3 direction)
	{
		rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

	public void SpawnJoyStick()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		variableJoystick = Instantiate(joystick, canvas.transform).GetComponent<VariableJoystick>();
	}
}