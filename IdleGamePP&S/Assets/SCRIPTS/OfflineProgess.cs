using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OfflineProgess : MonoBehaviour {
	PlayerPrefs playerPrefs; // Reference to PlayerPrefs for storing data just in case
	private const string offlineProgressKey = "offlineProgress"; // Key to store the last active time
    float offlineEarnings; // Earnings accumulated while offline
    // Use this for initialization
    public void Start() // called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        {
            StartCoroutine(DelayStart());
        }
    }   
            IEnumerator DelayStart () {
        yield return new WaitForSeconds(1f); // Wait for 1 second to ensure Gamemanager is initialized
		if (PlayerPrefs.HasKey(offlineProgressKey)) // Check if we have stored offline progress
		{
			long storedTime = Convert.ToInt64(PlayerPrefs.GetString(offlineProgressKey));
			DateTime lastTime = DateTime.FromBinary(storedTime);
			TimeSpan offlineDuration = DateTime.UtcNow - lastTime;
			Debug.Log("Offline duration: " + offlineDuration.TotalSeconds + " seconds");
			if (offlineDuration.TotalSeconds > 0) // Calculate earnings only if offline duration is greater than 0
            {
                 offlineEarnings = PlayerPrefs.GetFloat("generation") * (float)offlineDuration.TotalSeconds;
                
            }
		}
        if (PlayerPrefs.HasKey("prestige"))
        {
            float storedPrestige = PlayerPrefs.GetFloat("prestige");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().prestigepoints = storedPrestige;
            Debug.Log("Restored prestige points: " + storedPrestige);
        }
		if (PlayerPrefs.HasKey("value")) // Restore stored value
        {
            float storedValue = PlayerPrefs.GetFloat("value");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number = storedValue;
            Debug.Log("Restored value: " + storedValue);
        }
		else Debug.Log("no offline progess detected");
		
		if (PlayerPrefs.HasKey("generators")) // Restore stored generators
        {
            string generators = PlayerPrefs.GetString("generators");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().LoadGenUmanager();
            Debug.Log("Restored generators: " + generators);
        }
        else
        {
            Debug.Log("no generator key detected");
        }
		if (PlayerPrefs.HasKey("upgrades")) // Restore stored upgrades
        {
            //string serializedUpgrades = PlayerPrefs.GetString("upgrades");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().LoadGUPGmanager();
 
        }
        else
        {
            Debug.Log("no upgrades key detected");
        }
        if (PlayerPrefs.HasKey("challenge")) // Restore stored challenge state
        {
            int challengeState = PlayerPrefs.GetInt("challenge");
            if (challengeState == 1)
            {
                GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().challenge = true;
                Debug.Log("Restored challenge state: Active");
            }
            else
            {
                GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().challenge = false;
                Debug.Log("Restored challenge state: Inactive");
            }
        }
        if (PlayerPrefs.HasKey("challengecomplete"))
        {
            int challengecomplete = PlayerPrefs.GetInt("challengecomplete");
            if (challengecomplete == 1)
            {
                GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().challengecomplete = true;
                Debug.Log("Restored challengecomplete state: Active");
            }
            else
            {
                GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().challengecomplete = false;
                Debug.Log("Restored challengecomplete state: Inactive");
            }
        }
        GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().StartGenerating(); // Restart generation after loading
        GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number += offlineEarnings; // Add offline earnings to current number
        Debug.Log("Added offline earnings: " + offlineEarnings); // Devlog the earnings
        print("loading complete"); // Devlog loading complete
	} 
	
	void OnApplicationQuit() // called when the application is about to quit
    {
        long currentTime = DateTime.UtcNow.ToBinary(); // Get the current time in binary format
        PlayerPrefs.SetString(offlineProgressKey, currentTime.ToString()); // Store the current time
		PlayerPrefs.SetFloat("value", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number); // to save the current value
		PlayerPrefs.SetString("generators", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().SaveGenUmanager()); // to save the current generators
        PlayerPrefs.SetString("upgrades", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().SaveGUPGmanager()); // to save the current upgrades
		PlayerPrefs.SetFloat("generation", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().calulatedgen *10); // to save the current generation rate
        PlayerPrefs.SetFloat("prestige", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().prestigepoints); // to save the current prestige points
        if (GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().challenge)
            PlayerPrefs.SetInt("challenge", 1); // to save the current challenge active state
        else
        PlayerPrefs.SetInt("challengecomplete", 0); // to save the current challenge active state
        if (GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().challengecomplete)
            PlayerPrefs.SetInt("challengecomplete", 1); // to save the current challengecomplete active state
        else
            PlayerPrefs.SetInt("autobuyersactive", 0); // to save the current challengecomplete active state
        print(GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().calulatedgen*10);  // Devlog generation rate
        PlayerPrefs.Save();
        print("saved");
    }
}
