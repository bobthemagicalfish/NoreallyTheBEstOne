using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[System.Serializable]
public class BlackSmithinfo
{
    public string Name;
    public int Level;
    public string Race;
    public int Exp;
    public float Workspeed;
    public float Effeciny;
    public int PayPerPeriod;
    public int CosttoHire;

    public enum MyState
    {
        Idle,
        Working,
        NoMats,


    };
    public MyState myStatus;
    public  BlackSmithinfo()
    {
        Name = "";
        myStatus = MyState.Idle;
        Level = 0;
        Race = "";
        Exp = 0;
        Workspeed = 0;
        Effeciny = 0;
        PayPerPeriod = 0;
        CosttoHire = 0;
    }

    public BlackSmithinfo GenerateBlacksmith()
    {
        BlackSmithinfo temp = new BlackSmithinfo();
        temp.Name = "Darky" + Random.Range(0, 10);

		int tempRenown = GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().GetRenown();

        int maxlevel = tempRenown / 20;
        temp.Level = Random.Range(1, maxlevel + 1);
        temp.CosttoHire = Mathf.RoundToInt(temp.Level * Random.Range(.5f, 2f) * Random.Range(1, 20)) ;
        temp.Effeciny = (float)System.Math.Round((double)Random.Range(.5f, 2f),2);
        temp.Workspeed = (float)System.Math.Round((double)Random.Range(.5f, 2f),2);
        temp.PayPerPeriod = Mathf.RoundToInt((temp.CosttoHire * temp.Level) / 4);
        return temp;
    }

}

[System.Serializable]
public class BlackSmithQueue{

	public ArmorInfo MyArmor;
	public WeaponInfo MyWeapon;
	public bool expand;
	public float Timeleft;

	public BlackSmithQueue(){
		MyArmor=new ArmorInfo();
		MyWeapon=new WeaponInfo();
		expand = false;
		Timeleft = 0.0f;
	}

	public BlackSmithQueue (WeaponInfo weap,float time){
		Timeleft = time;
		MyWeapon = weap;
		expand = false;
	}

	public BlackSmithQueue(ArmorInfo armo,float time){
		Timeleft = time;
		MyArmor = armo;
		expand = false;
	}
		

}



public class BlackSmithMenu : MonoBehaviour {
    public  BlackSmithinfo myBlack= new BlackSmithinfo();
	public int craftingnumber=1;
	public Dropdown Mydropdown;	
	public GUISkin IncomeSkin;
    public int blacksmithrespawntimer=0;
    public int maxblacksallowed=3;
    public int menuselct = 0;
    public int menuissetto = 0;
	public ItemLib MyItemLib;

    public List<BlackSmithinfo> myblacksList=new List<BlackSmithinfo>();

    bool justboughtblacksmith =false;
    public bool Imactive=false;
   public bool BlackMenuActive =false;
	public bool Mouseoverme=false;
    public Rect MyMenu;
	public Rect MyMineralMenu;
	public Rect MyErrorMenu;
    public string[] menuOP;
   
	public List<WeapArmorBaseInfo> Weaponbases;
	public List<WeapArmorBaseInfo> Armorbases;

	public List<BlackSmithQueue> BlackQueue = new List<BlackSmithQueue> ();


	public List<WeaponInfo> BlacksmithWeaponInv = new List<WeaponInfo> ();
	public List<ArmorInfo> BlackSmithArmorInv = new List<ArmorInfo>();

	GUIStyle CenterLabelstyle;

	// Use this for initialization
	void Start () {
        menuOP = new string[] { "Job Info","Crafting", "Hire BlackSmith","Shop" };



        for (int i = 0; i < maxblacksallowed; i++)
        {
            myblacksList.Add(myBlack.GenerateBlacksmith());
           
        }

		MyMenu=new Rect(Screen.width/2.5f, Screen.height/2.75f ,400 ,300);
		MyMineralMenu=new Rect(Screen.width/2.5f, Screen.height/2.75f ,200 ,40);
		MyErrorMenu=new Rect(Screen.width/2.5f, Screen.height/2.75f ,200 ,80);


        MainMoney.BlackBought +=imbought;

       
	}
	
	// Update is called once per frame
	void Update () {






        blacksmithrespawntimer +=1;
	//	MyMenu=new Rect(Screen.width/2.5f, Screen.height/2.75f ,400 ,300);
        if (blacksmithrespawntimer == 1000)     
        {
          blacksmithspawnontimer();
        }


		blackSmithWorking();

        if (Input.GetKeyDown(KeyCode.B)&&Imactive==true)
        {
            if (BlackMenuActive  == true)
            {
                BlackMenuActive  = false;
            }
            else
            {
                BlackMenuActive = true;
            }
        }


	}

    public void imbought()
    {
        Imactive = true;
		MyItemLib = GameObject.FindGameObjectWithTag ("HeroController").GetComponent<ItemLib>();

		foreach (WeapArmorBaseInfo temp in MyItemLib.WeaponBases) {
			CraftingWeapontypebuttontextlist.Add  (temp.name);
			Weaponbases.Add (WeapArmorBaseInfo.Copyme (temp));

		}
		CraftWeaponChoices = WeapArmorBaseInfo.Copyme( Weaponbases.Find (x => x.name == "Sword"));
		foreach (WeapArmorBaseInfo temp in MyItemLib.ArmorBases) {
			CraftingArmortypebuttontextlist.Add(temp.name);
			Armorbases.Add(WeapArmorBaseInfo.Copyme (temp));

		}
		foreach(ArmorSize temp in MyItemLib.ArmorSizes)
		{
			ArmorSizeNames.Add (temp.Name);
		}
    }

	public void MenuOn(){
		BlackMenuActive=true;

	}

    // either adds another blackismith in or deltes the oldest one asumed to bne one in front and makes another
    public void blacksmithspawnontimer()
    {

        if(myblacksList.Count != maxblacksallowed)
        {
            myblacksList.Add(myBlack.GenerateBlacksmith());
          

        }
        else 
        {
            myblacksList.RemoveAt(0);
            myblacksList.Add(myBlack.GenerateBlacksmith());
           
        }

        blacksmithrespawntimer = 0;
    }


