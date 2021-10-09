using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerRotate : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    public Vector3 y = Vector3.zero;
	[SerializeField]
	private GameObject joystick;
	[HideInInspector]
	public GameSetupController gameSetup;

	public void FixedUpdate()
    {
		if (!variableJoystick)
			return;
		Vector3 additiveVector = new Vector3(0, variableJoystick.Horizontal, 0);
		ApplyRotation(additiveVector);
		int viewID = GetComponent<PhotonView>().ViewID;
		gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "rotation", JsonUtility.ToJson(additiveVector), viewID.ToString());
	}

	public void ApplyRotation(Vector3 vector)
	{
		transform.Rotate(y + vector, Space.Self);
	}

	public void SpawnJoyStick()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		variableJoystick = Instantiate(joystick, canvas.transform).GetComponent<VariableJoystick>();
	}
}