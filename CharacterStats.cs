
using UnityEngine;

public class CharacterStats : MonoBehaviour {
	public int maxHealth = 12;
	public int currentHealth { get; private set;}

	public Stats health;
	public Stats armorRating;
	public Stats damage;
	public int magic;
	public int range;
 	public int itemHold;

	void Awake(){
		currentHealth = maxHealth;
	}


		
	public void TakeDamage(int damageTaken){
		damageTaken -= armorRating.GetValue();
		Mathf.Clamp (damageTaken, 0, int.MaxValue);
		currentHealth -= damageTaken;
		Debug.Log (transform.name + " takes " + damage + " damage.");

		if (currentHealth < 1) {
			Die ();
		}
	}

	public virtual void Die(){

	}
}
