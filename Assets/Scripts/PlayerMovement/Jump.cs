using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Jump : MonoBehaviour
{
	[SerializeField]
	[Range(200, 400)]
	private float jumpForce = 0;
	public Rigidbody rb;

	[SerializeField]
	private bool isGrounded = true;

	[SerializeField]
	private GameObject jumpButton;
	[SerializeField]
	private GameObject jumpButtonPrefab;

	public GameSetupController gameSetup;

	void Update()
    {

#if UNITY_EDITOR
		bool shouldJump = Input.GetKeyDown(KeyCode.Space);
		ApplyJump(shouldJump);
#endif

	}

	public void ApplyJump(bool shouldJump)
	{

		if (shouldJump && isGrounded)
		{
			isGrounded = false;
			Vector3 jumpVector = Vector3.up * jumpForce;
			rb.AddForce(jumpVector);
			int viewID = GetComponent<PhotonView>().ViewID;
			gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "jump", JsonUtility.ToJson(jumpVector), viewID.ToString());
		}	
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Floor"))
		{
			isGrounded = true;
		}
	}

	public void SpawnJumpButton()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		jumpButton = Instantiate(jumpButtonPrefab, canvas.transform);
		jumpButton.GetComponent<Button>().onClick.AddListener(delegate { ApplyJump(true); });

		Debug.Log(jumpButton);
	}
}
