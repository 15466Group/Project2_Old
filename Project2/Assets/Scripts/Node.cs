using UnityEngine;
using System.Collections;
using System;

public class Node : IEquatable<Node> {

	public bool free { get; set;}
	public Vector3 loc { get; set;}
	public bool isGoal { get; set;}
	public int i { get; set;}
	public int j { get; set;}
	public float f { get; set;}
	public float g { get; set;}
	public float h { get; set;}
	public bool isSwamp { get; set; }
//	public bool open;
//	public bool closed { get; set;}

	public Node (bool isFree, Vector3 pos, bool isG, int newi, int newj, float heuristic, bool swamp) {
		free = isFree;
		loc = pos;
		isGoal = isG;
		i = newi;
		j = newj;
		f = Mathf.Infinity;
		g = Mathf.Infinity;
		h = heuristic;
		isSwamp = swamp;
//		open = true;
//		closed = false;
	}

	public bool Equals(Node other){
		if (other == null) return false;
		return (this.i == other.i && this.j == other.j);
	}

}
