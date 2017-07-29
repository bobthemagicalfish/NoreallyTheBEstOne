using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//didd you gert thisssssssssssssssssssssssssssssssss
//or thissss
public class HeroAI : MonoBehaviour {
//stats default
	public int heroHP = 100;
	public int maxHeroHp=100;
	public int HeroGold=20;
	public int heroStamina = 50;
	public int maxHeroStamina=100;
	public int Speed = 5;
	public int defaultSpeed = 5;
	public int attackSpeed=1;
	public int killCount=0;
	public int heroExp=0;
	public int heroWaittime =0;
	public int heroAttackwaittime=0;
	public int heroSightRadius = 10;
    public int enemiesAroundMe = 0;
	public int HeroLevel = 1;
	public int HeroIdNumber = -1;
	//public int MaxWeight;
//	public int CurrentWeight;
	public int staminacounter=0;

	// section for ai shoping stuff
	public string ShopingQualityAtLeastName = "Common";
	public int ShopingQualityAtLeastNum = 31;

	public Dictionary<string,int> Weapontypebias = new Dictionary<string, int> ();
	public Dictionary<string,int> Armortypebias = new Dictionary<string, int> ();
	public Dictionary<string,int> Weaponstatsbias =new Dictionary<string, int> ();
	public Dictionary<string,int> Armorstatsbias =new Dictionary<string, int> ();
	public Dictionary<string,int> ArmorClassbias = new Dictionary<string, int> ();

	public bool Ranged= false;
	public int WeaponArmorbias = 0;
	public int ShoppingBuyingModif = 0;



	private Color makeSeeThur = new Color(0f,0f,1f,0.5f);
	
	private float turnSpeed = 5f;
	private float shoulder = 0.75f;
	private float avoidBuildings = 1.0f;
	private float findBuildings = 3.0f;
	public float heroWaittimestamp=0.0f;
	public float heroAttacktimestamp=0.0f;
	public float TimeBeforeTryDelivery = 0.0f;
	public float LastTimeShopped=0.0f;
	public float MaxDistanceForHerosz=0.0f;
	public float MinDistanceForHerosz=-40.0f;
	public float MaxDistanceForHerosx=50.0f;


	public float[] maxxz = new float[2];

	public bool Isground =false;
    public bool  BlackSmithBought=false;
	public bool lefthit =false;




	public Vector3 heroMovement;
	public Vector3 heroLookat ;
	public Vector3 GoingTo;

	public List<Vector3> GoAroundWaypoints;
	public List<Vector3> MissionWaypoints;

	public Quaternion heroRotate;
	
	private GameObject Incomegameobject;
	private GameObject inn;
	public GameObject heroController;
	public GameObject myCurrentBounty;
	public GameObject enemyTarget;
	public PlayerController BountyList;
	public BuildingLocation MyBuildings;
	public ResNode resNodeTarget;
	public GameObject ShoppingAt;

	private HeroSpawner heroSpawner;
	private MainMoney mainMon;
	private InnMenu innMenu;
	private badGuyAi badguyai;
	public HeroInv MyInv ;
	//public DeliveryItem MyPackage;
//	public PickupInfo MyPickup;

	public Transform myTransform;
	public Rigidbody myRigid;
	
	
	private Vector3 leftShoulder;
	private Vector3 rightShoulder;
	private Vector3 centermass;
	
	public RaycastHit hit;
	public RaycastHit fogHit;

	public enum Role
	{
		Hero,
		Worker,
	    Merchant,

	}
		

	public Role myrole;
	
	public bool canAttack=true;
	public bool heroHPLOW=false;


	public enum HeroState
	{
		Idle,
		Exploring,//heros only
		Market,
		Attacking,
		Inn,
		Bounty,
		WaitingAtInn,
		RunningAway,
		Dead,
		Goingup,
		Store,
		DeliveryPickup,
		DeliveryDeliver,
		Working,//workers only
	};
	
	public Color myColor;
	public Color myCurrentColor;
	
	public HeroState heroState;

	public GameObject FogOfwarmesh;


	public Vector3 tempoy ;


	// for workers only
	public GameObject WorkPlace;
	public GameObject Home;
	public WorkerController MyWorkControl;
	public LayerMask mask;
	void Start()
	{
		gameObject.GetComponent<SphereCollider> ().radius = heroSightRadius;
	//	gameObject.GetComponent<Renderer>().material.color=Color.blue;
	//	myColor=gameObject.GetComponent<Renderer>().material.color;
	//	myColor = Color.blue;
		myCurrentColor=myColor;
		heroState = HeroState.Inn;
        MainMoney.BlackBought +=Blacksmith;
		myTransform=transform;
		myRigid=GetComponent<Rigidbody>();
		MyWorkControl = GameObject.FindGameObjectWithTag ("WorkerController").GetComponent<WorkerController> ();
		GoAroundWaypoints = new List<Vector3> ();
		MyInv = gameObject.GetComponent<HeroInv> ();
		MyBuildings = GameObject.Find ("BuildingManager").GetComponent<BuildingLocation> ();


        BountyList= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		heroSpawner = GameObject.Find("HeroController").GetComponent<HeroSpawner>();

		if (myrole == Role.Hero) {
			maxxz = heroSpawner.HowFarCanIGo (HeroLevel);

			MaxDistanceForHerosx = maxxz [0];
			MaxDistanceForHerosz = maxxz [1];
		}
		//gameObject.name= "Hero Number " + Random.Range(1,100).ToString();
		
		Incomegameobject= GameObject.FindGameObjectWithTag("PlayerTotals");
		mainMon = Incomegameobject.GetComponent<MainMoney>();	
		
		inn = GameObject.FindGameObjectWithTag("Inn");	
		innMenu = inn.GetComponent<InnMenu>();


		
		
	
		
	}
	
	void FixedUpdate(){
		//chedcks to make sure on the ground
		if ((Isground ==true)&&heroState!=HeroState.Dead){
		myRigid.velocity =heroMovement;
		}
	//	Terrain.activeTerrain.detailObjectDistance = 400;
	}
	 public RaycastHit temp;
	void Update () {
		CheckForBounty();

		Moving ();
	
        DoIRun();

	


		myCurrentColor.r=(myColor.r*((float)heroHP/(float)maxHeroHp));
		myCurrentColor.g=(myColor.g*((float)heroHP/(float)maxHeroHp));
		myCurrentColor.b=(myColor.b*((float)heroHP/(float)maxHeroHp));

		gameObject.GetComponent<Renderer>().material.color=myCurrentColor;
		//MaxDistanceForHerosz=heroSpawner.maxDistancez;
	//	MaxDistanceForHerosx = heroSpawner.maxDistancex;

		if (heroHP<=0)
		{
			heroState=HeroState.Dead;	
			IAmDead();	
		}
		


	}





