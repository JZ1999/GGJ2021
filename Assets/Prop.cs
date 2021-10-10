using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
	[SerializeField]
	private AudioSource propSound;
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private GameObject particles;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void interactWithProp()
	{
		activatePropSound();
		activateAnimator();
		activateParticles();
	}

	private void activateParticles()
	{
		if (particles)
		{
			Instantiate(particles, transform);
			Destroy(particles, 1000);
		}
	}

	private void activatePropSound()
	{
		if(!propSound.isPlaying)
			propSound.Play();
	}

	private void activateAnimator()
	{
		anim.SetBool("Shake", true);
	}
}
