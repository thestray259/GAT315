using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        // create quadtree root node
        rootNode = new BVHNode(bodies);
        // insert bodies starting at root node
        //bodies.ForEach(body => rootNode.Insert(body));
    }

    public override void Query(AABB aabb, List<Body> results)
    {
        rootNode.Query(aabb, results);
    }

    public override void Query(Body body, List<Body> results)
    {
        Query(body.shape.GetAABB(body.position), results);
    }

    public override void Draw()
    {
        rootNode?.Draw();
    }
}