	void OnGUI()
	{
		CenterLabelstyle =new GUIStyle(GUI.skin.label);
		CenterLabelstyle.alignment = TextAnchor.UpperCenter;
	//	GUI.skin = IncomeSkin; 
		Mouseoverme = false;
		if (BlackMenuActive==true){
			// left , top, width,hieght
            MyMenu=  GUI.Window(6,MyMenu,BlackMenu,"",GUIStyle.none);
			if(MyMenu.Contains(Event.current.mousePosition)){

				Mouseoverme = true;
			}
           
        }
		if (MineralMenu == true&&menuselct==1) {
			MyMineralMenu=new Rect(MyMineralMenu.x, MyMineralMenu.y,420 ,GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().RawResource.Count*20.10f+50);
			MyMineralMenu=  GUI.Window(7,MyMineralMenu,MineralMenudisplay,"");

			if(MyMineralMenu.Contains(Event.current.mousePosition)){

				Mouseoverme = true;
			}


		}

		if (CraftError == true) {
			MyErrorMenu = GUI.Window (1000, MyErrorMenu, MyErrordisplay, "Invalid Craft order");
			if(MyErrorMenu.Contains(Event.current.mousePosition)){

				Mouseoverme = true;
			}

		}

	}


	public bool MouseOverMe(){

		return Mouseoverme;


	}


	public void MyErrordisplay(int menu)
	{


		GUI.Label (new Rect (30,20, 200, 40), CraftErrorMessage);

		if(GUI.Button(new Rect(85,55,40,20),"Ok"))
			{

				CraftErrorMessage="";
					CraftError=false;
			}

	}



	List<ResourceAmount> MyResAmount = new List<ResourceAmount> ();
	bool SortByName=true;
	bool SortByDR=false;
	bool SortByDur=false;
	bool SortByDM=false;
	bool SortByAss=false;
	bool SortByDef=false;
	List<MatInfo> MyMatInfo = new List<MatInfo> ();




	public void MineralMenudisplay(int menu)
	{
		GUI.Box(new Rect(0,0,MyMineralMenu.width,MyMineralMenu.height),"Material Bounus Info");
		GUI.Box(new Rect(0,0,MyMineralMenu.width,MyMineralMenu.height),"Material Bounus Info");

		MyResAmount = GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ().RawResource;
		List<MatInfo> MyMatInfo = new List<MatInfo> ();

		foreach (ResourceAmount temp in MyResAmount) {
			MyMatInfo.Add (MyItemLib.GetMatInfo (temp.Name));

		}

		if (SortByName == true) {
			MyMatInfo.Sort ((p1, p2) => p1.Name.CompareTo (p2.Name));
		} else if (SortByDM == true) {
			MyMatInfo.Sort ((p1, p2) => p1.BaseMeleeDamage.CompareTo (p2.BaseMeleeDamage));
		} else if (SortByDR == true) {
			MyMatInfo.Sort ((p1, p2) => p1.BaseRangeDamage.CompareTo (p2.BaseRangeDamage));
		} else if (SortByDur == true) {
			MyMatInfo.Sort ((p1, p2) => p1.BaseDurablity.CompareTo (p2.BaseDurablity));
		} else if (SortByAss == true) {
			MyMatInfo.Sort ((p1, p2) => p1.BaseBeauty.CompareTo (p2.BaseBeauty));
		} else if (SortByDef == true) {
			MyMatInfo.Sort ((p1, p2) => p1.BaseDefense.CompareTo (p2.BaseDefense));
		}else {
			MyMatInfo.Sort ((p1, p2) => p1.BaseWeight.CompareTo (p2.BaseWeight));
		}


		if (GUI.Button (new Rect (30, 20, 50, 20), "Name", "Label")) {
			SortByName = true;
			SortByDM=false;
			SortByDR=false;
			SortByDur=false;
			SortByDef = false;
			SortByAss=false;
			
		
		}
		if (GUI.Button (new Rect (95, 20, 55, 40), "Damage\nMeele", "Label")) {
			SortByName = false;
			SortByDM=true;
			SortByDR=false;
			SortByDur=false;
			SortByAss=false;
			SortByDef = false;
		}
		if (GUI.Button (new Rect (150, 20, 55, 40), "Damage\nRange", "Label")) {
			SortByName = false;
			SortByDM=false;
			SortByDR=true;
			SortByDur=false;
			SortByAss=false;
			SortByDef = false;
		}
		if (GUI.Button (new Rect (205, 20, 55, 40), "Defense", "Label")) {
			SortByName = false;
			SortByDM=false;
			SortByDR=false;
			SortByDur=false;
			SortByAss=false;
			SortByDef = true;
		}
		if (GUI.Button (new Rect (260, 20, 55, 20), "Durablity", "Label")) {
			SortByName = false;
			SortByDM=false;
			SortByDR=false;
			SortByDur=true;
			SortByAss=false;
			SortByDef = false;
		}
		if (GUI.Button (new Rect (310, 20, 55, 40), " Weight\nPer Unit", "Label")) {
			SortByName = false;
			SortByDM=false;
			SortByDR=false;
			SortByDur=false;
			SortByAss=false;
			SortByDef = false;
		}
		if (GUI.Button (new Rect (360, 20, 55, 40), "Aesthetic", "Label")) {
			SortByName = false;
			SortByDM=false;
			SortByDR=false;
			SortByDur=false;
			SortByAss=true;
			SortByDef = false;
		}
		float space = 0.0f;
		//GUI.Label (new Rect (30, 20, 50, 20), "Name");

		foreach( MatInfo temp in MyMatInfo)
		{
			GUI.Label (new Rect (5, 50+space, 100, 20), temp.Name);
		
			GUI.Label (new Rect (95, 50 + space, 40, 20), temp.BaseMeleeDamage.ToString(),	CenterLabelstyle);
			GUI.Label (new Rect (150, 50 + space, 40, 20), temp.BaseRangeDamage.ToString(),	CenterLabelstyle);
			GUI.Label (new Rect (205, 50 + space, 40, 20), temp.BaseDefense.ToString(),	CenterLabelstyle);
			GUI.Label (new Rect (260, 50 + space, 40, 20), temp.BaseDurablity.ToString(),	CenterLabelstyle);
			GUI.Label (new Rect (310, 50 + space, 40, 20), temp.BaseWeight.ToString(),	CenterLabelstyle);
			GUI.Label (new Rect (360, 50 + space, 40, 20), temp.BaseBeauty.ToString(),	CenterLabelstyle);
			space += 20.0f;
		}
		GUI.DragWindow ();

	}


	public string CraftingTypebuttontext ="Weapon";
	public string CraftingWeapontypebuttontext ="Sword";
	public string ArmorSelectedName="Light";

