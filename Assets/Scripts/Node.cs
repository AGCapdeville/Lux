


using System;
using System.Collections.Generic;
using UnityEngine;


public class Node {

    public int X;
    public int Z;

    public Node LastNode;
    public float G;
    public double H;
    public float F;



    public Node(int x, int z) {
        X = x;
        Z = z;
    }

    public float CalculateWeight (Node end, Node lastNode, int moveCost) {
        LastNode = lastNode;
        H = Math.Sqrt( Math.Pow(end.X - X, 2) + Math.Pow(end.Z - Z, 2) );
        G = moveCost + lastNode.G;
        F = (float)H + G;
        return F;
    }

    public void Testing(Vector3 position, HashSet<Space> subset, int spacingWidth, int spacingLength) {
        Debug.Log("START");

        // List<Tuple<int,int>> map = GenerateMap(5,5);

        List<Tuple<float, float>> map = ConvertSubsetToMap(subset);

        // start working through logic:
        
        // Node start = new Node(position.x, position.z);

        Node start = new Node(0,0); // where character is located
        Node end = new Node(1,2); // location player clicked on to move to

        List<Node> openList = new List<Node>(){start};
        List<Node> closedList = new List<Node>();

        Node currentNode = start;

        Node leftNode = new Node(currentNode.X - spacingWidth, currentNode.Z);
        Node rightNode = new Node(currentNode.X + spacingWidth, currentNode.Z);
        Node upNode = new Node(currentNode.X, currentNode.Z + spacingLength);
        Node downNode = new Node(currentNode.X, currentNode.Z - spacingLength);

        Tuple<float, float> temp = new Tuple<float, float>(leftNode.X, leftNode.Z);
        if (map.Contains(temp)) {
            leftNode.CalculateWeight(end, currentNode, 1);
            openList.Add(leftNode);
        }

        temp = new Tuple<float, float>(rightNode.X, rightNode.Z);
        if (map.Contains(temp)) {
            rightNode.CalculateWeight(end, currentNode, 1);
            openList.Add(rightNode);
        }

        temp = new Tuple<float, float>(upNode.X, upNode.Z);
        if (map.Contains(temp)) {
            upNode.CalculateWeight(end, currentNode, 1);
            openList.Add(upNode);
        }

        temp = new Tuple<float, float>(downNode.X, downNode.Z);
        if (map.Contains(temp)) {
            downNode.CalculateWeight(end, currentNode, 1);
            openList.Add(downNode);
        }


        foreach(Node cn in openList) {
            Debug.Log("[" + cn.X + "][" + cn.Z + "], G: " + cn.G + ", H: " + cn.H + ", F: " + cn.F);
        }


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