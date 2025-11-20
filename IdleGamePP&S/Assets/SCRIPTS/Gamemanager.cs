using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    //
    // !!! CHANGE PUBLIC VARIABLES IN UNITY INSPECTOR- NOT HERE !!!
    //
    public List<int> value = new List<int>();
    public Text NumberText, NumberAs;
    public float Number = 10;
    public GameObject[] Upgrades = new GameObject[8];
    public GameObject[] Generators = new GameObject[8];
    public GameObject[] Achievements = new GameObject[16];
    public float CPS, generation;
    public GameObject[] menus = new GameObject[8];
    private bool isGenerating = false; // Track if generation is active
    public float growthrate = 1.15f; 
    public float totalGeneration = 0;
    public float calulatedgen;
    public List<Text> texts = new List<Text>();
    public Font FontToUse;
    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true; // Allow the application to run in the background for idle play
        for (int i = 0; i < Generators.Length; i++) // initialize generators
        {
            Generators[i].GetComponent<GUmanager>().cost = Mathf.Pow(8, (i * ((i != 0 && i != 1) ? 1.3f : 1f)) + 1); // Cost increases exponentially
            Generators[i].GetComponent<GUmanager>().purchasemultiplier = 1.6f + ((i+1) * (growthrate)); // Incremental multiplier
            Generators[i].GetComponent<GUmanager>().change = 0.02f * Mathf.Pow(6, (i * 1.3f) + 1);//this is kinda messed up   // Change increases with index
            Generators[i].GetComponent<GUmanager>().multiplier = 1f;
            Generators[i].GetComponent<GUmanager>().speed = 1f;
            Generators[i].GetComponent<GUmanager>().amountowned = 0;
        }
        for (int i = 0; i < Upgrades.Length; i++) // initialize upgrades
        {
            Upgrades[i].GetComponent<GUmanager>().cost = Mathf.Pow(12, (i * 1.1f) + 1); // Cost increases exponentially
            Upgrades[i].GetComponent<GUmanager>().purchasemultiplier = 6f + ((i+1) * (growthrate)); // Incremental multiplier
            Upgrades[i].GetComponent<GUmanager>().change = 0;
            Upgrades[i].GetComponent<GUmanager>().multiplier = 1.4f;
            Upgrades[i].GetComponent<GUmanager>().speed = 1f;
            Upgrades[i].GetComponent<GUmanager>().amountowned = 0;
            Upgrades[i].GetComponent<GUmanager>().description = "2x production";
        }
        if (PlayerPrefs.HasKey("offlineProgress")) // Restore offline progress
        {
            float storedValue = PlayerPrefs.GetFloat("offlineProgress");
            Number += storedValue;
            Debug.Log("Restored offline progress: " + storedValue);
        }
        texts = new List<Text>(Resources.FindObjectsOfTypeAll(type: typeof(Text)) as Text[]); // find all text objects in scene
        foreach (Text txt in texts) // set font for all text objects
        {
            txt.font = FontToUse;
        }
        
        //for (int i = 0; i < Achievements.Length; i++)
        //    {
        //     Achievements[i] = GameObject.Find("AchievementHolder").transform.GetChild(i).gameObject;
        //    }
        //achivements will provide flat multipliers 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) ) // press P to clear all saved data
        {
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.Quote)) // press ' to print amount owned of each generator
        {
            for (int i = 0; i < Generators.Length; i++)
            {
              print(Generators[i].GetComponent<GUmanager>().amountowned);
            }
        }
        if (Number < Mathf.Pow(10,6)) // update NumberText based on size of Number
        NumberText.text = Number.ToString("F1");
        else
        NumberText.text = Number.ToString("E1");
        totalGeneration= 0; 
        for (int i = 0; i < Generators.Length; i++) // calculate total generation from all generators
        {
            totalGeneration += (Generators[i].GetComponent<GUmanager>().change * Generators[i].GetComponent<GUmanager>().amountowned);
            Generators[i].GetComponent<GUmanager>().description = Generators[i].GetComponent<GUmanager>().change.ToString("F1") + "/s";
        }
        generation = totalGeneration;
        calulatedgen = (Generators[0].GetComponent<GUmanager>().change * Generators[0].GetComponent<GUmanager>().amountowned) + (Generators[1].GetComponent<GUmanager>().change * Generators[1].GetComponent<GUmanager>().amountowned) + (Generators[2].GetComponent<GUmanager>().change * Generators[2].GetComponent<GUmanager>().amountowned) + (Generators[3].GetComponent<GUmanager>().change * Generators[3].GetComponent<GUmanager>().amountowned) + (Generators[4].GetComponent<GUmanager>().change * Generators[4].GetComponent<GUmanager>().amountowned) + (Generators[5].GetComponent<GUmanager>().change * Generators[5].GetComponent<GUmanager>().amountowned) + (Generators[6].GetComponent<GUmanager>().change * Generators[6].GetComponent<GUmanager>().amountowned) + (Generators[7].GetComponent<GUmanager>().change * Generators[7].GetComponent<GUmanager>().amountowned);
        NumberAs.text = (totalGeneration*10).ToString("F1") + "/s"; // update CPS text  
    }

    public void SubNumber(float sub) // subtracts sub from Number if needed
    {
        Number -= sub;
        NumberText.text = Number.ToString();
    }

    public void BuyGenerator(int index) // buys generator if enough money is available
    {
        if (index >= 0 && index < Generators.Length)
        {
            if (Number >= Generators[index].GetComponent<GUmanager>().cost)
            {
                Number -= Generators[index].GetComponent<GUmanager>().cost; // subtract cost from Number
                Generators[index].GetComponent<GUmanager>().cost *= Generators[index].GetComponent<GUmanager>().purchasemultiplier; // increase cost for next purchase
                Generators[index].GetComponent<GUmanager>().amountowned++; // increment amount owned
                print("bought generator");
            }
        }
    }
    public void BuyUpgrade(int index) // buys upgrade and applies its effect to the corresponding generator
    {
        if (index >= 0 && index < Upgrades.Length)
        {
            if (Number >= Upgrades[index].GetComponent<GUmanager>().cost)
            {
                Number -= Upgrades[index].GetComponent<GUmanager>().cost;
                Upgrades[index].GetComponent<GUmanager>().cost *= Upgrades[index].GetComponent<GUmanager>().purchasemultiplier; // increase cost for next purchase
                Upgrades[index].GetComponent<GUmanager>().amountowned++; // increment amount owned
                Generators[index].GetComponent<GUmanager>().change *= Upgrades[index].GetComponent<GUmanager>().multiplier; // apply upgrade effect
                print("bought upgrade");
            }
        }
    }
    public void StartGenerating()
    {
        isGenerating = true; // Set generating to true
        StartCoroutine(ConstantTime());
        print("generating");
    }

    IEnumerator ConstantTime() // Coroutine for constant time interval
    {
        while (isGenerating)
        {
            Number += generation; // Add generation to Number
            yield return new WaitForSeconds(0.1f);
            
        }
    }

    public void OpenGen()
    {
        menus[0].SetActive(true);
        menus[1].SetActive(false);
        menus[2].SetActive(false);
        menus[3].SetActive(false);
    }

    public void OpenUpg()
    {
        menus[1].SetActive(true);
        menus[0].SetActive(false);
        menus[2].SetActive(false);
        menus[3].SetActive(false);
    }
    public void OpenAch()
    {
        menus[2].SetActive(true);
        menus[0].SetActive(false);
        menus[1].SetActive(false);
        menus[3].SetActive(false);
    }
    public void OpenPres()
    {
        menus[3].SetActive(true);
        menus[0].SetActive(false);
        menus[1].SetActive(false);
        menus[2].SetActive(false);
    }


    [System.Serializable]
    public class GUsavedata
    {

       public float cost;
       public float amountowned;
       public float change;
       public float multiplier;
       public float speed;
       public float purchasemultiplier;
       public string description;
       
    }
    [System.Serializable]
    public class SaveDataArray
    {
        public GUsavedata[] items;
    }

    public string SaveGenUmanager() // saves generator data to json
    {
        SaveDataArray saveData = new SaveDataArray();
        saveData.items = new GUsavedata[Generators.Length];
        for (int i = 0; i < Generators.Length; i++)
        {
            if (Generators[i] == null) // skip null generators
                continue; 
            GUmanager g = Generators[i].GetComponent<GUmanager>();
            saveData.items[i] = new GUsavedata // create new savedata object
            {
                cost = g.cost,
                amountowned = g.amountowned,
                change = g.change,
                multiplier = g.multiplier,
                speed = g.speed,
                purchasemultiplier = g.purchasemultiplier,
                description = g.description
            };
        }
        string json = JsonUtility.ToJson(saveData);
        print(json);
        return json;
    }
    public void LoadGenUmanager() // loads generator data from json
    {
        if (!PlayerPrefs.HasKey("generators")) return; // check if key exists
        string json = PlayerPrefs.GetString("generators");
        SaveDataArray saveData = JsonUtility.FromJson<SaveDataArray>(json);
        for (int i = 0; i < Generators.Length; i++)
        {
            if (Generators[i] != null) // skip null generators
            {
                GUmanager g = Generators[i].GetComponent<GUmanager>();
                GUsavedata savedata = saveData.items[i];
                g.cost = savedata.cost;
                g.amountowned = savedata.amountowned;
                g.change = savedata.change;
                g.multiplier = savedata.multiplier;
                g.speed = savedata.speed;
                g.purchasemultiplier = savedata.purchasemultiplier;
                g.description = savedata.description;
                print("loaded generator data for generator " + i + ": " + json);
            }
            else {
                print("could not load generator data for generator " + i + ": null generator object");  
            }
        }
       
            
           
        
    }
    public string SaveGUPGmanager() // saves upgrade data to json
    {
        SaveDataArray saveData = new SaveDataArray();
        saveData.items = new GUsavedata[Upgrades.Length];
        for (int i = 0; i < Upgrades.Length; i++)
        {
            if (Upgrades[i] == null) // skip null upgrades
                continue;
            GUmanager g = Upgrades[i].GetComponent<GUmanager>();
            saveData.items[i] = new GUsavedata
            {
                cost = g.cost,
                amountowned = g.amountowned,
                change = g.change,
                multiplier = g.multiplier,
                speed = g.speed,
                purchasemultiplier = g.purchasemultiplier,
                description = g.description
            };
        }
        string json = JsonUtility.ToJson(saveData);
        print(json);
        return json;
    }
    public void LoadGUPGmanager() // loads upgrade data from json
    {
        if (!PlayerPrefs.HasKey("upgrades")) return;
        string json = PlayerPrefs.GetString("upgrades");
        SaveDataArray saveData = JsonUtility.FromJson<SaveDataArray>(json);
        for (int i = 0; i < Upgrades.Length; i++)
        {
            if (Upgrades[i] == null) // skip null upgrades
                continue;
            GUmanager g = Upgrades[i].GetComponent<GUmanager>();
            GUsavedata savedata = saveData.items[i];
            g.cost = savedata.cost;
            g.amountowned = savedata.amountowned;
            g.change = savedata.change;
            g.multiplier = savedata.multiplier;
            g.speed = savedata.speed;
            g.purchasemultiplier = savedata.purchasemultiplier;
            g.description = savedata.description;
            print("loaded upgrade data for item " + i + ": " + json);
        }
    }

}

