using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

	public Text nameText;
	public Text levelText;
	public Slider hpSlider;
	public Text AttackPP;
	public Text SuperAttackPP;
	public Text HealPP;


	public void SetHUD(Unit unit)
	{
		nameText.text = unit.unitName;
		levelText.text = "Lvl " + unit.unitLevel;
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.currentHP;
/*		AttackPP.text = "PP:" + unit.attackPP;
		SuperAttackPP.text = "PP:" + unit.superAttackPP;
		HealPP.text = "PP:" + unit.healPP;*/
    }

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}

}
