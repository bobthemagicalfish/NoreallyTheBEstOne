using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class RangeForItems{
	public string Name;
	public int BounusHigh;
	public int BounusLow;
	public float Chance;

	public RangeForItems()
	{		
		Name = "";
		BounusHigh = 0;
		BounusLow = 0;
		Chance=0.0f;

	}
	public RangeForItems(string na,int boush,int boushl, float chan)
	{		
		Name = na;
		BounusHigh = boush;
		BounusLow = boushl;
		Chance=chan;

	}

	public List<RangeForItems> GetRanges(int average)
	{	
		int RangeModHigh = average + 20;
		int RangeModLow = average - 20;
		List<RangeForItems> MyRanges = new List<RangeForItems> ();
		foreach (ItemQualityDesc temp in GameObject.FindGameObjectWithTag("HeroController").GetComponent<ItemLib>().QuailtyDesc) {
				
			if (temp.RangeValueLow > RangeModHigh || temp.RangeValueHigh < RangeModLow) {

			} else {
				// checking to see if low is between 2 ranges and getting chance
				if (temp.RangeValueLow <= RangeModLow && temp.RangeValueHigh >= RangeModLow) {

					MyRanges.Add (new RangeForItems (temp.Name, temp.BounsHigh, temp.BounsLow, Mathf.Round(( ( (float)(temp.RangeValueHigh - RangeModLow+1) / (float)(RangeModHigh - RangeModLow+1) ) * 100)) ) );
				}
				// checking to see if neither low or high fall in the range have to add plus 1 to get all the numbers 
				else if (temp.RangeValueLow >= RangeModLow && temp.RangeValueHigh <= RangeModHigh) {
					MyRanges.Add (new RangeForItems (temp.Name, temp.BounsHigh, temp.BounsLow, Mathf.Round(( ( ( (float) (temp.RangeValueHigh - temp.RangeValueLow)+1 ) / (float)(RangeModHigh - RangeModLow+1)) * 100))));

				} else if (temp.RangeValueLow <= RangeModHigh && temp.RangeValueHigh >= RangeModHigh) {
					MyRanges.Add (new RangeForItems (temp.Name, temp.BounsHigh, temp.BounsLow, Mathf.Round( (( (float)(RangeModHigh-temp.RangeValueLow+1) / (float)(RangeModHigh - RangeModLow+1)) * 100))));

				} else {

					Debug.Log ("errorin rangefinding");
				}


			}

		}


			
		return MyRanges;
		}



	}
[System.Serializable]
public class LastTimeLookedAt{
	public int Who;
	public float time;

	public LastTimeLookedAt(){
		Who = 0;
		time = 0.0f;


	}
	public LastTimeLookedAt(int whois , float thetime){
		Who = whois;
		time = thetime;

	}
	public bool Equals(LastTimeLookedAt other)
	{
		if (other == null) 
			return false;
		return (this.Who.Equals(other.Who));
	}
	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;
		LastTimeLookedAt c = obj as LastTimeLookedAt ;
		if ((System.Object)c == null)
			return false;
		return Who == c.Who;
	}


}

[System.Serializable]
public class DeliveryItem{
	public string Origin;
	public string Destination;
	public int  Price;
	public Vector3 DesVec;
	public List<MatInfo> Mats;
	public List<WeaponInfo> Weapon ;
	public List<ArmorInfo> Armor;


	public DeliveryItem(){
		Mats = new List<MatInfo>();
		Weapon = new List<WeaponInfo>();
	 	Armor = new List<ArmorInfo> ();
		Origin = "";
		Destination = "";
		Price = 0;
		DesVec = new Vector3 ();
	}

	public DeliveryItem(string org, string dest,int Price2, Vector3  desvect, List<MatInfo> mat,List<WeaponInfo> weap,List<ArmorInfo> arm){
		Mats = mat;
		Weapon = weap;
		Price = Price2;
		Armor = arm;
		Origin = org;
		Destination = dest;
		DesVec = desvect;

	}

