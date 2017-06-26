using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InnMenu : MonoBehaviour {
	public int costOfInn = 2;
	public bool innMenuActive = false;
    public int selectedinmenu =0;
	public int MaxRooms=5;
	public int CurrentOcup = 0;
    public string[] menunames = { "Inn Info", "Services Info" };
	public int costOfUpgrade = 100;
	// Use this for initialization
    public Rect InnMenuinfo;
	
	void Start()
	{
        InnMenuinfo=new Rect(Screen.width/2.5f, Screen.height/2.75f ,250   ,250);
	}
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (innMenuActive == true)
            {
                innMenuActive = false;
            }
            else
            {
                innMenuActive = true;
            }
       }
	}
	
	public void MenuOn(){
		innMenuActive=true;
		
	}

	public bool Mouseover=false;


	void OnGUI()
	{
		Mouseover=false;
		if (innMenuActive==true){

            InnMenuinfo = GUI.Window(5, InnMenuinfo, InnMenuinfos, "");
			if(InnMenuinfo.Contains(Event.current.mousePosition)){

				Mouseover = true;
			}

		}
		
		
		
		
	}
			
	public bool MouseOverMe(){

		return Mouseover;


	}



    void InnMenuinfos(int windownumb)
    {
        // left , top, width,hieght
		GUI.Box(new Rect(0,0,InnMenuinfo.width,InnMenuinfo.height),"Inn Menu");

        selectedinmenu= GUILayout.Toolbar(selectedinmenu,menunames);
        if (selectedinmenu==0)
        {
            GUI.Label(new Rect(10,50,300,50),"Rooms : "+ MaxRooms + " Buy new room: " +costOfUpgrade.ToString() +" gold");

            if (GUI.Button(new Rect(10,70,80,20),"Buy Room"))
            {
				if (GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().CanIBuy(costOfUpgrade,true))
                {
                    // sends negivtive money to add function
                  //  GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().AddGold(-costOfUpgrade);
					MaxRooms += 1;
                    GameObject.FindGameObjectWithTag("HeroController").GetComponent<HeroSpawner>().MaxHeroIncrease();
                }



            }


            GUI.Label(new Rect(10,100,100,30),"Room Cost : "+costOfInn.ToString());
         //   GUI.Label(new Rect(10,120,300,30),"Cost of room");


            if (GUI.Button(new Rect(110, 100, 60,20),"+1 Gold"))
            {
                costOfInn=costOfInn+1;

            }
            if (GUI.Button(new Rect(180, 100, 60,20),"-1 Gold"))
            {
                if (costOfInn>0)
                {
                    costOfInn=costOfInn-1;  

                }
            }

        }
		if(selectedinmenu==1){

			if(GUI.Button(new Rect(10,70,80,20),"Buy Worker")){

				GameObject.FindGameObjectWithTag ("WorkerController").GetComponent<WorkerController> ().BuyWorker ();
		
			}



		}
        if (GUI.Button(new Rect(125,200,50,30),"Close"))
        {
            innMenuActive=false;

        }

        GUI.DragWindow();

    }
}
