


using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Node {

    public float X;
    public float Z;

    public Node LastNode;
    public double H; // Heuristic: Cost to get to goal
    public float G; // Cost to get to here so far
    public float F; // Total Weight

    private float MoveCost;

    public Node(float x, float z, float cost) {
        X = x;
        Z = z;
        MoveCost = cost;
    }

    public float CalculateWeight (Node end, Node lastNode) {
        LastNode = lastNode;
        H = Math.Sqrt( Math.Pow(end.X - X, 2) + Math.Pow(end.Z - Z, 2) );
        G = MoveCost + lastNode.G;
        F = (float)H + G;
        return F;
    }

    public void Testing(Vector3 position, HashSet<Space> subset, int spacingWidth, int spacingLength) {
        Debug.Log("START");

        // List<Tuple<int,int>> map = GenerateMap(5,5);

        List<Tuple<float, float>> map = ConvertSubsetToMap(subset);

        // start working through logic:
        
        Node start = new Node(position.x, position.z, 1);

        // Node start = new Node(0,0,1); // where character is located
        Node end = new Node(10,20,1); // location player clicked on to move to

        List<Node> openList = new List<Node>(){start};
        List<Node> closedList = new List<Node>();

        List<Node> path = new List<Node>(){start};

        Node n = start;
        while (n.X != end.X && n.Z != end.Z) {
            n = FindNextNode(n, end, spacingWidth, spacingLength, openList, map);
            path.Add(n);
        }
        
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log("N(" + i + "): [" + path[i].X + "][" + path[i].Z + "], G: " + path[i].G + ", H: " + path[i].H + ", F: " + path[i].F);
        }
    }


    private Node FindNextNode(Node currentNode, Node end, int spacingWidth, int spacingLength, List<Node> openList, List<Tuple<float, float>> map) {
        Debug.Log("Starting Node: [" + (currentNode.X) +"][" + (currentNode.Z) +"]");
        Debug.Log("Ending Node: [" + (end.X ) + "][" + (end.Z) + "]");

        Node leftNode = new Node(currentNode.X - spacingWidth, currentNode.Z, 1);
        Node rightNode = new Node(currentNode.X + spacingWidth, currentNode.Z, 1);
        Node upNode = new Node(currentNode.X, currentNode.Z + spacingLength, 1);
        Node downNode = new Node(currentNode.X, currentNode.Z - spacingLength, 1);

        Tuple<float, float> temp = new Tuple<float, float>(leftNode.X, leftNode.Z);
        if (map.Contains(temp)) {
            leftNode.CalculateWeight(end, currentNode);
            openList.Add(leftNode);
        }

        temp = new Tuple<float, float>(rightNode.X, rightNode.Z);
        if (map.Contains(temp)) {
            rightNode.CalculateWeight(end, currentNode);
            openList.Add(rightNode);
        }

        temp = new Tuple<float, float>(upNode.X, upNode.Z);
        if (map.Contains(temp)) {
            upNode.CalculateWeight(end, currentNode);
            openList.Add(upNode);
        }

        temp = new Tuple<float, float>(downNode.X, downNode.Z);
        if (map.Contains(temp)) {
            downNode.CalculateWeight(end, currentNode);
            openList.Add(downNode);
        }


        float minDist = 1000f;
        Node minNode = new Node(currentNode.X, currentNode.Z, 1);
        foreach(Node cn in openList) {
            Debug.Log("Node: [" + cn.X + "][" + cn.Z + "], G: " + cn.G + ", H: " + cn.H + ", F: " + cn.F);
            if (cn.F < minDist && cn.F != 0) {
                minDist = cn.F;
                minNode = cn;
            }
        }

        return minNode;
    }


    private List<Tuple<float, float>> ConvertSubsetToMap(HashSet<Space> spaces) {
        List<Tuple<float,float>> result = new List<Tuple<float, float>>();

        foreach (Space space in spaces)
        {
            result.Add(new Tuple<float,float>(space.Object.transform.position.x, space.Object.transform.position.z));
        }

        return result;
    }

    private List<Tuple<int, int>> GenerateMap(int rowLength, int columnLength)
    {
        List<Tuple<int,int>> map = new List<Tuple<int,int>>();
        
        for (int row = 0; row < rowLength; row++)
        {
            for (int col = 0; col < columnLength; col++)
            {
                Tuple<int, int> t = new Tuple<int, int>(col, row);
                map.Add(t);
            }
        }

        return map;
    }
}