	public bool Isempty(){
		if (Mats.Count == 0 & Weapon.Count == 0 && Armor.Count == 0) {
			return true;


		} else {
			return false;
		}
	}
	public int DelWeight(){

		int weight = 0;

		if(Mats.Count!=0){
			foreach(MatInfo temp in Mats){

				weight += temp.BaseWeight;
			}

		}
		if(Weapon.Count!=0){

			foreach(WeaponInfo temp in Weapon){

				weight += temp.Weight;
			}
		}
		if(Armor.Count!=0){
			foreach(ArmorInfo temp in Armor){

				weight += temp.Weight;
			}

		}
		return weight;

	}

}




[System.Serializable]
public class ItemQualityDesc{
	public string Name ;
	public int BounsLow;
	public int BounsHigh;
	public int RangeValueLow;
	public int RangeValueHigh;
	public float PriceMod;

	ItemQualityDesc()
	{
		PriceMod = 0.0f;
		Name = "";
		BounsLow = 0;
		BounsHigh = 0;
		RangeValueLow = 0;
		RangeValueHigh = 0;
	
	}
	public ItemQualityDesc(string na, int lo,int hi,int rangelow,int rangehigh,float pricemod)
	{
		Name = na;
		BounsLow = lo;
		BounsHigh = hi;
		RangeValueLow=rangelow ;
		RangeValueHigh = rangehigh;
		PriceMod = pricemod;
	
	}


}




[System.Serializable]
public class WeaponEnchantInfo{
	public string Name;
	public string Damagetype;
	public int DamageMod;
	public int DurMod;
	public int Weightmod;
	public int Rangemod;
	public int HeroLevelreq;
	public int Wizardlevelreq;

  

	public WeaponEnchantInfo()
	{
	
		DurMod = 0;
		HeroLevelreq = 0;
		Weightmod = 0;
		Rangemod = 0;
		Damagetype = "";
		Name = "";
		Rangemod = 0;
        Wizardlevelreq = 0;



	}

}
[System.Serializable]
public class ArmorEnchantinfo
{
	public int Damagereducmod;
	public  int Durablitymod;
	public int Weightmod;
	public string Damageredtype;
	public string Name;
	public int HeroLevelreq;
    public int Wizardlevelreq;


	public ArmorEnchantinfo()
	{
		 Damagereducmod=0;
		 Durablitymod=0;
		Weightmod=0;
		Damageredtype="";
		Name="";
		HeroLevelreq = 0;
        Wizardlevelreq = 0;
	
	}
	
}
[System.Serializable]
public class Partinfo{
	public string Name;
	public MatInfo Mat;

	public float DamagePercent;
	public float WeightPercent;
	public float DurPercent;
	public float DefPercent;
	public float PricePercent;


	public Partinfo()
	{
		Name="";
		Mat=new MatInfo();
	}
	public Partinfo(string na,MatInfo ma,float damange,float weight,float dur,float def,float price){
		Name=na;
		Mat=MatInfo.copyMe(ma);
		DamagePercent = damange;
		WeightPercent = weight;
		DurPercent = dur;
		DefPercent = def;
		PricePercent = price;
	}

}
[System.Serializable]
public class ArmorSize{
	public string Name;
	public float Weightmod;
	public float Defensemod;
	public float Durablitymod;
	public float Matuseage;

	public ArmorSize(){
		Name = "";
		Weightmod = 0.0f;
		Defensemod = 0.0f;
		Durablitymod = 0.0f;
		Matuseage = 0.0f;
	}

	public ArmorSize(string nam,float weight,float defense,float durablity,float Matu){
		Name = nam;
		Weightmod = weight;
		Defensemod = defense;
		Durablitymod = durablity;
		Matuseage = Matu;

	}

	public static ArmorSize copyme (ArmorSize copy)
	{
		ArmorSize temp = new ArmorSize ();
		temp.Defensemod = copy.Defensemod;
		temp.Weightmod = copy.Weightmod;
		temp.Name = copy.Name;
		temp.Durablitymod = copy.Durablitymod;
		return (temp);

	}
}


[System.Serializable]
public class WeapArmorBaseInfo{
	public string name;
	public int BaseDamage;
	public int BaseDurablity;
	public int BaseRange;
	public int BaseDefense;
	public int BaseBeauty;
	public bool Onehand;
	public int BaseWeight;
	public int CostToCraftPerSlot;
	public int TimeToCraft;
	public int Price;
	public Texture CraftingPic;
	public List<Partinfo> Parts;



