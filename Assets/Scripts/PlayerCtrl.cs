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

	[SerializeField] private GameObject m_projectilePrefab;

	[Header("Fire Rate")]
	[SerializeField] private FloatRef m_weaponBaseFireRate;
	[SerializeField] private FloatRef m_weaponCurrentFireRate;
	[SerializeField] private FloatRef m_weaponMaxFireRate;

	[Header("Fire Lanes")]
	[SerializeField] private FireLane[] m_firelanes;
	private int m_currentFireLane = 3;
	/*[SerializeField] private IntRef m_weaponBaseFireLanes;
	[SerializeField] private IntRef m_weaponCurrentFireLanes;
	[SerializeField] private IntRef m_weaponMaxFireLanes;*/

	[Header("Projectile Speed")]
	[SerializeField] private FloatRef m_weaponBaseSpeed;
	[SerializeField] private FloatRef m_weaponCurrentSpeed;
	[SerializeField] private FloatRef m_weaponMaxSpeed;

	private float m_lastFire;

	void Start(){
		m_playerCurrentSpeed.SetValue(m_playerBaseSpeed.value);
		m_weaponCurrentFireRate.SetValue(m_weaponBaseFireRate);
	}

	void Update(){
		GetKeyMove();
		LimitMove();
		GetMouseAim();
		if(Input.GetMouseButton(0)){
			Fire();
		}
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
		transform.position = new Vector3(x, y, -2);
	}

	private void GetMouseAim(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 aimDir = (mousePos - transform.position).normalized;
		float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

		m_aimTransform.transform.eulerAngles = new Vector3(0, 0, angle);
	}

	private void Fire(){
		if(Time.time >= m_lastFire + m_weaponCurrentFireRate){
			m_lastFire = Time.time;
			Debug.Log("Fire");
			for(int i = 0; i < m_firelanes[m_currentFireLane].muzzles.Length; i++){
				TrashMan.spawn(m_projectilePrefab, m_firelanes[m_currentFireLane].muzzles[i].position, m_firelanes[m_currentFireLane].muzzles[i].rotation);
			}
		}
	}
}
