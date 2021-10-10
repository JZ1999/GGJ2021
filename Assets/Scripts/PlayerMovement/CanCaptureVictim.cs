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
            UiButton.SetActive(true);
        }
        if (other.CompareTag("Victims"))
        {
            prop = other.gameObject;
            UiButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Props"))
        {
            prop = null;
            UiButton.SetActive(false);
        }
        if(other.CompareTag("Victims"))
        {
            prop = null;
            UiButton.SetActive(false);
        }
    }
    public void Capture()
    {
		prop.GetComponent<Prop>().interactWithProp();
    }

    public void CaptureVictim()
    {
        Debug.Log(prop.name);
        prop.GetComponent<JoystickPlayerExample>().SetTimeAnda(); //llamar lo online de jsopeh para que la ande el
    }
	public void SpawnCapture()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		UiButton = Instantiate(buttonPrefab, canvas.transform);
        if(gameObject.CompareTag("Victims"))
		    UiButton.GetComponent<Button>().onClick.AddListener(Capture);
        else
            UiButton.GetComponent<Button>().onClick.AddListener(CaptureVictim);
    }
}