	public WeapArmorBaseInfo()
	{
		CostToCraftPerSlot = 0;
		name = "opps";
		BaseDamage = 0;
		BaseDurablity = 0;
		BaseRange = 0;
		BaseBeauty = 0;
		Parts = new List<Partinfo>();
		Onehand = true;
		BaseWeight = 0;
		TimeToCraft = 0;
		Price = 0;
	}
	public WeapArmorBaseInfo(string namey, int BaseDam,int BaseDur,int BaseRan,int BaseDef,int BaseWe,bool One,List<Partinfo> Party,int CraftPerSlot,int TimetoC,int baseprice,int basebeaut)
	{
		BaseBeauty = basebeaut;
		BaseDefense = BaseDef;
		name = namey;
		BaseWeight = BaseWe;
		Onehand = One;
		BaseDamage = BaseDam;
		BaseDurablity = BaseDur;
		BaseRange = BaseRan;

		Parts = new List<Partinfo>();
		foreach (Partinfo temper in Party) {
			Partinfo temp = new Partinfo ();
			temp.Mat=MatInfo.copyMe( temper.Mat);
			temp.Name = temper.Name;
			temp.DamagePercent = temper.DamagePercent;
			temp.DefPercent = temper.DefPercent;
			temp.DurPercent = temper.DurPercent;
			temp.WeightPercent = temper.WeightPercent;
			temp.PricePercent = temper.PricePercent;

			Parts.Add (temp);
		}
		CostToCraftPerSlot = CraftPerSlot;
		TimeToCraft = TimetoC;
		Price = baseprice;
	}

	public static WeapArmorBaseInfo Copyme(WeapArmorBaseInfo copy)
	{
		WeapArmorBaseInfo temp = new WeapArmorBaseInfo (); 
		temp.BaseBeauty = copy.BaseBeauty;
		temp.name = copy.name;
		temp.BaseDamage=copy.BaseDamage;
		temp.BaseDurablity= copy.BaseDurablity;
		temp.BaseRange=copy.BaseRange;
		temp.BaseDefense=copy.BaseDefense; 
		foreach (Partinfo temper in copy.Parts) {
			Partinfo tempy = new Partinfo ();
			tempy.Mat=MatInfo.copyMe( temper.Mat);
			tempy.Name = temper.Name;
			tempy.DamagePercent = temper.DamagePercent;
			tempy.DefPercent = temper.DefPercent;
			tempy.DurPercent = temper.DurPercent;
			tempy.WeightPercent = temper.WeightPercent;
			tempy.PricePercent = temper.PricePercent;

			temp.Parts.Add (tempy);
		}
		temp.BaseWeight = copy.BaseWeight;
		temp.Onehand = copy.Onehand;
		temp.CostToCraftPerSlot=copy.CostToCraftPerSlot;
		temp.TimeToCraft = copy.TimeToCraft;
		temp.Price = copy.Price;
		return temp;


	}
}



[System.Serializable]
public class WeaponInfo{
	public int Damage;
    public  int MaxDurablity;
    public  int CurrentDurablity;
	//public MatInfo MatType;
	public int Weight;
	public int Range;
	public int SellPrice;
	public int BasePrice;
	public int Beauty;
	public string Damagetype;
	public string Name;
	public bool OneHanded;
	public string Quality;
	public int QualityNum;
	public List<WeaponEnchantInfo> Enchants;
	public WeapArmorBaseInfo MyBaseinfo;
	public int ID;

	public WeaponInfo()
	{
		ID = -1;
		Damage = 0;
        CurrentDurablity = 0;
        MaxDurablity = 0;
		 SellPrice=0;
		BasePrice=0;
		Weight = 0;
		Range = 0;
		Damagetype = "";
		Name = "";
		OneHanded = true;
		Quality = "";
		Beauty = 0;
		Enchants = new List<WeaponEnchantInfo> ();
		MyBaseinfo = new WeapArmorBaseInfo();
		QualityNum = 0;
	}
	public WeaponInfo(int Dam,int MaxDur,int CurDur,int Weigh,int RAng, string DamgTyp, string Namey, bool Onehan,string Qual,int qualNum,WeapArmorBaseInfo MyBase,int Sellprice,int Baseprice,int beauty,int IDS)
	{
		Beauty = beauty;
		Damage = Dam;
		CurrentDurablity = CurDur;
		MaxDurablity = MaxDur;
		SellPrice = Sellprice;
		BasePrice = Baseprice;
		Weight = Weigh;
		Range = RAng;
		Damagetype = DamgTyp;
		Name = Namey;
		OneHanded = Onehan;
		Quality = Qual;
		Enchants = new List<WeaponEnchantInfo> ();
		MyBaseinfo =WeapArmorBaseInfo.Copyme( MyBase) ;
		QualityNum = qualNum;
		ID = IDS;
	}