    void DoIRun()
    {
        if (heroHP<=(maxHeroHp/5) || ((heroHP<=(maxHeroHp/2)&&heroState==HeroState.Exploring)))
        {
            heroHPLOW=true;
            Speed=defaultSpeed;
            heroState=HeroState.Inn;    
        }



     
    }
	

	void Moving()
	{	
		
		
		leftShoulder=myTransform.position - (myTransform.right * shoulder);
		rightShoulder = myTransform.position  + (myTransform.right * shoulder);
		centermass=myTransform.position;
		
		
		if(heroState==HeroState.Attacking)
		{
			if (enemyTarget==null )
			{
				badguyai=null;
				heroState=HeroState.Idle;
				Speed=defaultSpeed;
			}
			else
			{

				temp = new RaycastHit ();
				Physics.Raycast (myTransform.position, enemyTarget.transform.position-myTransform.position,out temp,30.0f,mask, QueryTriggerInteraction.Ignore);
				Debug.DrawRay (myTransform.position , enemyTarget.transform.position-myTransform.position, Color.green);
			//	Debug.DrawLine (myTransform.position, enemyTarget.transform.position,Color.green);
//				Debug.Log(Vector3.Distance(myTransform.position, enemyTarget.transform.position)+this.name);
//				enemyTarget = temp.collider.gameObject;
			  //  GoingTo= new Vector3(enemyTarget.transform.position.x,enemyTarget.transform.position.y,enemyTarget.transform.position.z);
				GoingTo= new Vector3(temp.point.x,temp.point.y,temp.point.z);
				//if (Vector3.Distance(temp.point,myTransform.position)<=gameObject.GetComponent<HeroInv>().MainHandWeapon.Range)
				if (temp.distance<=MyInv.MainHandWeapon.Range)
				{
					Speed=0;
	
				}
				else
				{
				Speed =defaultSpeed;	
				}
				
				if ((temp.distance<=MyInv.MainHandWeapon.Range)&&canAttack==true)
				{
					
					badguyai.TakeDamage(MyInv.MainHandWeapon.Damage);
					canAttack=false;
					StartCoroutine(AttackSpeedWait(attackSpeed));
					
				}
			}
			
		}
		// how hero deals with each boyunty
		if(heroState == HeroState.Bounty)
		{
			if (myCurrentBounty!=null)
			{
				
				//Debug.Log(myCurrentBounty.GetComponent<BountyController>().bountyType.ToString());
				
				if (myCurrentBounty.GetComponent<BountyController>().GetBountyType()=="Explore")
					{
						if ( Vector3.Distance(myTransform.position,myCurrentBounty.transform.position) >=5)
						{
							GoingTo = myCurrentBounty.transform.position;
//							Debug.Log("going their" + this.name);
						}
						else
						{
						 myCurrentBounty.GetComponent<BountyController>().Explored();
						}
					
			
			
				}
				else if (myCurrentBounty.GetComponent<BountyController>().GetBountyType()=="Destory")
				{
						if ( Vector3.Distance(myTransform.position,myCurrentBounty.transform.position) >=myCurrentBounty.GetComponent<SphereCollider>().radius)
						{
								GoingTo = myCurrentBounty.transform.position;
							
						}
						else
						{
							 GoingTo = new Vector3(Random.Range((myCurrentBounty.transform.position.x+myCurrentBounty.GetComponent<SphereCollider>().radius),(myCurrentBounty.transform.position.x-myCurrentBounty.GetComponent<SphereCollider>().radius)),1.5f,Random.Range((myCurrentBounty.transform.position.z+myCurrentBounty.GetComponent<SphereCollider>().radius),(myCurrentBounty.transform.position.z-myCurrentBounty.GetComponent<SphereCollider>().radius)));
						
						}
					
				}
				
				else if (myCurrentBounty.GetComponent<BountyController>().GetBountyType()=="Guard")
				{
						if ( Vector3.Distance(myTransform.position,myCurrentBounty.transform.position) >=myCurrentBounty.GetComponent<SphereCollider>().radius)
						{
								GoingTo = myCurrentBounty.transform.position;
							
						}
						else
						{
							 GoingTo = new Vector3(Random.Range((myCurrentBounty.transform.position.x+myCurrentBounty.GetComponent<SphereCollider>().radius),(myCurrentBounty.transform.position.x-myCurrentBounty.GetComponent<SphereCollider>().radius)),1.5f,Random.Range((myCurrentBounty.transform.position.z+myCurrentBounty.GetComponent<SphereCollider>().radius),(myCurrentBounty.transform.position.z-myCurrentBounty.GetComponent<SphereCollider>().radius)));
						
						}
					
				}
				
				
			}
			else 
			{
				heroState = HeroState.Idle;	
				
			}
			
			
			
		}
		
		
				// hero randomly exploring
		if (heroState==HeroState.Exploring)
		{
			if (myCurrentBounty!=null)
			{
				heroState=HeroState.Bounty;	
				
			}
			if (enemyTarget != null) {
				enemyTarget = null;
			}
			if (Vector3.Distance(GoingTo,myTransform.position) <=20)
			{
				Speed=0;
				//heroState=HeroState.Waiting;
				
				StartCoroutine(WaitInn(2.0f));
				
			}
			
		}
		
		if (heroState == HeroState.DeliveryPickup) {
			if (Vector3.Distance (GoingTo, myTransform.position) <= 5) {
				Speed = 0;
				// checking to see what im picking up at to get package from each type
				if (MyInv.MyPickup.Pickup.tag == "ResourceNode") {
			//		resNodeTarget=DeliveryPickupAt.GetComponent<ResNode> ();

					//	if (MyPickup.Pickup.GetComponent<ResNode> ().DoYouNeedPickup (HeroIdNumber) == true) {
				
					if ( MyInv.NewDelivry( MyInv.MyPickup.Pickup.GetComponent<ResNode> ().PickupIsHere (MyInv.WeightLeft())) ==true) {
								heroState = HeroState.DeliveryDeliver;
								Speed = defaultSpeed;
						 

							} else {
								
							//	resNodeTarget = null;
								heroState = HeroState.Idle;
							
								Speed = defaultSpeed;
						   


							}

//						} else {
//
//							MyPickup.Pickup.GetComponent<ResNode> ().IDontWanna (HeroIdNumber);
//							//resNodeTarget = null;
//							MyPickup = new PickupInfo();
//							heroState = HeroState.Idle;
//							Speed = defaultSpeed;
//						}

				}
			} else {
				GoingTo = MyInv.PickupGoingto();

			}

		}

		if (heroState == HeroState.DeliveryDeliver) {
			if (Vector3.Distance (myTransform.position, MyInv.DevGoingto()) < 5) {
				Speed = 0;
				MyInv.DropOff ();
				heroState = HeroState.Idle;
				Speed = defaultSpeed;



			} else {
				GoingTo = MyInv.DevGoingto();
			}



		}

		// finding the inn to recharge
		if (heroState == HeroState.Inn)
		{
			/*if (myCurrentBounty!=null)
			{
				myCurrentBounty.GetComponent<BountyController>().RemoveHero(gameObject);	
				myCurrentBounty=null;
				
			}*/
			GoingTo = inn.transform.position;
		
			GoingTo.x = GoingTo.x+2.0f;
			GoingTo.y = GoingTo.y +0.5f;
			if ( Physics.Raycast(centermass ,inn.transform.position- myTransform.position , out hit, findBuildings) )
			{
				if (hit.collider.tag == "Inn")
				{
					GoAroundWaypoints=new List<Vector3>();
					Debug.DrawLine(rightShoulder,hit.point,Color.cyan);
					Speed=0;
					heroHPLOW=false;
					canAttack=true;
					if (myrole == Role.Hero) {
						if (innMenu.costOfInn <= HeroGold) {
							mainMon.AddGold (innMenu.costOfInn);
					
							HeroGold = HeroGold - innMenu.costOfInn;
							//heroStamina=100;
							//	heroHP = maxHeroHp;
							//sets hero waiting 
							heroWaittime = 5;
							heroWaittimestamp = Time.time + heroWaittime;
							heroState = HeroState.WaitingAtInn;
						

							//	StartCoroutine(WaitInn(5.0f));
						} else {
							Debug.Log ("broke");
							canAttack = true;
							heroWaittime = 10;
							heroWaittimestamp = Time.time + 10;
							heroState = HeroState.WaitingAtInn;
						
							//StartCoroutine(WaitInn(5.0f));
						}
					}
					else{
						heroWaittime = 5;
						heroWaittimestamp = Time.time + heroWaittime;
						heroState = HeroState.WaitingAtInn;


					}
				
				}
				
			}
			Debug.DrawRay (centermass,  inn.transform.position-myTransform.position , Color.blue);
		
		}

		if (heroState==HeroState.WaitingAtInn)
		{

            if ((heroHP < maxHeroHp) && (heroStamina < maxHeroStamina))
            {
                heroHP = heroHP + 1;
                heroStamina = heroStamina + 1;

            }
            else if (heroHP < maxHeroHp)
            {
                heroHP = heroHP + 1;
            }
            else if (heroStamina < maxHeroStamina)
            {
                heroStamina = heroStamina + 1;

            }
            else if (Time.time <= heroWaittime)
            {

            }
			else{
				heroHP=maxHeroHp;
				heroStamina=maxHeroStamina;
				Speed=defaultSpeed;

				if (LastTimeShopped <= Time.time) {
					LastTimeShopped = Time.time;
					heroState = HeroState.Store;
				} else {
					heroState = HeroState.Idle;
				}

			}

		}

		if(heroState==HeroState.Store){




			if(MyBuildings.IsStore()==true&& ShoppingAt==null){
				ShoppingAt = MyBuildings.FindStore ();



			}else if(ShoppingAt!=null){
				GoingTo = ShoppingAt.transform.position;

				if (Vector3.Distance(GoingTo,myTransform.position)<=10){

					Shoppingtime ();

					ShoppingAt = null;
					LastTimeShopped = Time.time + 30;
					heroState = HeroState.Idle;
				}
			}


			else{

				ShoppingAt = null;
				LastTimeShopped = Time.time + 30;
				heroState = HeroState.Idle;
			}





		}

		// hero thinking of what to do
		if (heroState==HeroState.Idle)
		{



			 if (myCurrentBounty != null) {
				heroState = HeroState.Bounty;	
				
			} else if (MyInv.DoIHavePickup()==true) {
				heroState = HeroState.DeliveryPickup;

			} else if (MyInv.DoIHaveDeliv()==true) {
				heroState = HeroState.DeliveryDeliver;
			}

			else if(ShoppingAt!=null){
				heroState = HeroState.Store;

			}
			else  {
				//Debug.Log(GameObject.FindGameObjectWithTag("BlackSmith").GetComponent<BlackSmith>().WhatLevelCanIBuy(2,HeroGold));
				//	GoingTo = new Vector3(Random.Range((MaxDistanceForHerosx*-1.0f),MaxDistanceForHerosx),1.5f,Random.Range(MinDistanceForHerosz,MaxDistanceForHerosz));

				RaycastHit hit2;
				Physics.Raycast (new Vector3 (Random.Range ((MaxDistanceForHerosx * -1.0f), MaxDistanceForHerosx), -40.0f, Random.Range (MinDistanceForHerosz, MaxDistanceForHerosz)), new Vector3 (0, 50.0f, 0), out hit2, 100.0f, (1 << 14) | (1 << 12));
				while (hit2.collider.name != "Ground") {

					Physics.Raycast (new Vector3 (Random.Range ((MaxDistanceForHerosx * -1.0f), MaxDistanceForHerosx), -40.0f, Random.Range (MinDistanceForHerosz, MaxDistanceForHerosz)), new Vector3 (0, 50.0f, 0), out hit2, 100.0f, (1 << 14) | (1 << 12));
				}
				GoingTo = hit2.point + Vector3.up;


				heroState = HeroState.Exploring;
			}

			
		}
		
		

		
		
		

		
		// counter to drain hero stamina but doesnt trigger inn state if in a fight
		// also doesnt lose stamina if resting at inn
		if (staminacounter == 60)
		
		{
			staminacounter =0;

			if (heroState!=HeroState.WaitingAtInn)
			{
			heroStamina= heroStamina-1;
			}

			if (heroStamina <=0 && heroState!= HeroState.Attacking && heroState!= HeroState.Bounty)
			{
				heroState=HeroState.Inn;
			}
		}
		
		
		






		if (GoAroundWaypoints.Count != 0) {
			heroLookat = GoAroundWaypoints [0] - myTransform.position;
			//Debug.Log (Vector3.Distance (GoAroundWaypoints [0],myTransform.position)+this.name);
			float temp = (Vector3.Distance (GoAroundWaypoints [0], myTransform.position));
			if (temp<= 1.5f) {
 				GoAroundWaypoints.RemoveAt (0);
			}

		} else {
			heroLookat = (GoingTo - myTransform.position);
		}



		tempoy = heroLookat;
		heroLookat = heroLookat.normalized;
		//static bool Raycast(Vector3 origin, Vector3 direction, RaycastHit hitInfo, float distance);
		// checking to see if running into anything but another hero and moving out of the way 
		
		if ((Physics.Raycast (rightShoulder, myTransform.forward, out hit, avoidBuildings, ~((1 << 10) | (1 << 9) | (1 << 12) | (1 << 8) | (1 << 16) | (1 << 14)),QueryTriggerInteraction.Ignore)) && Speed != 0) {

			if (hit.collider.isTrigger == false) {
				
				Debug.DrawLine (rightShoulder, hit.point, Color.blue);

				if (GoAroundWaypoints.Count == 0) {
					GoAroundWaypoints = FindWayAround (hit);
					heroLookat = heroLookat + hit.normal * 40.0f;
				}
			}



			//Debug.DrawLine(rightShoulder,hit.collider.bounds.min,Color.black);
			//Debug.DrawLine(rightShoulder,hit.collider.bounds.max,Color.black);
				
		
		} else if ((Physics.Raycast (leftShoulder, myTransform.forward, out hit, avoidBuildings, ~((1 << 10) | (1 << 9) | (1 << 12) | (1 << 8) | (1 << 16) | (1 << 14)),QueryTriggerInteraction.Ignore)) && Speed != 0) {

			if (hit.collider.isTrigger == false) {
				
				Debug.DrawLine (leftShoulder, hit.point, Color.red);

				if (GoAroundWaypoints.Count == 0) {
					if (ShoppingAt != null) {
						if (hit.collider.gameObject.name != ShoppingAt.name) {

							GoAroundWaypoints = FindWayAround (hit);
							heroLookat = heroLookat + hit.normal * 40.0f;
						}
					}
					else{
						GoAroundWaypoints = FindWayAround (hit);
						heroLookat = heroLookat + hit.normal * 40.0f;

					}
				}
				//Debug.DrawLine(leftShoulder,hit.collider.bounds.min,Color.black);
				//Debug.DrawLine(leftShoulder,hit.collider.bounds.max,Color.white);

			}
		}
		lefthit = false;
		
       


		
		heroRotate = Quaternion.LookRotation(heroLookat);
		
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation , heroRotate ,Time.deltaTime*turnSpeed );

