using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carriage : MonoBehaviour
{
	[Header("Carriage Property")]
	[SerializeField] float m_scrollSpeed;

    // Update is called once per frame
    void Update()
    {
		transform.position += Vector3.up * m_scrollSpeed * Time.deltaTime;
    }

	public float GetScrollSpeed()
	{
		return m_scrollSpeed;
	}
}
