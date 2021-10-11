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
	public string propName;
	public WinLoseManager gameManager;
	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (propSound.isPlaying)
			gameManager.AddSum(0.001f);
	}

	public void interactWithProp()
	{
		activatePropSound();
		activateAnimator();
		activateParticles();
	}

	public void deactivateProp()
	{
		deactivatePropSound();
		deactivateAnimator();
		deactivateParticles();
	}

	private void activateParticles()
	{
		if (particles)
		{
			Instantiate(particles, transform);
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

	private void deactivateParticles()
	{
		if (particles)
		{
			GameObject particleObject = transform.GetComponentInChildren<ParticleSystem>()?.gameObject;
			if(particleObject)
				Destroy(particleObject);
		}
	}

	private void deactivatePropSound()
	{
		if (propSound.isPlaying)
			propSound.Stop();
	}

	private void deactivateAnimator()
	{
		anim.SetBool("Shake", false);
	}

}
