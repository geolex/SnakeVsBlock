using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
	[Header("Movement Properties")]
	[SerializeField] float m_slideSpeed;

	[Header("Play Area")]
	[SerializeField] float m_leftBound;
	[SerializeField] float m_rightBound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 buffer = transform.position;

		buffer.x = Mathf.Clamp(buffer.x + Input.GetAxis("Horizontal") * m_slideSpeed * Time.deltaTime, m_leftBound, m_rightBound);

		transform.position = buffer;
    }
}
