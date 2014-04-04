using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PowerPick : MonoBehaviour 
{
	CharacterController m_firstPlayer;
	[SerializeField]
	private List<AbilityToolTip> m_abilityList;

	private int m_currentSelection;
	// Use this for initialization
	void Start () 
	{
		m_currentSelection = 0;

		GameObject[] a = GameObject.FindGameObjectsWithTag("AbilityToolTip");
		for(int i = 0; i < a.Length; i++)
			m_abilityList.Add((AbilityToolTip)a[i].GetComponent("AbilityToolTip"));

		m_abilityList.Sort((AbilityToolTip x, AbilityToolTip y) => {return x.name.CompareTo(y.name);});
	}
	
	// Update is called once per frame
	void Update () 
	{
		//m_abilityList[m_currentSelection].
	}

	void OrganiseList()
	{

	}
}
