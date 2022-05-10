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
        if (nodeBodies.Count > 0) 
        {
            nodeAABB.center = nodeBodies[0].position;
            nodeAABB.size = Vector3.zero;

            nodeBodies.ForEach(body => this.nodeAABB.Expand(body.shape.GetAABB(body.position)));
        }
    }

    public void Split()
    {
        int length = nodeBodies.Count;
        int half = length / 2;
        if (half >= 1)
        {
            left = new BVHNode(nodeBodies.GetRange(0, half)); // first half
            right = new BVHNode(nodeBodies.GetRange(length - half, half)); // second half 

            nodeBodies.Clear();
        }
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
