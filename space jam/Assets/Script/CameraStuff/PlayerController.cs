using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{

		private float checkForDoubleTime;
		private float delay=.5f;
		private float bountyRadius=40f;

		public float MaxX;
		public float MaxZ;
		public float[] maxxz = new float[2];
	
		public int bountyCost=0;
		public int maxRespondToBounty=0;
		public int timeToGuard=0;


		public Vector3 spawnPointpos;
         public Quaternion spawnPointRot;

		public GameObject BountyFlag;
		private GameObject bountySize;
        private HeroSpawner heroSpawn;
		private MainMoney mymainmoney;
	    private BuildingLocation buildingloc;

		private bool bountyMenu =false;
		private bool ableToRayCastForBounty=true;
		private bool doubleClick = false;
        public bool OpenBounty = false;
		
		private bool bountyExplore =false;
		private bool bountyAttack = false;
		private bool bountyGuard = false;

		private string maxherotorespond="";
		private string timeToGuardFor="60";
		private string bountyCoststring ="";
	
		private RaycastHit mouseHit;
	
		public enum BountyError
		{
			NoError,
			NoMoney,
			NoHero,
			NoSelection,
			NoGuardTime,
			NoPlayerMoney,
		
		
		};

		public BountyError BountError;

	
		public List<GameObject> BountyList;

	void Awake()
	{
	BountyList=new List<GameObject>();	
	}
	
	void Start()
	{
		BountError=BountyError.NoError;
		bountySize = GameObject.Find("BountySpotlight");
        heroSpawn = GameObject.Find("HeroController").GetComponent<HeroSpawner>();    
		mymainmoney = GameObject.Find ("GUIIncometab").GetComponent<MainMoney> ();
		spawnPointpos= gameObject.transform.position;
        spawnPointRot = gameObject.transform.rotation;
		buildingloc = GameObject.Find ("BuildingManager").GetComponent<BuildingLocation> ();
		maxxz = heroSpawn.HowFarCanIGo (1);
		MaxX = maxxz [0];
		MaxZ = maxxz [1];
	}
	void Update()
	{
	
        CheckBountyforavail();
	if(Input.GetKeyDown(KeyCode.Backspace))
		{
		
		transform.position 	=spawnPointpos;
          transform.rotation = spawnPointRot;
			
		}
		
	if(Input.GetMouseButtonDown(0))
		{
		//	Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition),
			
		}
	
	}
	
	void LateUpdate ()
	{
		ClearBounty();
		

		
	//	Debug.DrawLine(new Vector3(-MaxX ,1,-50),new Vector3(MaxX ,1,-50));
	//	Debug.DrawLine(new Vector3(-MaxX ,1,25),new Vector3(MaxX ,1,25));
	//	Debug.DrawLine(new Vector3(-150,1,-50),new Vector3(-150,10,-50));
	//	Debug.DrawLine(new Vector3(-150,1,-50),new Vector3(-150,10,MaxZ));
		
		//checking to make sure mouse isnt over a gui element from mainmoney which controls all gui popups for the most part
		
		// this section is for bounty menu popup 
		//Debug.Log (GUI.GetNameOfFocusedControl());
      //  if((Input.GetMouseButtonUp(0)) && (ableToRayCastForBounty==true) )
		if (Input.GetMouseButtonDown (1)&&mymainmoney.mouseIsOverMe!=true&&buildingloc.IsMouseOverMenu()!=true) {
			
			Ray mouseLook = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (Camera.main.transform.position, mouseLook.direction, out mouseHit, 1000f, ((1 << 14) | (1 << 11) | (1 << 15))))
				Debug.Log (mouseHit.collider.name);
				if ((mouseHit.collider.gameObject.name == "Ground")) {
						
					bountyMenu = true;
					ableToRayCastForBounty = false;
					
				}
		}



		if((Input.GetMouseButtonUp(0)&&mymainmoney.mouseIsOverMe!=true&&buildingloc.IsMouseOverMenu()!=true) )
		{
			
			if(doubleClick==false)
			{
				
			doubleClick=true;
				checkForDoubleTime=Time.time;
			}
			else
			{
				Ray mouseLook = Camera.main.ScreenPointToRay(Input.mousePosition);
	
				Debug.DrawRay(Camera.main.transform.position,mouseLook.direction*20f);
				
				
				if (Physics.Raycast(Camera.main.transform.position,mouseLook.direction,out mouseHit,1000f,((1<<14)|(1<<11)|(1<<15))))
				{
					
				
					//Debug.Log(mouseHit.collider.gameObject.name.ToString());
//					if ((mouseHit.collider.gameObject.name == "Ground") && (mouseHit.point.x >= -MaxX) && (mouseHit.point.x <= MaxX) && (mouseHit.point.z >= -50) && (mouseHit.point.z <= MaxZ) && ((GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ().IsAWindowOpen ()) != true) && (GameObject.FindGameObjectWithTag ("Inn").GetComponent<InnMenu> ().innMenuActive != true) && (GameObject.FindGameObjectWithTag ("BlackSmith").GetComponent<BlackSmithMenu> ().blackMenuActive () != true)) {
//						
//						bountyMenu = true;
//						ableToRayCastForBounty = false;
//					
						
					 if (mouseHit.collider.gameObject.name == "Inn") {
						mouseHit.collider.gameObject.GetComponent<InnMenu> ().MenuOn ();						
					} else if (mouseHit.collider.gameObject.name == "BlackSmith") {
						mouseHit.collider.gameObject.GetComponent<BlackSmithMenu> ().MenuOn ();

					} else if (mouseHit.collider.gameObject.tag == "ResourceNode") {

						mouseHit.collider.gameObject.GetComponent<ResNode> ().Menuon ();
					}
				
				}
			
			}
		}
		
		if (doubleClick==true)
		{
		if((Time.time-checkForDoubleTime)>delay)
			{
			doubleClick=false;	
			}
			
		}
		


	}

	// bounty menu stuff down here
	void OnGUI()
	{
		
		
		if (bountyMenu==true)
		{
		//	GameObject.Find("BountySizeOnGround").transform.position= mouseHit.point+Vector3.up*2;
			bountySize.transform.position=new Vector3(mouseHit.point.x,50,mouseHit.point.z);
			bountySize.GetComponent<Light> ().intensity = 8;
			
			GUI.Box(new Rect(50,30,300,250),"Enter amount below to pay each hero");
		
			
			bountyCoststring= GUI.TextField(new Rect(150,50,100,20),bountyCoststring);
			int.TryParse(bountyCoststring,out bountyCost);
			

			
		
			
			GUI.Label(new Rect(70,120,100,100),"Enter max amount of heroes allowed to respond below");
			
			maxherotorespond= GUI.TextField(new Rect(70,190,50,20),maxherotorespond);
			int.TryParse(maxherotorespond,out maxRespondToBounty);
			
			//52 max 15 min
			// left , top, width,hieght
//			GUI.Label(new Rect(70,70,150,30),"Size of Bounty Area");
//			bountyRadius= GUI.HorizontalSlider(new Rect(70,90,100,30),bountyRadius,10.0f,50.0f);
//			
//			bountySize.transform.localScale= new Vector3(bountyRadius,0.1f,bountyRadius);

			GUI.Label(new Rect(70,70,150,30),"Size of Bounty Area");
			bountyRadius= GUI.HorizontalSlider(new Rect(70,90,100,30),bountyRadius,10.0f,50.0f);

			bountySize.GetComponent<Light> ().spotAngle = bountyRadius;
			
			//toggle select for what kind of bounty it is
			if(bountyAttack==false && bountyGuard==false)
			{
			
				bountyExplore=GUI.Toggle(new Rect(200,90,100,20),bountyExplore,"Explore Here");
		
			}
			
			if(bountyExplore==false && bountyGuard==false)
			{
			
				bountyAttack=GUI.Toggle(new Rect(200,120,100,20),bountyAttack,"Attack Here");
		
			}
			
			if(bountyAttack==false && bountyExplore==false)
			{
			
				bountyGuard=GUI.Toggle(new Rect(200,150,100,20),bountyGuard,"Guard Here");
		
			}
			
			if( bountyGuard==true)
			{
				GUI.Label(new Rect(200,70,100,50),"Time to guard for in seconds");
				timeToGuardFor= GUI.TextField(new Rect(200,110,100,20),timeToGuardFor);
				int.TryParse(timeToGuardFor,out timeToGuard);
				
				
				
			}
			
			if(GUI.Button(new Rect(70,240,100,30),"Declare bounty") )
			{
				if (bountyCost<=0)
				{
					BountError=BountyError.NoMoney;	
					
				}
				else if (maxRespondToBounty<=0)
				{
				BountError=BountyError.NoHero;	
					
				}
				else if (bountyAttack==false && bountyExplore==false&& bountyGuard==false)
				{
					
					BountError=BountyError.NoSelection;
				}
				else if ( (bountyGuard==true) && timeToGuard<=0 )
				{
					
					BountError=BountyError.NoGuardTime;
				}
				else if ( GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().CanIBuy(bountyCost*maxRespondToBounty,false)!=false)
				{
				BountError=BountyError.NoPlayerMoney;	
					
				}
				else
				{
					NewBounty(bountyCost);
					bountyMenu=false;
	
					ableToRayCastForBounty=true;
					GameObject.Find("BountySizeOnGround").transform.position= mouseHit.point+Vector3.up*200;
				}
		
			}
			
			
			if(GUI.Button(new Rect(230,240,100,30),"Cancel"))
			{
				
				bountyMenu=false;
				bountyCoststring="";
				bountyCost=0;
				timeToGuard=0;
				timeToGuardFor="60";
				maxherotorespond="";
				maxRespondToBounty=0;
				ableToRayCastForBounty=true;
				//GameObject.Find("BountySizeOnGround").transform.position= mouseHit.point+Vector3.up*200;
				bountyAttack=false;
				bountyExplore=false;
				bountyGuard=false;
				bountySize.GetComponent<Light> ().intensity = 0;
			}
				
			
		}
		
		
		
		
		
		
		
	}
	
	
	void NewBounty(int Bounty)
	{
		GameObject setTheBounty;
		BountyList.Add(Instantiate(BountyFlag,mouseHit.point,transform.rotation) as GameObject);
		setTheBounty= BountyList[BountyList.Count-1];
		setTheBounty.name=Bounty.ToString();
		
		setTheBounty.GetComponent<BountyController>().bountyAmount=Bounty;
        setTheBounty.GetComponent<SphereCollider>().radius = (bountyRadius/2)/3;
		setTheBounty.GetComponent<BountyController>().maxHeroAllowed=maxRespondToBounty;
		if (bountyAttack==true)
		{
			setTheBounty.GetComponent<BountyController>().SetBountyType(2);
			
		}
		else if (bountyExplore==true)
		{
			
			setTheBounty.GetComponent<BountyController>().SetBountyType(1);
		}
		else
		{
			
			setTheBounty.GetComponent<BountyController>().SetBountyType(3);
			setTheBounty.GetComponent<BountyController>().Timer=timeToGuard;
		}
		
		
		
		
		
		
		BountyList = BountyList.OrderByDescending(potato => potato.GetComponent<BountyController>().bountyAmount).ToList();
		
		
		bountyAttack=false;
		bountyExplore=false;
		bountyGuard=false;
		timeToGuard=0;
		timeToGuardFor="60";
		maxherotorespond="";
		maxRespondToBounty=0;
		bountyCoststring="";
		bountyCost=0;
		bountySize.GetComponent<Light> ().intensity = 0;
	
	}
	
	void ClearBounty()
	{
		// needs using System.Linq; to do orderby  orderby(randomnamedoesntmatter => randomnamedoestmatter.whatyourorderinglistby)
	

		for (int i =0; i<BountyList.Count;i++)
		{
		if (BountyList[i]==null)
			{
			BountyList.RemoveAt(i);	
			}
			
		}
		// sort list by biggest bpunty on top
			BountyList = BountyList.OrderByDescending(potato => potato.GetComponent<BountyController>().bountyAmount).ToList();
		
	}

    void CheckBountyforavail()
    {
        if (BountyList.Count > 0)
        {
            OpenBounty = false;
            for (int i = 0; i < BountyList.Count; i++)
            {
                if (BountyList[i].GetComponent<BountyController>().AmIFull() == false)
                {
                    OpenBounty = true;
                }
            }

        }
        else
        {
            OpenBounty = false;

        }

    }
}

