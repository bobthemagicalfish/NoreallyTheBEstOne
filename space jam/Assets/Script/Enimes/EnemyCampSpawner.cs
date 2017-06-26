using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//************** respondable for spawning enemy camps in each zone and setting monster types for camps to spawn
[System.Serializable]
public class EnemyCampSpawner : MonoBehaviour {
	public GameObject EnemyCamp;
	public GameObject setter;

	public int[] NumberOfCampsPerZone;
	public int[] SpawnTimerPerZone;

	public float[] zoneMaxDistancez;
	public float[] zoneMaxDistancex;

	public float[] SpawnTimerTimeStampPerZone;

	public List<GameObject> zoneOneCamps;
	public List<GameObject> zoneTwoCamps;
	public List<GameObject> zoneThreeCamps;
	public List<GameObject> zoneFourCamps;


	
	private MyEnemyBaseClass BadGuyForCamp;

	public bool CanSpawn=false;

	public bool Loaded = false;
	// Use this for initialization
	void Start () {
		NumberOfCampsPerZone= new int[4];
		NumberOfCampsPerZone[0]=5;
		NumberOfCampsPerZone[1]=5;
		NumberOfCampsPerZone[2]=5;
		NumberOfCampsPerZone[3]=5;

		//away from inn
		zoneMaxDistancez=new float[5];
		zoneMaxDistancez[0]=25.0f;
		zoneMaxDistancez[1]=130.0f;
		zoneMaxDistancez[2]=300.0f;
		zoneMaxDistancez[3]=450.0f;
		zoneMaxDistancez[4]=600.0f;
		// side to side
		zoneMaxDistancex=new float[5];
		zoneMaxDistancex[0]=130.0f;
		zoneMaxDistancex[1]=130.0f;
		zoneMaxDistancex[2]=130.0f;
		zoneMaxDistancex[3]=130.0f;
		zoneMaxDistancex[4]=130.0f;

		SpawnTimerPerZone= new int[4];
		SpawnTimerPerZone[0]=30;
		SpawnTimerPerZone[1]=30;
		SpawnTimerPerZone[2]=30;
		SpawnTimerPerZone[3]=30;


		SpawnTimerTimeStampPerZone = new float[4];
		SpawnTimerTimeStampPerZone[0]=0.0f;
		SpawnTimerTimeStampPerZone[1]=0.0f;
		SpawnTimerTimeStampPerZone[2]=0.0f;
		SpawnTimerTimeStampPerZone[3]=0.0f;

		zoneOneCamps=new List<GameObject>();
		zoneTwoCamps=new List<GameObject>();
		zoneThreeCamps=new List<GameObject>();
		zoneFourCamps=new List<GameObject>();
		EnemyCamp = Resources.Load("Units\\EnemyCampPrefab") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Loaded == false) {
			Loadstuff ();
		}

		if((zoneOneCamps.Count < NumberOfCampsPerZone[0])&& CanSpawn==true)
		{
			
//		Debug.Log(hit2.point);
		//Debug.Log(hit2.point+new Vector3(0,(EnemyCamp.transform.localScale.y),0));
			zoneOneCamps.Add(Instantiate(EnemyCamp,SpawnLocation(1)+new Vector3(0,((EnemyCamp.transform.localScale.y/2)+.1f),0),transform.rotation) as GameObject);
			setter=zoneOneCamps[zoneOneCamps.Count-1];
			BadGuyForCamp = this.GetComponent<EnemyLib>().EnemyName(1);
			setter.gameObject.GetComponent<BadGuyCampController>().MyEnemyInfo=BadGuyForCamp;
		//	setter.gameObject.GetComponent<BadGuyCampController>().name=BadGuyForCamp.name+" Camp";
		//	setter.gameObject.GetComponent<BadGuyCampController>().maxDistanceFromBase=(10+Random.Range(0,10));
		}

		if((zoneTwoCamps.Count < NumberOfCampsPerZone[1])&& CanSpawn==true)
		{

		
			zoneTwoCamps.Add(Instantiate(EnemyCamp,SpawnLocation(2)+new Vector3(0,((EnemyCamp.transform.localScale.y/2)+.1f),0),transform.rotation) as GameObject);
			setter=zoneTwoCamps[zoneTwoCamps.Count-1];
			BadGuyForCamp = this.GetComponent<EnemyLib>().EnemyName(1);
			setter.gameObject.GetComponent<BadGuyCampController>().MyEnemyInfo=BadGuyForCamp;

		}

		if((zoneThreeCamps.Count < NumberOfCampsPerZone[2])&& CanSpawn==true)
		{

	
			zoneThreeCamps.Add(Instantiate(EnemyCamp,SpawnLocation(3)+new Vector3(0,((EnemyCamp.transform.localScale.y/2)+.1f),0),transform.rotation) as GameObject);
			setter=zoneThreeCamps[zoneThreeCamps.Count-1];
			BadGuyForCamp = this.GetComponent<EnemyLib>().EnemyName(1);
			setter.gameObject.GetComponent<BadGuyCampController>().MyEnemyInfo=BadGuyForCamp;

		}

		if((zoneFourCamps.Count < NumberOfCampsPerZone[3])&& CanSpawn==true)
		{
			
			zoneFourCamps.Add(Instantiate(EnemyCamp,SpawnLocation(4)+new Vector3(0,((EnemyCamp.transform.localScale.y/2)+.1f),0),transform.rotation) as GameObject);
			setter=zoneFourCamps[zoneFourCamps.Count-1];
			BadGuyForCamp = this.GetComponent<EnemyLib>().EnemyName(1);
			setter.gameObject.GetComponent<BadGuyCampController>().MyEnemyInfo=BadGuyForCamp;

		}
		
		// check for dead camp
		for(int i = 0 ; i < zoneOneCamps.Count ; i++)
		{
			
			if (zoneOneCamps[i].Equals(null))
			{
				zoneOneCamps.RemoveAt(i);	
				
			}
	
		}
	}

	Vector3 SpawnLocation (int zone)
	{
		RaycastHit hit2;
		Physics.Raycast(new Vector3(Random.Range(-zoneMaxDistancex[zone],zoneMaxDistancex[zone]),-40.0f,Random.Range(zoneMaxDistancez[zone-1],zoneMaxDistancez[zone])), new Vector3(0,50.0f,0), out hit2,100.0f,(1<<14)|(1<<12)) ;
		while (hit2.collider.name != "Ground") {

			Physics.Raycast(new Vector3(Random.Range(-zoneMaxDistancex[0],zoneMaxDistancex[0]),-40.0f,Random.Range(zoneMaxDistancez[zone-1],zoneMaxDistancez[zone])), new Vector3(0,50.0f,0), out hit2,100.0f,(1<<14)|(1<<12)) ;
		}
		return hit2.point;
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