	public static WeaponInfo copyMe(WeaponInfo copy)
	{
		
		WeaponInfo temp = new WeaponInfo ();
		temp.ID= copy.ID;
		temp.Beauty = copy.Beauty;
		temp.SellPrice = copy.SellPrice;
		temp.BasePrice = copy.BasePrice;
		temp.CurrentDurablity=copy.CurrentDurablity;
		temp.Damage = copy.Damage;
		temp.Damagetype = copy.Damagetype;
		temp.QualityNum = copy.QualityNum;
		foreach (WeaponEnchantInfo tempo in copy.Enchants) {
			WeaponEnchantInfo tempy = new WeaponEnchantInfo ();
			tempy.DamageMod = tempo.DamageMod;
			tempy.Damagetype = tempo.Damagetype;
			tempy.DurMod = tempo.DurMod;
			tempy.HeroLevelreq = tempo.HeroLevelreq;
			tempy.Name = tempo.Name;
			tempy.Rangemod = tempo.Rangemod;
			tempy.Weightmod = tempo.Weightmod;
			tempy.Wizardlevelreq = tempo.Wizardlevelreq;
			temp.Enchants.Add (tempy);


		}
		//temp.MatType = MatInfo.copyMe( copy.MatType);
		temp.MaxDurablity = copy.MaxDurablity;
		temp.Name = copy.Name;
		temp.OneHanded = copy.OneHanded;
		temp.Quality = copy.Quality;
		temp.Range = copy.Range;
		temp.Weight = copy.Weight;
		temp.MyBaseinfo = WeapArmorBaseInfo.Copyme(copy.MyBaseinfo);
		return temp;
	}

	public bool Equals(WeaponInfo other)
	{
		if (other == null) 
			return false;
		return (this.ID.Equals(other.ID));
	}
	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;
		WeaponInfo c = obj as WeaponInfo ;
		if ((System.Object)c == null)
			return false;
		return ID == c.ID;
	}

}
[System.Serializable]
public class ArmorInfo{
	public int Damagereduc;
    public  int MaxDurablity;
	public  int CurrentDurablity;
	public int SellPrice;
	public int BasePrice;
	public int Weight;
	public int Beauty;
	public string Name;
	public string Quality;
	public int QualityNum;
	List<ArmorEnchantinfo> Enchants;
	public WeapArmorBaseInfo MyBaseinfo;
	public string Size;
	public string Location;
	public int ID;
	public ArmorInfo()
	{
		Damagereduc = 0;
        CurrentDurablity = 0;
        MaxDurablity = 0;
		SellPrice=0;
		BasePrice=0;
		Weight = 0;
		Name = "";
		Quality = "";
		Enchants = new List<ArmorEnchantinfo> ();
		MyBaseinfo = new WeapArmorBaseInfo ();
		Beauty = 0;
		QualityNum = 0;
		Size = "";
		Location = "";
	}

	public ArmorInfo(int Damgre, int curdur,int maxdur,int weight,string Nam, string Qual,int QualNum,WeapArmorBaseInfo Base,int Sellprice,int Baseprice,int beaut,string si,string loc,int IDS)
	{
		Beauty = beaut;
		Damagereduc = Damgre;
		CurrentDurablity = curdur;
		MaxDurablity = maxdur;
		SellPrice = Sellprice;
		BasePrice = Baseprice;
		Weight = weight;
		Name = Nam;
		Quality = Qual;
		QualityNum = QualNum;
		Enchants = new List<ArmorEnchantinfo>();
		MyBaseinfo = WeapArmorBaseInfo.Copyme( Base);
		Size = si;
		Location = loc;
		ID = IDS;
	}

