using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{
	[SerializeField] Text m_text;

	float m_resistance;
	SpriteRenderer m_renderer;

	private void Awake()
	{
		m_renderer = GetComponent<SpriteRenderer>();
	}

	public void SetResistance(float _value)
	{
		m_resistance = Mathf.Clamp(_value, 0, 100);

		int clampedResistance = Mathf.CeilToInt(m_resistance);
		float colorValue = m_resistance / 100;


		m_renderer.color = new Color(colorValue, 1 - colorValue, 0);
		m_text.text = clampedResistance.ToString();
	}

	

	private void OnCollisionStay(Collision collision)
	{
		SetResistance(m_resistance - Time.deltaTime);
	}
}
