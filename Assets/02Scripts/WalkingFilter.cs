using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WalkingFilter : MonoBehaviour {

	// Use this for initialization

	// this two variables are used to simulate the accelerometer in the editor mode by setting the bool to true
	// and moving the slider alternatevely
	[Range(0f , 5f)]
	public float debugAcc;
	public bool debugAccelerationOn=true;
	//animation of the player game object, used to call the walking state
	//public Animator animPlayer;

	// all these variables are used in the sript to manage the detection of acceleration variation peaks,
	// which is used at the same time, to detect when the player wants to move
	public float lowerLiM=-1.3f;
	public float upperLiM=-0.7f;
	public float walkMinTime=0.8f;
	public float speedFactor=0.01f;
	public float jumpTresHold=-0.6f;
	public float deltaDerivative=0.05f;
	public float derivativeThreshold=20;
	float acc_j_1,acc_j;
	public float Dacc;
	public bool jumping;
	public float JumpSpeed=250f;
	public float timeJump=1f;
	float max=0;


	//these are the time dependent variables used to obtain the derivate of the acceleration
	float LOWtime;
	float UPtime;
	float elapsed;

	// the head inside the GvrViewer
	public Transform head;
	// the accelerations in the 3 axis
	float accX,accY,accZ;
	// the player's rigid body
	Rigidbody RB;


	void Start ()
    {

		//set the values of time to limits
		UPtime=1000;
		LOWtime=500;

		//get the rigidbody of the player
		RB=gameObject.GetComponent<Rigidbody>();
		elapsed=0;

		Invoke("restartMax",2);

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

		//obtain accelerations
		accX=Input.acceleration.x;
		accY=Input.acceleration.y;
		accZ=Input.acceleration.z;

		// override the acceleration on the editor 
		if (debugAccelerationOn) {
			accY = -debugAcc;
		}



		elapsed+=Time.fixedDeltaTime;


		//obtain derivative for NOT for each timeUpdate, but instead:
		if(elapsed > deltaDerivative) 
		{
			acc_j=accY;

			//derivative expression
			Dacc=Mathf.Abs( (acc_j-acc_j_1)/elapsed);


			// obtain max value using comparison
			max=Mathf.Max(Dacc,max);

			// reset last iteration
			acc_j_1=accY;

			elapsed=0;
		}

		//check downstep
		if(accY>upperLiM)
		{	
			UPtime=Time.fixedTime;
		}
		else if(Time.fixedTime-UPtime>walkMinTime)
		{
			UPtime=1000;
		}
	
		//check upstep
		if(accY<lowerLiM)
		{	
			LOWtime=Time.fixedTime;
		}
		else if(Time.fixedTime-UPtime>walkMinTime)
		{
			UPtime=500;
		}


		//check jump
		if (Mathf.Abs (UPtime - LOWtime) < walkMinTime && Dacc < derivativeThreshold) {
			
			move (1 / Mathf.Abs (UPtime - LOWtime));
			//animPlayer.SetFloat("Forward",10);
		} else {
			//animPlayer.SetFloat("Forward",-1);
		}

		if(Dacc > derivativeThreshold && jumping==false)
		{
			jump();

		}


	}


	//set the animation to walk
	public void move(float v)
	{
		//use with static animations
		RB.MovePosition(transform.position+head.transform.forward*v*speedFactor+transform.up*0.01f);

		//animPlayer.SetFloat("Forward",1);
		
	}


	// JUMPING FUNCTION: change animation here to jump
	public void jump()
	{

		RB.AddForce(new Vector3(0,JumpSpeed*RB.mass,0));
		Invoke("stopJump",timeJump);

		jumping=true;
		//animPlayer.SetFloat("Jump",1);

	}


	// it is called after a certain time to stop jumping
	public void stopJump()
	{

		//RB.AddForce(JumpSpeed*RB.mass/10*head.forward);
		jumping=false;
		//animPlayer.SetFloat("Jump",-1);
	}


	public void restartMax()
	{
		max=0;
	}

}
