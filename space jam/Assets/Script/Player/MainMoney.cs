using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ResourceAmount
{
	public string Name;

	public float UPoor;
	public float UCommon;
	public float UGood;
	public float USuperior;

	public float RPoor;
	public float RCommon;
	public float RGood;
	public float RSuperior;

	public ResourceAmount()
	{
		Name = "";
		UPoor = 0;
		UCommon = 0;
		UGood = 0;
		USuperior = 0;
		RPoor = 0;
		RCommon = 0;
		RGood = 0;
		RSuperior = 0;
	}
	public ResourceAmount(string name)
	{
		Name = name;
		UPoor = 100;
		UCommon = 100;
		UGood = 100;
		USuperior = 100;
		RPoor = 100;
		RCommon = 100;
		RGood = 100;
		RSuperior = 100;

	}




}

public class MainMoney : MonoBehaviour {
	public float ScreenHeight ;
	public float ScreenWidth;


	public int PlayerGold =100;
	public int PlayerRenown=0;
	public int PlayerHeroes = 0;
	public int ResourceMax = 110;
	int errorwindowid =9999;
	public GUISkin IncomeSkin;
	
	public Rect heroInfoWindow ;
	public bool showHeroScreen=false;
	
	public Rect goldBuyWindow;
	public bool showGoldScreen=false;
	public Vector2 Myscreensize= new Vector2(1900,1080);



	public bool mouseIsOverMe = false;
	public bool mouseIsOverMe2 = false;
	public bool Errorbox=false;
	public string Errortext="";
    public delegate void BlacksmithBought();
    public static event BlacksmithBought BlackBought;


	public Rect ErrorREctbox;
	public List<ResourceAmount> RawResource = new List<ResourceAmount> ();



	// Use this for initialization Screen.height/1920.0f;
	void Start () {
		ScreenHeight = Screen.height;
		ScreenWidth = Screen.width;
		heroInfoWindow = new Rect(ScreenWidth/2.5f, ScreenHeight/2.75f ,495,200);
        goldBuyWindow = new Rect(ScreenWidth/2.5f, ScreenHeight/2.75f ,500 ,500);
		ErrorREctbox = new Rect (ScreenWidth / 2.5f, ScreenHeight / 2.75f, 200, 100);

		ItemLib.ItemsBeDone += ItemListDone ;
	}
	
	// Update is called once per frame
	void Update () {
		ScreenHeight = Screen.height;
		ScreenWidth = Screen.width;
		PlayerHeroes = GameObject.Find("HeroController").GetComponent<HeroSpawner>().heroCount;
		
		if (Input.GetKeyDown(KeyCode.H))
		{
			
			if (showHeroScreen==false)
			{
				//heroInfoWindow = new Rect(ScreenWidth/2.5f, ScreenHeight/2.75f ,500	 ,500);
				showHeroScreen=true;	
			}
			else
			{
				showHeroScreen=false;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.G))
		{
			
			if (showGoldScreen==false)
			{
			//	goldBuyWindow = new Rect(ScreenWidth/2.5f, ScreenHeight/2.75f ,500	 ,500);
				showGoldScreen=true;	
			}
			else
			{
				showGoldScreen=false;
			}
		}

	}


	// shit to do once itemlistdone built mainly for building rescoure info
	void ItemListDone ()
	{
		foreach (MatInfo temp in GameObject.FindGameObjectWithTag("HeroController").GetComponent<ItemLib>().MatType) {
			RawResource.Add (new ResourceAmount (temp.Name));

		}




	}

