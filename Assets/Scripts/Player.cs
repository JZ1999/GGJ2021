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

		if (GetComponent<PhotonView>().IsMine)
		{
			playerCamera.SetActive(true);
			GetComponent<SimpleSampleCharacterControl>().SpawnJoyStick();
			//GetComponent<PlayerRotate>().SpawnJoyStick();
			//GetComponent<Jump>().SpawnJumpButton();

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