	public static ArmorInfo CopyMe(ArmorInfo copy)
	{
		ArmorInfo temp = new ArmorInfo ();
		temp.Beauty = copy.Beauty;
		temp.Damagereduc = copy.Damagereduc;
		temp.CurrentDurablity = copy.CurrentDurablity;
		temp.Enchants = copy.Enchants;
		temp.BasePrice = copy.BasePrice;
		temp.SellPrice = copy.SellPrice;
		temp.MaxDurablity = copy.MaxDurablity;
		temp.MyBaseinfo = WeapArmorBaseInfo.Copyme (copy.MyBaseinfo);
		temp.Name = copy.Name;
		temp.Quality = copy.Quality;
		temp.Weight =copy.Weight;
		temp.QualityNum = copy.QualityNum;
		temp.Size = copy.Size;
		temp.Location = copy.Location;
		temp.ID = copy.ID;
		return temp;
	}

	public bool Equals(ArmorInfo other)
	{
		if (other == null) 
			return false;
		return (this.ID.Equals(other.ID));
	}
	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;
		ArmorInfo c = obj as ArmorInfo ;
		if ((System.Object)c == null)
			return false;
		return ID == c.ID;
	}



	
}

[System.Serializable]
public class MatInfo{
    public string Name;
    public int BaseDurablity;
	public int BaseDefense;
	public int BaseBeauty;
	public int BaseMeleeDamage;
	public int BaseRangeDamage;
	public int BaseWeight;
	public float PriceMod;
    public int HeroLevelreq;
    public int Blacksmithlevelreq;
	public int CanSpawnIn;
	public int NodeMinAmount;
	public int NodeMaxAmount;
	public int TimeToMineOne;
	public int TimeToRefine;
	public int TimetoSmith;
	// base quality of material poor ,common,good,Supperior
	public int Quality;
	public string QualityName;
	// low average good rich
	public float Density;
	public string DensityName;
	public bool isMineral;
	public bool isRefine;
    public MatInfo()
    {
		BaseDefense = 0;
		BaseBeauty = 0;
		BaseMeleeDamage = 0;
		BaseRangeDamage = 0;
		BaseWeight = 0;
		TimeToRefine = 0;
		TimetoSmith = 0;
		isMineral = true;
		TimeToMineOne = 0;
        BaseDurablity = 0;
        Name = "";
        HeroLevelreq = 0;
        Blacksmithlevelreq = 0;
		CanSpawnIn = 0;
		NodeMaxAmount = 0;
		NodeMinAmount = 0;
		Quality = 0;
		QualityName = "";
		Density = 0.0f;
		DensityName="";
		PriceMod = 0.0f;
		isRefine = false;
    }

	public MatInfo(int Basedur,string Myname, int herolevel,int blacklevl,int CanSpawnlevel, int NodeMin, int NodeMax,int TimeMine,bool Minorwud,int BaseDef,int BaseBea,int BaseMelDam,int BaseRangDam,int TimeRef,int TimeSmit,int BaseWeigh,float price)
    {
		BaseDefense = BaseDef;
		BaseBeauty = BaseBea;
		BaseMeleeDamage = BaseMelDam;
		BaseRangeDamage = BaseRangDam;
		TimeToRefine = TimeRef;
		TimetoSmith = TimeSmit;
		isMineral = Minorwud;
        BaseDurablity = Basedur;
        Name = Myname;
        HeroLevelreq = herolevel;
        Blacksmithlevelreq = blacklevl;
		CanSpawnIn = CanSpawnlevel;
		NodeMaxAmount = NodeMax;
		NodeMinAmount = NodeMin;
		TimeToMineOne = TimeMine;
		//Quality = 1.0f;
		//Density = 0.5f;
		BaseWeight = BaseWeigh;
		PriceMod = price;
		isRefine = false;
    }
	public static MatInfo copyMe(MatInfo copy){

		MatInfo temp = new MatInfo ();
		temp.BaseDurablity = copy.BaseDurablity;
		temp.Blacksmithlevelreq = copy.Blacksmithlevelreq;
		temp.CanSpawnIn = copy.CanSpawnIn;
		temp.Density = copy.Density;
		temp.DensityName = copy.DensityName;
		temp.HeroLevelreq = copy.HeroLevelreq;
		temp.Name = copy.Name;
		temp.NodeMaxAmount = copy.NodeMaxAmount;
		temp.NodeMinAmount = copy.NodeMinAmount;
		temp.Quality = copy.Quality;
		temp.QualityName = copy.QualityName;
		temp.TimeToMineOne = copy.TimeToMineOne;
		temp.isMineral = copy.isMineral;
		temp.PriceMod = copy.PriceMod;

		temp.BaseDefense = copy.BaseDefense;
		temp.BaseBeauty = copy.BaseBeauty;
		temp.BaseMeleeDamage = copy.BaseMeleeDamage;
		temp.BaseRangeDamage = copy.BaseRangeDamage ;
		temp.TimeToRefine = copy.TimeToRefine;
		temp.TimetoSmith = copy.TimetoSmith;
		temp.BaseWeight = copy.BaseWeight;
		temp.isRefine = copy.isRefine;
		return temp;
	}



}



