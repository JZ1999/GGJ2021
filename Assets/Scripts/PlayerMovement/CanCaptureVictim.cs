using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanCaptureVictim : MonoBehaviour
{
    public GameObject UiButton;
    public GameObject victim; 
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
        Destroy(victim);
    }
}
