using UnityEngine;
using System.Collections;


public class badGuyAi : MonoBehaviour {
	public int Hp = 50;
	public int maxHp=50;
	public int Speed =3;
	public int DefaultSpeed=3;
	public int Damage=2;
	public int sightRadius=7;
	public int maxDistanceFromHome=10;
	public int myGoldValue=5;
	public int myRenown=1;
	public int myExp=5;
    public float lastherosightingtimer=0;
	public float turnSpeed = 10.0f;
	public Vector3 homeBaseLocation;
	
	public Vector3 movingTowards;
	public Vector3 lookAt;
	
	public Quaternion myRotate;
	
	public Transform myTransform;
	public Rigidbody myRigidbody;
	
	public GameObject enemyTarget;
	public HeroAI heroai;
	public float distancetotarget;
	public Color myColor;
	public Color myCurrentColor;
	public bool isGround=false;
	public bool canAttack = true;
	public float attackSpeed = 1.0f;
	public float attackRange= 1.0f;

	public LayerMask Mask;
	private RaycastHit hit;
	
	
	
	public enum EnemyState
		
	{
		Idle,
		Attacking,
		Randommovemntget,
		Randommovent,
		
	};
	
	public EnemyState myState;
	// Use this for initialization
	void Start () {
		//gameObject.name=Random.Range(0,100).ToString();
	//	myColor= gameObject.GetComponent<Renderer>().material.color;
		//GetComponent<Renderer>().enabled=true;
		//myColor=Color.green;
		myTransform=transform;
		homeBaseLocation=myTransform.position;
		myRigidbody=GetComponent<Rigidbody>();
		myState=EnemyState.Randommovemntget;

		myCurrentColor = myColor;
        //sets the collision dector
        gameObject.GetComponent<SphereCollider> ().radius =sightRadius/myTransform.lossyScale.x ;
	}
	void FixedUpdate()
	{
		
		if (isGround ==true)
		{
			
			myRigidbody.velocity=Speed*myTransform.forward;
			Debug.DrawRay(myTransform.position,myTransform.forward,Color.green);
			Debug.DrawRay(myTransform.position,lookAt,Color.yellow);


			//Quaternion deltaRotation = Quaternion.Euler(0.0f,lookAt.y,0.0f);
			//myRigidbody.MoveRotation(myRigidbody.rotation * deltaRotation);



		}
		
	}
	// Update is called once per frame
	void Update () {
	Moving();
		
		myCurrentColor.r=(myColor.r*((float)Hp/(float)maxHp));
		myCurrentColor.g=(myColor.g*((float)Hp/(float)maxHp));
		myCurrentColor.b=(myColor.b*((float)Hp/(float)maxHp));
	//	myColor.r=((float)Hp/(float)maxHp);	
		gameObject.GetComponent<Renderer> ().material.color = myCurrentColor;

		//Debug.DrawRay(myTransform.position,myTransform.forward*10f);
        if (lastherosightingtimer != 0 && lastherosightingtimer < Time.time)
        {
            GetComponent<Renderer>().enabled=false;
           // gameObject.GetComponent<EnemyHpDisplay>().GetComponent<Renderer>().enabled = false;
            gameObject.GetComponentInChildren<EnemyHpDisplay>().GetComponent<Renderer>().enabled = false;
            lastherosightingtimer = 0;
        }

        if (Hp<=0)
        {
            Iamdead();


        }

	
	}
	
