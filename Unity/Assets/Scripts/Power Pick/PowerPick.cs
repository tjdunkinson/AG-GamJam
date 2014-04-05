using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;


public class PowerPick : MonoBehaviour 
{
	[SerializeField]
    private List<AbilityToolTip> m_abilityList;
 
    /// Draft Board Movement ///
    private float m_moveTimer = 0;
    private int m_currentSelection;
    [SerializeField]
    private int m_width;
    private int m_noOfAbility;
    [SerializeField]
    private int m_currentPlayer;
    int m_draftRound;

    private int m_currentPlayerSelection = -1;
    TextMesh m_abilityName;
    TextMesh m_abilityDesc;

    /// End Draft Board ///

    /// Initialize Player Block ///
    bool m_Initialized = false;
    private List<PowerPickPlayer> m_players;
    /// End Player Init ///

	// Use this for initialization
	void Start () 
	{
        InputManager.Setup();
        m_draftRound = 0;
		m_currentSelection = 0;
        m_players = new List<PowerPickPlayer>();

		GameObject[] a = GameObject.FindGameObjectsWithTag("AbilityToolTip");

		for(int i = 0; i < a.Length; i++)
			m_abilityList.Add((AbilityToolTip)a[i].GetComponent("AbilityToolTip"));

		m_noOfAbility = m_abilityList.Count - 1;
        m_abilityName = (TextMesh)GameObject.Find("PowerPickName").GetComponent("TextMesh");
        m_abilityDesc = (TextMesh)GameObject.Find("PowerPickInfo").GetComponent("TextMesh");
		m_abilityList.Sort((AbilityToolTip x, AbilityToolTip y) => {return x.name.CompareTo(y.name);});
    }
	
	// Update is called once per frame
	void Update () 
	{
        InputManager.Update();

        if (m_Initialized)
        {
            m_currentSelection = Mathf.Clamp(m_currentSelection, FirstAvailableNode(), m_noOfAbility);

            selectionInput();
            m_abilityName.text = m_abilityList[m_currentSelection].name;
            m_abilityDesc.text = m_abilityList[m_currentSelection].m_description;

            if (m_currentPlayerSelection >= 0)
            {
                //greys ability out
                m_abilityList[m_currentPlayerSelection].m_selected = true;

                //should add the ability to the players list
                m_players[m_currentPlayer].AddToChosenAbilities(m_abilityList[m_currentPlayerSelection]);
                //resets the current player selected
                m_currentPlayerSelection = -1;
                

                //resets current hovered
                m_currentSelection = FirstAvailableNode();
                m_abilityList[m_currentSelection].m_hover = true;

                m_currentPlayer++;
                if (m_currentPlayer > m_players.Count - 1)
                {
                    m_currentPlayer = 0;
                    m_draftRound++;
                }
            }
        }
        else
        {
            InitializePlayers();
            //creates a random order for the players to draft in.
            for (int i = 0; i < m_players.Count; i++) 
            {   
                   PowerPickPlayer temp = m_players[i];
                   int randomIndex = Random.Range(i, m_players.Count);
                   m_players[i] = m_players[randomIndex];
                   m_players[randomIndex] = temp;
            }
        }

	}

