using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedSoundObject : MonoBehaviour
{
    public float velocity;
    private Rigidbody rb;
    private GameObject triggerObject;
    public SoundSO soundSO;
    public AudioSource audioSource;
    public WinLoseManager winLoseManager;
    public GameObject unbrokenObject;
    public GameObject brokenObject;
    
    // Update is called once per frame
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero)
        {
            velocity = rb.velocity.magnitude;
        }
        else
        {
            if(velocity != 0)
            {
                
                if (triggerObject)
                {
                    Debug.Log("entro");
                    CalcultatedSound();
                }
                velocity = 0;
            }
        }
    }

    private void CalcultatedSound() {
        switch (Mathf.RoundToInt(velocity))
        {
            default:
                winLoseManager.AddSum(velocity);
                brokenObject.transform.position = gameObject.transform.position;
                brokenObject.SetActive(true);
                audioSource.clip =  soundSO.sourceList[0].GetComponent<AudioSource>().clip;
                audioSource.Play();
                unbrokenObject.SetActive(false);
                break;
        };
        audioSource.clip = null;
    }
    private void OnCollisionEnter(Collision collision)
    {

        triggerObject = collision.gameObject;
    }
    private void OnTriggerExit()
    {
        triggerObject = null;
    }
}
