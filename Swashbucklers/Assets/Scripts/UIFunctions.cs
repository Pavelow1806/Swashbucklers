﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using Newtonsoft.Json;
using Assets.Scripts;
using System.Linq;

public class UIFunctions : MonoBehaviour {

    //UI Button controls and Misc UI control functions (Attached to UI Manager GameObject)

    //Sound effect objects:
    public AudioSource UpgradeStuff;
    public AudioSource SellStuff;
    public AudioSource Click;
    public AudioSource CloseW;

    //Canvas objects:
    private GameObject UI;
    private GameObject Music;
    private GameObject MM;

    //Floats to store & display the combat scene loading countdown
    private float CurrTime = 0.0f;
    private float StartTime = 0.0f;
    //Boolean to check if the game is starting (combat scene being loaded)
    private bool Starting = false;
	
	// Update is called once per frame
	void Update () {
        if (Starting)
        {
            //If the Combat scene is about to be loaded display the countdown to it on the relevant UI element:
            GameObject.Find("xCombat").transform.GetChild(2).GetComponent<Text>().text = "Starting Combat in.. " + Mathf.Round(StartTime - Time.time).ToString();
        }
	}
    void Start()
    {
        //Initialise game-object variable on start:
        MM = GameObject.Find("EventSystem");
        Music = MM.transform.GetChild(0).gameObject;
    }
    //Button Controls:

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //          Miscellaneous button controls:
    public void CloseUI()
    {
        //Function which closes the UI if it's open
        //Play the close window sound effect
        CloseW.Play();
        UI = GameObject.Find("UI");
        Debug.Log("UI Closed");
        //If the UI has been closed set the current location of the AI-controlled player ship to the start
        GameObject.Find("Main Camera").GetComponent<ManagementTap>().currLocation = "Start";
        //Hide the UI:
        UI.SetActive(false);
    }
    public void StartCombat()
    {
        //Function to start combat (Activated when player clicks yes to starting combat)
        //Play generic click UI element click sound
        Click.Play();
        //De-activate the buttons on the UI (Yes / No buttons)
        GameObject.Find("xCombat").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("xCombat").transform.GetChild(1).gameObject.SetActive(false);
        //Set the countdown text initially (TBC by update in this class (above))
        GameObject.Find("xCombat").transform.GetChild(2).GetComponent<Text>().text = "Starting Combat in.. 5";
        //Set the starting boolean to true
        Starting = true;
        //Set the current time variable to be the time since the game started.
        CurrTime = Time.time;
        //Set the start time variable to be the time since the game started + 5 seconds. (primarily to allow the AI-controlled player ship to move upwards and give the player some time to prepare for battle)
        StartTime = Time.time + 5.0f;
        //Set the speed of the ship to a huge amount
        GameObject.Find("MenuPlayer").GetComponent<NavMeshAgent>().speed = 100;
        //Set the Moving to combat target waypoint boolean in the management class to true
        GameObject.Find("Main Camera").GetComponent<ManagementTap>().cTar = true;
        //switch scene to combat scene after 5 seconds
        Invoke("BeginGame", 5.0f);
    }
    void BeginGame()
    {
        //Used in StartCombat to load the scene using Invoke.
        SceneManager.LoadScene(1);
    }
    public void ClickObj()
    {
        //Used as a public function to allow other functions to use the attached click sound effect remotely.
        Click.Play();
    }
    public void ToggleMusic()
    {
        //Toggles the music on/off
        //Play generic click UI sound effect
        Click.Play();
        //Reverse the objects sound enabled status to enable or disable music
        Music.SetActive(!Music.activeInHierarchy);
    }
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //          Shipyard Button controls:

        //Upgrade button generic comments:
            //Click.Play();         - Plays Generic UI Button Click sound effect
            //If statement          - Checks if the users gold amount is greater than the cost of the item the player is trying to upgrade
                //UpgradeStuff.Play();  - Plays the successful upgrade item sound effect
                //SetInt #1             - Sets the players gold to be X amount lower than it is now dependant on the upgrade cost of that item
                //SetInt #2             - Increments upgrade level of item
            //SetFloat              - Sets the float (which is used in the combat scene) to the appropriate new amount based on the upgrade value of that item


    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //          Market Button Controls:
    public void SellAll()
    {
        //This function sells all of the players cargo items based on the table found in the brief of the assignment
        //Play the generic ui click sound effect
        Click.Play();
        var currentInventory = PlayerPrefs.GetString("Inventory");
        var inventory = JsonConvert.DeserializeObject<Inventory>(currentInventory);

        if (inventory.Items.Any())
        {
            //Play the selling sound effect if there was something to be sold
            SellStuff.Play();
            int goldMade = inventory.Items.Sum(i => (int)i.LootName * i.Count);
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + goldMade);
            inventory = new Inventory();
        }

        PlayerPrefs.SetString("Inventory", JsonConvert.SerializeObject(inventory));
    }
}
