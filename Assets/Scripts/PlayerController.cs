using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;

}


public class PlayerController : MonoBehaviour 
{
	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

	private float nextFire;
	private Rigidbody rb;
	private AudioSource audioSource ;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource >();
	}

	void Update()
	{
		//if (Input.GetButton("Fire1") && Time.time > nextFire) //MIK
		if( CrossPlatformInputManager.GetButton("Fire") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audioSource.Play();
		}
	}

	void FixedUpdate () 
	{
		float moveHorizontal = 0.0f;
		float moveVertical = 0.0f;

		#if UNITY_IOS
			moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");	
			moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");	
		#endif

		#if UNITY_EDITOR
			moveHorizontal = Input.GetAxis ("Horizontal");	
			moveVertical = Input.GetAxis ("Vertical");	
		#endif

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;

		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

	}
}
