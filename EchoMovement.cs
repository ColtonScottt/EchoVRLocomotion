using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class EchoMovement : MonoBehavior
{
	[Header("Base")]

	public XRButtonManager xrb;
	public Rigidbody player;

	[Header("Transforms")]
	
	public Transform rHand;
	public Transform lHand;
	public Transform head;

	[Header("Floats")]
	
	public float acceleration = 4.0f;
	public float boostSpeed = 4.0f;
	public float breakDivider = 0.3f;
	public float maxSpeed = 5.0f;

	[Header("Sound Effects")]

	public AudioSource head;
	public List<AudioClip> audioClips = new List<AudioClip>();

	//these are unlisted variables
	
	int boosts;
	bool canBoost;
	
	void FixedUpdate()
	{
		//boosting
		if(xrb.leftPrimaryButton && player.velocity.magnitude)
		{
			if(player.velocity.magnitude <= maxSpeed)
			{
				player.AddForce(lHand.Vector3.forward * acceleration);
			}
			else
			{
				player.AddForce(lHand.Vector3.foward * acceleration);
				player.AddForce(lHand.Vector3.back * acceleration);
			}

			head.PlayOneShot(audioClips[0], PlayerPrefs.GetFloat("SFX Vol"));

		}

		if(xrb.rightPrimaryButton && player.velocity.magnitude)
		{
			if(player.velocity.magnitude <= maxSpeed)
			{
				player.AddForce(rHand.Vector3.forward * acceleration);
			}
			else
			{
				player.AddForce(rHand.Vector3.foward * acceleration);
				player.AddForce(rHand.Vector3.back * acceleration);
			}

			head.PlayOneShot(audioClips[0], PlayerPrefs.GetFloat("SFX Vol"));
		}

		// breaking
		if(xrb.rightThumbStickPressed)
		{
			player.velocity = player.veloctiy / breakDivider;
			head.PlayOneShot(audioClips[1], PlayerPrefs.GetFloat("SFX Vol"));
		}

		// back-boosting
		if(xrb.leftThumbStickPressed && boosts < 0 && canBoost)
		{
			player.AddForce(head.Vector3.forward * boostSpeed, ForceMode.Impulse);
			head.PlayOneShot(audioClips[2], PlayerPrefs.GetFloat("SFX Vol"));
			canBoost = false;
			StartCorutine("CanBoostDelay");
		}
	}

	// boost delay and addition
	// this is so the boosts don't get spammed
	public IEnumerator CanBoostDelay()
	{
		yield return new WaitForSeconds(1);
		canBoost = true;
	}
	
	public IEnumerator AddBoosts()
	{
		yield return new WaitForSeconds(3);
		if(boosts < 2)
		{
			boosts ++;
		}
	}
}