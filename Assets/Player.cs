using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{

	[SerializeField]
	private GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log(GetComponent<PhotonView>().IsMine);

		if (GetComponent<PhotonView>().IsMine)
		{
			playerCamera.SetActive(true);
			GetComponent<JoystickPlayerExample>().SpawnJoyStick();
			GetComponent<PlayerRotate>().SpawnJoyStick();

			if (GetComponentInChildren<CanCaptureVictim>()) {
				GetComponentInChildren<CanCaptureVictim>().SpawnCapture();
			}
				
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
