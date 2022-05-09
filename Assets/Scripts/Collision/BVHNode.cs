using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode 
{
    AABB nodeAABB;
    List<Body> nodeBodies = new List<Body>();

    BVHNode left;
    BVHNode right;

    public BVHNode(List<Body> bodies) 
    { 
        nodeBodies = bodies; 
        ComputeBoundary(); 
        Split(); 
    }

    public void ComputeBoundary()
    {

    }

    public void Split()
    {

    }

    public void Query(AABB aabb, List<Body> results)
    {
        // check if query aabb intersects node aabb, return if not 
        if (!nodeAABB.Contains(aabb)) return;

        // add intersecting node bodies 
        results.AddRange(nodeBodies);

        left?.Query(aabb, results); 
        right?.Query(aabb, results); 
    }

    public void Draw()
    {
        nodeAABB.Draw(Color.cyan);

        left?.Draw();
        right?.Draw(); 
    }
}
