using UnityEngine;

using System.Collections;
//adds in list support
 //    List<int> myIntList = new List<int>();
    //List<MyOwnClass> myClassList = new List<MyOwnClass>(); 
//************This controls the camps itself***********
using System.Collections.Generic;


public class BadGuyCampController : MonoBehaviour {
	public int maxBadGuys =3;
	public int currentBadGuys;
	public int badGuySpawnCounter;
	public int diffZone=1;
	public int maxDistanceFromBase =10;

	public GameObject setter;
	private GameObject temp;
	public GameObject BaddiesType;

	public Vector3 myTransform;

	public bool canSpawn=true;

	public List<GameObject> listOfCampBaddies;

	private badGuyAi Badguyai;

	public MyEnemyBaseClass MyEnemyInfo;


	// Use this for initialization
	void Awake()
	{
		listOfCampBaddies=new List<GameObject>();
	}
	void Start () {

	myTransform=transform.position;
		BaddiesType=(Resources.Load("Units\\BadGuyPrefab") )as GameObject;

      

		GetComponent<Renderer>().enabled=true;

        listOfCampBaddies.Add(Instantiate(BaddiesType,transform.position,transform.rotation) as GameObject);
		setter = listOfCampBaddies[listOfCampBaddies.Count-1];

		this.maxBadGuys = Random.Range (MyEnemyInfo.MinSpawn, MyEnemyInfo.MaxSpawn + 1);
			
        this.name = MyEnemyInfo.Name+" Camp" +Random.Range(1,10);

		//Spawns Boss which will have stronger value at some point
		setter.gameObject.name=MyEnemyInfo.Name;
		setter.gameObject.GetComponent<badGuyAi> ().maxDistanceFromHome = MyEnemyInfo.maxDistanceFromBase;
		setter.gameObject.GetComponent<badGuyAi> ().maxHp = MyEnemyInfo.Maxhp;
		setter.gameObject.GetComponent<badGuyAi> ().Hp = MyEnemyInfo.Maxhp;
		setter.gameObject.GetComponent<badGuyAi> ().myGoldValue = MyEnemyInfo.Gold;
		setter.gameObject.GetComponent<badGuyAi> ().myColor = MyEnemyInfo.MyColor;
		setter.gameObject.GetComponent<badGuyAi> ().ChangeMySize(MyEnemyInfo.MySizeDim);

				if (GetComponent<Renderer>().enabled==true)
				{	
					setter.GetComponent<Renderer>().enabled=true;
				}
				currentBadGuys=currentBadGuys+1;
				
	}
	
	// Update is called once per frame
	void Update () {
		if (badGuySpawnCounter == 60)
		{
			if ((listOfCampBaddies.Count < maxBadGuys)&& canSpawn==true)
			{
                listOfCampBaddies.Add(Instantiate(BaddiesType,transform.position,transform.rotation) as GameObject);
			//	listOfCampBaddies.Add(Instantiate(BaddiesType,transform.position+transform.right*(Random.Range(-10,11))+transform.forward*(Random.Range(-10,11)),transform.rotation) as GameObject);
				setter = listOfCampBaddies[listOfCampBaddies.Count-1];
				setter.gameObject.name=MyEnemyInfo.Name;

				setter.gameObject.GetComponent<badGuyAi> ().maxDistanceFromHome = MyEnemyInfo.maxDistanceFromBase;
				setter.gameObject.GetComponent<badGuyAi> ().maxHp = MyEnemyInfo.Maxhp;
				setter.gameObject.GetComponent<badGuyAi> ().Hp = MyEnemyInfo.Maxhp;
				setter.gameObject.GetComponent<badGuyAi> ().myGoldValue = MyEnemyInfo.Gold;
				setter.gameObject.GetComponent<badGuyAi> ().myColor = MyEnemyInfo.MyColor;
				setter.gameObject.GetComponent<badGuyAi> ().ChangeMySize(MyEnemyInfo.MySizeDim);
				if (GetComponent<Renderer>().enabled==true)
				{
					
					setter.GetComponent<Renderer>().enabled=true;
				}
				currentBadGuys=currentBadGuys+1;
				//Debug.Log(listOfCampBaddies.Count);
			}
			badGuySpawnCounter=0;
		}
		
		
		
		
		// check thur list and removes enemys that are dead
		for(int i = 0 ; i < listOfCampBaddies.Count ; i++)
		{
			
			if (listOfCampBaddies[i].Equals(null))
			{
				listOfCampBaddies.RemoveAt(i);	
				currentBadGuys=currentBadGuys-1;
			}
	
		}
		if (listOfCampBaddies.Count==0)
		{
			Debug.Log("Camp destory");
			Destroy (gameObject);
			
		}
			badGuySpawnCounter=badGuySpawnCounter+1;
			//Debug.Log(listOfCampBaddies.Count);
	}
	
	void OnTriggerStay(Collider other)
	{
		
		if (other.GetComponent<Collider>().tag == "Heros")	
		{		
			GetComponent<Renderer>().enabled=true;
            badGuySpawnCounter = 0;
			canSpawn=false;	
		}
		
		
	}
	
	void OnTriggerExit(Collider other)
	{
	//	Debug.Log("Left");
		if (other.GetComponent<Collider>().tag == "Heros")	
		{			
			canSpawn=true;	
		}

	}
}
