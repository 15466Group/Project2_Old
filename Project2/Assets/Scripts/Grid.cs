using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public float nodeSize;
	public GameObject goal;
	public GameObject plane;

	private int obstacleLayer;
	private int swampLayer;
	private int goalLayer;
	private int dynamicLayer;

	public Node[,] grid;
	private int gridWidth;
	private int gridHeight;
	private float worldWidth;
	private float worldHeight;
	private Vector3 worldNW; //world north west, top left corner of map/plane


//	public GameObject staticObj;
//	public GameObject dynamicObj;
	public GameObject swamps;

	// Use this for initialization
	public void initStart () {
		worldWidth = plane.transform.lossyScale.x * 10.0f; //plane
		worldHeight = plane.transform.lossyScale.z * 10.0f; //plane

		gridWidth = Mathf.RoundToInt(worldWidth / nodeSize);
		gridHeight = Mathf.RoundToInt(worldHeight / nodeSize);

		grid = new Node[gridWidth, gridHeight];

		worldNW = plane.transform.position - (plane.transform.right * worldWidth / 2.0f) + (plane.transform.forward * worldHeight / 2.0f);

		obstacleLayer = 1 << LayerMask.NameToLayer ("Obstacles");
		swampLayer = 1 << LayerMask.NameToLayer ("Swamp");
		goalLayer = 1 << LayerMask.NameToLayer ("Goal");
		dynamicLayer = 1 << LayerMask.NameToLayer ("Dynamic");

		initializeGrid ();
		updateGrid ();


	}
	
	// Update is called once per frame
//	void Update () {
//		updateGrid ();
//	}

	Node[,] getGrid() {
		return grid;
	}


