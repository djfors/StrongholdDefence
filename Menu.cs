using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public Toggle toggleTrolls, toggleWaves, toggleGangs;
	private Text roundTimeText;
	private Slider timeSlider;
	
	void Start()
	{
		roundTimeText = GameObject.Find("RoundTime").GetComponent<Text>();
		timeSlider = GameObject.Find("SetTimeSlider").GetComponent<Slider>();
		
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			toggleTrolls.isOn = PlayerPrefs.GetInt("Trolls", 0) == 1;
			toggleWaves.isOn = PlayerPrefs.GetInt("Waves", 0) == 1;
			toggleGangs.isOn = PlayerPrefs.GetInt("Gangs", 0) == 1;
			roundTimeText.text = string.Format("{0} min", PlayerPrefs.GetInt("RoundTime", 3));
			timeSlider.value = (int) PlayerPrefs.GetInt("RoundTime", 3);
		}
	}
	
	public void toggleTrollsSelected()
	{
		if (toggleTrolls.isOn)
			PlayerPrefs.SetInt("Trolls", 1);
		else
			PlayerPrefs.SetInt("Trolls", 0);
	}
	
	public void toggleWavesSelected()
	{
		if (toggleWaves.isOn)
			PlayerPrefs.SetInt("Waves", 1);
		else
			PlayerPrefs.SetInt("Waves", 0);
	}
	
	public void toggleGangsSelected()
	{
		if (toggleGangs.isOn)
			PlayerPrefs.SetInt("Gangs", 1);
		else
			PlayerPrefs.SetInt("Gangs", 0);
	}
	
    public void PlayGame()
	{
		PlayerPrefs.SetInt("RoundTime", (int) timeSlider.value);
		SceneManager.LoadScene(1);
	}
	
	public void ChangeTime()
	{
		roundTimeText.text = string.Format("{0} min", timeSlider.value);
	}
	
	public void QuitGame()
	{
		PlayerPrefs.DeleteAll();
		Application.Quit();
	}
}
