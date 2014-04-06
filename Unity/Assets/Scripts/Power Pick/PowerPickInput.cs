using UnityEngine;
using System.Collections;
using InControl;

public partial class PowerPick
{
    //input stuff

    /// <summary>
    /// Controls all the movement and selection around the board.
    /// </summary>
	
    void selectionInput()
    {
        //creates a delay between movements so we dont have teleportation due to joysticks repeating.
        m_moveTimer += Time.deltaTime;
        m_abilityList[m_currentSelection].changeColour(m_players[m_currentPlayer].GetColor());
        //Grab the values of the left joystick for the current player, invert Y.
        float xAxis = m_players[m_currentPlayer].GetController().LeftStickX;
        float yAxis = -m_players[m_currentPlayer].GetController().LeftStickY;

        //Make note of the original selection
        int originalSelect = m_currentSelection;

        //Will move the selection up.
        if (yAxis < -0.5f && m_moveTimer > 0.2f)
        {
            //minus the width of the board
            if (!(m_currentSelection - m_width < 0))
                m_currentSelection -= m_width;


            ///handles multiple tiles above it being already selected.
            ///will jump to the next one above it, or the next on to the right.
            while (m_abilityList[m_currentSelection].m_selected)
            {
                m_currentSelection -= m_width;
                if (m_currentSelection < 0)
                    m_currentSelection = FirstAvailableNode();
            }
            //reset movetime
            m_moveTimer = 0;
        }

        //Moves the selection down
        if (yAxis > 0.5f && m_moveTimer > 0.2f)
        {
            //Add on the width of the board
            if (!(m_currentSelection + m_width > m_noOfAbility))
                m_currentSelection += m_width;
            else
                m_currentSelection = LastAvailableNode();
            //handles movement over multiple selected tiles, move to the right if blocked.
            while (m_abilityList[m_currentSelection].m_selected)
            {
                m_currentSelection += m_width;
                if (m_currentSelection > m_noOfAbility)
                    m_currentSelection = NextAvailableNode(m_currentSelection - m_width);
            }
            //reset time
            m_moveTimer = 0;
        }

        //moves to the left on the board
        if (xAxis < -0.5f && m_moveTimer > 0.2f)
        {
            //make note of current height
            int currentHeight = m_currentSelection / m_width;

            //if not on the top left corner of the board
            if (!(m_currentSelection - 1 < 0))
            {
                m_currentSelection--;
                int newHeight = m_currentSelection / m_width;
                //will stop it wrapping around to next level
                if (newHeight < currentHeight)
                    m_currentSelection++;
            }

            //will move to the next available previous node
            if (m_abilityList[m_currentSelection].m_selected)
                m_currentSelection = PreviousAvailableNode(m_currentSelection);

            //reset the time
            m_moveTimer = 0;
        }

        //inverse of Left movement
        if (xAxis > 0.5f && m_moveTimer > 0.2f)
        {
            int currentHeight = m_currentSelection / m_width;

            if (!(m_currentSelection + 1 > m_noOfAbility))
            {
                m_currentSelection++;
                int newHeight = m_currentSelection / m_width;
                if (newHeight > currentHeight)
                    m_currentSelection--;
            }

            if (m_abilityList[m_currentSelection].m_selected)
                m_currentSelection = NextAvailableNode(m_currentSelection);

            m_moveTimer = 0;
        }

        //if player selects this ability set its hover to false, and set as selection.
        if (m_players[m_currentPlayer].GetController().Action1)
        {
            m_abilityList[m_currentSelection].m_hover = false;
            m_currentPlayerSelection = m_currentSelection;
        }

        //Will reset hover on previous ability tile if its different from current.
        int newSelection = m_currentSelection;
        if (newSelection != originalSelect)
        {
            m_abilityList[originalSelect].changeColour(Color.white);
            m_abilityList[originalSelect].m_hover = false;
            m_abilityList[newSelection].m_hover = true;
            m_abilityList[newSelection].changeColour(m_players[m_currentPlayer].GetColor());
        }

        if (m_pickTimer < 0)
        {
            m_abilityList[m_currentSelection].m_hover = false;
            m_abilityList[m_currentSelection].changeColour(Color.white);
            m_currentPlayerSelection = Random.Range(0, m_abilityList.Count);
            while(m_abilityList[m_currentPlayerSelection].m_selected == true)
                m_currentPlayerSelection = Random.Range(0, m_abilityList.Count);

        }

    }

    /// <summary>
    /// returns the index of the next closest available index in the ability board
    /// </summary>
    /// <param name="a_current"></param>
    /// <returns></returns>
    int NextAvailableNode(int a_current)
    {
        for (int i = a_current; i <= m_noOfAbility; i++)
            if (m_abilityList[i].m_selected == false)
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
        for (int i = a_current; i > FirstAvailableNode(); i--)
            if (m_abilityList[i].m_selected == false)
                return i;

        return 0;
    }

    /// <summary>
    /// Returns the index of the first available index in the ability board
    /// </summary>
    /// <returns></returns>
    int FirstAvailableNode()
    {
        for (int i = 0; i < m_abilityList.Count; i++)
            if (m_abilityList[i].m_selected == false)
                return i;

        return 0;
    }

    /// <summary>
    /// returns the index of the last available ability in the draft board
    /// </summary>
    /// <returns></returns>
    int LastAvailableNode()
    {
        for (int i = m_abilityList.Count - 1; i > 0; i--)
            if (m_abilityList[i].m_selected == false)
                return i;

        return 0;
    }

}