//	public void initializeGrid() {
////		Debug.Log ("FACK");
//		bool isGoal = false;
//		for (int i = 0; i < gridWidth; i++) {
//			for(int j = 0; j < gridWidth; j++) {
//				float xp = i * nodeSize + (nodeSize/2.0f) + worldNW.x;
//				float zp = -(j * nodeSize + (nodeSize/2.0f)) + worldNW.z;
//				Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
//				float h = Vector3.Distance(nodeCenter, goal.transform.position);
//				grid[i,j] = new Node(true, nodeCenter, isGoal, i, j, h);
//			}
//		}
//
//		int staticCount = staticObj.transform.childCount;
//		for (int i = 0; i < staticCount; i++) {
//			GameObject obj = staticObj.transform.GetChild (i).gameObject;
//			int[] bounds = getOccupiedBounds (obj);
//			for (int j = bounds[0]; j < bounds[1]; j++) {
//				for(int k = bounds[2]; k < bounds[3]; k++) {
//					float xp = j * nodeSize + (nodeSize/2.0f) + worldNW.x;
//					float zp = -(k * nodeSize + (nodeSize/2.0f)) + worldNW.z;
//					Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
//					Collider[] hits = Physics.OverlapSphere(nodeCenter, nodeSize/2.0f, obstacleLayer);
//					//bool isGoal = checkIfContainsGoal(hits);
//					float h = Vector3.Distance(nodeCenter, goal.transform.position);
//					if(hits.Length == 0) {
//						grid[j,k] = new Node(true, nodeCenter, isGoal, j, k, h);
//					}
//					else {
//						grid[j,k] = new Node(false, nodeCenter, isGoal, j, k, h);
//					}
//				}
//			}
//		}
//		Vector3 goalGridCoords = getGridCoords (goal.transform.position);
//		grid [(int) goalGridCoords.x, (int) goalGridCoords.z].isGoal = true;
//	}
//
//	public void updateGrid() {
//		bool isGoal = false;
//		int dynamicCount = dynamicObj.transform.childCount;
//		for (int i = 0; i < dynamicCount; i++) {
//			GameObject obj = dynamicObj.transform.GetChild (i).gameObject;
//			int[] bounds = getOccupiedBounds (obj);
//			for (int j = bounds[0]; j < bounds[1]; j++) {
//				for(int k = bounds[2]; k < bounds[3]; k++) {
//					float xp = j * nodeSize + (nodeSize/2.0f) + worldNW.x;
//					float zp = -(k * nodeSize + (nodeSize/2.0f)) + worldNW.z;
//					Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
//					Collider[] hits = Physics.OverlapSphere(nodeCenter, nodeSize/2.0f, obstacleLayer);
//					//bool isGoal = checkIfContainsGoal(hits);
//					float h = Vector3.Distance(nodeCenter, goal.transform.position);
//					if(hits.Length == 0) {
//						grid[j,k] = new Node(true, nodeCenter, isGoal, j, k, h);
//					}
//					else {
//						grid[j,k] = new Node(false, nodeCenter, isGoal, j, k, h);
//					}
//				}
//			}
//		}
//		Vector3 goalGridCoords = getGridCoords (goal.transform.position);
//		grid [(int) goalGridCoords.x, (int) goalGridCoords.z].isGoal = true;
//	}
//
//	int[] getOccupiedBounds(GameObject thingy) {
//		//stores the bounds as [minX, maxX, minZ, maxZ]
//		int[] bounds = new int[4];
//		Collider col = thingy.GetComponent <Collider>();
//		Vector3 max = col.bounds.max;
//		Vector3 min = col.bounds.min;
//		Vector3 gridCoordMax = getGridCoords (max);
//		Vector3 gridCoordMin = getGridCoords (min);
//
//		bounds [0] = (int) gridCoordMin.x;
//		bounds [1] = (int) gridCoordMax.x;
//		bounds [2] = (int) gridCoordMax.z;
//		bounds [3] = (int) gridCoordMin.z;
//		return bounds;
//
//	}

	public void initializeGrid(){
		//		Debug.Log ("old update");
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j ++) {
				float xp = i * nodeSize + (nodeSize/2.0f) + worldNW.x;
				float zp = -(j * nodeSize + (nodeSize/2.0f)) + worldNW.z;
				Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
				Collider[] hits = Physics.OverlapSphere(nodeCenter, nodeSize/2.0f, obstacleLayer | swampLayer | goalLayer | dynamicLayer); // | goalLayer
				bool isGoal = checkIfContainsTag(hits, "Player");
				float h = Vector3.Distance(nodeCenter, goal.transform.position);
				int len = hits.Length;
				if(len == 0) { //|| (len == 1 && isGoal)
					grid[i,j] = new Node(true, nodeCenter, isGoal, i, j, h, false);
				}
				else {
					bool isSwamp = checkIfContainsTag(hits, "Swamp");
					bool free;
					if (isSwamp && (len == 1 || (len == 2 && isGoal))){
						free = true;
					}
					else {
						free = false;
					}
					grid[i,j] = new Node(free, nodeCenter, isGoal, i, j, h, isSwamp);
//					if (isSwamp)
//						Debug.Log ("(" + i + ", " + j + ")"); 
				}
			}
		}
		if (swamps != null) {
			int swampCount = swamps.transform.childCount;
			for (int k = 0; k < swampCount; k++) {
				swamps.transform.GetChild (k).GetComponent<MeshCollider> ().enabled = false;
			}
		}
	}
	
	public void updateGrid(){
//		Debug.Log ("old update");
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j ++) {
				float xp = i * nodeSize + (nodeSize/2.0f) + worldNW.x;
				float zp = -(j * nodeSize + (nodeSize/2.0f)) + worldNW.z;
				Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
				Collider[] hits = Physics.OverlapSphere(nodeCenter, nodeSize/2.0f, obstacleLayer | swampLayer | goalLayer | dynamicLayer); // | goalLayer
				bool isGoal = checkIfContainsTag(hits, "Player");
				float h = Vector3.Distance(nodeCenter, goal.transform.position);
				int len = hits.Length;
				if(len == 0) { //|| (len == 1 && isGoal)
					grid[i,j] = new Node(true, nodeCenter, isGoal, i, j, h, grid[i,j].isSwamp);
				}
				else {
					bool isSwamp = checkIfContainsTag(hits, "Swamp");
					bool free;
					if (isSwamp && (len == 1 || (len == 2 && isGoal))){
						free = true;
					}
					else {
						free = false;
					}
					grid[i,j] = new Node(free, nodeCenter, isGoal, i, j, h, grid[i,j].isSwamp);
				}
			}
		}
	}

//	void OnDrawGizmos() {
//		for (int i = 0; i < gridWidth; i++) {
//			for (int j = 0; j < gridHeight; j ++) {
//				Gizmos.color = Color.red;
//				if (grid[i,j].isGoal){
//					Gizmos.color = Color.green;
//					Gizmos.DrawCube (grid [i, j].loc, new Vector3 (nodeSize, 1.0f, nodeSize));
//				}
//				if (!grid[i,j].free) {
//					Gizmos.DrawCube (grid [i, j].loc, new Vector3 (nodeSize, 1.0f, nodeSize));
//				}
//			}
//		}
//	}

	bool checkIfContainsTag(Collider[] hits, string tag){
		bool foundTag = false;
		foreach (Collider hit in hits) {
			if (hit.CompareTag(tag)){
				foundTag = true;
				break;
			}
		}
		return foundTag;
	}

	public Vector3 getGridCoords(Vector3 location) {
		float newx = location.x + worldWidth / 2.0f;
		float newz = -location.z + worldHeight / 2.0f;

		int i = (int)(newx / nodeSize);
		int j = (int)(newz / nodeSize);

		if (i < 0)
			i = 0;
		if (i > gridWidth)
			i = gridWidth - 1;
		if (j < 0)
			j = 0;
		if (j > gridHeight)
			j = gridHeight - 1;

		return new Vector3(i, 0.0f, j);
	}


}
