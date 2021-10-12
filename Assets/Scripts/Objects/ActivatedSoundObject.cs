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
    [Range(1, 15)]
    public float sumItemBroken;
    
    // Update is called once per frame
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero && rb.velocity.magnitude > 1)
        {
            velocity = rb.velocity.magnitude;
        }
        else
        {
            if(velocity != 0)
            {
                
                if (triggerObject)
                {
                    CalcultatedSound();
                }
                velocity = 0;
            }
        }
    }

    private void CalcultatedSound() {
        switch (Mathf.RoundToInt(velocity))
        {
            case 0:
                break;
            case 1:
                break;
            default:
                winLoseManager.AddSum(velocity * 10);
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
