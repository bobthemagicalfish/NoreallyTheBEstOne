using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HeroSpawner : MonoBehaviour {
	// x left and right z foward and back for max distance /// <summary>  ////////////xxx
	/// </summary>
	public int maxHero=1;
	public int heroCount=0;
	public int heroSpawnRate=60;
	public int heroIdNumber=1;
	public float spawnCounter=0.0f; //changed 7/27/17 Drew, changed to float
	// sides
	public float[] maxDistancex  ;
	// away from inn
	public float[] maxDistancez  ;


	public MainMoney mainMoney;
	public GameObject setter;
	public GameObject herospawne;
	public List<GameObject> HeroList;
	
    public bool HerosCanSpawn=false;
    public bool BlackSmithBought=false;

    private ItemLib myitemlist ;

	// Use this for initialization
	void Start () {
		HeroList = new List<GameObject> ();
		//Instantiate(herospawne,transform.position,transform.rotation);
        MainMoney.BlackBought +=Blacksmith;
		ItemLib.ItemsBeDone += ItemListDone ;
		maxDistancex = new float[4];
		// max disitances base on hero levels 
		//side to side
		maxDistancex [0] = 150.0f;
		maxDistancex [1] = 200.0f;
		maxDistancex [2] = 250.0f;
		maxDistancex [3] = 300.0f;
		//front to back
		maxDistancez = new float[4];
		
		maxDistancez [0] = 150.0f;
		maxDistancez [1] = 300.0f;
		maxDistancez [2] = 450.0f;
		maxDistancez [3] = 600.0f;
	}
	
	// Update is called once per frame
	void Update () {
        
		Debug.DrawLine (new Vector3 (maxDistancex [0], 1, maxDistancez [0]), new Vector3 (-maxDistancex [0], 1, maxDistancez [0]));
		Debug.DrawLine (new Vector3 (maxDistancex [1], 1, maxDistancez [1]), new Vector3 (-maxDistancex [1], 1, maxDistancez [1]));
		Debug.DrawLine (new Vector3 (maxDistancex [2], 1, maxDistancez [2]), new Vector3 (-maxDistancex [2], 1, maxDistancez [2]));
		Debug.DrawLine (new Vector3 (maxDistancex [3], 1, maxDistancez [3]), new Vector3 (-maxDistancex [3], 1, maxDistancez [3]));

		Debug.DrawLine (new Vector3 (maxDistancex [0], 1, maxDistancez [0]), new Vector3 (maxDistancex [0], 1, 40));
		Debug.DrawLine (new Vector3 (maxDistancex [1], 1, maxDistancez [1]), new Vector3 (maxDistancex [1], 1, maxDistancez [0]));
		Debug.DrawLine (new Vector3 (maxDistancex [2], 1, maxDistancez [2]), new Vector3 (maxDistancex [2], 1, maxDistancez [1]));
		Debug.DrawLine (new Vector3 (maxDistancex [3], 1, maxDistancez [3]), new Vector3 (maxDistancex [3], 1, maxDistancez [2]));

		Debug.DrawLine (new Vector3 (-maxDistancex [0], 1, maxDistancez [0]), new Vector3 (-maxDistancex [0], 1, 40));
		Debug.DrawLine (new Vector3 (-maxDistancex [1], 1, maxDistancez [1]), new Vector3 (-maxDistancex [1], 1, maxDistancez [0]));
		Debug.DrawLine (new Vector3 (-maxDistancex [2], 1, maxDistancez [2]), new Vector3 (-maxDistancex [2], 1, maxDistancez [1]));
		Debug.DrawLine (new Vector3 (-maxDistancex [3], 1, maxDistancez [3]), new Vector3 (-maxDistancex [3], 1, maxDistancez [2]));

//		if (Input.GetKeyDown(KeyCode.Z) && (heroCount <maxHero))
//		{
//			Instantiate(herospawne,transform.position,transform.rotation);
//		}

		if ((spawnCounter>=heroSpawnRate)&&HerosCanSpawn==true)
		{
            if (myitemlist == null)
            {
                myitemlist = this.GetComponent<ItemLib>();
            }
			if (heroCount<maxHero)
			{
				SpawnHero();
			}

		}

		if (heroCount != maxHero) {

			spawnCounter += Time.deltaTime; //changed 7/27/17 Drew, counts up by each real time second
		
			//Debug.Log("SpawnCounter: " + spawnCounter.ToString("f0")); converts the float into an integer for display purposes only

		} else {

			spawnCounter = 0.0f;
		}

		for (int i =0; i<HeroList.Count;i++) 
		{
			if (HeroList[i]== null)
			{
				HeroList.RemoveAt(i);
			}
		}
		

		heroCount = HeroList.Count;
	}
	
	public void MaxHeroIncrease()
	{
		maxHero=maxHero+1;	
		
	}

    void Blacksmith()
    {

        BlackSmithBought = true;

    }
	void ItemListDone()
	{

		HerosCanSpawn = true;
	}

	public float[] HowFarCanIGo(int mylevel)
	{
		float[] myxz = new float[2];
		if (mylevel == 1 || mylevel == 2) {
			myxz [0] = maxDistancex [0];
			myxz [1] = maxDistancez [0];

		}

		return myxz;
	}
	public float[] ZoneMax(int zone)
	{
		float[] myxz = new float[2];

			myxz [0] = maxDistancex [zone];
			myxz [1] = maxDistancez [zone];



		return myxz;

	}
//	public float[] ZoneMin(int zone)
//	{
//		float[] myxz = new float[2];
//
//			myxz [0] = maxDistancex [zone-1];
//			myxz [1] = maxDistancez [zone-1];
//
//
//
//		return myxz;
//
//	}

	public void SpawnHero()
	{
		HeroList.Add(Instantiate(herospawne,transform.position,transform.rotation)as GameObject);
		setter = HeroList [HeroList.Count - 1];
		setter.gameObject.name="Hero Number "+Random.Range(1,10);
		setter.gameObject.GetComponent<HeroAI>().myColor= Color.blue;
        setter.gameObject.GetComponent<HeroAI>().BlackSmithBought = BlackSmithBought;
        WeaponInfo test = myitemlist.GetWeapon(1);
        setter.gameObject.GetComponent<HeroInv>().MainHandWeapon = myitemlist.GetWeapon(1);
        setter.gameObject.GetComponent<HeroInv>().blacksmith = BlackSmithBought;
		setter.gameObject.GetComponent<HeroAI> ().HeroIdNumber = heroIdNumber;
		setter.gameObject.GetComponent<HeroAI> ().myrole = HeroAI.Role.Hero;
		//starting the ai shoping bias starting with weapon bais
	

		setter.gameObject.GetComponent<HeroAI> ().Weaponstatsbias.Add ("Damage", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Weaponstatsbias.Add ("Durablity", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Weaponstatsbias.Add ("Range", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Weaponstatsbias.Add ("Weight", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Weaponstatsbias.Add ("Beauty", Random.Range (0 , 10));
	



		// now doing armor bias
		setter.gameObject.GetComponent<HeroAI> ().Armorstatsbias.Add ("Defense", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Armorstatsbias.Add ("Durablity", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Armorstatsbias.Add ("Weight", Random.Range (0 , 10));
		setter.gameObject.GetComponent<HeroAI> ().Armorstatsbias.Add ("Beauty", Random.Range (0 , 10));

		//bias towards weapon types
		foreach(WeapArmorBaseInfo x in myitemlist.WeaponBases){
			setter.gameObject.GetComponent<HeroAI> ().Weapontypebias.Add (x.name, Random.Range (-10, 11));

		}
		//bias towrs armor types
		foreach(WeapArmorBaseInfo x in myitemlist.ArmorBases){
			setter.gameObject.GetComponent<HeroAI> ().Armortypebias.Add (x.name, Random.Range (-10, 11));

		}
		setter.gameObject.GetComponent<HeroAI> ().ArmorClassbias.Add ("Light", Random.Range (-10, 11));
		setter.gameObject.GetComponent<HeroAI> ().ArmorClassbias.Add ("Medium", Random.Range (-10, 11));
		setter.gameObject.GetComponent<HeroAI> ().ArmorClassbias.Add ("Heavy", Random.Range (-10, 11));

		heroIdNumber += 1;
		spawnCounter=0.0f;
	}

	public int GetNextIdNumber(){
		heroIdNumber+=1;
		return (heroIdNumber - 1);


	}
}
