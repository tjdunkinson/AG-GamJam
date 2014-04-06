using UnityEngine;
using System.Collections;

public partial class PowerPick
{
    private TextMesh m_abilityCurrentPlayer;
    private TextMesh m_abilityPickTime;
    private TextMesh m_abilityName;
    private TextMesh m_abilityDesc;
    private TextMesh m_startGameTimerText;

    void InitTextElements()
    { 
        m_abilityCurrentPlayer = (TextMesh)GameObject.Find("PowerPickCurrentPlayer").GetComponent("TextMesh");
        m_abilityPickTime = (TextMesh)GameObject.Find("PowerPickPickTime").GetComponent("TextMesh");
        m_abilityName = (TextMesh)GameObject.Find("PowerPickName").GetComponent("TextMesh");
        m_abilityDesc = (TextMesh)GameObject.Find("PowerPickInfo").GetComponent("TextMesh");
        m_startGameTimerText = (TextMesh)GameObject.Find("StartGameTimerText").GetComponent("TextMesh");
    }

    void UpdateTextsElements()
    {
        m_abilityCurrentPlayer.text = m_players[m_currentPlayer].GetName() + "S Turn To Pick";
        m_abilityPickTime.text = "Time To Pick: " + m_pickTimer.ToString("0.");

        m_abilityName.text = m_abilityList[m_currentSelection].name;
        m_abilityDesc.text = m_abilityList[m_currentSelection].m_description;
    }
}