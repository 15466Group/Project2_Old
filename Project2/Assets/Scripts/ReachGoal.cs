using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReachGoal: NPCBehaviour {

	public GameObject goal;
	private Vector3 endTarget;
	private float timer;
	private float searchTime;
	private bool hitNextNode;

	public List<Node> tempPositions;
	public Vector3 next;
	public Vector3 nextCoords;
	public Vector3 transCoords;
	public Vector3 endCoords;
	public float swampCost;
	private LayerMask dynamicLayer;

	private float arrivalRadius;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		dynamicLayer = 1 << LayerMask.NameToLayer ("Dynamic");
		endTarget = goal.transform.position;
		acceleration = base.calculateAcceleration (target);
		isWanderer = false;
		isReachingGoal = true;
		hitNextNode = true;
		next = transform.position;
		nextCoords = next;
		transCoords = next;
		endCoords = new Vector3 (next.x + 10.0f, 0.0f, next.z + 10.0f);
		tempPositions = new List<Node> ();
		inArrivalRadius = false;
		arrivalRadius = 20.0f;
	}

	public void nextStep () {
//		for(int i = 0; i < tempPositions.Count - 1; i++) {
//			Debug.DrawLine (tempPositions[i].loc, tempPositions[i+1].loc, Color.yellow);
//		}
		endTarget = goal.transform.position;
		target = nextTarget();
		checkArrival ();
		base.Update ();
	}

	Vector3 nextTarget (){
//		Vector3 nextCoords = G.getGridCoords (next);
//		Vector3 transCoords = G.getGridCoords (transform.position);
		// grid[.x, .z] == grid[i, j]
		if (nextCoords.x == transCoords.x && nextCoords.z == transCoords.z && 
		    (transCoords.x != endCoords.x || transCoords.z != endCoords.z)) {
			hitNextNode = true;
		}
		if (hitNextNode && tempPositions.Count > 0){
			next = tempPositions [0].loc;
			tempPositions.RemoveAt (0);
			hitNextNode = false;
		}
//		Debug.Log("t + " + transform.position + "n + " + next);
		return next;
	}

	public void assignedPath(List<Node> path){
		tempPositions = path;
		hitNextNode = true;
	}

	public void assignGridCoords(Vector3 nxtCrds, Vector3 trnsCrds, Vector3 endCrds){
		endCoords = endCrds;
		nextCoords = nxtCrds;
		transCoords = trnsCrds;
	}

	void checkArrival(){
		Collider[] hits = Physics.OverlapSphere (transform.position, arrivalRadius, dynamicLayer);
		if (hits.Length > 0) {
			inArrivalRadius = false;
		} else {
			inArrivalRadius = Vector3.Distance (goal.transform.position, transform.position) <= arrivalRadius;
		}
	}
}