using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CanCaptureVictim : MonoBehaviour
{
    public GameObject UiButton;
	public GameObject buttonPrefab;
	private GameObject victim;
	public GameSetupController gameSetup;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Victims"))
        {
            victim = other.gameObject;
            UiButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Victims"))
        {
            victim = null;
            UiButton.SetActive(false);
        }
    }
    public void Capture()
    {
        gameSetup.Destroy(victim.GetComponent<PhotonView>().ViewID);
    }

	public void SpawnCapture()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		UiButton = Instantiate(buttonPrefab, canvas.transform);
		UiButton.GetComponent<Button>().onClick.AddListener(Capture);
	}
}