	public List<string> CraftingWeapontypebuttontextlist;
	public List<string> CraftingArmortypebuttontextlist;
	public List<string> PartsNamelist= new List<string>();
	public List<string> ArmorSizeNames = new List<string> ();

	public bool craftingtypeon = false;
	public bool craftingItemtypeon =false;
	public bool ArmorSizeselect = false;
	public bool ShowArmorSize=false;
	public bool ArmorSizemouse =false;
	public bool[] Craftingpartsdropdown = new bool[3];
	public bool AllPartsPicked = false; 
	public bool MineralMenu=false;
	public bool CraftError=false;
	public Texture Swordtexture;

	public Vector2 Craftingpartscroll;
	public Vector2 ResultsScroll;
	public Vector2 ItemQueue;
	public Vector2 WeaponStore;
	public Vector2 ArmorStore;

	public WeapArmorBaseInfo CraftWeaponChoices ;

	public WeaponInfo CraftingWeapon;

	public ArmorInfo CraftingArmor;
	public string CraftErrorMessage = "";

	public RangeForItems rangeforcraft;



	public void SwapQueue(int Currentplace,int Newplace)
	{
		if (Newplace != -1 && Newplace != BlackQueue.Count) {
			BlackSmithQueue temp = BlackQueue [Newplace];

			BlackQueue [Newplace] = BlackQueue [Currentplace];
			BlackQueue [Currentplace] = temp;

		}




	}

    public void BlackMenu(int menu)

