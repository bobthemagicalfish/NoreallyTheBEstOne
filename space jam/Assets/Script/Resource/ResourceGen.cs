using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class  ResNodeinfo{
	public int AmountLeft;
	public MatInfo ResType;
	public bool IsMinebuilt;
	public int MineLevel;

	public ResNodeinfo()
	{
		AmountLeft=0;
		ResType=new MatInfo();
		IsMinebuilt=false;
		MineLevel=0;


	}
	public ResNodeinfo(int left,MatInfo res ,bool mine,int minelvl)
	{
		if (left!=0)
		{
			AmountLeft=left;
		}
		else{

			AmountLeft=Random.Range(res.NodeMinAmount,res.NodeMaxAmount+1);
		 
		}
		ResType=MatInfo.copyMe(res);
		IsMinebuilt=mine;
		MineLevel=minelvl;

		ResType.Quality = (Random.Range (0, 130));

		if (ResType.Quality <= 25) {
			ResType.QualityName = "Poor";
			ResType.Quality = 15;
		}
		else if (ResType.Quality <= 50) {
			ResType.QualityName="Common";
			ResType.Quality = 45;
		}
		else if(ResType.Quality <=75){
			ResType.QualityName="Good";
			ResType.Quality = 70;
		}
		else{
			ResType.QualityName = "Superiour";
			ResType.Quality = 85;
			}


		ResType.Density=   (float)System.Math.Round((double)(Random.Range (0.0f, 1.10f)),2);

		if (ResType.Density <= .25) {
			ResType.DensityName = "Sparse";
			ResType.Density = .25f;
		}
		else if (ResType.Density <= .50) {
			ResType.DensityName = "Average";
			ResType.Density = .50f;
		}
		else if(ResType.Density <=.75){
			ResType.DensityName="Abundant";
			ResType.Density = .75f;
		}
		else{
			ResType.DensityName = "Rich";
			ResType.Density = 1.0f;
		}


	}

	public static ResNodeinfo Copyme(ResNodeinfo copy)
	{
		ResNodeinfo temp = new ResNodeinfo ();
		temp.AmountLeft = copy.AmountLeft;
		temp.IsMinebuilt = copy.IsMinebuilt;
		temp.MineLevel = copy.MineLevel;
		temp.ResType = MatInfo.copyMe (copy.ResType);



		return temp;

	}



}
public class ResourceGen : MonoBehaviour {


	public int currentid=0;
	public bool canspawn =false;
	public bool Libbuilt =false;
	public bool Loaded = false;

	public int[] ZoneResZoneMax = new int[4];

	public GameObject ResNodeGameobject;

	public GameObject setter;

	public List<GameObject> ResNodeListZoneOne = new List<GameObject>();
	public List<GameObject> ResNodeListZoneTwo = new List<GameObject>();
	public List<GameObject> ResNodeListZoneThree = new List<GameObject>();
	public List<GameObject> ResNodeListZoneFour = new List<GameObject>();

	public HeroSpawner heroSpawner;

	public float[] zoneMaxDistancez;
	public float[] zoneMaxDistancex;

	public ResNodeinfo nodeMaker;


	// Use this for initialization
	void Start () {
	heroSpawner = GameObject.Find("HeroController").GetComponent<HeroSpawner>();

		ResNodeGameobject=(Resources.Load("RES\\ResNodePrefab") )as GameObject;

	    ItemLib.ItemsBeDone += ItemListDoneres ;

		// Range for resources to spawn from inn
		zoneMaxDistancez=new float[5];
		zoneMaxDistancez[0]=25.0f;
		zoneMaxDistancez[1]=130.0f;
		zoneMaxDistancez[2]=300.0f;
		zoneMaxDistancez[3]=450.0f;
		zoneMaxDistancez[4]=600.0f;
		// lenngh wise
		// [0] doesnt matter since were doing 4 zones where 0 is the min distance which only matters for the z above. 0-1 zone 1 distance 1-2 zone 2 so on
		zoneMaxDistancex=new float[5];
		zoneMaxDistancex[0]=130.0f;
		zoneMaxDistancex[1]=130.0f;
		zoneMaxDistancex[2]=130.0f;
		zoneMaxDistancex[3]=130.0f;
		zoneMaxDistancex[4]=130.0f;

		ZoneResZoneMax [0] = 10;
		ZoneResZoneMax[1] = 0;
		ZoneResZoneMax[2] = 0;
		ZoneResZoneMax [3] = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Loaded == false) {
			Loadstuff ();
		}