[System.Serializable]
public class ItemLib : MonoBehaviour {
	public List<WeaponInfo> Weaponlist; 
	public WeaponInfo Weaponset;

	public List<ArmorEnchantinfo> ArmorEnchantList;
	public ArmorEnchantinfo ArmorEnchantset;

	public List<ArmorInfo> ArmorList;
	public ArmorInfo Armorset;

	public List<WeaponEnchantInfo> WeaponEnchantList;
	public WeaponEnchantInfo WeaponEnchantset;

	public List<ItemQualityDesc> QuailtyDesc = new List<ItemQualityDesc> ();

    public List<MatInfo> MatType= new List<MatInfo> ();
   
	public List<WeapArmorBaseInfo> WeaponBases= new List<WeapArmorBaseInfo>();
	public List<WeapArmorBaseInfo> ArmorBases= new List<WeapArmorBaseInfo>();

	public List<ArmorSize> ArmorSizes = new List<ArmorSize> ();
	public bool IsLibdonebeingcreated = false;

    public GameObject heroSpawner ;

	public delegate void ItemsDone();
	public static event ItemsDone ItemsBeDone;

	public bool MakeList = true;
	// Use this for initialization
	void Start () {




      //  GameObject.Find("HeroController").GetComponent<HeroSpawner>().HerosCanSpawn=true ;

	}
	public bool output=false;
	// Update is called once per frame
	void Update () {
		if (MakeList==true){
			MakeTheList ();
		}
		if (output==true){
			foreach (Partinfo temp in WeaponBases.Find(x=>x.name=="Sword").Parts) {

				Debug.Log (temp.Mat.Name+temp.Name);
			}
		}
	}

	//public MatInfo(int Basedur,string Myname, int herolevel,int blacklevl,int CanSpawnlevel, int NodeMin, int NodeMax,int TimeMine,bool Minorwud,int BaseDef,int BaseBea,int BaseMelDam,int BaseRangDam,int TimeRef,int TimeSmit)

	public  MatInfo GetMatInfo(string finder){

		return (MatInfo.copyMe (MatType.Find (x => x.Name == finder)));

	}


	public ItemQualityDesc GetQualiy(int quality){
		foreach (ItemQualityDesc temp in QuailtyDesc) {

			if (quality < temp.RangeValueHigh) {
				return temp;

				}


		}

		return null;

	}