    {


        if (menuselct == 0)
        {
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y), "");
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y), "BlackSmith");
			MyMenu=new Rect(MyMenu.x, MyMenu.y ,400 ,300);
          //    GUI.Box(new Rect(100,100,350,300),"Insert BlackSmith Title Here");
            if (myBlack.Name == "")
            {
                GUI.Label(new Rect(10, 50, 300, 50), " No Black Smith ");
         
            }
			else
            {
             //   GUI.Label(new Rect(10, 50, 300, 50), " Current BlackSmith \n" + myBlack.Name);
                GUI.Label(new Rect( 10 , 50, 300, 300), "Current BlackSmith \n" + myBlack.Name + "\nLevel: " + myBlack.Level + "\nRace: " +myBlack.Race + "\nEfficienty: " + myBlack.Effeciny +
                    "\nWorkSpeed: " + myBlack.Workspeed + "\nCost Per Season: " +myBlack.PayPerPeriod + "\nCurrent status: "+myBlack.myStatus);

				//GUI.Label (new Rect (250,50, 200, 40), "BlackSmith Queue");
				float spacer=0.0f;
				int counter = 0;
				string name = "";
				GUI.Box (new Rect (130, 50, 270, 150), "BlackSmith Queue");
				GUI.Label (new Rect (140, 70, 100, 80), "Task");
				GUI.Label (new Rect (260, 70, 100, 80), "Time\nLeft");
			
				ItemQueue=	GUI.BeginScrollView(new Rect(130,100,270,90),ItemQueue,new Rect(0,0,180,40.0f+(BlackQueue.Count*30.0f)));

				foreach (BlackSmithQueue temp in BlackQueue) {

//					foreach (Partinfo loop in temp.MyArmor.MyBaseinfo.Parts) {
//						name += loop.Mat.Name;	
//					}


					if (temp.MyArmor!=null && temp.MyArmor.Name!="") {
						
						GUI.Label (new Rect (5, 0+spacer, 180, 25.0f), temp.MyArmor.Name);
						GUI.Label (new Rect (130, 0 + spacer, 180, 25.0f), (Mathf.Round(temp.Timeleft)).ToString());
					//	GUI.Label(new Rect(

						if (GUI.Button (new Rect (170, 0+ spacer, 20.0f, 20.0f), "U")) {
							SwapQueue (counter, counter - 1);
						}

						if (GUI.Button (new Rect (200, 0 + spacer, 20.0f, 20.0f), "D")) {
							SwapQueue (counter, counter + 1);
						}
						if (GUI.Button (new Rect (230, 0 + spacer, 20.0f, 20.0f), "X")) {
							foreach (Partinfo loop in temp.MyArmor.MyBaseinfo.Parts) {
								GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ().AddResource (loop.Mat, temp.MyArmor.MyBaseinfo.CostToCraftPerSlot, true);
							}
							BlackQueue.RemoveAt (counter);
						}

					} else {


						GUI.Label (new Rect (5, 0+spacer, 180, 25.0f), temp.MyWeapon.Name);
						GUI.Label (new Rect (130, 0 + spacer, 180, 25.0f), (Mathf.Round(temp.Timeleft) ).ToString());

						if (GUI.Button (new Rect (170, 0+ spacer, 20.0f, 20.0f), "U")) {
							SwapQueue (counter, counter - 1);
						}

						if (GUI.Button (new Rect (200, 0 + spacer, 20.0f, 20.0f), "D")) {
							SwapQueue (counter, counter + 1);
						}
						if (GUI.Button (new Rect (230,  0 + spacer, 20.0f, 20.0f), "X")) {
							foreach (Partinfo loop in temp.MyWeapon.MyBaseinfo.Parts) {
								GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ().AddResource (loop.Mat, temp.MyWeapon.MyBaseinfo.CostToCraftPerSlot, true);
							}
							BlackQueue.RemoveAt (counter);
						}
					}
					spacer += 25.0f;
					counter += 1;

				}
				GUI.EndScrollView();
            }

        }


     


		if (menuselct == 1) {
			// 0,0 is top left
			// left,right, down/up,width,heigh
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y-90), "");
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y-90), "");
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y-90), "BlackSmith");
			MyMenu=new Rect(MyMenu.x, MyMenu.y ,600 ,400);
		


			GUI.DrawTexture (new Rect (10, 80, 300, 160), Swordtexture, ScaleMode.StretchToFill);

			if (GUI.Button (new Rect (10, 50, 90, 30), CraftingTypebuttontext)) {
				craftingtypeon = true;


			}
			// chossing if making armor or weapon 
			if (craftingtypeon == true && (new Rect (10, 50, 90, 90).Contains (Event.current.mousePosition))) {
				GUI.Box (new Rect (10, 80, 90, 65), "");
				if (CraftingTypebuttontext == "Weapon") {
					if (GUI.Button (new Rect (15, 85, 80, 25), "Armor")) {
						CraftingTypebuttontext = "Armor";
						CraftingWeapontypebuttontext = CraftingArmortypebuttontextlist [0].ToString ();
						CraftWeaponChoices = WeapArmorBaseInfo.Copyme (Armorbases.Find (x => x.name == CraftingArmortypebuttontextlist [0].ToString ()));
						ShowArmorSize = true;
						craftingtypeon = false;
					}
				} else {
					if (GUI.Button (new Rect (15, 85, 80, 25), "Weapon")) {
						CraftingTypebuttontext = "Weapon";
						CraftingWeapontypebuttontext = CraftingWeapontypebuttontextlist [0].ToString ();
						CraftWeaponChoices = WeapArmorBaseInfo.Copyme (Weaponbases.Find (x => x.name == CraftingWeapontypebuttontextlist [0].ToString ()));
						ShowArmorSize = false;
						craftingtypeon = false;	
					}
				}
			} else {

				craftingtypeon = false;
			}
				
			//choosing what type of things to make 

			if (GUI.Button (new Rect (110, 50, 90, 30), CraftingWeapontypebuttontext)) {

				craftingItemtypeon =true;

			}

			if (craftingItemtypeon == true && (new Rect (110, 50, 90, (CraftingWeapontypebuttontextlist.Count * 30) + 30).Contains (Event.current.mousePosition))) {
		
				if (CraftingTypebuttontext == "Weapon") {
					float space = 0.0f;
					CraftingWeapon = new WeaponInfo ();
					GUI.Box (new Rect (110, 80, 90, CraftingWeapontypebuttontextlist.Count * 32), "");


					foreach (string temp in CraftingWeapontypebuttontextlist) {
						if (temp != CraftingWeapontypebuttontext) {

							if (GUI.Button (new Rect (110, 85 + space, 90, 25), temp)) {
								CraftingWeapontypebuttontext = temp;
								craftingItemtypeon = false;
								CraftWeaponChoices = WeapArmorBaseInfo.Copyme (Weaponbases.Find (x => x.name == CraftingWeapontypebuttontext));
							}

							space += 30.0f;
						}
					}

				} else {
					float space = 0.0f;
					CraftingArmor = new ArmorInfo ();
					GUI.Box (new Rect (110, 80, 90, CraftingArmortypebuttontextlist.Count * 32), "");


					foreach (string temp in CraftingArmortypebuttontextlist) {
						if (temp != CraftingWeapontypebuttontext) {
							if (GUI.Button (new Rect (110, 85 + space, 90, 25), temp)) {
								CraftingWeapontypebuttontext = temp;
								craftingItemtypeon = false;
							
								CraftWeaponChoices = WeapArmorBaseInfo.Copyme (Armorbases.Find (x => x.name == CraftingWeapontypebuttontext));
							}
							space += 30.0f;
						}
					}
				} 

			} else {

				craftingItemtypeon = false;
			}
			//picking armor type heavy light med
		//	GUI.Box (new Rect (200, 50, 90, ArmorSizeNames.Count * 30+30), "");
			if (ShowArmorSize == true) {
				if ((GUI.Button (new Rect (210, 50, 90, 30), ArmorSelectedName))) {
					ArmorSizeselect = true;
				}
					
				if (ArmorSizeselect == true	&&new Rect(210,50,90,ArmorSizeNames.Count*30+30).Contains(Event.current.mousePosition)) {
					float space = 0.0f;	
					GUI.Box (new Rect (210, 80, 90, ArmorSizeNames.Count * 30), "");
					foreach (string temp in ArmorSizeNames) {
						if (GUI.Button (new Rect (210, 85+space, 90, 25), temp)) {
							ArmorSelectedName = temp;
						}
						space += 30.0f;
					}
				}
				else{
					ArmorSizeselect=false;
				}
			}
			// material button
			if ((GUI.Button (new Rect (310, 50, 90, 30), "Material Info"))) {
				if (MineralMenu == true) {
					MineralMenu =false;
				} else {
					MineralMenu = true;
				}
			}
		



			int i = 0;
			if (CraftingTypebuttontext == "Weapon") {
				
				float space = 0.0f;
				PartsNamelist = new List<string> ();
				foreach (Partinfo looper in CraftWeaponChoices.Parts) {
					GUI.Label (new Rect (15+space, 240, 80, 20), looper.Name,CenterLabelstyle);
					if (GUI.Button (new Rect (15 + space, 260, 100, 40), looper.Mat.QualityName+"\n"+looper.Mat.Name )) {
						Craftingpartsdropdown [i] = true;
					
					}
					PartsNamelist.Add (looper.Name);
					i += 1;
					space += 150;
				}
				
			}

			// selecting armor type helment chest etc
			if (CraftingTypebuttontext == "Armor") {


				float space = 0.0f;
				PartsNamelist = new List<string> ();
				foreach (Partinfo looper in CraftWeaponChoices.Parts) {
					GUI.Label (new Rect (15+space, 240, 80, 20),looper.Name,CenterLabelstyle);
					if (GUI.Button (new Rect (15 + space, 260, 100, 40),looper.Mat.QualityName+"\n"+looper.Mat.Name )){
						
						Craftingpartsdropdown [i] = true;

					}
					PartsNamelist.Add (looper.Name);
					space += 150;
					i += 1;
				}

			}

			// drop down menu for picking parts
			for(int k= 0 ; k < CraftWeaponChoices.Parts.Count;k++)
			{
				if (Craftingpartsdropdown [k]==true) {
					//	Craftingpartsdropdown [1] = false;
					//	Craftingpartsdropdown [2] = false;
					if (new Rect (15+(150*k), 250, 150, 100).Contains (Event.current.mousePosition)||new Rect (15+(150*k), 301, 220, 80).Contains (Event.current.mousePosition)) {
					GUI.Box (new Rect (15 + (150 * k), 301, 220, 80),"");
						GUI.Box (new Rect (15 + (150 * k), 301, 220, 80),"");
						//GUI.Box (new Rect (15 + (150 * k), 250, 150, 100),"");
						Craftingpartscroll = GUI.BeginScrollView (new Rect (15+(150*k), 301, 220, 80), Craftingpartscroll, new Rect (0, 0, 200,GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().RawResource.Count*30.0f ),false,false);
						GUI.Box (new Rect (0, 0, 220, 1000), "");
						float spacer = 0.0f;
						foreach (ResourceAmount temp in GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().RawResource) {
							
							if (temp.RPoor >= CraftWeaponChoices.CostToCraftPerSlot || temp.RCommon >= CraftWeaponChoices.CostToCraftPerSlot 
							|| temp.RGood >= CraftWeaponChoices.CostToCraftPerSlot || temp.RSuperior >= CraftWeaponChoices.CostToCraftPerSlot) {
								GUI.Label (new Rect (5, 10 + spacer, 125, 40), temp.Name);
								if (temp.RPoor >= CraftWeaponChoices.CostToCraftPerSlot) {
									
									if (GUI.Button (new Rect (100, 10 + spacer, 20, 20), "P")) {
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat = MatInfo.copyMe (MyItemLib.GetMatInfo (temp.Name));
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.Quality = 15;
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.QualityName = "Poor";
									}
								}

								if (temp.RCommon >= CraftWeaponChoices.CostToCraftPerSlot) {
									if (GUI.Button (new Rect (125, 10 + spacer, 20, 20), "C")) {
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat = MatInfo.copyMe (MyItemLib.GetMatInfo (temp.Name));
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.Quality = 45;
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.QualityName = "Common";
									}
								}

								if (temp.RGood >= CraftWeaponChoices.CostToCraftPerSlot) {
									if (GUI.Button (new Rect (150, 10 + spacer, 20, 20), "G")) {
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat = MatInfo.copyMe (MyItemLib.GetMatInfo (temp.Name));
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.Quality = 70;
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.QualityName = "Good";
									}
								}

								if (temp.RSuperior >= CraftWeaponChoices.CostToCraftPerSlot) {
									if (GUI.Button (new Rect (175, 10 + spacer, 20, 20), "S")) {
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat = MatInfo.copyMe (MyItemLib.GetMatInfo (temp.Name));
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.Quality = 85;
										CraftWeaponChoices.Parts.Find (x => x.Name == PartsNamelist [k]).Mat.QualityName = "Superiour";
									}


								}
								spacer += 30.0f;
							}

						}
						GUI.EndScrollView();

					} else {
						Craftingpartsdropdown [k] = false;
					}

				

				}
			}

			int average = 0;
			rangeforcraft = new RangeForItems();
			List<RangeForItems> MyRanges= new List<RangeForItems>();
			AllPartsPicked = true;
			foreach (Partinfo temp in CraftWeaponChoices.Parts) {
				if (temp.Mat.Name == ""|| temp.Mat == null) {
					AllPartsPicked = false;
				} 
			}

			if (AllPartsPicked == true) {
				average = 0;
				foreach (Partinfo temp in CraftWeaponChoices.Parts)  {
					average += temp.Mat.Quality ;
				}
				average = (average / CraftWeaponChoices.Parts.Count);
				 MyRanges= rangeforcraft.GetRanges(average);
			}


			// shows the resuelts
		//	GUI.Box (new Rect (311,80, 271, 160), "Results Range");
			GUI.Box(new Rect (311, 80, 300, 160),"");
			if (CraftingTypebuttontext == "Weapon") {
				if (AllPartsPicked == true) {
					float spacer = 0.0f;

					GUI.Label (new Rect (313, 80, 250, 155),
						"Craft Time:"+CraftWeaponChoices.TimeToCraft+" Seconds "+ 
						"| Parts Cost:" + CraftWeaponChoices.CostToCraftPerSlot+" Per\n"+
						"\t            Possiable Results"+
					   	 "\nQuality:"+
						"\nChance:"+
						"\nDamage:" + 
						 "\nRange: "+
						"\nDurablity: "+
						 "\nWeight: " +
						 "\nLooks: " );
				//	GUI.Box (new Rect (371, 130, 230, 120), "");
					ResultsScroll = GUI.BeginScrollView (new Rect (370,110,230, 130), ResultsScroll, new Rect (0, 0,MyRanges.Count*75,95 ),false,false);
					int tempDam = 0;
					int tempDur = 0;
					int tempWei = 0;

					foreach(Partinfo x in CraftWeaponChoices.Parts){
						tempDam += Mathf.RoundToInt (x.DamagePercent*x.Mat.BaseMeleeDamage);
						tempDur += Mathf.RoundToInt (x.DurPercent * x.Mat.BaseDurablity);
						tempWei += Mathf.RoundToInt (x.WeightPercent * x.Mat.BaseWeight);

					}
						


					foreach (RangeForItems temp in MyRanges) {
						GUI.Box (new Rect (0 + spacer, 0, 70, 110), temp.Name);
						GUI.Label(new Rect (0 + spacer, 0, 70, 110),
							"\n"+temp.Chance+"%"+
							"\n"+(CraftWeaponChoices.BaseDamage+ temp.BounusLow+tempDam) +"-"+(CraftWeaponChoices.BaseDamage+temp.BounusHigh+tempDam) +
							"\n"+CraftWeaponChoices.BaseRange +
							"\n"+(CraftWeaponChoices.BaseDurablity+temp.BounusLow+tempDur)+"-"+(CraftWeaponChoices.BaseDurablity+tempDur+temp.BounusHigh)+
							"\n"+(CraftWeaponChoices.BaseWeight+tempWei)+
							"\nShit",CenterLabelstyle);
						spacer +=75.0f;
					}
					GUI.EndScrollView ();
				
				}
				else {
					GUI.Label (new Rect (313, 80, 250, 155),
						"Craft Time:"+CraftWeaponChoices.TimeToCraft+" Seconds "+ 
						"| Parts Cost:" + CraftWeaponChoices.CostToCraftPerSlot+" Per\n"+
						"\nQuality:"+
						"\nChance:"+
						"\nDamage:" + 
						"\nRange: "+
						"\nDurablity: "+
						"\nWeight: " +
						"\nLooks: " );
						}

						

			}
			if (CraftingTypebuttontext == "Armor") {
				if (AllPartsPicked == true) {
					float spacer = 0.0f;
					ArmorSize temparmorsize = MyItemLib.ArmorSizes.Find (x => x.Name == ArmorSelectedName);
					GUI.Label (new Rect (313, 80, 250, 155),
						"Craft Time:"+CraftWeaponChoices.TimeToCraft+" Seconds "+ 
						"| Parts Cost:" + CraftWeaponChoices.CostToCraftPerSlot*temparmorsize.Matuseage+" Per\n"+
						"\t            Possiable Results"+
						"\nQuality:"+
						"\nChance:"+
						"\nDefense:" + 
						"\nDurablity: "+
						"\nWeight: " +
						"\nLooks: " );
					//	GUI.Box (new Rect (371, 130, 230, 120), "");
					ResultsScroll = GUI.BeginScrollView (new Rect (370,110,230, 130), ResultsScroll, new Rect (0, 0,MyRanges.Count*75,95 ),false,false);

					int tempDef = 0;
					int tempDur = 0;
					int tempWei = 0;

					foreach(Partinfo x in CraftWeaponChoices.Parts){
						tempDef += Mathf.RoundToInt (x.DefPercent*x.Mat.BaseDefense);
						tempDur += Mathf.RoundToInt (x.DurPercent * x.Mat.BaseDurablity);
						tempWei += Mathf.RoundToInt (x.WeightPercent * x.Mat.BaseWeight);

					}



					foreach (RangeForItems temp in MyRanges) {
						GUI.Box (new Rect (0 + spacer, 0, 70, 110), temp.Name);
						GUI.Label(new Rect (0 + spacer, 0, 70, 110),
							"\n"+temp.Chance+"%"+
							"\n"+Mathf.RoundToInt((CraftWeaponChoices.BaseDefense+ temp.BounusLow+tempDef)*temparmorsize.Defensemod) +"-"+Mathf.RoundToInt((CraftWeaponChoices.BaseDefense+temp.BounusHigh+tempDef)*temparmorsize.Defensemod) +
							"\n"+Mathf.RoundToInt((CraftWeaponChoices.BaseDurablity+temp.BounusLow+tempDur)*temparmorsize.Durablitymod)+"-"+Mathf.RoundToInt((CraftWeaponChoices.BaseDurablity+temp.BounusHigh+tempDur)*temparmorsize.Durablitymod)+
								"\n"+Mathf.RoundToInt((CraftWeaponChoices.BaseWeight+tempWei)*temparmorsize.Weightmod)+
							"\nShit",CenterLabelstyle);
						spacer +=75.0f;
					}
					GUI.EndScrollView ();

				}
				else {
					ArmorSize temparmorsize = MyItemLib.ArmorSizes.Find (x => x.Name == ArmorSelectedName);
					GUI.Label (new Rect (313, 80, 250, 155),
						"Craft Time:"+CraftWeaponChoices.TimeToCraft+" Seconds "+ 
						"| Parts Cost:" + CraftWeaponChoices.CostToCraftPerSlot*temparmorsize.Matuseage+" Per\n"+
						"\nQuality:"+
						"\nChance:"+
						"\nDefense:" + 
						"\nDurablity: "+
						"\nWeight: " +
						"\nLooks: " );
				}



			}

			//craft button
			if (AllPartsPicked == true) {
				if ((GUI.Button (new Rect (500, 50, 90, 30), "Craft"))) {

					if (CraftError == false) {
						foreach (Partinfo loop in CraftWeaponChoices.Parts) {
							if (GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ().CanIHaveResource (loop.Mat, true, CraftWeaponChoices.CostToCraftPerSlot) == false) {
								CraftErrorMessage += "Not enough " + loop.Mat.QualityName + " " + loop.Mat.Name;
							}
						}

						if (CraftErrorMessage != "") {
							foreach (Partinfo loop in CraftWeaponChoices.Parts) {
								GameObject.FindGameObjectWithTag ("PlayerTotals").GetComponent<MainMoney> ().AddResource (loop.Mat, CraftWeaponChoices.CostToCraftPerSlot, true);
							}
							CraftError = true;
						} else {
							if (CraftingTypebuttontext == "Weapon") {

								BlackQueue.Add( new BlackSmithQueue(new WeaponInfo (0, 0, 0, 0, 0, "", MyItemLib.GetMyName(CraftWeaponChoices.Parts)+" "+CraftWeaponChoices.name, false, "",0, CraftWeaponChoices,0,0,0,craftingnumber),CraftWeaponChoices.TimeToCraft*myBlack.Workspeed));
								CraftWeaponChoices = WeapArmorBaseInfo.Copyme (Weaponbases.Find (x => x.name == CraftingWeapontypebuttontext));
								craftingnumber += 1;


							} else {
							//	BlackSmithArmorInv.Add (new ArmorInfo (1, 1, 1, 1, "", "", CraftWeaponChoices));
								BlackQueue.Add(new BlackSmithQueue(new ArmorInfo(1, 1, 1, 1, MyItemLib.GetMyName(CraftWeaponChoices.Parts)+" "+ CraftWeaponChoices.name,"",0, CraftWeaponChoices,0,0,0,ArmorSelectedName,CraftWeaponChoices.name,craftingnumber),CraftWeaponChoices.TimeToCraft*myBlack.Workspeed));
								CraftWeaponChoices = WeapArmorBaseInfo.Copyme (Armorbases.Find (x => x.name == CraftingWeapontypebuttontext));
								craftingnumber += 1;


							}

						}

					}
				}
			}



		}


    // hire blacksmith screen
        if (menuselct==2)
        {
        //    
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y), "");
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y), "BlackSmith");
			MyMenu=new Rect(MyMenu.x, MyMenu.y ,400 ,300);
            GUI.Label(new Rect(10,40,350,300),"With your level of renown these Blacksmiths are willing to work for you.");
            int spacer = 20;
            foreach (BlackSmithinfo x in myblacksList)
            {
                GUI.Label(new Rect( spacer , 80, 100, 300), "Name: " + x.Name + "\nLevel: " + x.Level + "\nRace: " +x.Race + "\nEfficienty: " + x.Effeciny + "\nWorkSpeed: " + x.Workspeed + "\n\nCost Per Season: " +x.PayPerPeriod);

                if (GUI.Button(new Rect( spacer , 210, 90, 30), "Hire For " +x.CosttoHire))
                {
                    if ( GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().CanIBuy(x.CosttoHire,true)==true)
                    myBlack = x;
                
                    justboughtblacksmith=true;
                    blacksmithrespawntimer = 0;
                }
                spacer += 120;
            }
            // remove blacksmith out of list after buying cant do ti in loop above otheriwse gives error when removed.
            if (justboughtblacksmith==true)
            {
                myblacksList.Remove(myBlack);

                justboughtblacksmith = false;
            }

  
         }
		// buy screen
		if (menuselct == 3) {
			MyMenu=new Rect(MyMenu.x, MyMenu.y ,600 ,350);
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y), "");
			GUI.Box (new Rect(0,0,MyMenu.size.x ,MyMenu.size.y), "BlackSmith");

