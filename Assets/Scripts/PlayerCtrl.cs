using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
	private float m_horizontalMove;
	private float m_verticalMove;

	[SerializeField] private Transform m_aimTransform;

	[SerializeField] private FloatRef m_playerBaseSpeed;
	[SerializeField] private FloatRef m_playerCurrentSpeed;
	[SerializeField] private FloatRef m_playerMaxSpeed;

	[SerializeField] private RectRef m_boardBorders;

	void Start(){
		m_playerCurrentSpeed.SetValue(m_playerBaseSpeed.value);
	}

	void Update(){
		GetKeyMove();
		LimitMove();
		GetMouseAim();
	}

	private void GetKeyMove(){
		m_horizontalMove = 0;
		m_verticalMove = 0;
		if(Input.GetKey(KeyCode.A))	m_horizontalMove -= 1;
		if(Input.GetKey(KeyCode.D))	m_horizontalMove += 1;
		if(Input.GetKey(KeyCode.W)) m_verticalMove += 1;
		if(Input.GetKey(KeyCode.S)) m_verticalMove -= 1;

		float xSpeed = m_playerCurrentSpeed.value * m_horizontalMove;
		float ySpeed = m_playerCurrentSpeed.value * m_verticalMove;
		transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);
	}

	private void LimitMove(){
		float x = transform.position.x;
		float y = transform.position.y;
		if(transform.position.x + 0.5f > m_boardBorders.value.xMax){
			x = m_boardBorders.value.xMax - 0.5f;
		}else if(transform.position.x - 0.5f < m_boardBorders.value.xMin){
			x = m_boardBorders.value.xMin + 0.5f;
		}

		if(transform.position.y + 0.5f > m_boardBorders.value.yMax){
			y = m_boardBorders.value.yMax - 0.5f;
		}else if(transform.position.y - 0.5f < m_boardBorders.value.yMin){
			y = m_boardBorders.value.yMin + 0.5f;
		}
		//x = transform.position.x + 0.5f > m_boardBorders.value.xMax ? m_boardBorders.value.xMax - 0.5f : transform.position.x;
		//x = transform.position.x - 0.5f < m_boardBorders.value.xMin ? m_boardBorders.value.xMin + 0.5f : transform.position.x;
		//y = transform.position.y + 0.5f < m_boardBorders.value.yMax ? m_boardBorders.value.yMax - 0.5f : transform.position.y;
		//y = transform.position.y - 0.5f > m_boardBorders.value.yMin ? m_boardBorders.value.yMin + 0.5f : transform.position.y;
		//if(transform.position.x + 0.5f > m_boardBorders.value.xMax) x = m_boardBorders.value.xMax - 0.5f;
		//if(transform.position.x - 0.5f < m_boardBorders.value.xMin) x = m_boardBorders.value.xMin + 0.5f;
		//if(transform.position.y + 0.5f > m_boardBorders.value.yMax) y = m_boardBorders.value.yMax - 0.5f;
		transform.position = new Vector3(x, y, -2);
	}

	private void GetMouseAim(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 aimDir = (mousePos - transform.position).normalized;
		float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

		m_aimTransform.transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
