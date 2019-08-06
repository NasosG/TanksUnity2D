using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [Header("Stats")]
    public int health;                      //The current health of the castle.
    //public int maxHealth;					//The maximum health of this castle.
    public Sprite destroyedSprite;
    public int smallCastles;
    public UI ui;

    void OnCollisionEnter2D(Collision2D col)
    {
        //we can't hit the big castle till we destroy every small castle 
        if (!ui.smallCastlesAreDestroyed && this.tag == "bigCastle")
            return;

        //if it is a rocket 
        if (col.gameObject.tag == "ProjectileRocket")
            health -= 2; // -2 hp if castle collides with a rocket
        //else this is a simple projectile
        else if (col.gameObject.tag == "Projectile")
            health--; // -1 hp if castle collides with a simple projectile
        //if castle gets destroyed
        if (health <= 0) {
            if(this.tag == "bigCastle")
                ui.SetCampaignWinScreen(); //if big castle is destroyed player wins
            this.GetComponent<SpriteRenderer>().sprite = destroyedSprite; //put the destroyed castle sprite
        }
    }

  
}
