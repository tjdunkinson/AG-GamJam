using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public partial class PowerPick : MonoBehaviour 
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
    private float m_pickTimer;

    private int m_currentPlayerSelection = -1;

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

        InitTextElements();
		m_noOfAbility = m_abilityList.Count - 1;
		m_abilityList.Sort((AbilityToolTip x, AbilityToolTip y) => {return x.name.CompareTo(y.name);});
    }
	
	// Update is called once per frame
	void Update () 
	{
        InputManager.Update();
        m_pickTimer -= Time.deltaTime;

        if (m_Initialized && m_draftRound != 4)
        {
            ManageDraft();
        }
        else if (m_draftRound == 4)
        {
            DraftComplete();
        }
        else
        {
            InitializePlayers();
        }

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
            m_players[i].SetName("Player " + (i + 1));
            m_players[i].AttachControler(InputManager.Devices[i]);

            if(i == 0)
                m_players[i].setColor(Color.red);
            if (i == 1)
                m_players[i].setColor(Color.green); 
            if (i == 2)
                m_players[i].setColor(Color.yellow);
            if (i == 3)
                m_players[i].setColor(Color.blue);
        }

        //disables inactive player UI elements
        for (int i = InputManager.Devices.Count; i < 4; i++)
        {
            GameObject a = GameObject.Find("P" + (i + 1).ToString() + "Anchor");
            a.gameObject.SetActive(false);
        }

        //creates a random order for the players to draft in.
        for (int i = 0; i < m_players.Count; i++)
        {
            PowerPickPlayer temp = m_players[i];
            int randomIndex = Random.Range(i, m_players.Count);
            m_players[i] = m_players[randomIndex];
            m_players[randomIndex] = temp;
        }
        
        m_pickTimer = 5;

        m_Initialized = true;
    }

    void ManageDraft()
    {
        selectionInput();
        UpdateTextsElements();
        PlayerHasPicked();
    }

    void DraftComplete()
    {
        for (int i = 0; i < m_abilityList.Count; i++)
            if(m_abilityList[i].m_selected != true)
                m_abilityList[i].Drop();
    }

    void PlayerHasPicked()
    {
        if (m_currentPlayerSelection >= 0)
        {
            m_pickTimer = 5;
            //greys ability out
            m_abilityList[m_currentPlayerSelection].m_selected = true;
            m_abilityList[m_currentPlayerSelection].changeColour(m_players[m_currentPlayer].GetColor());
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
            //m_abilityList[m_currentSelection].changeColour(m_players[m_currentPlayer].GetColor());
        }
    }
}