	void Iamdead()
	{
		Collider[] hitformoneyCollider = Physics.OverlapSphere(myTransform.position,15);
		int i = 0;
		
		while (i < hitformoneyCollider.Length)
		{
		//&&(hitformoneyCollider[i].collider.)
			if ((hitformoneyCollider[i].gameObject.tag=="Heros")&&(hitformoneyCollider[i].isTrigger==false))
			{
			
				hitformoneyCollider[i].gameObject.GetComponent<HeroAI>().HeroGold= hitformoneyCollider[i].gameObject.GetComponent<HeroAI>().HeroGold +myGoldValue;
				hitformoneyCollider[i].gameObject.GetComponent<HeroAI>().killCount=hitformoneyCollider[i].gameObject.GetComponent<HeroAI>().killCount+1;
				hitformoneyCollider[i].gameObject.GetComponent<HeroAI>().heroExp=hitformoneyCollider[i].gameObject.GetComponent<HeroAI>().heroExp+myExp;
				GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().AddRenown(myRenown);
			//	Debug.Log(hitformoneyCollider[i].collider.ToString());
				
			}
			
			i=i+1;
		}
		
		Destroy(gameObject);
		
	}
	

	
	void Fighting(){
		
		
		
		
	}
	
	void Moving(){
		Debug.DrawLine(myTransform.position,movingTowards,Color.red);

		if (myState==EnemyState.Randommovemntget)
		{
	
			//Debug.Log(homeBaseLocation.y-maxDistanceFromHome);
			RaycastHit hit2;

			movingTowards = new Vector3(Random.Range(homeBaseLocation.x-maxDistanceFromHome,homeBaseLocation.x+maxDistanceFromHome),homeBaseLocation.y-10.0f,Random.Range(homeBaseLocation.z-maxDistanceFromHome,homeBaseLocation.z+maxDistanceFromHome));
			Physics.Raycast(movingTowards, new Vector3(0,50.0f,0), out hit2,50.0f,1<<14) ;

			Debug.DrawLine(myTransform.position,hit2.point,Color.magenta);

			movingTowards = hit2.point + new Vector3(0, GetComponent<Collider>().bounds.size.y/2, 0);        
			
			myState=EnemyState.Randommovent;
			Speed=DefaultSpeed;
				
			
		}
		else if(myState==EnemyState.Attacking)
		{
			RaycastHit temp;
			// if error here make sure mask is set ***************************
			Physics.Raycast (myTransform.position, enemyTarget.transform.position-myTransform.position,out temp,30.0f,Mask, QueryTriggerInteraction.Ignore);
		//	Debug.DrawRay (myTransform.position+myTransform.up , enemyTarget.transform.position-myTransform.position, Color.blue);
			//Debug.DrawRay
			//movingTowards=enemyTarget.transform.position;
			movingTowards=temp.point;
			enemyTarget = temp.collider.gameObject;
		//	if(Vector3.Distance(myTransform.position,enemyTarget.transform.position) <=attackRange)
			if(temp.distance <=attackRange)
			{
				Speed=0;	
				
			}
			else
			{
				
			Speed = DefaultSpeed;	
			}
			
			if((temp.distance <=attackRange)&&canAttack==true)
			{
				canAttack=false;
				heroai.TakeDamage(Damage);
				StartCoroutine(AttackSpeedWait(attackSpeed));
				
			}
			
			
		}
		else if(myState==EnemyState.Randommovent)
		{
			
			if (Vector3.Distance(myTransform.position,movingTowards)<=3)
			{
				Speed=0;
				
				myState=EnemyState.Idle;
				
				StartCoroutine(Waitfornewdirection(10.0f));
				myState = EnemyState.Randommovemntget;
			}
			
		}
		//movingTowards= new Vector3(movingTowards.x,.5f,movingTowards.z);
		lookAt = (movingTowards- myTransform.position);
		//lookAt =lookAt.normalized;
		
		if (( Physics.Raycast( myTransform.position - (myTransform.right*.25f)+myTransform.up, myTransform.forward , out hit, 3.0f,~((1<<10)|(1<<9)|(1<<12)|(1<<8)|(1<<16)), QueryTriggerInteraction.Ignore))  && Speed != 0)
			{
				
				Debug.DrawLine(myTransform.position,hit.point,Color.blue);
			   // Debug.Log (hit.collider.name +this.name+this.homeBaseLocation);
		

				lookAt=lookAt + hit.normal *40.0f;
			
			}
		else if(( Physics.Raycast( myTransform.position + (myTransform.right*.25f)+myTransform.up, myTransform.forward , out hit, 3.0f,~((1<<10)|(1<<9)|(1<<12)|(1<<8)|(1<<16)), QueryTriggerInteraction.Ignore))  && Speed != 0)
			{
				
				Debug.DrawLine(myTransform.position,hit.point,Color.red);
			  //  Debug.Log (hit.collider.name+this.name+this.homeBaseLocation);
		
			
				lookAt=lookAt + hit.normal *40.0f;
			
			}
	
		
        Debug.DrawRay(myTransform.position,myTransform.forward*sightRadius,Color.cyan);
		
		myRotate = Quaternion.LookRotation(lookAt.normalized);

		myTransform.rotation = Quaternion.Slerp(myTransform.rotation,myRotate,Time.deltaTime*turnSpeed);
		
		
		
		
	}

	
		IEnumerator Waitfornewdirection(float wait)
	{
		
		yield return new WaitForSeconds (wait);
		
		
		Speed=DefaultSpeed;
	
	}
	
