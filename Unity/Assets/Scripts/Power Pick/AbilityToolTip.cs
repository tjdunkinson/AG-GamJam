using UnityEngine;
using System.Collections;

public class AbilityToolTip : MonoBehaviour
{
	[SerializeField]
	private string m_name;
	
	[SerializeField]
	public string m_description;

	public bool m_selected = false;
	public bool m_hover = false;
	private Vector3 m_defaultSize;
	private Vector3 m_maxSize;

	private PowerPick m_parentPowerPick;

	// Use this for initialization
	void Start () 
	{
		m_defaultSize = this.transform.localScale;
		m_maxSize = this.transform.localScale * 1.25f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_selected)
			this.renderer.material.SetFloat("_BWtoColor", 1);
        else
            this.renderer.material.SetFloat("_BWtoColor", 0);
        if (m_hover)
            OnHover();
        else
            NormalSize();
	}

	public void OnHover()
	{	
		if(!m_selected)
			transform.localScale = Vector3.Lerp(transform.localScale, m_maxSize, Time.deltaTime * 15);
	}

	void NormalSize()
	{
		transform.localScale = m_defaultSize;
	}

    public void Drop()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -50, transform.position.z), Time.deltaTime * Random.Range(.5f,1));
    }
}
