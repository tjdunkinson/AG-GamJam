using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PowerPickPlayer : MonoBehaviour {

    public InputDevice m_controller;
    private TextMesh m_text;

    private List<AbilityToolTip> m_chosenAbilities;

    private int m_currentAbilityCount;
    
    // Use this for initialization
	void Start ()
    {
        m_currentAbilityCount = 0;
        m_chosenAbilities = new List<AbilityToolTip>();
        GetAbilitySlots();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void AttachControler(InputDevice a_controller)
    {
        m_controller = a_controller;
    }

    public void AddToChosenAbilities(AbilityToolTip a_ability)
    {
        m_chosenAbilities[m_currentAbilityCount].m_selected = false;
        m_chosenAbilities[m_currentAbilityCount].name = a_ability.name;
        m_chosenAbilities[m_currentAbilityCount].m_description = a_ability.m_description;
        m_chosenAbilities[m_currentAbilityCount].gameObject.renderer.material = a_ability.gameObject.renderer.material;
        m_chosenAbilities[m_currentAbilityCount].gameObject.SetActive(true);
        m_currentAbilityCount++;
    }

    void GetAbilitySlots()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform abilitySlot = transform.GetChild(i);
            abilitySlot.gameObject.AddComponent("AbilityToolTip");
            AbilityToolTip tobeAdded = (AbilityToolTip)abilitySlot.gameObject.GetComponent("AbilityToolTip");
            m_chosenAbilities.Add(tobeAdded);
        }
        
        m_chosenAbilities.Sort((AbilityToolTip x, AbilityToolTip y) => { return x.name.CompareTo(y.name); });
    }
}