		if (canspawn == true && Libbuilt == true) 
		{
			if (ResNodeListZoneOne.Count < ZoneResZoneMax[0]) 
			{
				ResNodeListZoneOne.Add(Instantiate(ResNodeGameobject, spawnresnode (1),transform.rotation) as GameObject);
				nodeMaker=new ResNodeinfo(0, GameObject.Find("HeroController").GetComponent<ItemLib>().MatGen(1),false,0);
				ResNodeListZoneOne [ResNodeListZoneOne.Count - 1].gameObject.GetComponent<ResNode> ().Myres = ResNodeinfo.Copyme (nodeMaker);
				ResNodeListZoneOne [ResNodeListZoneOne.Count - 1].gameObject.name = nodeMaker.ResType.Name+" Node";
				ResNodeListZoneOne [ResNodeListZoneOne.Count - 1].gameObject.GetComponent<ResNode> ().myId = currentid;
				currentid += 1;
			//	Debug.Log (nodeMaker.ResType.Density + " " +nodeMaker.ResType.Name);
				
				//setter.gameObject.name = nodeMaker.ResType.Name + " Node" +setter.gameObject.GetComponent<ResNode>().Myres.ResType.Density+" " +setter.gameObject.GetComponent<ResNode>().Myres.ResType.Quality+" " +setter.gameObject.GetComponent<ResNode>().Myres.AmountLeft+" " +ResNodeListZoneOne.Count;

			}

			if (ResNodeListZoneTwo.Count < ZoneResZoneMax[1]) 
			{
				ResNodeListZoneTwo.Add(Instantiate(ResNodeGameobject, spawnresnode (2),transform.rotation) as GameObject);
				setter = ResNodeListZoneTwo [ResNodeListZoneTwo.Count - 1];
				nodeMaker=new ResNodeinfo(0, GameObject.Find("HeroController").GetComponent<ItemLib>().MatGen(1),false,0);
				setter.gameObject.GetComponent<ResNode> ().Myres = nodeMaker;
				setter.gameObject.name = nodeMaker.ResType.Name + " Node";
				setter.gameObject.GetComponent<ResNode> ().myId = currentid;
				currentid += 1;
			}

			if (ResNodeListZoneThree.Count < ZoneResZoneMax[2]) 
			{
				ResNodeListZoneThree.Add(Instantiate(ResNodeGameobject, spawnresnode (3),transform.rotation) as GameObject);
				setter = ResNodeListZoneThree [ResNodeListZoneThree.Count - 1];
				nodeMaker=new ResNodeinfo(0, GameObject.Find("HeroController").GetComponent<ItemLib>().MatGen(1),false,0);
				setter.gameObject.GetComponent<ResNode> ().Myres = nodeMaker;
				setter.gameObject.name = nodeMaker.ResType.Name + " Node";
				setter.gameObject.GetComponent<ResNode> ().myId = currentid;
				currentid += 1;
			}
			if (ResNodeListZoneFour.Count < ZoneResZoneMax[3]) 
			{
				ResNodeListZoneFour.Add(Instantiate(ResNodeGameobject, spawnresnode (4),transform.rotation) as GameObject);
				setter = ResNodeListZoneFour [ResNodeListZoneFour.Count - 1];
				nodeMaker=new ResNodeinfo(0, GameObject.Find("HeroController").GetComponent<ItemLib>().MatGen(1),false,0);
				setter.gameObject.GetComponent<ResNode> ().Myres = nodeMaker;
				setter.gameObject.name = nodeMaker.ResType.Name + " Node";
				setter.gameObject.GetComponent<ResNode> ().myId = currentid;
				currentid += 1;
			}
			nodeMaker = null;
			setter = null;
		}
	
	}

	void ItemListDoneres()
	{
		canspawn = true;
		Libbuilt = true;

	}


	Vector3 spawnresnode(int zone)
	{
		int loopcount = 0;
		RaycastHit hit;

		Physics.Raycast (new Vector3 (Random.Range (-zoneMaxDistancex [zone], zoneMaxDistancex [zone]), -40.0f, Random.Range (zoneMaxDistancez [zone-1], zoneMaxDistancez [zone])), new Vector3 (0, 50.0f, 0), out hit, 100.0f, (1 << 14) |(1<<16));
		// seeing if hit another res node to spread them out
		while (hit.collider.name!="Ground" && loopcount<=20) {
			loopcount += 1;
			Physics.Raycast (new Vector3 (Random.Range (-zoneMaxDistancex [zone], zoneMaxDistancex [zone]), -40.0f, Random.Range (zoneMaxDistancez [zone-1], zoneMaxDistancez [zone])), new Vector3 (0, 50.0f, 0), out hit, 100.0f, 1 << 14 |1<<16);


		}
		 
//		Debug.Log (loopcount);
		return hit.point +Vector3.up ;
	}



	void Loadstuff()
	{
		GameObject loader = GameObject.FindGameObjectWithTag ("HeroController");
		float[] tempar ;
		tempar =new float[2];

		tempar= loader.GetComponent<HeroSpawner> ().ZoneMax (0);
		zoneMaxDistancex[0]=tempar[0]*.8f;
		zoneMaxDistancex[1]=tempar[0]*.8f;

		zoneMaxDistancez[1]=tempar[1]*.8f;


		tempar= loader.GetComponent<HeroSpawner> ().ZoneMax (1);
		zoneMaxDistancex[2]=tempar[0]*.8f;
		zoneMaxDistancez[2]=tempar[1]*.8f;

		tempar= loader.GetComponent<HeroSpawner> ().ZoneMax (2);
		zoneMaxDistancex[3]=tempar[0]*.9f;
		zoneMaxDistancez[3]=tempar[1]*.9f;

		tempar= loader.GetComponent<HeroSpawner> ().ZoneMax (3);
		zoneMaxDistancex[4]=tempar[0]*.9f;
		zoneMaxDistancez[4]=tempar[1]*.9f;



	}
}