//			GUI.Box (new Rect (5, 60, 290,200), "Armor");
//			float spacer = 0.0f;
//			foreach (ArmorInfo temp in BlackSmithArmorInv) {
//				GUI.Label (new Rect (10, 80 + spacer, 200, 40), temp.Quality+" "+ temp.Name);
//				spacer += 50;
//			}


			float	spacer = 0.0f;




			GUI.Box (new Rect (305, 60, 290,200), "Weapon");

			WeaponStore = GUI.BeginScrollView(new Rect(305,80,290,180),WeaponStore,new Rect (0, 0, 170,BlacksmithWeaponInv.Count*55.0f));

			foreach (WeaponInfo temp in BlacksmithWeaponInv) {
				//GUI.Button (new Rect (0, 0 + spacer, 20f, 20f), ">");
				GUI.Label (new Rect (20,0 + spacer, 280, 40),temp.Quality+" "+ temp.Name);
				GUI.Label(new Rect(20,18+spacer,190,40),"Damage: " +temp.Damage +" Durablity: " +temp.MaxDurablity+"\nLooks: " +temp.Beauty+ " Weight: " +temp.Weight);

				if (GUI.Button (new Rect (200, 18 + spacer, 20, 20), "-")) {
					if (Input.GetKey(KeyCode.LeftShift)) {
						temp.SellPrice -= 5;
					} else {
						temp.SellPrice -= 1;
					}
					if (temp.SellPrice < 0) {
						temp.SellPrice = 0;

					}
				}
				if (GUI.Button (new Rect (245, 18+ spacer, 20, 20), "+")) {
					if ( Input.GetKey(KeyCode.LeftShift)) {
						temp.SellPrice += 5;
					} else {
						temp.SellPrice += 1;
					}	
				}
				GUI.Label (new Rect (225, 18 + spacer, 190, 40), temp.SellPrice.ToString());
				spacer += 55;
			}

			GUI.EndScrollView();


			spacer = 0.0f;

			GUI.Box (new Rect (5, 60, 290,200), "Armor");

			ArmorStore = GUI.BeginScrollView(new Rect(5, 80, 290,180),ArmorStore,new Rect (0, 0, 170,BlackSmithArmorInv.Count*55.0f));

			foreach (ArmorInfo temp in BlackSmithArmorInv) {
				//GUI.Button (new Rect (0, 0 + spacer, 20f, 20f), ">");
				GUI.Label (new Rect (20,0 + spacer, 280, 40),temp.Quality+" "+ temp.Name);
				GUI.Label(new Rect(20,18+spacer,190,40),"Defense: " +temp.Damagereduc +" Durablity: " +temp.MaxDurablity+"\nLooks: " +temp.Beauty+ " Weight: " +temp.Weight);

				if (GUI.Button (new Rect (200, 18 + spacer, 20, 20), "-")) {
					if (Input.GetKey(KeyCode.LeftShift)) {
						temp.SellPrice -= 5;
					} else {
						temp.SellPrice -= 1;
					}
					if (temp.SellPrice < 0) {
						temp.SellPrice = 0;

					}
				}
				if (GUI.Button (new Rect (245, 18+ spacer, 20, 20), "+")) {
					if ( Input.GetKey(KeyCode.LeftShift)) {
						temp.SellPrice += 5;
					} else {
						temp.SellPrice += 1;
					}	
				}
				GUI.Label (new Rect (225, 18 + spacer, 190, 40), temp.SellPrice.ToString());
				spacer += 55;
			}

			GUI.EndScrollView();

		}