	void OnGUI () {
		GUI.skin = IncomeSkin;
	
		IncomeTab();

		ResourceTab ();
		//GUI.Box(new Rect (ScreenWidth - (500.0f / Myscreensize.x) * ScreenWidth, (1.0f / Myscreensize.y) * ScreenHeight, (500.0f / Myscreensize.x) * ScreenWidth, ScreenHeight - (650.0f / Myscreensize.y) * ScreenHeight),"");


		if(Errorbox==true){
			ErrorREctbox=GUI.Window(errorwindowid,ErrorREctbox, Errorpopup,"");
			}



	
	//	MyMenu=  GUI.Window(3,MyMenu,ResourceTab,"Resources");

//		Debug.Log (GUI.depth.ToString ());
		mouseIsOverMe=false;	
		if(MyMenu.Contains(Event.current.mousePosition)||MyMenu2.Contains(Event.current.mousePosition))
		{
		//	Debug.Log("mouseovermainmoney1");
			mouseIsOverMe=true;
		}
	
		if( showHeroScreen==true)
		{
			if(heroInfoWindow.Contains(Event.current.mousePosition)){
				mouseIsOverMe=true;
				//Debug.Log("mouseoverheroscren");
			}
		
			heroInfoWindow=GUI.Window(0,heroInfoWindow,HeroInformationClick,"");
			
		}
		if(showGoldScreen ==true)
		{
			if(goldBuyWindow.Contains(Event.current.mousePosition)){
				mouseIsOverMe=true;
				//Debug.Log("mouseovergoldscren");
			}

			goldBuyWindow=GUI.Window(1,goldBuyWindow,GoldBuyClick,"");	
			
		}

	//	GUI.skin= null;

			
	}
	
	

	public  string[] menuOP={"Resources","Weapons","Armor"};
	public Rect MyMenu;
	public int menuselect;
	//upper right menu listing resources



