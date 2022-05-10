using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        queryResultCount = 0;
        List<Body> sorted = new List<Body>(bodies);

        // sort bodies along x-axis (position.x) 
        //sorted.Sort(); 
        sorted.OrderBy(x => x.position.x).ToList(); 

        // create BVH root node
        rootNode = new BVHNode(sorted);
        // insert bodies starting at root node
        //bodies.ForEach(body => rootNode.Insert(body));
    }

    public override void Query(AABB aabb, List<Body> results)
    {
        rootNode.Query(aabb, results);
        queryResultCount = queryResultCount + results.Count;
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