		heroMovement = myTransform.forward*Speed;
		Debug.DrawRay(centermass,myTransform.forward*heroSightRadius,Color.cyan);
		staminacounter=staminacounter+1;
	

		
	}






	private List<Vector3> FindWayAround(RaycastHit hit)
	{
		List<Vector3> orginal = new List<Vector3> ();


		// 4 corners of the box collider which all models will have , probably
		orginal.Add(new Vector3(hit.collider.bounds.max.x,hit.collider.bounds.min.y,hit.collider.bounds.max.z));
		orginal.Add(new Vector3(hit.collider.bounds.max.x,hit.collider.bounds.min.y,hit.collider.bounds.min.z));
		orginal.Add(new Vector3(hit.collider.bounds.min.x,hit.collider.bounds.min.y,hit.collider.bounds.min.z));
		orginal.Add(new Vector3(hit.collider.bounds.min.x,hit.collider.bounds.min.y,hit.collider.bounds.max.z));

		// list to blow out the corners to be our go to postiions to walk around
		List<Vector3> expanded = new List<Vector3> ();

		Vector3 center = hit.collider.bounds.center;
		foreach (Vector3 temp in orginal) {
			Vector3 direction = center - temp;
			direction = direction.normalized;
			Vector3 gohere = temp + (direction * -2.0f);

			RaycastHit hit2;
			Physics.Raycast(gohere-(Vector3.up*30.0f),Vector3.up, out hit2,100.0f,(1<<14)|(1<<8)) ;

			gohere =hit2.point +(Vector3.up*1.5f) ;
			expanded.Add (gohere);
		}


		//Debug.DrawLine (center, expanded [0], Color.green,10.0f);
	//	Debug.DrawLine (center, expanded [1], Color.green,10.0f);
	//	Debug.DrawLine (center, expanded [2], Color.green,10.0f);
	//	Debug.DrawLine (center, expanded [3], Color.green,10.0f);

		// get the 2 closest points by comparing distances and deleting the top 2
		List<float> distancefromme = new List<float> ();
		List<Vector3> twoclosest = new List<Vector3> ();

		twoclosest=new List<Vector3>( expanded);

		foreach (Vector3 temp in expanded){
			distancefromme.Add(Vector3.Distance(myTransform.position,temp));
		}

		float max = Mathf.Max (distancefromme.ToArray ());
		int deleter = distancefromme.IndexOf(max);
		distancefromme.RemoveAt (deleter);
		twoclosest.RemoveAt (deleter);

		 max = Mathf.Max (distancefromme.ToArray ());
		deleter = distancefromme.IndexOf(max);
		distancefromme.RemoveAt (deleter);
		twoclosest.RemoveAt (deleter);

		List<Vector3> Path1 = new List<Vector3> ();
		List<Vector3> Path2 = new List<Vector3> ();

		Path1.Add (twoclosest[0]);
		Path2.Add (twoclosest [1]);
		RaycastHit hitpath1left;
		RaycastHit hitpath2left;
		RaycastHit hitpath1right;
		RaycastHit hitpath2right;

		Physics.Raycast (Path1[0]- (myTransform.right * shoulder), GoingTo-(Path1[0]- (myTransform.right * shoulder)), out hitpath1left, 10.0f, ~((1 << 10) | (1 << 9) | (1 << 14) | (1 << 8) | (1 << 16)));
		Physics.Raycast (Path2[0]- (myTransform.right * shoulder), GoingTo-(Path2[0]- (myTransform.right * shoulder)), out hitpath2left, 10.0f, ~((1 << 10) | (1 << 9)| (1 << 14) | (1 << 8) | (1 << 16)));

		Physics.Raycast (Path1[0]+ (myTransform.right * shoulder), GoingTo-(Path1[0]+ (myTransform.right * shoulder)), out hitpath1right, 10.0f, ~((1 << 10) | (1 << 9) | (1 << 14) | (1 << 8) | (1 << 16)));
		Physics.Raycast (Path2[0]+ (myTransform.right * shoulder), GoingTo-(Path2[0]+ (myTransform.right * shoulder)), out hitpath2right, 10.0f, ~((1 << 10) | (1 << 9)| (1 << 14) | (1 << 8) | (1 << 16)));

		//Debug.DrawRay (Path1 [0], GoingTo-Path1[0],Color.green,10.0f);
	//	Debug.DrawRay (Path2 [0], GoingTo-Path2[0],Color.green,10.0f);
		// ray casting to see if my shoping target is around a corner
//		if (ShoppingAt != null) {
//			if (((hitpath1left.collider.gameObject.name == ShoppingAt.gameObject.name && hitpath1right.collider.gameObject.name == ShoppingAt.gameObject.name) && (hitpath2right.collider.gameObject.name != ShoppingAt.gameObject.name || hitpath2left.collider.gameObject.name != ShoppingAt.gameObject.name))) {
//				return Path1;
//
//			}
//
//			if ((hitpath1left.collider.gameObject.name != ShoppingAt.gameObject.name || hitpath1right.collider.gameObject.name != ShoppingAt.gameObject.name) && hitpath2right.collider.gameObject.name == ShoppingAt.gameObject.name && hitpath2left.collider.gameObject.name == ShoppingAt.gameObject.name) {
//				return Path2;
//
//			}
//			if ((hitpath1left.collider.gameObject.name == ShoppingAt.gameObject.name && hitpath1right.collider.gameObject.name == ShoppingAt.gameObject.name) && (hitpath2right.collider.gameObject.name == ShoppingAt.gameObject.name && hitpath2left.collider.gameObject.name == ShoppingAt.gameObject.name)) {
//
//				if (Vector3.Distance (Path1 [0], myTransform.position) < Vector3.Distance (Path2 [0], myTransform.position)) {
//
//					return Path1;
//				} else {
//					return Path2;
//				}
//
//			}
//		}
		// sees if stores in sight form either corner so it doesnt get stuck with building
		if(ShoppingAt!=null){
			if(hitpath1left.collider!=null){
				if(hitpath1left.collider.gameObject.name == ShoppingAt.gameObject.name){
					return Path1;

				}

			}
			if(hitpath1right.collider!=null){
				if(hitpath1right.collider.gameObject.name == ShoppingAt.gameObject.name){
					return Path1;

				}

			}
			if(hitpath2left.collider!=null){
				if(hitpath2left.collider.gameObject.name == ShoppingAt.gameObject.name){
					return Path2;

				}

			}
			if(hitpath2right.collider!=null){
				if(hitpath2right.collider.gameObject.name == ShoppingAt.gameObject.name){
					return Path2;

				}

			}


			
		}




		// raycasting from the corners towards the destion if both are blocking building well need to go one more corner over else one of the corners has free line of sight

		// check to see if one is null over the other 
		if (((hitpath1left.collider == null && hitpath1right.collider==null)&&( hitpath2right.collider != null|| hitpath2left.collider!=null))) {
			return Path1;

		}

		if ((hitpath1left.collider != null || hitpath1right.collider!=null)&& hitpath2right.collider == null&& hitpath2left.collider==null) {
			return Path2;

		}
		if ((hitpath1left.collider == null && hitpath1right.collider == null) && (hitpath2right.collider == null && hitpath2left.collider == null)) {

			if (Vector3.Distance (Path1 [0], myTransform.position) < Vector3.Distance (Path2 [0], myTransform.position)) {

				return Path1;
			} else {
				return Path2;
			}
		
		}
		List<Vector3> NextChoice = new List<Vector3> (expanded);
		NextChoice.Remove (Path1 [0]);
		NextChoice.Remove (Path2 [0]);

		RaycastHit Canisee;
		Physics.Raycast (NextChoice[0], Path1[0]-NextChoice[0], out Canisee, 30.0f, ~((1 << 10) | (1 << 9) | (1 << 14) | (1 << 8) | (1 << 16)));
		//Debug.DrawLine (NextChoice [0], Path1 [0],Color.red);

		Debug.DrawRay(NextChoice[0],Path1[0]-NextChoice[0],Color.red,2.0f);
		if (Canisee.collider == null) {
			Path1.Add (NextChoice [0]);

		} else {
			Path1.Add (NextChoice [1]);
			Debug.Log(Canisee.collider.name +" "+this.name);
		}
		RaycastHit Canisee2;
		Physics.Raycast (NextChoice[0], Path2[0]-NextChoice[0], out Canisee2, 30.0f, ~((1 << 10) | (1 << 9) | (1 << 14) | (1 << 8) | (1 << 16)));
		//Debug.DrawLine (NextChoice [0], Path2 [0],Color.blue);

		Debug.DrawRay(NextChoice[0],Path2[0]-NextChoice[0],Color.blue,2.0f);
		if (Canisee2.collider == null) {
			Path2.Add (NextChoice [0]);

		} else {
			Path2.Add (NextChoice [1]);
			Debug.Log(Canisee2.collider.name +" "+this.name);
		}
		float path1distance = 0;
		float path2distance = 0;

		if (Path1.Count > 1) {
			path1distance += Vector3.Distance (myTransform.position, Path1 [0]);
			path1distance += Vector3.Distance (Path1 [0], Path1 [1]);
			path1distance += Vector3.Distance (Path1 [1], GoingTo);
		} else {
			path1distance += Vector3.Distance (myTransform.position, Path1 [0]);
			path1distance += Vector3.Distance (Path1 [0], GoingTo);

		}
		if (Path2.Count > 1) {
			path2distance += Vector3.Distance (myTransform.position, Path2 [0]);
			path2distance += Vector3.Distance (Path2 [0], Path2 [1]);
			path2distance += Vector3.Distance (Path2 [1], GoingTo);
		} else {
			path2distance += Vector3.Distance (myTransform.position, Path2 [0]);
			path2distance += Vector3.Distance (Path2 [0], GoingTo);

		}
		if (path1distance <= path2distance) {
			return (Path1);
		} else {
			return (Path2);
		}

	


	}

	private void CheckForBounty()
	{
		
		
        if ((BountyList.OpenBounty==true)&& ((heroState==HeroState.Idle) || (heroState==HeroState.Exploring))&&myCurrentBounty==null)
		{
			
			for(int i =0; i <BountyList.BountyList.Count;i++)
			{
				
				GameObject temp = BountyList.BountyList[i];
				if (temp.GetComponent<BountyController>().HeroAdd(gameObject)==true)
				{
					heroState=HeroState.Bounty;
					myCurrentBounty=temp;
				}
			}
			
		}	
		else 
		{
			if (myCurrentBounty!= null)
			{
				if((myCurrentBounty.GetComponent<BountyController>().Checkingifiminthelist(gameObject)) ==false)
				{
				
					myCurrentBounty=null;	
				}
				
			}
			
			
		}
			
			
			
		
		
	}
	
	
	
	 public void TakeDamage(int Damage)
	{
		Damage= Damage-MyInv.ChestArmor.Damagereduc-MyInv.ShieldArmor.Damagereduc;
		if (Damage >0)
		{
			heroHP=heroHP-Damage;
		}
	}
	
	public void AddMoney(int Money)
	{
		if (Money>0)
		{
			HeroGold=HeroGold+Money;	
			Debug.Log("money making game " + Money);
		}
		
	}
	
	
	public void IAmDead()
	{
		myRigid.useGravity=false;
		myTransform.position = new Vector3(myTransform.position.x,myTransform.position.y+.5f,myTransform.position.z);	
		if (myTransform.position.y >= 300f) {
			MainMoney.BlackBought -= Blacksmith;

			MyWorkControl.WorkerDied (HeroIdNumber);
				Destroy (gameObject);
			
				


		}
	}
	
	
	public bool WorkerGoHereForPickUp(PickupInfo source){
		if (MyInv.DoIHavePickup() ==false) {
				heroState = HeroState.DeliveryPickup;
			MyInv.NewPickup(source);
				//	resNodeTarget = source.transform.parent.gameObject.GetComponent<ResNode> ();
			//	DoingPickupdropoff = true;


			return true;
		}
		else{
			return false;
		}

	}
	
	
	void OnTriggerEnter (Collider other)
	{
//		if (other.gameObject.tag=="FogOfWar")
//		{
//			Debug.Log(other.gameObject.name);
//			Destroy( other.gameObject);
//			
//		}
//	

		if (other.gameObject.tag == "Mine" &&( heroState==HeroState.Exploring || heroState==HeroState.Inn)&&MyInv.MyDeliv.Isempty()==true) {
			
			if (other.transform.parent.gameObject.GetComponent<ResNode> ().DoYouNeedPickup (HeroIdNumber) == true) {

				if (other.transform.parent.gameObject.GetComponent<ResNode> ().HowMuchPay () > 0) {
					heroState = HeroState.DeliveryPickup;
					GoingTo = other.transform.parent.position;
					//resNodeTarget = other.transform.parent.gameObject.GetComponent<ResNode> ();
					MyInv.NewPickup(new PickupInfo(other.transform.parent.gameObject,other.transform.parent.gameObject.GetComponent<ResNode> ().myId));
				} else {

					other.transform.parent.gameObject.GetComponent<ResNode> ().IDontWanna (HeroIdNumber);
				}
			}
		
		}
	}
	
	
	
	void OnTriggerExit (Collider other)
	{
//		if ((other.tag=="Enemy")&&enemyTarget!=null&&heroState!=HeroState.Dead)
//		{
//			enemyTarget=null;
//			heroState = HeroState.Idle;
//			badguyai=null;
//			
//		}
	
	}
		
	
	void OnTriggerStay(Collider other)
	{

		if (other.tag=="Heros")
		{
			
			//Debug.Log ("hero");
		}
        // setting target when finding enemy
		if (other.tag=="Enemy"&&enemyTarget==null&&heroState!=HeroState.Dead&&heroHPLOW==false)
		{
			Collider[] colliders = Physics.OverlapSphere(myTransform.position, heroSightRadius,1<<9);
			if (colliders.Length!=0)
			{
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
				heroState=HeroState.Attacking;
				badguyai=enemyTarget.GetComponent<badGuyAi>();
				GoingTo=enemyTarget.transform.position;
			}
		}
        // getting how many enemies are around
        if (other.tag == "Enemy") 
        {
            //Debug.Log (other.tag+other.name+this.name);
            Collider[] colliders = Physics.OverlapSphere(myTransform.position, heroSightRadius,1<<9);

                
                    enemiesAroundMe = colliders.Length/2;
           

          
        }
		
	}
	/*
	 				enemyTarget=other.gameObject;
				heroState=HeroState.Attacking;
				badguyai=enemyTarget.GetComponent<badGuyAi>();
				GoingTo=enemyTarget.transform.position;
				*/	
	void OnCollisionEnter(Collision other){
		
		if (other.gameObject.name =="Ground")		
		{
			Isground = true;	
		}
		
		
	}
	void OnCollisionExit(Collision other){
		
		if (other.gameObject.name =="Ground")		
		{
			Isground = false;	
		}
		
		
	}
	

		IEnumerator AttackSpeedWait(float wait)
	{
		
		yield return new WaitForSeconds (wait);
		canAttack=true;
	
	}
	
	


	
	IEnumerator WaitInn(float wait)
	{
		
		yield return new WaitForSeconds (wait);
		
		heroState=HeroState.Idle;
		Speed=defaultSpeed;
	
	}

	public void PickupCanceled(){
		MyInv.NewPickup(new PickupInfo());
		heroState = HeroState.Idle;


	}



    void Blacksmith()
    {

        BlackSmithBought = true;
		MyInv.blacksmith = true;
    }




	public int GetWeaponTotalforshoping(WeaponInfo weap){
		int total=0;
		int mod=0 ;
		int output = 0;
		Weaponstatsbias.TryGetValue("Damage",out mod);

		if((weap.Damage)>(MyInv.MainHandWeapon.Damage))
		{
			if ((weap.Damage - MyInv.MainHandWeapon.Damage) > mod) {
				total += mod + (weap.Damage - MyInv.MainHandWeapon.Damage);
			}
			else{
				total += 2 * (weap.Damage - MyInv.MainHandWeapon.Damage);

			}

		}
		else if((weap.Damage)<(MyInv.MainHandWeapon.Damage)){
			if ((weap.Damage - MyInv.MainHandWeapon.Damage) > -1*mod) {
				total += -1*mod + (weap.Damage - MyInv.MainHandWeapon.Damage);
			}
			else{
				total += 2 * (weap.Damage - MyInv.MainHandWeapon.Damage);

			}


		}
		else{

			total -= 5;

		}

		Weaponstatsbias.TryGetValue("Durablity",out mod);
	
		if((weap.MaxDurablity)>(MyInv.MainHandWeapon.MaxDurablity))
		{
			if ((weap.MaxDurablity - MyInv.MainHandWeapon.MaxDurablity) > mod) {
				total += mod + (weap.MaxDurablity - MyInv.MainHandWeapon.MaxDurablity);
			}
			else{
				total += 2 * (weap.MaxDurablity - MyInv.MainHandWeapon.MaxDurablity);

			}

		}
		else if((weap.MaxDurablity)<(MyInv.MainHandWeapon.MaxDurablity)){
			if ((weap.MaxDurablity - MyInv.MainHandWeapon.MaxDurablity) > -1*mod) {
				total += -1*mod + (weap.MaxDurablity - MyInv.MainHandWeapon.MaxDurablity);
			}
			else{
				total += 2 * (weap.MaxDurablity - MyInv.MainHandWeapon.MaxDurablity);

			}


		}
		else{

			total -= 5;

		}


		Weaponstatsbias.TryGetValue("Weight",out mod);

		if((weap.Weight)>(MyInv.MainHandWeapon.Weight))
		{
			if ((weap.Weight - MyInv.MainHandWeapon.Weight) > mod) {
				total += mod + (weap.Weight - MyInv.MainHandWeapon.Weight);
			}
			else{
				total += 2 * (weap.Weight - MyInv.MainHandWeapon.Weight);

			}

		}
		else if((weap.Weight)<(MyInv.MainHandWeapon.Weight)){
			if ((weap.Weight - MyInv.MainHandWeapon.Weight) > -1*mod) {
				total += -1*mod + (weap.Weight - MyInv.MainHandWeapon.Weight);
			}
			else{
				total += 2 * (weap.Weight - MyInv.MainHandWeapon.Weight);

			}


		}else{

			total -= 5;

		}

		Weaponstatsbias.TryGetValue ("Range", out mod);
	
		if((weap.Range)>(MyInv.MainHandWeapon.Range))
		{
			if ((weap.Range - MyInv.MainHandWeapon.Range) > mod) {
				total += mod + (weap.Range - MyInv.MainHandWeapon.Range);
			}
			else{
				total += 2 * (weap.Range - MyInv.MainHandWeapon.Range);

			}

		}
		else if((weap.Range)<(MyInv.MainHandWeapon.Range)){
			if ((weap.Range - MyInv.MainHandWeapon.Range) > -1*mod) {
				total += -1*mod + (weap.Range - MyInv.MainHandWeapon.Range);
			}
			else{
				total += 2 * (weap.Range - MyInv.MainHandWeapon.Range);

			}


		}else{

			total -= 5;

		}

		Weaponstatsbias.TryGetValue ("Beauty", out mod);



		if((weap.Beauty)>(MyInv.MainHandWeapon.Beauty))
		{
			if ((weap.Beauty - MyInv.MainHandWeapon.Beauty)/10 > mod) {
				total += mod + ((weap.Beauty - MyInv.MainHandWeapon.Beauty)/10);
			}
			else{
				total += 2 * ((weap.Beauty - MyInv.MainHandWeapon.Beauty)/10);

			}

		}
		else if((weap.Beauty)<(MyInv.MainHandWeapon.Beauty)){
			if ((weap.Beauty - MyInv.MainHandWeapon.Beauty) > -1*mod) {
				total += -1*mod + ((weap.Beauty - MyInv.MainHandWeapon.Beauty)/10);
			}
			else{
				total += 2 * ((weap.Beauty - MyInv.MainHandWeapon.Beauty)/10);

			}


		}else{

			total -= 5;

		}


	    Weapontypebias.TryGetValue (weap.MyBaseinfo.name, out output);
		total += output;
		if (HeroGold <= 0) {
			total -= 1000;
		
		}
		else if(weap.SellPrice/HeroGold <=.5f){
			total += 20;

		}
		else if(weap.SellPrice/HeroGold >=.8f){
			total -= 20;

		}

		Debug.Log (" weapon "+weap.Name + total);

		return total;
	}


	public int GetArmoirTotalforshoping(ArmorInfo arm){
		int total=0;
		int Armormod=0 ;
		int ArmorSizeMod=0;
		int output = 0;
		ArmorClassbias.TryGetValue (arm.Size, out ArmorSizeMod);
		total = ArmorSizeMod;


		Armorstatsbias.TryGetValue("Defense",out Armormod);

		ArmorInfo CurrentArmor = MyInv.Getarmor (arm.Location);

		if((arm.Damagereduc)>(CurrentArmor.Damagereduc))
		{
			if ((arm.Damagereduc - CurrentArmor.Damagereduc) > Armormod) {
				total += Armormod + (arm.Damagereduc - CurrentArmor.Damagereduc);
			}
			else{
				total += 2 * (arm.Damagereduc - CurrentArmor.Damagereduc);

			}

		}
		else if((arm.Damagereduc)<(CurrentArmor.Damagereduc)){
			if ((arm.Damagereduc - CurrentArmor.Damagereduc) > -1*Armormod) {
				total += -1*Armormod + (arm.Damagereduc - CurrentArmor.Damagereduc);
			}
			else{
				total += 2 * (arm.Damagereduc - CurrentArmor.Damagereduc);

			}


		}
		else{

			total -= 5;

		}

		Armorstatsbias.TryGetValue("Durablity",out Armormod);

		if((arm.MaxDurablity)>(CurrentArmor.MaxDurablity))
		{
			if ((arm.MaxDurablity - CurrentArmor.MaxDurablity) > Armormod) {
				total += Armormod + (arm.MaxDurablity- CurrentArmor.MaxDurablity);
			}
			else{
				total += 2 * (arm.MaxDurablity- CurrentArmor.MaxDurablity);

			}

		}
		else if((arm.MaxDurablity)<(CurrentArmor.MaxDurablity)){
			if ((arm.MaxDurablity- CurrentArmor.MaxDurablity) > -1*Armormod) {
				total += -1*Armormod + (arm.MaxDurablity - CurrentArmor.MaxDurablity);
			}
			else{
				total += 2 * (arm.MaxDurablity - CurrentArmor.MaxDurablity);

			}


		}
		else{

			total -= 5;

		}


		Armorstatsbias.TryGetValue("Weight",out Armormod);

		if((arm.Weight)>(CurrentArmor.Weight))
		{
			if ((arm.Weight - CurrentArmor.Weight) > Armormod) {
				total += Armormod + (arm.Weight - CurrentArmor.Weight);
			}
			else{
				total += 2 * (arm.Weight - CurrentArmor.Weight);

			}

		}
		else if((arm.Weight)<(CurrentArmor.Weight)){
			if ((arm.Weight - CurrentArmor.Weight) > -1*Armormod) {
				total += -1*Armormod + (arm.Weight - CurrentArmor.Weight);
			}
			else{
				total += 2 * (arm.Weight - CurrentArmor.Weight);

			}


		}else{

			total -= 5;

		}


		Armorstatsbias.TryGetValue ("Beauty", out Armormod);



		if((arm.Beauty)>(CurrentArmor.Beauty))
		{
			if ((arm.Beauty - CurrentArmor.Beauty)/10 > Armormod) {
				total += Armormod + ((arm.Beauty - CurrentArmor.Beauty)/10);
			}
			else{
				total += 2 * ((arm.Beauty - CurrentArmor.Beauty)/10);

			}

		}
		else if((arm.Beauty)<(CurrentArmor.Beauty)){
			if ((arm.Beauty - CurrentArmor.Beauty) > -1*Armormod) {
				total += -1*Armormod + ((arm.Beauty - CurrentArmor.Beauty)/10);
			}
			else{
				total += 2 * ((arm.Beauty - CurrentArmor.Beauty)/10);

			}


		}else{

			total -= 5;

		}


		Armortypebias.TryGetValue (arm.MyBaseinfo.name, out output);

		total += output;
		if (HeroGold <= 0) {
			total -= 1000;

		}
		else if(arm.SellPrice/HeroGold <=.5f){
			total += 20;

		}
		else if(arm.SellPrice/HeroGold >=.8f){
			total -= 20;

		}

		Debug.Log (" Armour "+arm.Name + total);

		return total;
	}



	public void Shoppingtime(){
		List<WeaponInfo> weaponlist = new List<WeaponInfo> ();
		List<ArmorInfo> armorlist = new List<ArmorInfo> ();
		WeaponInfo WeaponChoice= new WeaponInfo();
		ArmorInfo ArmorChoice = new ArmorInfo();
		float currentweapontotal = 0;
		float itemchoicetotal = 0;
		float currentarmortotal = 0;

		if(ShoppingAt.tag=="BlackSmith"){

			weaponlist = ShoppingAt.GetComponent<BlackSmithMenu> ().WhatWeaponCanBuy (HeroGold);
			armorlist = ShoppingAt.GetComponent<BlackSmithMenu> ().WhatArmorCanBuy (HeroGold);

			if(weaponlist.Count!=0 && armorlist.Count!=0){
				if(Random.Range(0,101)+WeaponArmorbias >=50){
			//	 loops thur all weapons and find the best one need to add logic to allow all the are imporvment so could buy one thats not best but still afforadble
					currentweapontotal = GetWeaponTotalforshoping (MyInv.MainHandWeapon);
					foreach(WeaponInfo x in weaponlist){
							
							itemchoicetotal = GetWeaponTotalforshoping (x);
						if (WeaponChoice.Name != "") {
							if (itemchoicetotal > currentweapontotal && itemchoicetotal > GetWeaponTotalforshoping (WeaponChoice)) {
								WeaponChoice = WeaponInfo.copyMe (x);
							  
							}
						}
						else{

							if (itemchoicetotal > currentweapontotal) {
								WeaponChoice = WeaponInfo.copyMe (x);
								}
						}

					}
					// buy the weapon
					if(GetWeaponTotalforshoping( WeaponChoice)>=30&&WeaponChoice.Name!=""){
						if( ShoppingAt.GetComponent<BlackSmithMenu> ().BuyWeapon(WeaponChoice)==true)
						{

							mainMon.AddGold (WeaponChoice.SellPrice);
							HeroGold -= WeaponChoice.SellPrice;
							MyInv.EquipWeapon(WeaponChoice,"Main");
						}
					}

				}
				else{
					foreach(ArmorInfo x in armorlist){
						if (MyInv.DoIHaveThisArmor (x.Location) == true) {
							currentarmortotal = GetArmoirTotalforshoping (MyInv.Getarmor(x.Location));
						}else{
							currentarmortotal = -30 ;
						}


						itemchoicetotal = GetArmoirTotalforshoping (x);
						if (ArmorChoice.Name != "") {
							if (itemchoicetotal > currentarmortotal && itemchoicetotal > GetArmoirTotalforshoping (ArmorChoice)) {

								ArmorChoice = ArmorInfo.CopyMe (x);
							}
						}
						else{
							if (itemchoicetotal > currentarmortotal) {

								ArmorChoice = ArmorInfo.CopyMe (x);
							}

						}

					}
					if (ArmorChoice.Name != "") {
						if (GetArmoirTotalforshoping (ArmorChoice) >= 30) {
							if (ShoppingAt.GetComponent<BlackSmithMenu> ().BuyArmor (ArmorChoice) == true) {

								mainMon.AddGold (ArmorChoice.SellPrice);
								HeroGold -= ArmorChoice.SellPrice;
								MyInv.EquipArmor (ArmorChoice, ArmorChoice.Location);
							}
						}
					}

				}





			}else if(weaponlist.Count!=0){
				currentweapontotal = GetWeaponTotalforshoping (MyInv.MainHandWeapon);
				foreach(WeaponInfo x in weaponlist){

					itemchoicetotal = GetWeaponTotalforshoping (x);
					if (WeaponChoice.Name != "") {
						if (itemchoicetotal > currentweapontotal && itemchoicetotal > GetWeaponTotalforshoping (WeaponChoice)) {
							WeaponChoice = WeaponInfo.copyMe (x);

						}
					}
					else{

						if (itemchoicetotal > currentweapontotal) {
							WeaponChoice = WeaponInfo.copyMe (x);
						}
					}

				}
				// buy the weapon
				if(GetWeaponTotalforshoping( WeaponChoice)>=30&&WeaponChoice.Name!=""){
					if( ShoppingAt.GetComponent<BlackSmithMenu> ().BuyWeapon(WeaponChoice)==true)
					{

						mainMon.AddGold (WeaponChoice.SellPrice);
						HeroGold -= WeaponChoice.SellPrice;
						MyInv.EquipWeapon(WeaponChoice,"Main");
					}
				}

			}
			else if (armorlist.Count!=0){
				foreach(ArmorInfo x in armorlist){
					if (MyInv.DoIHaveThisArmor (x.Location) == true) {
						currentarmortotal = GetArmoirTotalforshoping (MyInv.Getarmor(x.Location));
					}else{
						currentarmortotal = -30 ;
					}


					itemchoicetotal = GetArmoirTotalforshoping (x);
					if (ArmorChoice.Name != "") {
						if (itemchoicetotal > currentarmortotal && itemchoicetotal > GetArmoirTotalforshoping (ArmorChoice)) {

							ArmorChoice = ArmorInfo.CopyMe (x);
						}
					}
					else{
						if (itemchoicetotal > currentarmortotal) {

							ArmorChoice = ArmorInfo.CopyMe (x);
						}

					}

				}
				if (ArmorChoice.Name != "") {
					if (GetArmoirTotalforshoping (ArmorChoice) >= 30) {
						if (ShoppingAt.GetComponent<BlackSmithMenu> ().BuyArmor (ArmorChoice) == true) {

							mainMon.AddGold (ArmorChoice.SellPrice);
							HeroGold -= ArmorChoice.SellPrice;
							MyInv.EquipArmor (ArmorChoice, ArmorChoice.Location);
						}
					}
				}

			}
			else{

				Debug.Log ("Aintgotshit");
			}






		}




	}

	
	
	
	
	
}
