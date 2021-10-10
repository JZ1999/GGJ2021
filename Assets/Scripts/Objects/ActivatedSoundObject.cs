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
            case 0:
                Debug.Log("muy poquito");
                break;
            case 1:
                Debug.Log("poquito");
                break;
            case 2:
                Debug.Log("poco");
                break;
            case 3:
                Debug.Log("normal");
                break;
            case 4:
                Debug.Log("mucho");
                break;
            default:
                winLoseManager.AddSum(velocity);
                audioSource.clip =  soundSO.sourceList[0].GetComponent<AudioSource>().clip;
                audioSource.Play();
                break;
        };
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
