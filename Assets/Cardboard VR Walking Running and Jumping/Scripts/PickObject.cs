using UnityEngine;
using System.Collections;

public class PickObject : MonoBehaviour {

	// Use this for initialization
	public Transform refPosition;
	public Transform head;
	public float rotatingSpeed=0.5f;
	public float ampli=0.1f;
	public PlayerShooting shoot;

	Vector3 zeropos;
	void Start () {
		zeropos=transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.rotation=Quaternion.Euler(0,Time.fixedTime*rotatingSpeed,0);
		transform.position=zeropos+new Vector3(0,ampli*Mathf.Sin( Time.fixedTime*rotatingSpeed/5),0);
	}

	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag=="Player")
		{

			//attach weapon to player
			transform.position=refPosition.position;
			transform.rotation=refPosition.rotation;
			shoot.canShoot=true;
			transform.parent=head;

			Destroy(this);
		}
	}
}
