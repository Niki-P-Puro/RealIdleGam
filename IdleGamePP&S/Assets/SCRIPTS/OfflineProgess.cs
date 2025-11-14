using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OfflineProgess : MonoBehaviour {
	PlayerPrefs playerPrefs;
	private const string offlineProgressKey = "offlineProgress";
    float offlineEarnings;
    // Use this for initialization
    public void Start()
    {
        {
            StartCoroutine(DelayStart());
        }
    }   
            IEnumerator DelayStart () {
        yield return new WaitForSeconds(1f); // Wait for 1 second to ensure Gamemanager is initialized
		if (PlayerPrefs.HasKey(offlineProgressKey))
		{
			long storedTime = Convert.ToInt64(PlayerPrefs.GetString(offlineProgressKey));
			DateTime lastTime = DateTime.FromBinary(storedTime);
			TimeSpan offlineDuration = DateTime.UtcNow - lastTime;
			Debug.Log("Offline duration: " + offlineDuration.TotalSeconds + " seconds");
			if (offlineDuration.TotalSeconds > 0)
            {
                 offlineEarnings = PlayerPrefs.GetFloat("generation") * (float)offlineDuration.TotalSeconds;
                
            }
		}
		if (PlayerPrefs.HasKey("value"))
        {
            float storedValue = PlayerPrefs.GetFloat("value");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number = storedValue;
            Debug.Log("Restored value: " + storedValue);
        }
		else Debug.Log("no offline progess detected");
		
		if (PlayerPrefs.HasKey("generators"))
        {
            string generators = PlayerPrefs.GetString("generators");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().LoadGenUmanager();
            Debug.Log("Restored generators: " + generators);
        }
        else
        {
            Debug.Log("no generator key detected");
        }
		if (PlayerPrefs.HasKey("upgrades"))
        {
            string serializedUpgrades = PlayerPrefs.GetString("upgrades");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().LoadGUPGmanager();
 
        }
        else
        {
            Debug.Log("no upgrades key detected");
        }
        GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().StartGenerating();
        GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number += offlineEarnings;
        Debug.Log("Added offline earnings: " + offlineEarnings);
        print("loading complete");
	}
	
	void OnApplicationQuit()
    {
        long currentTime = DateTime.UtcNow.ToBinary();
        PlayerPrefs.SetString(offlineProgressKey, currentTime.ToString());
		PlayerPrefs.SetFloat("value", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number);
		PlayerPrefs.SetString("generators", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().SaveGenUmanager());
        PlayerPrefs.SetString("upgrades", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().SaveGUPGmanager());
		PlayerPrefs.SetFloat("generation", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().calulatedgen *10);
        print(GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().calulatedgen*10); 
        PlayerPrefs.Save();
        print("saved");
    }
}
