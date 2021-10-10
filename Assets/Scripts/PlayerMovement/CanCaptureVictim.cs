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
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Props"))
        {
            prop = null;
            UiButton.SetActive(false);
        }
    }
    public void Capture()
    {
		prop.GetComponent<Prop>().interactWithProp();
    }

	public void SpawnCapture()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		UiButton = Instantiate(buttonPrefab, canvas.transform);
		UiButton.GetComponent<Button>().onClick.AddListener(Capture);
	}
}