		IEnumerator AttackSpeedWait(float wait)
	{
		
		yield return new WaitForSeconds (wait);
		
		
		canAttack=true;
	
	}
	
	public void TakeDamage(int damage)
	{
		
	Hp= Hp-damage;
        if (Hp<=0)
        {
            Iamdead();

        }
	}
	
	
	void OnCollisionEnter(Collision other){
		
		if (other.gameObject.name =="Ground")		
		{
			isGround = true;	
		}
		

		
	}
	void OnCollisionExit(Collision other){
		
		if (other.gameObject.name =="Ground")		
		{
			isGround = false;	
		}

		
	}
	
		void OnTriggerEnter (Collider other)
	{

	}
	
	void OnTriggerExit (Collider other)
	{
		

		if ((other.GetComponent<Collider>().tag =="Heros") && enemyTarget != null )
		{
			
			enemyTarget = null;
			heroai =null;
			myState=EnemyState.Randommovemntget;
		}
		
	}
	
	void OnTriggerStay(Collider other)
	{
		
		
		
		if ((other.tag =="Heros") && enemyTarget == null )
		{
            Collider[] colliders = Physics.OverlapSphere(myTransform.position, sightRadius,1<<10);
			if (colliders.Length!= 0)
			{
				GetComponent<Renderer>().enabled=true;
                 
              //  gameObject.GetComponent<EnemyHpDisplay>().GetComponent<Renderer>().enabled = true;
                gameObject.GetComponentInChildren<EnemyHpDisplay>().GetComponent<Renderer>().enabled = true;
				
				Collider nearestCollider = null;
				float minSqrDistance = Mathf.Infinity;
				
				for (int i = 0; i < colliders.Length; i++)
				{
					float sqrDistanceToCenter = (myTransform.position - colliders[i].transform.position).sqrMagnitude;
					
					if (sqrDistanceToCenter < minSqrDistance)
					{
						minSqrDistance = sqrDistanceToCenter;
						nearestCollider = colliders[i];
					}
				}
				
			//	Debug.Log (nearestCollider.tag+nearestCollider.name+this.name);
				enemyTarget=nearestCollider.gameObject;
				heroai = enemyTarget.GetComponent<HeroAI>();
				myState=EnemyState.Attacking;
				movingTowards= enemyTarget.transform.position;
			}
			
		}
        if (other.tag == "Heros")
        {

            lastherosightingtimer = Time.time+10;
        }
		
		
	}

	public void ChangeMySize(Vector3 change)
	{

		this.transform.localScale += new Vector3 (change.x - this.transform.localScale.x, change.y - this.transform.localScale.y, change.z - this.transform.localScale.z);


	}
}
