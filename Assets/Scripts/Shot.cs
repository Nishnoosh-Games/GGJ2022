using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
    [Header("Projectile Speed")]
	[SerializeField] private FloatRef m_weaponCurrentSpeed;
	[SerializeField] private RectRef m_boardBorder;

	void Update(){
		transform.Translate(transform.InverseTransformDirection(transform.right) * m_weaponCurrentSpeed * Time.deltaTime);

		/*if(transform.position.y > Camera.main.orthographicSize || 
			transform.position.y < Camera.main.orthographicSize * -1 ||
			transform.position.x > Camera.main.orthographicSize * Camera.main.aspect ||
			transform.position.x < Camera.main.orthographicSize * Camera.main.aspect * -1){
				TrashMan.despawn(gameObject);
		}*/
		if(transform.position.x < m_boardBorder.value.xMin ||
			transform.position.x > m_boardBorder.value.xMax ||
			transform.position.y < m_boardBorder.value.yMin ||
			transform.position.y > m_boardBorder.value.yMax){
				TrashMan.despawn(gameObject);
			}
	}
}