	void MakeTheList()
	{
		// rock type here
		MatType.Add(new MatInfo(10,"Wuud",0,0,1,10,50,200,true,1,1,1,2,10,10,1,0.70f));
		MatType.Add (new MatInfo(15,"Irun",0,0,1,10,50,200,true,2,2,4,4,10,10,3,1.0f));
		MatType.Add (new MatInfo(5,"Sliver",1,1,1,10,50,200,true,2,3,6,6,10,10,2,1.3f));
		MatType.Add (new MatInfo(5,"Guld",2,2,2,10,50,200,true,2,4,8,8,10,10,4,1.6f));
		MatType.Add (new MatInfo(20,"Steal",1,1,2,10,50,200,true,5,2,10,6,10,10,5,1.9f));
		MatType.Add (new MatInfo(30,"Mithreal",2,2,2,10,50,200,true,4,12,10,6,10,10,2,2.1f));
		MatType.Add (new MatInfo(40,"Addamantoum",3,3,3,10,50,200,true,6,10,12,5,10,10,8,2.4f));
		MatType.Add (new MatInfo(50,"Unobtanium",4,4,4,10,50,200,true,8,4,16,12,10,10,10,3.0f));


		// tree type down here

		MatType.Add (new MatInfo(10,"Red Wuud",0,0,2,10,50,200,false,2,1,2,2,10,10,2,1.2f));
		MatType.Add (new MatInfo(10,"IrunBark",1,1,2,10,50,200,false,2,1,2,2,10,10,3,1.4f));

		//(string na, int lo,int hi,int range)
		QuailtyDesc.Add  (new ItemQualityDesc ("Shoddy",-4,-2,-50,10,0.5f));
		QuailtyDesc.Add  (new ItemQualityDesc ("Poor",-2,0,11,30,0.8f));	
		QuailtyDesc.Add  (new ItemQualityDesc ("Common",-1,1,31,59,1.0f));           
		QuailtyDesc.Add  (new ItemQualityDesc ("Good",2,4,60,85,1.2f));
		QuailtyDesc.Add  (new ItemQualityDesc ("Superior",4,6,86,95,1.4f));
		QuailtyDesc.Add  (new ItemQualityDesc ("Legendary",6,8,96,1000,1.6f));

		ArmorSizes.Add (new ArmorSize ("Light", 0.5f, 0.6f, 0.6f,0.5f));
		ArmorSizes.Add (new ArmorSize ("Medium", 1.0f, 1.0f, 1.0f, 1.0F));
		ArmorSizes.Add (new ArmorSize ("Heavy", 1.5f, 1.6f, 1.6f,2f));
		WepArmBaseMaker ();

		WeaponMaker();
		WeaponsEnchantmaker ();
		ArmorMaker ();
		ArmorEnchantMaker ();

		ItemsBeDone ();
		MakeList = false;
	}

	// next 4 functions for making starting weapon stats 
     void WepArmBaseMaker(){
		List<Partinfo> tempy = new List<Partinfo>();

		tempy.Add (new Partinfo("Blade", new MatInfo (),0.8f,0.5f,0.33f,0.0f,0.5f));
		tempy.Add (new Partinfo("Hilt", new MatInfo (),0.2f,0.2f,0.33f,0.0f,0.2f));
		tempy.Add (new Partinfo("Scabbard", new MatInfo (),0.0f,0.3f,0.33f,0.0f,0.3f));

		WeaponBases.Add(new WeapArmorBaseInfo("Sword",5,5,2,0,5,true,tempy,2,10,20,10));

		tempy= new List<Partinfo>();

		tempy.Add (new Partinfo("Head", new MatInfo (),0.8f,0.2f,0.3f,0.0f,0.5f));
		tempy.Add (new Partinfo("Shaft", new MatInfo (),0.2f,0.8f,0.7f,0.0f,0.5f));
		WeaponBases.Add(new WeapArmorBaseInfo("Spear",3,5,4,0,3,true,tempy,1,5,10,10));

		tempy= new List<Partinfo>();

		tempy.Add (new Partinfo("Grip", new MatInfo (),0.8f,0.2f,0.3f,0.0f,0.5f));
		tempy.Add (new Partinfo("Bow Limb", new MatInfo (),0.2f,0.8f,0.7f,0.0f,0.5f));
		WeaponBases.Add(new WeapArmorBaseInfo("Bow",2,5,10,0,3,false,tempy,2,8,15,10));

		tempy= new List<Partinfo>();


		tempy.Add (new Partinfo("Helemt", new MatInfo (),0,1,1,1,1));
			
		ArmorBases.Add(new WeapArmorBaseInfo("Helmet",0,5,0,5,2,false,tempy,1,5,10,10));

		tempy= new List<Partinfo>();


		tempy.Add (new Partinfo("Chest", new MatInfo (),0f,0.5f,0.5f,0.5f,0.5f));
		tempy.Add (new Partinfo("Shoulder", new MatInfo (),0f,0.2f,0.2f,0.2f,0.2f));
		tempy.Add (new Partinfo("Arms", new MatInfo (),0f,0.3f,0.3f,0.3f,0.3f));
		ArmorBases.Add(new WeapArmorBaseInfo("Chestpiece",0,5,0,5,7,true,tempy,2,10,30,10));


		tempy=new List<Partinfo>();


		tempy.Add (new Partinfo("Legging", new MatInfo (),0f,0.5f,0.5f,0.5f,0.5f));
		tempy.Add (new Partinfo("Greaves", new MatInfo (),0f,0.5f,0.5f,0.5f,0.5f));

		ArmorBases.Add(new WeapArmorBaseInfo("Legging",0,5,0,5,3,true,tempy,2,8,15,10));

	}

