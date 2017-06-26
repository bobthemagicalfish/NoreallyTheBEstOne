using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class PickupInfo{
	public GameObject Pickup;
	public int id;
	public GameObject Worker;
	public float Timerequsted;

	public PickupInfo(){

		Pickup=null;
		 id = -1;
		Timerequsted = 0.0f;

	}

	public PickupInfo(GameObject set, int ids){
		id = ids;
		Pickup = set;
		Timerequsted = Time.time;
	}

	public bool Equals(PickupInfo other)
	{
		if (other == null) {
			return false;
		}

		if(this.Pickup.name.Equals(other.Pickup.name) && this.id.Equals(other.id)){
			return true;
		}
		return false;
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;
		PickupInfo c = obj as PickupInfo ;
		if ((System.Object)c == null)
			return false;
		

		if(this.Pickup.name.Equals(c.Pickup.name) && this.id.Equals(c.id)){
			return true;
		}
		return false;
	}

}

public class WorkerController : MonoBehaviour {


	public List<GameObject> WorkerList;
	public int WorkerCost;

	public MainMoney mymoney;

	public GameObject setter;
	public GameObject Spawnee;
	public List<PickupInfo> PickupQueue;
	public bool BlackSmithBought;

	private ItemLib myitemlist ;
	// Use this for initialization
	void Start () {
		PickupQueue = new List<PickupInfo> ();
		WorkerList = new List<GameObject> ();
		WorkerCost = 10;
		mymoney = GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ();
		myitemlist = GameObject.FindGameObjectWithTag ("HeroController").GetComponent<ItemLib> ();
		MainMoney.BlackBought +=Blacksmith;
	}
	
	// Update is called once per frame
	void Update () {
	

		WorkerAssigner ();


		for (int i =0; i<WorkerList.Count;i++) 
		{
			if (WorkerList[i]== null)
			{
				WorkerList.RemoveAt(i);
			}
		}
		

	}


	public void BuyWorker(){
		
		if (mymoney.CanIBuy (WorkerCost,true)) {
			SpawnWorker ();


		} else {
	
		}

	}

	public bool INeedAPickUp(GameObject source,int id){
		PickupInfo temp = new PickupInfo (source, id);
		if (PickupQueue.Contains (temp) == false) {

			PickupQueue.Add (temp);
			return true;
		} 
		return	false;

	}

	public void WorkerAssigner(){

		if (PickupQueue.Count != 0) {
			for (int i = 0; i < PickupQueue.Count; i++) {

				foreach (GameObject work in WorkerList) {
					if (work.GetComponent<HeroInv> ().DoIHavePickup() ==false && work.GetComponent<HeroInv>().DoIHaveDeliv()==false&&PickupQueue [i].Worker==null) {
						if (work.GetComponent<HeroAI> ().WorkerGoHereForPickUp (PickupQueue[i]) == true) {
							PickupQueue [i].Worker = work;
						}

					}

				}
			}

		}


	}

	public void WorkerDied(int dead){
		
		        
		foreach(PickupInfo temp in PickupQueue){
			if (temp.Worker != null) {
				
				if (temp.Worker.GetComponent<HeroAI> ().HeroIdNumber == dead) {
					temp.Worker = null;

				}
			}
		}
	
		WorkerList.Remove (WorkerList.Find(x=>x.GetComponent<HeroAI>().HeroIdNumber==dead));

	}

	public void PickupDone(PickupInfo finder){
		PickupQueue.Remove (finder);

	}

	public void CancelPickup(GameObject source){

		for(int i =0;i<PickupQueue.Count;i++){

			if (PickupQueue[i].Pickup==source){
				if (PickupQueue [i].Worker != null) {
					PickupQueue [i].Worker.GetComponent<HeroAI> ().PickupCanceled ();
				}
				PickupDone (PickupQueue[i]);
				i = -1;
			}

		}

//		foreach(PickupInfo temp in PickupQueue){
//
//			if (temp.Pickup==source){
//				PickupDone (temp);
//
//			}
//
//		}


	}

	public void SpawnWorker(){

		WorkerList.Add (Instantiate (Spawnee, this.transform.position,this.transform.rotation) as GameObject);

		WeaponInfo test = myitemlist.GetWeapon(1);
		WorkerList [WorkerList.Count - 1].gameObject.GetComponent<HeroInv>().EquipWeapon(test,"Main");
		WorkerList [WorkerList.Count - 1].gameObject.GetComponent<HeroInv>().blacksmith = BlackSmithBought;


		WorkerList [WorkerList.Count - 1].gameObject.name="Worker "+Random.Range(1,10);
		WorkerList [WorkerList.Count - 1].gameObject.GetComponent<HeroAI>().myrole = HeroAI.Role.Worker;
		WorkerList [WorkerList.Count - 1].gameObject.GetComponent<HeroAI>().myColor= Color.red;
		WorkerList [WorkerList.Count - 1].gameObject.GetComponent<HeroAI>().BlackSmithBought = BlackSmithBought;

	

		WorkerList [WorkerList.Count - 1].gameObject.GetComponent<HeroAI>().HeroIdNumber =GameObject.FindGameObjectWithTag ("HeroController").GetComponent<HeroSpawner>().GetNextIdNumber() ;

	}

	void Blacksmith()
	{

		BlackSmithBought = true;

	}




}
