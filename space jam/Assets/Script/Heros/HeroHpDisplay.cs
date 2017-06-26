using UnityEngine;
using System.Collections;

public class HeroHpDisplay : MonoBehaviour {
	public GameObject myParent;
	public Transform mytransform;
	
	
	public GameObject mainCamera;
	public Transform datCamera;
	public Vector3 Displaytowards;
	// Use this for initialization
	void Start () {
		mytransform=transform;
		datCamera= Camera.main.transform;
		Displaytowards = new Vector3(55f,39f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (myParent.GetComponent<HeroAI>().heroHP >=0)
		{
		GetComponent<TextMesh>().text=myParent.name + "\n"+ myParent.GetComponent<HeroAI>().heroHP.ToString();
		
		}
		else
		{
		GetComponent<TextMesh>().text="X X";	
		}
		

	}
	
	void LateUpdate()
	{
		
	mytransform.rotation = datCamera.transform.rotation;	
	}
}

/*
heroRotate = Quaternion.LookRotation(heroLookat);
		
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation , heroRotate ,Time.deltaTime*turnSpeed );*/