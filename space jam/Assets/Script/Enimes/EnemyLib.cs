using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MyEnemyBaseClass

{
	public int Maxhp;
	public int Gold;
	public string Name;
	public int maxDistanceFromBase;
	public Color MyColor;
	public Vector3 MySizeDim;
	public Vector3 MyColSize;
	public int MaxSpawn;
	public int MinSpawn;


	// Use this for initialization
	public MyEnemyBaseClass()
	{
		MyColSize = new Vector3 (1, 1, 1);
		MySizeDim = new Vector3 (1, 1, 1);
		Maxhp = 0;
		Gold = 0;
		Name = "Error";
		maxDistanceFromBase = 10;
		MyColor = Color.black;
		MaxSpawn = 5;
		MinSpawn = 1;
	}
}


public class EnemyLib : MonoBehaviour {
	public List<MyEnemyBaseClass> EnemyNamesZoneOne;
	public List<string> EnemyNamesZoneTwo;
	public List<string> EnemyNamesZoneThree;
	public List<string> EnemyNamesZoneFour;

	private MyEnemyBaseClass TheCreator;


	// Use this for initialization
	void Start () {
		EnemyNamesZoneOne= new List<MyEnemyBaseClass>();
		TheCreator = new MyEnemyBaseClass ();
		TheCreator.Name = "Cubion";
		TheCreator.Maxhp = 30;
		TheCreator.Gold = 5;
		TheCreator.maxDistanceFromBase = 10;
		TheCreator.MyColor = Color.green;
		TheCreator.MyColSize = new Vector3 (1.2f, 1.0f, 1.2f);
		TheCreator.MySizeDim = new Vector3 (1.0f, 1.0f, 1.0f);
		TheCreator.MaxSpawn = 5;
		TheCreator.MinSpawn = 2;
		EnemyNamesZoneOne.Add(TheCreator);

		TheCreator = new MyEnemyBaseClass ();
		TheCreator.Name = "Rectro";
		TheCreator.Maxhp = 40;
		TheCreator.Gold = 5;
		TheCreator.maxDistanceFromBase = 10;
		TheCreator.MyColor = Color.cyan;
		TheCreator.MyColSize = new Vector3 (1.2f, 1.0f, 1.2f);
		TheCreator.MySizeDim = new Vector3 (2.5f, 1.3f, 1.0f);
		TheCreator.MaxSpawn = 3;
		TheCreator.MinSpawn = 1;
		EnemyNamesZoneOne.Add(TheCreator);


		GameObject.Find ("EnemyCampSpawner").GetComponent<EnemyCampSpawner> ().CanSpawn = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public MyEnemyBaseClass EnemyName(int Zone)
	{
		if (Zone == 1) 
		{			return EnemyNamesZoneOne[Random.Range(0,EnemyNamesZoneOne.Count)];
		}
		return EnemyNamesZoneOne[0];
	}


}
