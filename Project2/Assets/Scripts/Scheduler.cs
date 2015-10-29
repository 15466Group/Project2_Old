using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scheduler : MonoBehaviour {
	
	public GameObject goal;
	public GameObject characters; //empty gameobject containing children of in game characters

	public GameObject staticObstacles;
	public GameObject dynamicObstacles;

	private Grid G;
	private Graph graph;
	private ReachGoal reachGoal;

	//each soldier has complete control for one frame 
	private int iChar;
	private int numChars;

	private float timer;
	private float searchTime;

	// Use this for initialization
	void Start () {
		iChar = 0;
		numChars = characters.transform.childCount;
		Debug.Log ("numChars: " + numChars);

		timer = 0.0f;
		searchTime = 2.0f;

		G = GetComponent<Grid> ();
		G.initStart ();
		graph = new Graph (G);
		for (int i = 0; i < numChars; i++) {
			Transform child = characters.transform.GetChild(i);
			reachGoal = child.GetComponent<ReachGoal> ();
			reachGoal.Start();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//update grid
		//update graph
		//feed graph to character CharList[iChar]
		//every char is moving at each frame, but at frame i % numChars,
		//	char i is moving according to new graph and everyone else is moving according to their 'current' graphs
		Transform currChar = characters.transform.GetChild (iChar);
		iChar = (iChar + 1) % numChars;
		reachGoal = currChar.GetComponent<ReachGoal> ();
		Vector3 start = currChar.transform.position;
		Vector3 end = goal.transform.position;
		List<Node> path = reachGoal.tempPositions;
		reachGoal.assignGridCoords (graph.g.getGridCoords(reachGoal.next), 
		                            graph.g.getGridCoords(currChar.transform.position),
		                            graph.g.getGridCoords(goal.transform.position));
//		G.updateGrid ();
//		graph = new Graph(G);
		graph.g.updateGrid ();
		timer += Time.deltaTime;
		reachGoal.assignedPath (graph.getPath (start, end, reachGoal.swampCost));
//		timer = 0.0f;
//		if (timer >= searchTime || path.Count == 0) {
//			reachGoal.assignedPath (graph.getPath (start, end));
//			timer = 0.0f;
//		}
		reachGoal.nextStep ();
	}
}
