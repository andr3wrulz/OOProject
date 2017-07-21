using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

    public int GetLoot(float maxHealth)
    {
        int probability = Random.Range(1, 2);
        float health = calculateHealth(maxHealth);
        int gold = calculateGold(GameControl.control.playerData.gold);

        // if the player's health is less than 20% of his original health make it more likely to get health.
        if ((float)(GameControl.control.playerData.health) <= (float)(.2 * maxHealth))
        {
            // 2/3 possibility of gaining health.

            probability = Random.Range(1, 3);
        }

        // if the player is at full health then just add gold.
        if (probability % 2 == 0 || GameControl.control.playerData.health == maxHealth)
        {
            
            GameControl.control.playerData.addGold(gold);
            // remove loot bag from the game
            GameObject.Destroy(this.gameObject);
            return gold;
        }
        else
        {
            GameControl.control.playerData.gainHealth(health);
            // remove loot bag from the game
            GameObject.Destroy(this.gameObject);
            return -1 * (int)health;
        }
    }

    float calculateHealth(float maxHealth)
    {
        // gain 20% of health

        float health = (float)(.2 * maxHealth);
        
        // don't let the player excede the maxHealth available
        if(health + GameControl.control.playerData.health > maxHealth)
        {
            health = maxHealth - GameControl.control.playerData.health;
        }

        return health;
    }
    int calculateGold(int gold)
    {
        return 10;
    }

}
