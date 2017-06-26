using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuildingLocation : MonoBehaviour {
	public GameObject Inn;
	public List<GameObject> Blacksmith;
	public GameObject Tradepost;
	public GameObject Alchemy;
	public GameObject Guardpost;

	
	// Update is called once per frame
	void Update () {
		//moo.text = Inn.transform.position.x.ToString() + " " +Inn.transform.position.y.ToString();
	}
	
	Vector3 FindInn(){
		
	return Inn.transform.position;	
	}

	public bool IsStore(){

		if(Blacksmith.Count!=0){

			return true;
		}

		return false;
	}

	public GameObject FindStore() {
		return Blacksmith [0];

	}

	public void AddBlacksmith(GameObject newone){
		Blacksmith.Add (newone);


	}

	public bool IsMouseOverMenu(){
		foreach(GameObject x in Blacksmith){
			if (x.GetComponent<BlackSmithMenu>().MouseOverMe()==true){
			//	Debug.Log ("mouseonblack");
				return true;
			}

		}

		if (Inn.GetComponent<InnMenu>().MouseOverMe()==true){
		//	Debug.Log ("mouseoninn");
			return true;
		}

		return false;

	}
}