    /// <summary>
    /// Controls all the movement and selection around the board.
    /// </summary>
	void selectionInput()
	{
        //creates a delay between movements so we dont have teleportation due to joysticks repeating.
        m_moveTimer += Time.deltaTime;

        //Grab the values of the left joystick for the current player, invert Y.
        float xAxis = m_players[m_currentPlayer].m_controller.LeftStickX;
        float yAxis = -m_players[m_currentPlayer].m_controller.LeftStickY;
    
        //Make note of the original selection
		int originalSelect = m_currentSelection;
    
        //Will move the selection up.
		if(yAxis < -0.5f && m_moveTimer > 0.2f)
		{
            //minus the width of the board
			if(!(m_currentSelection - m_width < 0))
				m_currentSelection -= m_width;
    

            ///handles multiple tiles above it being already selected.
            ///will jump to the next one above it, or the next on to the right.
			while(m_abilityList[m_currentSelection].m_selected)
			{
				m_currentSelection -= m_width;
				if(m_currentSelection < 0)
					m_currentSelection = FirstAvailableNode();
			}
            //reset movetime
			m_moveTimer = 0;
		}
    
        //Moves the selection down
		if(yAxis > 0.5f && m_moveTimer > 0.2f)
		{		
            //Add on the width of the board
			if(!(m_currentSelection + m_width > m_noOfAbility))
				m_currentSelection += m_width;
		
            //handles movement over multiple selected tiles, move to the right if blocked.
			while(m_abilityList[m_currentSelection].m_selected)
			{
				m_currentSelection += m_width;
				if(m_currentSelection > m_noOfAbility)
					m_currentSelection = NextAvailableNode(m_currentSelection - m_width);
			}
            //reset time
			m_moveTimer = 0;
		}
    
        //moves to the left on the board
		if(xAxis < -0.5f && m_moveTimer > 0.2f)
		{
            //make note of current height
			int currentHeight = m_currentSelection / m_width;
    
            //if not on the top left corner of the board
			if(!(m_currentSelection - 1 < 0))
			{
				m_currentSelection--;
				int newHeight = m_currentSelection / m_width;
                //will stop it wrapping around to next level
				if(newHeight < currentHeight)
					m_currentSelection++;
			}
		
            //will move to the next available previous node
			if(m_abilityList[m_currentSelection].m_selected)
				m_currentSelection = PreviousAvailableNode(m_currentSelection);

            //reset the time
			m_moveTimer = 0;
		}
		
        //inverse of Left movement
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
    
        //if player selects this ability set its hover to false, and set as selection.
		if( m_players[m_currentPlayer].m_controller.Action1)
		{
			m_abilityList[m_currentSelection].m_hover = false;
			m_currentPlayerSelection = m_currentSelection;
		}
    
        //Will reset hover on previous ability tile if its different from current.
		int newSelection = m_currentSelection;
		if(newSelection != originalSelect)
		{
			m_abilityList[originalSelect].m_hover = false;
			m_abilityList[newSelection].m_hover = true;
		}
    
	}

    /// <summary>
    /// returns the index of the next closest available index in the ability board
    /// </summary>
    /// <param name="a_current"></param>
    /// <returns></returns>
	int NextAvailableNode(int a_current)
	{
		for(int i = a_current; i <= m_noOfAbility; i++)
			if(m_abilityList[i].m_selected == false)
				return i;

		return FirstAvailableNode();
	}

    /// <summary>
    /// Returns the index of the closest previous available index in teh abilitiy board
    /// </summary>
    /// <param name="a_current"></param>
    /// <returns></returns>
	int PreviousAvailableNode(int a_current)
	{
		for(int i = a_current; i > FirstAvailableNode(); i--)
			if(m_abilityList[i].m_selected == false)
				return i;
		
		return 0;
	}

    /// <summary>
    /// Returns the index of the first available index in the ability board
    /// </summary>
    /// <returns></returns>
	int FirstAvailableNode()
	{
		for(int i = 0; i < m_abilityList.Count; i++)
			if(m_abilityList[i].m_selected == false)
				return i;
		
		return 0;
	}

    /// <summary>
    /// returns the index of the last available ability in the draft board
    /// </summary>
    /// <returns></returns>
	int LastAvailableNode()
	{
		for(int i = m_abilityList.Count - 1; i > 0; i--)
			if(m_abilityList[i].m_selected == false)
				return i;
		
		return 0;
	}

    /// <summary>
    /// Creates and assigns controllers to a list of PowerPickPlayers
    /// </summary>
    void InitializePlayers()
    {

        for (int i = 0; i <= InputManager.Devices.Count - 1; i++)
        {
            GameObject a = GameObject.Find("P" + (i + 1).ToString() + "Anchor");
            a.AddComponent("PowerPickPlayer");
            m_players.Add((PowerPickPlayer)a.GetComponent("PowerPickPlayer"));
            m_players[i].AttachControler(InputManager.Devices[i]);
        }

        for (int i = InputManager.Devices.Count; i < 4; i++)
        {
            GameObject a = GameObject.Find("P" + (i + 1).ToString() + "Anchor");
            a.gameObject.SetActive(false);
        }
        m_Initialized = true;
    }
}
