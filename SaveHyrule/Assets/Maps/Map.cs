using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Map {

	public List<List<char>> map;
	public List<List<int>> h;
	public List<List<bool>> visited;


	public int distance(int x1, int x2, int y1, int y2) {
		return (int)Mathf.Sqrt (Mathf.Pow((x1-x2),2)+Mathf.Pow((y1-y2),2));
	}

	public int manhattan_distance(int x1, int x2, int y1, int y2) {
		return (9 * ( Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2) ));
	}


	public void setVisitedBoard()
	{
		visited = new List<List<bool>>();

		for (int i = 0; i < map.Count; i++) {
			visited.Add(new List<bool>());
			for (int j = 0; j < map.Count; j++) {
				visited [i].Add (false);
			}
		}
	}

	public bool visit(int x, int y)
	{
		if (x >= map.Count || y >= map.Count || x < 0 || y < 0)
			return false;
		
		if (visited [x] [y]) {
			return false;
		} else {
			visited [x] [y] = true;
			return true;
		}
	}

	public void unvisit()
	{
		for (int i = 0; i < visited.Count; i++) {
			for (int j = 0; j < visited.Count; j++) {
				visited [i] [j] = false;
			}
		}
	}


	// ta errado aqui

	public void createHeuristicBoard()
	{
		h = new List<List<int>> ();

		for (int i = 0; i < map.Count; i++) {
			h.Add (new List<int> ());
			for (int j = 0; j < map.Count; j++) {
				h [i].Add (0);
			}
		}
	}

	// setHeuristicValue -> assume que o board de heuristica ja foi criado
	public void setHeuristicValue (int x, int y)
	{
		for (int i = 0; i < map.Count; i++) {
			for (int j = 0; j < map.Count; j++) {
				h [i] [j] = manhattan_distance (x, i, y, j);
			}
		}
	}


	public int getH(int x, int y) {
		if (x >= map.Count || y >= map.Count || x < 0 || y < 0)
			return -1; // error

		return h [x] [y];
	}

	public int getCount () { return map.Count; }

	public char getPos(int x, int y) 
	{ 
		if (x >= map.Count || y >= map.Count || x < 0 || y < 0)
			return 'E'; // error
		
		return map [x] [y]; 
	}

}
