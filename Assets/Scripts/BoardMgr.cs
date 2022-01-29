using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMgr : MonoBehaviour {
    [SerializeField] private GameObject m_tilePrefab;
	[SerializeField] private IntRef m_worldBaseSize;
	[SerializeField] private IntRef m_worldMaxSize;
	[SerializeField] private IntRef m_worldCurrentSize;
	[SerializeField] private IntRef m_worldSizeIncrease;
	[SerializeField] private RectRef m_boardBorders;

	[SerializeField] private float m_tilesSlideInSpeed;
	[SerializeField] private float m_cameraZoomOutSpeed;
	[SerializeField] private AnimationCurve m_cameraZoomOutCurve;

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
		
		m_boardBorders.SetValue(new Rect(transform.position.x - m_worldCurrentSize.value / 2f, transform.position.y - m_worldCurrentSize.value / 2f, m_worldCurrentSize, m_worldCurrentSize));
	}

	public void IncreaseBoard(){
		int diff;
		if(m_worldCurrentSize + (m_worldSizeIncrease * 2) <= m_worldMaxSize){
			diff = m_worldSizeIncrease;
			m_worldCurrentSize.SetValue(m_worldCurrentSize.value + (m_worldSizeIncrease.value * 2));
		}else{
			diff = m_worldMaxSize - m_worldCurrentSize;
			if(diff % 2 != 0 || diff / 2 == 0) return;
			m_worldCurrentSize.SetValue(m_worldMaxSize);
		}

		GameObject[,] newBoard = new GameObject[m_worldCurrentSize, m_worldCurrentSize];
		List<GameObject> newTiles = new List<GameObject>();
		List<Vector3> newTilesTargetPos = new List<Vector3>();
		List<Vector3> newTilesCurrentPos = new List<Vector3>();
		Vector3 m_startPosition = new Vector3((m_worldCurrentSize - 1) / 2f * -1, (m_worldCurrentSize - 1) / 2f, 0);
		Vector3 tilePosition;
		for(int i = 0; i < m_worldCurrentSize; i++){
			for(int j = 0; j < m_worldCurrentSize; j++){
				if((i < diff || i > m_worldCurrentSize - 1 - diff) || (j < diff || j > m_worldCurrentSize - 1 - diff)){
					tilePosition = m_startPosition + new Vector3(i, -j ,0);
					GameObject newTile = Instantiate<GameObject>(m_tilePrefab, tilePosition * 5, Quaternion.identity, this.transform);
					newTile.name = $"Tile_{i}x{j}";
					newBoard[i, j] = newTile;
					newTiles.Add(newTile);
					newTilesTargetPos.Add(tilePosition);
					newTilesCurrentPos.Add(tilePosition * 5);
				}else{
					newBoard[i, j] = m_tiles[i - diff, j - diff];
					newBoard[i, j].name = $"Tile_{i}x{j}";
					newBoard[i, j].transform.position = m_startPosition + new Vector3(i, -j ,0);
				}
			}
		}
		m_tiles = newBoard;
		//StartCoroutine(SlideTilesIn(newTiles, newTilesCurrentPos, newTilesTargetPos));
		StartCoroutine(SlideAndZoom(newTiles, newTilesCurrentPos, newTilesTargetPos));
	}

	void Start(){
		BuildBoard();
	}

	/*void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			IncreaseBoard();
			/*if(m_worldCurrentSize > Camera.main.orthographicSize * 2){
				StartCoroutine(ZoomOutCamera(m_worldCurrentSize/2));
			}*
		}
	}*/

	IEnumerator SlideTilesIn(List<GameObject> tiles, List<Vector3> currentPositions, List<Vector3> targetPositions){
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(currentPositions[0], targetPositions[0]);
		float fractionOfJourney = 0;
        while(fractionOfJourney < 1){
			float distCovered = (Time.time - startTime) * m_tilesSlideInSpeed;
			fractionOfJourney = distCovered / journeyLength;
			for(int i = 0; i < tiles.Count; i++){
				tiles[i].transform.position = Vector3.Lerp(currentPositions[i], targetPositions[i], fractionOfJourney);
			}
			yield return null;
		}
	}

	IEnumerator ZoomOutCamera(float targetSize){
		print(targetSize);
		float startTime = Time.time;
		float currentSize = Camera.main.orthographicSize;
		//float targetSize = Camera.main.orthographicSize + (m_worldSizeIncrease * 2);
		float journeyLength = targetSize - currentSize;
		float fractionOfJourney = 0;
		while(fractionOfJourney < 1){
			float distCovered = (Time.time - startTime) * m_cameraZoomOutSpeed;
			fractionOfJourney = distCovered / journeyLength;
			Camera.main.orthographicSize = Mathf.LerpUnclamped(currentSize, targetSize, m_cameraZoomOutCurve.Evaluate(fractionOfJourney));
			yield return null;
		}
	}

	IEnumerator SlideAndZoom(List<GameObject> tiles, List<Vector3> currentPositions, List<Vector3> targetPositions){
		float startTime = Time.time;
        float journeyLength = Vector3.Distance(currentPositions[0], targetPositions[0]);
		float currentCamSize = Camera.main.orthographicSize;
		float fractionOfJourney = 0;

		bool resizeCamera = false;
		float targetCamSize = 0;
		if(m_worldCurrentSize >= (Camera.main.orthographicSize * 2)){
			resizeCamera = true;
			targetCamSize = m_worldCurrentSize/2 + 1;
		}
        while(fractionOfJourney < 1){
			float distCovered = (Time.time - startTime) * m_tilesSlideInSpeed;
			fractionOfJourney = distCovered / journeyLength;
			for(int i = 0; i < tiles.Count; i++){
				tiles[i].transform.position = Vector3.Lerp(currentPositions[i], targetPositions[i], fractionOfJourney);
			}
			
			if(resizeCamera){
				Camera.main.orthographicSize = Mathf.LerpUnclamped(currentCamSize, targetCamSize, m_cameraZoomOutCurve.Evaluate(fractionOfJourney));
			}
			yield return null;
		}
		m_boardBorders.SetValue(new Rect(transform.position.x - m_worldCurrentSize.value / 2f, transform.position.y - m_worldCurrentSize.value / 2f, m_worldCurrentSize, m_worldCurrentSize));
	}
}
