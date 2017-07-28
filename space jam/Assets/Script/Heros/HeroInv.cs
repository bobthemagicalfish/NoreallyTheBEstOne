using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HeroInv : MonoBehaviour {
//	public int weaponDamage=10;
//	public int armor=3;
//	public int shield=0;
//	public int accessory=0;
//	public int weaponLVL=1;
//	public int armorLVL=1;
//	public int shieldLVL=1;

	public bool blacksmith = false;
	// Use this for initialization
	public WeaponInfo MainHandWeapon;
	public WeaponInfo OffHandWeapon;
	public ArmorInfo ShieldArmor;
	public ArmorInfo ChestArmor;
	public ArmorInfo HeadArmor;
	public ArmorInfo LegArmor;
	public ArmorInfo FeetArmor;
	public DeliveryItem MyDeliv;
	public PickupInfo MyPickup;
	public int MaxWeight;
	public MainMoney MyMoney;
	public int CurrentWeight;
	public bool DeliveryFailed=false;
	public float TimeBeforeTryDelivery=0.0f;
	public HeroAI MyHero;
	void Start () {
		MyPickup = new PickupInfo ();
		MyDeliv = new DeliveryItem ();
		MaxWeight = 50;
		MyMoney = GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ();
		MyHero = this.gameObject.GetComponent<HeroAI> ();
//		MainHandWeapon = new WeaponInfo ();
//		OffHandWeapon= new WeaponInfo ();
//		ShieldArmor= new ArmorInfo();
//		ChestArmor = new ArmorInfo();
//		HeadArmor= new ArmorInfo();
//		LegArmor= new ArmorInfo();
//		FeetArmor= new ArmorInfo();
	}
	
	// Update is called once per frame
	void Update () {

		if (DeliveryFailed == true) {
			if (TimeBeforeTryDelivery < Time.time) {

				DeliveryFailed = false;
			}
		}
	//	MyDeliv = new DeliveryItem ();
//		if (blacksmith == false)
//		{
//			if (GameObject.FindGameObjectWithTag ("BlackSmith").transform.position.y >= 0) {
//				blacksmith = true;
//			
//			}
//		}
	}

	public void EquipWeapon(WeaponInfo NewWeapon,string location)
	{
		if (location=="Main")
		{
			CurrentWeight = (CurrentWeight - MainHandWeapon.Weight) + NewWeapon.Weight;
			MainHandWeapon=WeaponInfo.copyMe(NewWeapon);
		}
		else if (location=="Off")
		{
			CurrentWeight = (CurrentWeight - OffHandWeapon.Weight) + NewWeapon.Weight;
			OffHandWeapon=NewWeapon;

		}

	}


	public bool DoIHaveThisArmor(string location){

		if (location == "Chestpiece")
		{
			if(ChestArmor.Name==""){

				return false;

			}
			else{
				return true;
			}
		}
		else if(location=="Shield")
		{
			if(ShieldArmor.Name==""){

				return false;

			}
			else{
				return true;
			}
		}
		else if(location=="Helmet")
		{
			if(HeadArmor.Name==""){

				return false;

			}
			else{
				return true;
			}
		}

		else if(location=="Leg")
		{
			if(LegArmor.Name==""){

				return false;

			}
			else{
				return true;
			}
		}
		else if(location=="Feet")
		{
			if(FeetArmor.Name==""){

				return false;

			}
			else{
				return true;
			}
		}
		Debug.Log ("No Armor returned something wrong bro1"+location);
		return false;

	}

	public ArmorInfo Getarmor(string location){
		if (location == "Chestpiece")
		{
			return ChestArmor;
		}
		else if(location=="Shield")
		{
			return ShieldArmor;
		}
		else if(location=="Helmet")
		{
			return HeadArmor;
		}
		else if(location=="Leg")
		{
			return LegArmor;	
		}
		else if(location=="Boots")
		{
			return FeetArmor;
		}


		Debug.Log ("No Armor returned something wrong bro2"+location);
		return null;


	}


	public void EquipArmor(ArmorInfo NewArmor,string location)
	{
		if (location == "Chestpiece")
		{
			if (ChestArmor != null) {
				CurrentWeight = (CurrentWeight - ChestArmor.Weight) + NewArmor.Weight;
			}
			ChestArmor=NewArmor;
		}
		else if(location=="Shield")
		{
			if (ShieldArmor != null) {
				CurrentWeight = (CurrentWeight - ShieldArmor.Weight) + NewArmor.Weight;
			}
			ShieldArmor=NewArmor;
		}
		else if(location=="Helmet")
		{
			if (HeadArmor != null) {
				CurrentWeight = (CurrentWeight - HeadArmor.Weight) + NewArmor.Weight;
			}
			HeadArmor=NewArmor;
		}
		else if(location=="Legging")
		{
			if (LegArmor != null) {
				CurrentWeight = (CurrentWeight - LegArmor.Weight) + NewArmor.Weight;
			}
			LegArmor=NewArmor;
		}
		else if(location=="Boots")
		{
			if (FeetArmor != null) {
				CurrentWeight = (CurrentWeight - FeetArmor.Weight) + NewArmor.Weight;
			}
			FeetArmor=NewArmor;
		}



		
	}

	public int WeightLeft(){

		return(MaxWeight - CurrentWeight);
	}

	public bool CanICarry(int weight){

		if(CurrentWeight+weight >MaxWeight){

			return false;
		}
		return true;
	}

	public void NewPickup(PickupInfo newone){
		MyPickup = newone;

	}

	public bool NewDelivry(DeliveryItem newone){

		if (newone.Destination != "" && newone.DesVec != new Vector3 () && newone.Isempty () == false){
			
			MyDeliv = newone;
			CurrentWeight += newone.DelWeight();
			MyHero.MyWorkControl.PickupDone (MyPickup);
			MyPickup = new PickupInfo ();

			return 	true;
		}

		MyHero.MyWorkControl.PickupDone (MyPickup);


		if (MyHero.myrole == HeroAI.Role.Hero && MyPickup.Pickup.tag=="ResourceNode") {
			
			MyPickup.Pickup.GetComponent<ResNode> ().IDontWanna (MyHero.HeroIdNumber);
		}
		MyPickup = new PickupInfo ();
		return false;
	
	}



	public void DropOff(){


		if (MyDeliv.Armor.Count != 0) {


		}
		if (MyDeliv.Weapon.Count != 0) {


		}
		if (MyDeliv.Mats.Count != 0) {
			List<MatInfo> Stillneed = new List<MatInfo> ();
			foreach (MatInfo temp in MyDeliv.Mats) {
				if ((MyMoney.CanIBuy (MyDeliv.Price,true) == true) && (MyMoney.IsResourceMax (temp, 1, temp.isRefine) == false)) {
					MyHero.HeroGold += MyDeliv.Price;
					MyMoney.AddResource (temp, 1, temp.isRefine);
				} else {
					DeliveryFailed = true;
					Stillneed.Add (temp);
					TimeBeforeTryDelivery = Time.time+20.0f;
				}

			}
			if(DeliveryFailed==true){
				MyDeliv.Mats = Stillneed;

			}
		}

			
		if (DeliveryFailed == false) {
			MyDeliv = new DeliveryItem ();
		} 
		//	resNodeTarget = null;
		MyPickup = new PickupInfo ();

	}


	public Vector3 DevGoingto(){

		return MyDeliv.DesVec;
	}

	public Vector3 PickupGoingto(){

		return MyPickup.Pickup.transform.position;
	}
	public bool DoIHavePickup(){
		if(MyPickup.id==-1){

			return false;
		}
		return true;
	}
	public bool DoIHaveDeliv(){
		if(MyDeliv.Isempty()==false && DeliveryFailed==false){
			return true;
		}
		return false;
	}


}
