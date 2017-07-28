using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class ResNode : MonoBehaviour {

	public ResNodeinfo Myres=null;
	public WorkerController MyWorkContorl;
	public Rect MyMenu;
	public Rect MyMenuColl;
	public bool StoreRoom;
	public bool AllowWorkerColl;
	public bool IsMineBuilt;
	public bool Broke;
	public bool myMenuon=false;
	public bool MinerHired = false;
	public bool surveyed = false;
	public bool AllowHerocoll=false;
	public bool HeroOnWay = false;
	public bool PickupRequested;
	public bool CancelSent=false;

	public int mywindownumber;
	public int MiningTimer =0;
	public int MaxResHold =3;
	public int ResHold=0;
	public int myId=-1;
	public MainMoney myMainmoney;
	public string MenuTitle="";
	public string WorkName="";
	public int PerUnit=2;
	public int WorkerPickup=1;
	public List<LastTimeLookedAt> HeroChecker ;

	public 
	// Use this for initialization
	void Start () {
		HeroChecker = new List<LastTimeLookedAt> ();
		MyMenu = new Rect(Screen.width/2.5f, Screen.height/2.75f ,200 ,200);
		MyMenuColl= new Rect(Screen.width/2.5f, Screen.height/2.75f ,200 ,200);
		myMainmoney = GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney>();
		gameObject.GetComponent<Renderer> ().material.color = Color.cyan;
		mywindownumber =Random.Range(20,20000);
		MyWorkContorl = GameObject.FindGameObjectWithTag ("WorkerController").GetComponent<WorkerController> ();
		gameObject.GetComponentInChildren<ResourceNodeTExt>().GetComponent<Renderer>().enabled = true; 

	}
	
	// Update is called once per frame
	void Update () {


		if (IsMineBuilt == true) {


			if (MiningTimer >= (float)Myres.ResType.TimeToMineOne / ( (float)Myres.MineLevel / 4) && (MaxResHold != ResHold)) {
				Myres.AmountLeft -= 1;
				ResHold += 1;
				MiningTimer = 0;


			}
			if (MaxResHold != ResHold) {
				MiningTimer += 1;
			}
//			if (AllowHerocoll == true) {
//				if (myMainmoney.CanIBuy(PerUnit,false) == false) {
//					Broke = true;
//
//				} else {
//					Broke = false;
//				}
//
//
//			}


			if(ResHold>=WorkerPickup && AllowWorkerColl==true && PickupRequested==false){

				if(MyWorkContorl.INeedAPickUp (this.gameObject as GameObject,myId)==true){
					CancelSent = false;
					PickupRequested = true;
				}

			}

			if(AllowWorkerColl==false && CancelSent==false){
				MyWorkContorl.CancelPickup (this.gameObject);
				CancelSent = true;
				PickupRequested = false;

			}

			foreach (LastTimeLookedAt temp in HeroChecker) {
				if (temp.time<=Time.time){
					HeroChecker.Remove (temp);
				}
			
			}


		}
	}

	void OnGUI()
	{
		if (myMenuon == true)  {

			if (surveyed == true) {
				MenuTitle = this.name;
				MyMenu = GUI.Window (mywindownumber, MyMenu, ResMenu, "");
			} else {
				if (Myres.ResType.isMineral == true) {
					MenuTitle = "Unknown Mineral Node";
					MyMenu = GUI.Window (mywindownumber, MyMenu, ResMenu, "" );
				} else {
					MenuTitle = "Unknown Wuud Node";
					MyMenu = GUI.Window (mywindownumber, MyMenu, ResMenu, "" );
				}
			}



		} 


		

	}

	public void ResMenu(int windowid)
	{
		GUI.Box (new Rect (0, 0, MyMenu.width, MyMenu.height), MenuTitle );
		GUI.Box (new Rect (0, 0, MyMenu.width, MyMenu.height), MenuTitle );
		if (surveyed == false) {
			if (GUI.Button (new Rect (105, 110, 90, 40), "Survey Node:\n 5 Gold")) {
				if (myMainmoney.CanIBuy (5,true)) {
					surveyed = true;
				}
			}

			GUI.Label (new Rect (20, 20, 150, 100), "Amount  : ?");
			GUI.Label (new Rect (20, 40, 150, 100), "Quailty : ?");
			GUI.Label (new Rect (20, 60, 150, 100), "Density : ?");

		} else if (IsMineBuilt == false) {


			GUI.Label (new Rect (20,20 , 150, 100), "Quailty: ");
			GUI.Label (new Rect (75, 20, 150, 100), Myres.ResType.QualityName);
			GUI.Label (new Rect (20, 40, 150, 100), "Density: ");
			GUI.Label (new Rect (75, 40, 150, 100), Myres.ResType.DensityName);
			GUI.Label (new Rect (20, 60, 150, 100), "Amount Left: ");
			GUI.Label (new Rect (95, 60, 150, 100), Myres.AmountLeft.ToString ());

		} else {



			GUI.Label (new Rect (20, 20, 150, 100), "Quailty: ");
			GUI.Label (new Rect (75, 20, 150, 100), Myres.ResType.QualityName);
			GUI.Label (new Rect (20, 40, 150, 100), "Density: ");
			GUI.Label (new Rect (75, 40, 150, 100), Myres.ResType.DensityName);
			GUI.Label (new Rect (20, 60, 150, 100), "Amount Left: ");
			GUI.Label (new Rect (95, 60, 150, 100), Myres.AmountLeft.ToString ());
			GUI.Label (new Rect (20, 80, 150, 100), "Waiting for pickup: ");
			GUI.Label (new Rect (130, 80, 150, 100), ResHold.ToString ());

			GUI.Label (new Rect (170, 20, 150, 100), "Mine Level: " + Myres.MineLevel);
			GUI.Label(new Rect(170,40,150,100) ,"Time to Mine: " +((float)Myres.ResType.TimeToMineOne / ( (float)Myres.MineLevel / 4))/100 +" Sec");



		}


		if (IsMineBuilt == false) {
			if (GUI.Button (new Rect (5, 110, 90, 40), "Build Mine: \n 40 Gold")) {
				if (myMainmoney.CanIBuy (40,true)) {
					IsMineBuilt = true;
					surveyed = true;
					transform.Find ("Mine").gameObject.GetComponent<SphereCollider> ().enabled = true;
					transform.Find ("Mine").gameObject.GetComponent<MeshRenderer> ().enabled = true;
					Myres.MineLevel += 1;
					MyMenu = new Rect (Screen.width / 2.5f, Screen.height / 2.75f, 300, 300);
				}
			}
		} else {
			
			if (GUI.Button(new Rect(20,110,120,20),"Collection Methods"))
			{
				myMainmoney.AddResource (Myres.ResType, ResHold,false);
				ResHold = 0;
			}

			AllowHerocoll=GUI.Toggle(new Rect(20,130,135,20),AllowHerocoll,"Pay Heros to collect ");
			if (AllowHerocoll == true) {
				GUI.Label (new Rect (200, 110, 100, 20), "Per Piece Delivered");

				if (GUI.Button (new Rect (200, 130, 20, 20), "-")) {
					if (Input.GetKey(KeyCode.LeftShift)) {
						PerUnit -= 5;
					} else {
						PerUnit -= 1;
					}
					if (PerUnit < 0) {
						PerUnit = 0;
					}
				}

				GUI.Label(new Rect(225, 130, 100, 20),PerUnit.ToString());

				if (GUI.Button (new Rect (240, 130, 20, 20), "+")) {
					if ( Input.GetKey(KeyCode.LeftShift)) {
						PerUnit += 5;
					} else {
						PerUnit += 1;
					}	
				}

			}

			AllowWorkerColl=GUI.Toggle(new Rect(20,170,150,20),AllowWorkerColl,"Send Worker to collect ");
			if (AllowWorkerColl == true) {
				GUI.Label (new Rect (200, 150, 100, 20), "Pickup At Amount");

				if (GUI.Button (new Rect (200, 170, 20, 20), "-")) {
					if (Input.GetKey(KeyCode.LeftShift)) {
						WorkerPickup -= 5;
					} else {
						WorkerPickup -= 1;
					}
					if (WorkerPickup < 1) {
						WorkerPickup = 1;
					}
				}

				GUI.Label(new Rect(225, 170, 100, 20),WorkerPickup.ToString());

				if (GUI.Button (new Rect (240, 170, 20, 20), "+")) {
					if ( Input.GetKey(KeyCode.LeftShift)) {
						WorkerPickup += 5;
					} else {
						WorkerPickup += 1;
					}	
				}

			}


		}




		if (GUI.Button(new Rect(MyMenu.width-21,1,18,15),"X"))
		{
			myMenuon=false;
			//Debug.Log (MyMenu.yMin);
		}

		GUI.DragWindow();
	}


	public bool DoYouNeedPickup(int ID){
		
		if (AllowHerocoll == true && myMainmoney.CanIBuy(PerUnit,false) == true && (HeroChecker.Contains(new LastTimeLookedAt(ID,0.0f) )==false)&& ResHold!=0) {
			return true;

		}
		return false;

	}




	public bool DoYouNeedPickupWorker(GameObject worker){
		if (AllowWorkerColl == true && ResHold != 0 && WorkerPickup >= ResHold&& (worker.GetComponent<HeroInv>().WeightLeft()<=Myres.ResType.BaseWeight)) {
			PickupRequested = true;
			return true;
		}
		return false;


	}

	public int HowMuchPay(){
		return PerUnit;

	}

	public DeliveryItem PickupIsHere(int MaxWeight){
		
		DeliveryItem temp = new DeliveryItem ();
		int howmuch = 0;
		temp.Price = PerUnit;
		temp.DesVec = GameObject.FindGameObjectWithTag ("Inn").gameObject.transform.position;
		temp.Origin = this.gameObject.name;
		temp.Destination = "Inn";	
		while ((howmuch * Myres.ResType.BaseWeight <= MaxWeight)&&(howmuch<ResHold)) {
			howmuch += 1;
			temp.Mats.Add (Myres.ResType);

		}
		ResHold -= howmuch;
		PickupRequested = false;
		return temp;

	}

	public void IDontWanna(int temp){



		//if HeroChecker.

		if (HeroChecker.Contains (new LastTimeLookedAt(temp,0.0f))) {
			
		} else {

			HeroChecker.Add (new LastTimeLookedAt (temp, Time.time+30.0f));
		}

	}


	public void Menuon()
	{
		myMenuon = true;

	}


}