//		if (GUI.Button(new Rect(MyMenu.xMax,260,50,30),"Close"))
//		{
//			BlackMenuActive=false;
//
//		}
		if (GUI.Button(new Rect(MyMenu.width-21,1,18,15),"X"))
		{
			BlackMenuActive=false;
			//Debug.Log (MyMenu.yMin);
		}

		GUILayout.BeginArea(new Rect(0,20,MyMenu.size.x ,MyMenu.size.y));
		menuselct= GUILayout.Toolbar(menuselct, menuOP);
		GUILayout.EndArea();
        GUI.DragWindow();

    }

    public bool blackMenuActive()
    {
        if (BlackMenuActive == true)
        {
            return true;
        }
        return false;
    }
	public void blackSmithWorking()
	{
		if (BlackQueue.Count != 0) {
			if (BlackQueue [0].MyArmor != null) {
				if (BlackQueue [0].MyArmor.Name != "") {
					BlackQueue [0].Timeleft -= Time.deltaTime;
					if (BlackQueue [0].Timeleft <= 0.0f) {
						ArmorStatMaker (BlackQueue [0].MyArmor);
						BlackSmithArmorInv.Add (ArmorInfo.CopyMe( BlackQueue [0].MyArmor));
						BlackQueue.RemoveAt (0);
					}
				}
			}

		}


		if (BlackQueue.Count != 0) {
			if (BlackQueue [0].MyWeapon != null) {
				if (BlackQueue [0].MyWeapon.Name != "") {
					BlackQueue [0].Timeleft -= Time.deltaTime;
					if (BlackQueue [0].Timeleft <= 0.0f) {
						WeaponStatMaker (BlackQueue [0].MyWeapon);
						BlacksmithWeaponInv.Add (WeaponInfo.copyMe (BlackQueue [0].MyWeapon));
						BlackQueue.RemoveAt (0);
					}
				}
			}
		}

	}


	public void ArmorStatMaker(ArmorInfo GiveStats){

		int qual = 0;

		foreach (Partinfo j in GiveStats.MyBaseinfo.Parts) {
			qual += j.Mat.Quality;


		}
		qual = qual / GiveStats.MyBaseinfo.Parts.Count;

		qual = Random.Range (qual - 20, qual + 21);
		ArmorSize Sizeinfo = MyItemLib.ArmorSizes.Find (x => x.Name == ArmorSelectedName);
		ItemQualityDesc temp = MyItemLib.GetQualiy (qual);
		// 


		GiveStats.Quality = temp.Name;
		GiveStats.BasePrice = (int)(GiveStats.MyBaseinfo.Price*temp.PriceMod);
		// gets base range for armor class
		GiveStats.Damagereduc= (int)(Random.Range (GiveStats.MyBaseinfo.BaseDefense + temp.BounsLow, GiveStats.MyBaseinfo.BaseDefense + temp.BounsHigh + 1)*Sizeinfo.Defensemod);
		GiveStats.MaxDurablity = (int)(Random.Range (GiveStats.MyBaseinfo.BaseDurablity + temp.BounsLow, GiveStats.MyBaseinfo.BaseDurablity + temp.BounsHigh + 1)*Sizeinfo.Durablitymod);
		//GiveStats.MaxDurablity = GiveStats.CurrentDurablity;
	
	
		GiveStats.Weight = (int)(GiveStats.MyBaseinfo.BaseWeight*Sizeinfo.Weightmod);
		GiveStats.Beauty = GiveStats.MyBaseinfo.BaseBeauty;
		// adds the modifered for the material types used
		foreach (Partinfo j in GiveStats.MyBaseinfo.Parts) {
			
			GiveStats.Damagereduc += Mathf.RoundToInt(j.DefPercent* j.Mat.BaseDefense);
			GiveStats.MaxDurablity += Mathf.RoundToInt (j.DurPercent * j.Mat.BaseDurablity);
			GiveStats.Weight += Mathf.RoundToInt (j.WeightPercent * j.Mat.BaseWeight);
			GiveStats.SellPrice += (int)(GiveStats.BasePrice * j.PricePercent*j.Mat.PriceMod);
			GiveStats.Beauty += (int)(GiveStats.Beauty * j.PricePercent*j.Mat.BaseBeauty);


		}
		GiveStats.CurrentDurablity = GiveStats.MaxDurablity;
	//	GiveStats.SellPrice = GiveStats.BasePrice;


	}





	public void WeaponStatMaker(WeaponInfo GiveStats)
	{
		int qual = 0;
	
		foreach (Partinfo j in GiveStats.MyBaseinfo.Parts) {
			qual += j.Mat.Quality;


		}
		qual = qual / GiveStats.MyBaseinfo.Parts.Count;

		qual = Random.Range (qual - 20, qual + 21);

		ItemQualityDesc temp = MyItemLib.GetQualiy (qual);
		// 


		GiveStats.Quality = temp.Name;
		GiveStats.BasePrice = (int)(GiveStats.MyBaseinfo.Price*temp.PriceMod);

		GiveStats.Damage = Random.Range (GiveStats.MyBaseinfo.BaseDamage + temp.BounsLow, GiveStats.MyBaseinfo.BaseDamage + temp.BounsHigh + 1);
		GiveStats.MaxDurablity = Random.Range (GiveStats.MyBaseinfo.BaseDurablity + temp.BounsLow, GiveStats.MyBaseinfo.BaseDurablity + temp.BounsHigh + 1);
		//GiveStats.MaxDurablity = GiveStats.CurrentDurablity;
		GiveStats.Range = GiveStats.MyBaseinfo.BaseRange;
		GiveStats.OneHanded = GiveStats.MyBaseinfo.Onehand;
		GiveStats.Weight = GiveStats.MyBaseinfo.BaseWeight;
		GiveStats.Beauty = GiveStats.MyBaseinfo.BaseBeauty;
		foreach (Partinfo j in GiveStats.MyBaseinfo.Parts) {
			Debug.Log (j.Name + (j.DamagePercent * j.Mat.BaseMeleeDamage));
			GiveStats.Damage += Mathf.RoundToInt(j.DamagePercent * j.Mat.BaseMeleeDamage);
			GiveStats.MaxDurablity += Mathf.RoundToInt (j.DurPercent * j.Mat.BaseDurablity);
			GiveStats.Weight += Mathf.RoundToInt (j.WeightPercent * j.Mat.BaseWeight);
			GiveStats.SellPrice += (int)(GiveStats.BasePrice * j.PricePercent*j.Mat.PriceMod);
			GiveStats.Beauty += (int)(GiveStats.Beauty * j.PricePercent*j.Mat.BaseBeauty);


		}
		GiveStats.CurrentDurablity = GiveStats.MaxDurablity;
		GiveStats.SellPrice = GiveStats.BasePrice;
	}


	public List<WeaponInfo> WhatWeaponCanBuy(int gold){
		List<WeaponInfo> temp = new List<WeaponInfo> ();

		foreach(WeaponInfo x in BlacksmithWeaponInv){

			if (x.SellPrice <= gold){

				temp.Add (x);
			}

		}
		return temp;


	}

	public List<ArmorInfo> WhatArmorCanBuy(int gold){
		List<ArmorInfo> temp = new List<ArmorInfo> ();

		foreach(ArmorInfo x in BlackSmithArmorInv){

			if (x.SellPrice <= gold){

				temp.Add (x);
			}

		}
		return temp;


	}

	public bool BuyWeapon(WeaponInfo buying){

		if(BlacksmithWeaponInv.Contains(buying)==true){
			BlacksmithWeaponInv.Remove (buying);
			return true;
		}
		return false;

	}
	public bool BuyArmor(ArmorInfo buying){

		if(BlackSmithArmorInv.Contains(buying)==true){
			BlackSmithArmorInv.Remove (buying);
			return true;
		}
		return false;

	}

}



