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
        if (other.CompareTag("Victims"))
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
        if(other.CompareTag("Victims"))
        {
            prop = null;
			if(UiButton)
				UiButton.SetActive(false);
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

    public void CaptureVictim()
	{
		Debug.Log("soy capturador");
		int viewID = GetComponentInParent<PhotonView>().ViewID;
		if (prop.CompareTag("Victims"))
		{
			viewID = prop.GetComponent<PhotonView>().ViewID;
			Debug.Log("se envio");
			gameSetup.GetComponent<PhotonView>().RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "catch", "", viewID.ToString());
			prop.GetComponent<SimpleSampleCharacterControl>().SetTimeAnda(); //llamar lo online de jsopeh para que la ande el
		} else if (prop.CompareTag("Props")) {
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
        if(transform.parent.gameObject.CompareTag("Victims"))
		    UiButton.GetComponent<Button>().onClick.AddListener(Capture);
        else
            UiButton.GetComponent<Button>().onClick.AddListener(CaptureVictim);
    }
}