	private void ResourceTab()
	{

		MyMenu= new Rect (ScreenWidth - (500.0f / Myscreensize.x) * ScreenWidth, (0.0f / Myscreensize.y) * ScreenHeight, (500.0f / Myscreensize.x) * ScreenWidth, ScreenHeight - (600.0f / Myscreensize.y) * ScreenHeight);
		GUI.Box (MyMenu,"Inventory");
		GUI.Box (MyMenu,"Inventory");
		//x y width heigh small right bbig left and small up big down
		GUI.Button(new Rect (ScreenWidth - (480.0f / Myscreensize.x) * ScreenWidth, (20.0f / Myscreensize.y) * ScreenHeight, (140.0f / Myscreensize.x) * ScreenWidth, 20.0f),"Resources");
		float spacer = 20.0f;

		foreach (ResourceAmount temp in RawResource) {
			if (temp.UPoor != 0 || temp.UGood != 0 || temp.UCommon != 0 || temp.USuperior != 0) {
				GUI.Label (new Rect (ScreenWidth - (480.0f / Myscreensize.x) * ScreenWidth, ((50.0f + spacer) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), temp.Name);
				GUI.Label (new Rect (ScreenWidth - (370.0f / Myscreensize.x) * ScreenWidth, ((50.0f + spacer) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), "[ " + temp.UPoor + "|" + temp.RPoor + " ]"  );
				GUI.Label (new Rect (ScreenWidth - (280.0f / Myscreensize.x) * ScreenWidth, ((50.0f + spacer) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), "[ " + temp.UCommon + "|" + temp.RCommon + " ]" );
				GUI.Label (new Rect (ScreenWidth - (190.0f / Myscreensize.x) * ScreenWidth, ((50.0f + spacer) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), "[ " + temp.UGood + "|" + temp.RGood + " ]" );
				GUI.Label (new Rect (ScreenWidth - (100.0f / Myscreensize.x) * ScreenWidth, ((50.0f + spacer) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), "[ " + temp.USuperior + "|" + temp.RSuperior + " ]" );
				spacer += 20.0f;
			}
		}

		if (spacer == 20.0f) {
			GUI.Label (new Rect (ScreenWidth - (480.0f / Myscreensize.x) * ScreenWidth, ((50) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), "No Resources In Stockpile");
		} else {
			GUI.Label (new Rect (ScreenWidth - (480.0f / Myscreensize.x) * ScreenWidth, ((50) / Myscreensize.y) * ScreenHeight, (400.0f / Myscreensize.x) * ScreenWidth, 20.0f), "Type");
			GUI.Label (new Rect (ScreenWidth - (350.0f / Myscreensize.x) * ScreenWidth, ((50) / Myscreensize.y) * ScreenHeight, (350.0f / Myscreensize.x) * ScreenWidth, 20.0f), "Poor");
			GUI.Label (new Rect (ScreenWidth - (275.0f / Myscreensize.x) * ScreenWidth, ((50) / Myscreensize.y) * ScreenHeight, (350.0f / Myscreensize.x) * ScreenWidth, 20.0f), "Common");
			GUI.Label (new Rect (ScreenWidth - (170.0f / Myscreensize.x) * ScreenWidth, ((50) / Myscreensize.y) * ScreenHeight, (350.0f / Myscreensize.x) * ScreenWidth, 20.0f), "Good");
			GUI.Label (new Rect (ScreenWidth - (95.0f / Myscreensize.x) * ScreenWidth, ((50) / Myscreensize.y) * ScreenHeight, (350.0f / Myscreensize.x) * ScreenWidth, 20.0f), "Superior");
		}






	}

	public Rect MyMenu2 ;

	// lower left menu
	private void IncomeTab()	
	{
		
		
	//	GUI.matrix =  Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(ScreenHeight,ScreenWidth,1.0f));
		MyMenu2= new Rect(0.0f,(720.0f/Myscreensize.y) * ScreenHeight ,(260.0f/Myscreensize.x) *ScreenWidth,ScreenHeight-(720.0f/Myscreensize.y) * ScreenHeight );
			GUI.Box(MyMenu2," ");
			GUI.Box(MyMenu2," ");
		// x pixel, y pixel, hieght, length
			if (GUI.Button( new Rect (0.0f,(740.0f/Myscreensize.y) * ScreenHeight ,(200.0f/Myscreensize.x) *ScreenWidth,(40.0f/Myscreensize.y) *ScreenHeight)," Gold : "+ PlayerGold.ToString())) {
				
					if (showGoldScreen ==true)
					{
						showGoldScreen=false;	
					}
					else
					{
						
						showGoldScreen=true;
					}
			}
		
		
		
		//"Gold : " + PlayerGold.ToString()
			if (GUI.Button( new Rect (0.0f,(795.0f/Myscreensize.y) * ScreenHeight ,(200.0f/Myscreensize.x) *ScreenWidth,(40.0f/Myscreensize.y) *ScreenHeight), " Renown : " + PlayerRenown.ToString() )) {
				
			}
		
		
		

			if (GUI.Button( new Rect (0.0f,(850.0f/Myscreensize.y) * ScreenHeight ,(200.0f/Myscreensize.x) *ScreenWidth,(40.0f/Myscreensize.y) *ScreenHeight), " Heroes : " + PlayerHeroes.ToString() )) 
			{
				
				if (showHeroScreen==true)
				{
					showHeroScreen=false;	
				}
				else
				{
					heroInfoWindow = new Rect(ScreenWidth/2.5f, ScreenHeight/2.75f ,500	 ,223);
					showHeroScreen=true;
				}
			}
		
		
		// checking to see if mouse is over box and disabling ray casting in playercontroller for bounty placement
			Rect checkForMouse=new Rect(0.0f,(720.0f/Myscreensize.y) * ScreenHeight ,(260.0f/Myscreensize.x) *ScreenWidth,ScreenHeight-(720.0f/Myscreensize.y) * ScreenHeight );
		
//			if(checkForMouse.Contains(Event.current.mousePosition))
//			{
//			
//				mouseIsOverMe=true;
//			}
//			else
//			{
//				mouseIsOverMe=false;	
//				
//		}
	//		GUI.Box(new Rect(ScreenWidth/2.5f, ScreenHeight/2.75f ,200 ,200)," ");
			
			
	}

	private void Errorpopup(int windownumb){
		GUI.Box (new Rect (0, 0, 200, 200), "");
		GUI.Box (new Rect (0, 0, 200, 200), "");
		GUI.Label (new Rect (60.0f, 20.0f, 100.0f, 40.0f), Errortext);
		if (GUI.Button (new Rect (80, 50, 50, 20), "Ok")){

			Errorbox = false;
		}


	}
	public bool IsResourceMax(MatInfo res,int amount,bool Refined){

		if (Refined == false) {
			if (res.QualityName == "Poor") {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UPoor + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}


			} else if (res.QualityName == "Common") {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UCommon + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			} else if (res.QualityName == "Good") {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UGood + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			} else {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].USuperior + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			}
		} else {

			if (res.QualityName == "Poor") {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RPoor + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			} else if (res.QualityName == "Common") {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RCommon + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			} else if (res.QualityName == "Good") {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RGood + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			} else {
				if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RSuperior + amount > ResourceMax) {
					return true;
				} else {
					return false;
				}
			}
			Debug.LogError ("Resmax is wrong");
			return true;

		}


	}


	public void AddResource(MatInfo res,int amount,bool Refined)
	{
		if (Refined == false) {
			if (res.QualityName == "Poor") {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UPoor += amount;
			} else if (res.QualityName == "Common") {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UCommon += amount;
			} else if (res.QualityName == "Good") {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UGood += amount;
			} else {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].USuperior += amount;
			}
		} else {

			if (res.QualityName == "Poor") {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RPoor += amount;
			} else if (res.QualityName == "Common") {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RCommon += amount;
			} else if (res.QualityName == "Good") {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RGood += amount;
			} else {
				RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RSuperior += amount;
			}



		}
			

	



	}

	public void AddGold(int gold)
	{
		
	PlayerGold= PlayerGold +gold;
	}
	
	
	public void AddRenown(int gold)
	{
		
	PlayerRenown = PlayerRenown +gold;
	}

	public int GetRenown(){


		return PlayerRenown;
	
	}
	
	public bool IsAWindowOpen()
	{
		if ((showGoldScreen==true) || (showHeroScreen==true)||(mouseIsOverMe==true))
		{
				
			return true;
		}
		
		return false;
		
	}
	
	void GoldBuyClick(int windowID)
	{

		//scrollViewVector= GUI.BeginScrollView(new Rect(2,20,495,475),scrollViewVector,new Rect(0,0,800,1000),true,true);
		
		
		//GUI.Label(new Rect(170f,10f,50f,500f),"asdg\n");
		//GUI.Label(new Rect(250f,10f,70f,500f),"ghda\n");
		GUI.Box(new Rect(0,0,goldBuyWindow.width,goldBuyWindow.height),"Buy Stuff");
		if(GameObject.FindGameObjectWithTag("BlackSmith").transform.position.y<-1)
		{
           
			GUI.Label(new Rect(10f,15f,300f,500f),"Should you purchase our prefab Blacksmith\nYou shale have the mean to make your own weapons and armor \nThat any true blue inn keeper would be proud to own." );
			if (GUI.Button(new Rect(10,100,210,30),"Buy Blacksmith for 50 Gold"))
			{
                if ( CanIBuy(50,true)==true)
				{
					//PlayerGold=PlayerGold-50;
					GameObject.FindGameObjectWithTag("BlackSmith").transform.position= new Vector3(-14,0,-37);
					GameObject.Find ("BuildingManager").GetComponent<BuildingLocation> ().AddBlacksmith (GameObject.FindGameObjectWithTag("BlackSmith"));
                    BlackBought();
				}
			}
		}
		if(GameObject.FindGameObjectWithTag("StorageYard").transform.position.y<-1)
		{

			GUI.Label(new Rect(10f,150f,300f,500f),"Should you purchase our prefab StorageYard\n Expane your storage and collection for your mining and haversting \n" );
			if (GUI.Button(new Rect(10,200,210,30),"Buy StorageYard for 50 Gold"))
			{
				if ( CanIBuy(50,true)==true)
				{
					//PlayerGold=PlayerGold-50;
					GameObject.FindGameObjectWithTag("StorageYard").transform.position= new Vector3(26,2.4f,-34);
				
				}
			}
		}
	
		Rect checkForMouse=new Rect(0,0,800,1000);

	//	GUI.EndScrollView();
		GUI.DragWindow();
	
	}

	public bool CanIBuy(int buyamount,bool buynow)
    {
        if (buyamount > PlayerGold)
        {
			Errorbox = true;
			Errortext="Not Enought Gold";
            return false;
        }
		if (buynow == true) {
			PlayerGold = PlayerGold - buyamount;
		} 
		return true;
    }



	public bool CanIHaveResource(MatInfo res,bool isRefined,float amount)
	{





		if (isRefined == false) {
					if (res.QualityName == "Poor") {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UPoor - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UPoor -= amount;
							return true;
						}
					} else if (res.QualityName == "Common") {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UCommon - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UCommon -= amount;
							return true;
						}
					} else if (res.QualityName == "Good") {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UGood -amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].UGood -= amount;
							return true;
						}
					} else {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].USuperior - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].USuperior  -= amount;
							return true;
						}
					}
				} else {

					if (res.QualityName == "Poor") {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RPoor - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RPoor -= amount;
							return true;
						}
					} else if (res.QualityName == "Common") {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RCommon - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RCommon -= amount;
							return true;
						}
					} else if (res.QualityName == "Good") {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RGood - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RGood -= amount;
							return true;
						}
					} else {
						if (RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RSuperior - amount >= 0.0f) {
							RawResource [RawResource.FindIndex (x => x.Name == res.Name)].RSuperior  -= amount;
							return true;
						}
					}



				}

		return false;

	}





	private Vector2 scrollViewVector = Vector2.zero;

	void HeroInformationClick(int windowID)
	{
		GUI.Box (new Rect (0, 0, 495,200), "Hero Info");
		
		List<GameObject> allTheHeros = GameObject.FindGameObjectWithTag("HeroController").GetComponent<HeroSpawner>().HeroList;
		string HeroListName="";
		string HeroListHp="";
		string HeroListStatus="";
		string HeroListGold="";
		

//		Debug.Log(allTheHeros.Length);
		for (int i = 0; i <allTheHeros.Count;i++)
		{
			HeroListName=HeroListName + allTheHeros[i].name+"\n";
			if (allTheHeros[i].name.Length>26)
			{
			HeroListHp=HeroListHp + allTheHeros[i].GetComponent<HeroAI>().heroHP+"\n\n";
			HeroListStatus=HeroListStatus + allTheHeros[i].GetComponent<HeroAI>().heroState+"\n\n";			
			HeroListGold=HeroListGold+ allTheHeros[i].GetComponent<HeroAI>().HeroGold+"\n\n";		
			
			}
			else
			{
			HeroListHp=HeroListHp + allTheHeros[i].GetComponent<HeroAI>().heroHP+"\n";
			HeroListStatus=HeroListStatus + allTheHeros[i].GetComponent<HeroAI>().heroState+"\n";
			HeroListGold=HeroListGold+ allTheHeros[i].GetComponent<HeroAI>().HeroGold+"\n";
			}

		}
		
		
		
		//26 char long names before overflow
		//										size of scrollable area to view              how big it really is
			
		scrollViewVector= GUI.BeginScrollView(new Rect(2,20,495,200),scrollViewVector,new Rect(0,0,800,1000),false,false);
			GUI.Label(new Rect(5f,10f,150f,500f),"Hero Name\n" +HeroListName);
			GUI.Label(new Rect(170f,10f,50f,500f),"Hero HP\n" +HeroListHp);
			GUI.Label(new Rect(250f,10f,70f,500f),"Hero Gold\n" +HeroListGold);
			Rect checkForMouse=new Rect(0,0,495,200);
	
					if(checkForMouse.Contains(Event.current.mousePosition))
					{
					
						mouseIsOverMe=true;
					}
					else
					{
						mouseIsOverMe=false;	
						
				}

			GUI.EndScrollView();
			GUI.DragWindow();
		
	}
	

}