//*************old code for buttons incase it breaks**********

//            GUI.Label(new Rect(150, 80, 100, 300), "Name: "+myblacksList[1].Name +"\nLevel: "+myblacksList[1].Level+"\nRace: "+myblacksList[0].Race+"\nEfficienty: "+myblacksList[2].Effeciny+"\nWorkSpeed: "+myblacksList[1].Workspeed+"\n\nCost Per Season: "+myblacksList[0].PayPerPeriod);
//            if (GUI.Button(new Rect(150,210,70,30),"Hire For "+myblacksList[1].CosttoHire))
//            {
//
//                myBlack = myblacksList[1];
//                myblacksList.Clear();
//                blacksmithrespawntimer = 0;
//            }
//
//            GUI.Label(new Rect(250, 80, 100, 300), "Name: "+myblacksList[2].Name +"\nLevel: "+myblacksList[1].Level+"\nRace: "+myblacksList[0].Race+"\nEfficienty: "+myblacksList[2].Effeciny+"\nWorkSpeed: "+myblacksList[2].Workspeed+"\n\nCost Per Season: "+myblacksList[0].PayPerPeriod);
//            if (GUI.Button(new Rect(250,210,70,30),"Hire For "+myblacksList[2].CosttoHire))
//            {
//              
//                myBlack = myblacksList[2];
//                myblacksList.Clear();
//            }
