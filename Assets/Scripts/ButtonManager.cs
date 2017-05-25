using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void newGameBtn(string newGameLevel)
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void saveGameBtn(string saveState)
    {
        //add stuff
    }

    public void loadGameBtn(int loadState) 
    {
        //add stuff
    }

    public void enterDungeonBtn(string dungeon) //input Player player as a parameter
    {
        SceneManager.LoadScene(dungeon);
    }

    public void enterStoreBtn(string store) //add param player.gold
    {
        SceneManager.LoadScene(store);
    }

    public void returnToMainMenuBtn(string menu)
    {
        SceneManager.LoadScene(menu);
    }

    public void characterBtn(string characterSheet)
    {
        SceneManager.LoadScene(characterSheet);
    }
}
