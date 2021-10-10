using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CanCaptureVictim : MonoBehaviour
{
    public GameObject UiButton;
	public GameObject buttonPrefab;
	private GameObject prop;
	public GameSetupController gameSetup;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Props"))
        {
            prop = other.gameObject;
			if (UiButton)
			{
				UiButton.SetActive(true);
			}
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Props"))
        {
            prop = null;
			if (UiButton)
			{
				UiButton.SetActive(false);
			}
        }
    }
    public void Capture()
    {
		int viewID = GetComponentInParent<PhotonView>().ViewID;
		if (transform.parent.CompareTag("Victims"))
		{
			PropInfo propInfo = new PropInfo()
			{
				name = prop.GetComponent<Prop>().name,
				interactionType = "activate"
			};
			gameSetup.GetComponent<PhotonView>().RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "prop", JsonUtility.ToJson(propInfo), viewID.ToString());
			prop.GetComponent<Prop>().interactWithProp();	
		} else if(transform.parent.CompareTag("Capturer"))
		{
			PropInfo propInfo = new PropInfo()
			{
				name = prop.GetComponent<Prop>().name,
				interactionType = "deactivate"
			};
			gameSetup.GetComponent<PhotonView>().RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "prop", JsonUtility.ToJson(propInfo), viewID.ToString());
			prop.GetComponent<Prop>().deactivateProp();
		}
		
    }

	public void SpawnCapture()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		UiButton = Instantiate(buttonPrefab, canvas.transform);
		UiButton.GetComponent<Button>().onClick.AddListener(Capture);
	}
}
