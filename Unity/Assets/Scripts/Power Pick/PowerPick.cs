using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PowerPick : MonoBehaviour 
{
	[SerializeField]private List<AbilityToolTip> m_abilityList;

	[SerializeField]private int m_currentSelection;
	private int m_width = 8;
	private int m_height = 5;
	[SerializeField]private int m_noOfAbility;

	private int m_currentPlayerSelection = -1;

	private string m_currentPlayer = "P1";

    [SerializeField]private List<string> m_Players;
	private float m_moveTimer = 0;

    [SerializeField]
    private float player1;
    [SerializeField]
    private float player2;
	// Use this for initialization
	void Start () 
	{
		m_currentSelection = 0;

		GameObject[] a = GameObject.FindGameObjectsWithTag("AbilityToolTip");

		for(int i = 0; i < a.Length; i++)
			m_abilityList.Add((AbilityToolTip)a[i].GetComponent("AbilityToolTip"));

		m_noOfAbility = m_abilityList.Count - 1;

		m_abilityList.Sort((AbilityToolTip x, AbilityToolTip y) => {return x.name.CompareTo(y.name);});

		for(int i = 0; i < m_abilityList.Count; i++)
			Debug.Log (m_abilityList[i].name);      
	}
	
	// Update is called once per frame
	void Update () 
	{

        //InitiatePlayers();
		m_moveTimer += Time.deltaTime;
		m_currentSelection = Mathf.Clamp(m_currentSelection, FirstAvailableNode(), m_noOfAbility);
		
		//selectionInput();

		if(m_currentPlayerSelection >= 0)
		{
			m_abilityList[m_currentPlayerSelection].m_selected = true;
			m_currentPlayerSelection = -1;
			m_currentSelection = FirstAvailableNode();
		}

        //player1 = Input.GetAxis("P1 Ability 1");
        //player2 = Input.GetAxis("P2 Ability 1");

	}

	void selectionInput()
	{
		float xAxis = Input.GetAxis(m_currentPlayer + " Movement X");
		float yAxis = Input.GetAxis(m_currentPlayer + " Movement Y");

		int originalSelect = m_currentSelection;

		if(yAxis < -0.5f && m_moveTimer > 0.2f)
		{
			if(!(m_currentSelection - m_width < 0))
				m_currentSelection -= m_width;

			while(m_abilityList[m_currentSelection].m_selected)
			{
				m_currentSelection -= m_width;
				if(m_currentSelection < 0)
					m_currentSelection = FirstAvailableNode();
			}
			m_moveTimer = 0;
		}

		if(yAxis > 0.5f && m_moveTimer > 0.2f)
		{		
			if(!(m_currentSelection + m_width > m_noOfAbility))
				m_currentSelection += m_width;
		
			while(m_abilityList[m_currentSelection].m_selected)
			{
				m_currentSelection += m_width;
				if(m_currentSelection > m_noOfAbility)
					m_currentSelection = NextAvailableNode(m_currentSelection - m_width);
			}
			m_moveTimer = 0;
		}

		if(xAxis < -0.5f && m_moveTimer > 0.2f)
		{
			int currentHeight = m_currentSelection / m_width;

			if(!(m_currentSelection - 1 < 0))
			{
				m_currentSelection--;
				int newHeight = m_currentSelection / m_width;
				if(newHeight < currentHeight)
					m_currentSelection++;
			}
		
			if(m_abilityList[m_currentSelection].m_selected)
				m_currentSelection = PreviousAvailableNode(m_currentSelection);

			m_moveTimer = 0;
		}
		
		if(xAxis > 0.5f && m_moveTimer > 0.2f)
		{		
			int currentHeight = m_currentSelection / m_width;

			if(!(m_currentSelection + 1 > m_noOfAbility))
			{
				m_currentSelection++;
				int newHeight = m_currentSelection / m_width;
				if(newHeight > currentHeight)
					m_currentSelection--;
			}
		
			if(m_abilityList[m_currentSelection].m_selected)
				m_currentSelection = NextAvailableNode(m_currentSelection);

			m_moveTimer = 0;
		}

		if(Input.GetKeyUp(KeyCode.Return))
		{
			m_abilityList[m_currentSelection].m_hover = false;

			m_currentPlayerSelection = m_currentSelection;
		}

		int newSelection = m_currentSelection;
		if(newSelection != originalSelect)
		{
			m_abilityList[originalSelect].m_hover = false;
			m_abilityList[newSelection].m_hover = true;
		}

	}

	int NextAvailableNode(int a_current)
	{
		for(int i = a_current; i <= m_noOfAbility; i++)
			if(m_abilityList[i].m_selected == false)
				return i;

		return FirstAvailableNode();
	}

	int PreviousAvailableNode(int a_current)
	{
		for(int i = a_current; i > FirstAvailableNode(); i--)
			if(m_abilityList[i].m_selected == false)
				return i;
		
		return 0;
	}

	int FirstAvailableNode()
	{
		for(int i = 0; i < m_abilityList.Count; i++)
			if(m_abilityList[i].m_selected == false)
				return i;
		
		return 0;
	}

	int LastAvailableNode()
	{
		for(int i = m_abilityList.Count - 1; i > 0; i--)
			if(m_abilityList[i].m_selected == false)
				return i;
		
		return 0;
	}

    void InitiatePlayers()
    {
        bool p1 = false, p2 = false, p3 = false, p4 = false;

        //if (Input.GetAxis("P1 Ability 1") == 1 && p1 == false)
        //{
        //    p1 = true;
        //    m_Players.Add("P1");
        //}
        //
        //if (Input.GetAxis("P2 Ability 1") == 1 && p2 == false)
        //{
        //    p2 = true;
        //    m_Players.Add("P2");
        //}
        //
        //if (Input.GetAxis("P3 Ability 1") == 1 && p3 == false)
        //{
        //    p3 = true;
        //    m_Players.Add("P3");
        //}
        //
        //if (Input.GetAxis("P4 Ability 1") == 1 && p4 == false)
        //{
        //    p4 = true;
        //    m_Players.Add("P4");
        //}
        //
    }
}
