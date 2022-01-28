using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMgr : MonoBehaviour {
    [SerializeField] private GameObject m_tilePrefab;
	[SerializeField] private IntRef m_worldBaseSize;
	[SerializeField] private IntRef m_worldMaxSize;
	[SerializeField] private IntRef m_worldCurrentSize;
	[SerializeField] private IntRef m_worldSizeIncrease;

	private GameObject[,] m_tiles;

	public void BuildBoard(){
		m_worldCurrentSize.SetValue(m_worldBaseSize.value);

		m_tiles = new GameObject[m_worldCurrentSize, m_worldCurrentSize];
		Vector3 m_startPosition = new Vector3((m_worldCurrentSize - 1) / 2f * -1, (m_worldCurrentSize - 1) / 2f, 0);
		Vector3 tilePosition;
		for(int i = 0; i < m_worldCurrentSize.value; i++){
			for(int j = 0; j < m_worldCurrentSize.value; j++){
				tilePosition = m_startPosition + new Vector3(i, -j ,0);
				GameObject newTile = Instantiate<GameObject>(m_tilePrefab, tilePosition, Quaternion.identity, this.transform);
				newTile.name = $"Tile_{i}x{j}";
				m_tiles[i, j] = newTile;
			}
		}
		
	}

	public void IncreaseBoard(){
		int diff;
		if(m_worldCurrentSize + (m_worldSizeIncrease * 2) < m_worldMaxSize){
			diff = m_worldSizeIncrease;
			m_worldCurrentSize.SetValue(m_worldCurrentSize.value + (m_worldSizeIncrease.value * 2));
		}else{
			diff = m_worldMaxSize - m_worldCurrentSize;
			if(diff % 2 != 0) return;
			m_worldCurrentSize = m_worldMaxSize;
		}

		GameObject[,] newBoard = new GameObject[m_worldCurrentSize, m_worldCurrentSize];
		List<GameObject> newTiles = new List<GameObject>();
		Vector3 m_startPosition = new Vector3((m_worldCurrentSize - 1) / 2f * -1, (m_worldCurrentSize - 1) / 2f, 0);
		Vector3 tilePosition;
		for(int i = 0; i < m_worldCurrentSize; i++){
			for(int j = 0; j < m_worldCurrentSize; j++){
				if((i < diff || i > m_worldCurrentSize - 1 - diff) || (j < diff || j > m_worldCurrentSize - 1 - diff)){
					tilePosition = m_startPosition + new Vector3(i, -j ,0);
					GameObject newTile = Instantiate<GameObject>(m_tilePrefab, tilePosition, Quaternion.identity, this.transform);
					newTile.name = $"Tile_{i}x{j}";
					newBoard[i, j] = newTile;
				}else{
					newBoard[i, j] = m_tiles[i - diff, j - diff];
					newBoard[i, j].name = $"Tile_{i}x{j}";
					newBoard[i, j].transform.position = m_startPosition + new Vector3(i, -j ,0);
				}
			}
		}
		m_tiles = newBoard;
	}

	void Start(){
		BuildBoard();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			IncreaseBoard();
		}
	}
}