	void WeaponMaker(){
		Weaponlist = new List<WeaponInfo> ();
		Weaponset = new WeaponInfo (5,5,5,10,2,"","Wuddy Sword",true,"",0,WeaponBases.Find(x=>x.name=="Sword"),10,10,10,0);


//
//		Weaponset.Damage = 10;
//      //  Weaponset.MatType = MatType.Find(x=>x.Name== "Wuud");
//		Weaponset.Weight = 1;
//		Weaponset.Range = 2;
//		Weaponset.Name="Wuudy Sword";
//		Weaponset.OneHanded = true;
//		Weaponset.Quality = "";
		Weaponlist.Add (Weaponset);
	}

    public WeaponInfo GetWeapon(int pos)
    {
		WeaponInfo temp = new WeaponInfo ();
        if (pos <= Weaponlist.Count)
        {
			temp = WeaponInfo.copyMe (Weaponlist [pos-1]);
        }
		else
		{
			temp = WeaponInfo.copyMe (Weaponlist [0]);
		}
        return temp;



    }

	void WeaponsEnchantmaker()
	{
		WeaponEnchantList = new List<WeaponEnchantInfo> ();
		WeaponEnchantset = new WeaponEnchantInfo ();


		WeaponEnchantset.DurMod = 0;
		WeaponEnchantset.HeroLevelreq = 0;
		WeaponEnchantset.Weightmod = 0;
		WeaponEnchantset.Rangemod = 0;
		WeaponEnchantset.Damagetype = "Potato";
		WeaponEnchantset.Name = "Error";
		WeaponEnchantset.Rangemod = 0;
		Weaponset.Quality = "";
	
		
	}

	void ArmorMaker(){
		
		ArmorList= new  List<ArmorInfo>();
		Armorset= new ArmorInfo();




//		Armorset.Damagereduc = 1;
//		
//        Armorset.MatType = MatType.Find(x=>x.Name== "Wuud");
//		Armorset.Weight = 5;
//		Armorset.Name = "Wuuden Armor";
//		Armorset.Quality = "";
//		ArmorList.Add (Armorset);

	}

	public string GetMyName(List<Partinfo> input)
	{
		bool same = true;
		string samename = input [0].Mat.Name;
		string name = "";
		if (input.Count == 1) {
			name = input [0].Mat.Name;

		} else {

			foreach (Partinfo temp in input) {
				if (samename!=temp.Mat.Name){
					same = false;

				}


				name += temp.Mat.Name.Substring (0, 2);

			}
		}
		if (same == false) {
			return name;
		} else {
			return samename;
		}


	}


	void ArmorEnchantMaker()
	{
		 ArmorEnchantList= new List<ArmorEnchantinfo>();
		ArmorEnchantset = new  ArmorEnchantinfo();


		ArmorEnchantset.Damagereducmod=0;
		ArmorEnchantset.Durablitymod=0;
		ArmorEnchantset.Weightmod=0;
		ArmorEnchantset.Damageredtype="Potto";
		ArmorEnchantset.Name="Potato";
		ArmorEnchantset.HeroLevelreq = 0;

		
	}

	public MatInfo MatGen(int levelofarea)
	{
		List<MatInfo> tempo = new List<MatInfo> ();
		foreach(MatInfo looker in MatType)
		{
			if( looker.CanSpawnIn <=levelofarea)
			{
				tempo.Add (looker);


			}

		}

		foreach(MatInfo looker in MatType)
		{
			if( looker.CanSpawnIn <=levelofarea)
			{
				tempo.Add (looker);


			}

		}

		return tempo[Random.Range(0,tempo.Count)] ;
	}


}
