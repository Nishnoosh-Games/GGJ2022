using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLane : MonoBehaviour{
    [SerializeField] private Transform[] m_muzzles;

	public Transform[] muzzles{
		get{
			return m_muzzles;
		}
	}
}
