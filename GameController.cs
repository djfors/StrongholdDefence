using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text timeCounter, moneyCounter;

	public static int moneyBank;

    private DateTime timerEnd;
	private TimeSpan delta;

    private void Start()
    {
		GameController.moneyBank = 3000;
        timerEnd = DateTime.Now.AddSeconds(PlayerPrefs.GetInt("RoundTime") * 60);
		SceneManager.sceneLoaded += OnSceneLoaded;	
    }

    private void Update()
    {
		// учёт денег
		if (moneyBank > 100000)
			moneyBank = 100000;
		moneyCounter.text = moneyBank.ToString();
		
		// отслеживание времени
		delta = timerEnd - DateTime.Now;
		
		if (Stronghold.hp <= 0)
		{
			GameOver("YOU LOSE");
		}
        else if (delta.TotalSeconds <= 0 && Stronghold.hp > 0)
        {
            GameOver("YOU WIN");
        }
		else 
		{	
			timeCounter.text = delta.Minutes.ToString("00") + ":" + delta.Seconds.ToString("00");
		}
    }
	
	private void GameOver(string text)
	{
		Time.timeScale = 0f;
		
		GameObject.Find("Scroll/Text").GetComponent<Text>().text = text;
		GameObject.Find("Scroll").GetComponent<Canvas>().enabled = true;
		GameObject.Find("HUD").SetActive(false);
	}
	
	// очистка сцены при перезагрузке
	void OnSceneLoaded (Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().buildIndex == 1) 
		{
			GameObject.Find("Scroll").GetComponent<Canvas>().enabled = false;
			GameObject.Find("HUD").SetActive(true);
			Time.timeScale = 1f;			
		}
	}
	
	public void PlayGame()
	{
		SceneManager.LoadScene(1);
	}    
	
	public void GoBackToMenu()
	{
		Time.timeScale = 0f;
		SceneManager.LoadScene(0);
	}
}
