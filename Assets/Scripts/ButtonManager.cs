using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public int newGame;
	public int loadGame;
	public int townMenu;
	public int dungeon;
	public int store;
	public int characterSheet;
	public int mainMenu;

	// ------------------ Main Menu ------------------
	public void loadNewGameScene()
    {
		SceneManager.LoadScene(newGame);
    }

	public void loadLoadGameScene() 
    {
		SceneManager.LoadScene(loadGame);
    }

	public void loadTownMenuScene() 
	{
		SceneManager.LoadScene(townMenu);
	}


	// ------------------ Town Menu ------------------
	public void saveGameBtn()
	{
		//add stuff
	}

    public void loadDungeonScene()
    {
        SceneManager.LoadScene(dungeon);
    }

    public void loadStoreScene()
    {
        SceneManager.LoadScene(store);
    }

    public void loadMainMenuScene()
    {
		SceneManager.LoadScene(mainMenu);
    }

    public void loadCharacterSheetScene()
    {
        SceneManager.LoadScene(characterSheet);
    }
}
