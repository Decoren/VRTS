using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float timeBetweenBullets = 0.15f;
	public GameObject bullet;
	public GameObject refNozzle;
	public float bulletSpeed;
    private float elapsed;
    private Ray shootRay;
	//public GvrViewer CB;
    public bool canShoot;

	//private PlayerHealth playerHealthScript;

    public void Awake ()
    {
       

    }


    void FixedUpdate ()
    {
		elapsed += Time.deltaTime;
															//|| CB.Triggered==true
		if( canShoot==true && (Input.GetButton ("Fire1")    ) && elapsed >= timeBetweenBullets && Time.timeScale != 0 )
        {
            Shoot ();
        }

		
    }


    void Shoot ()
    {
		elapsed = 0f;
    
		GameObject instancebullet=GameObject.Instantiate(bullet,refNozzle.transform.position   ,refNozzle.transform.rotation*Quaternion.Euler(0,90,0)) as GameObject;
		instancebullet.GetComponent<Rigidbody>().velocity=refNozzle.transform.forward*bulletSpeed;
       	

        
    }
}
