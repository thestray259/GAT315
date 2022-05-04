using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{
    public struct AABBStruct
    {
        public Vector2 center;
        public Vector2 size;
        public Vector2 extents { get => size * 0.5f; }
        public Vector2 min { get { return min; } set { SetMinMax(value, max); } }
        public Vector2 max { get { return max; } set { SetMinMax(min, value); } }
        public AABBStruct(Vector2 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
        }

        public bool Contains(AABBStruct aabb)
        {
            return (aabb.min.x <= max.x && aabb.max.x >= min.x) &&
                   (aabb.min.y <= max.y && aabb.max.y >= min.y);
        }

        public bool Contains(Vector2 point)
        {
            return (point.x >= min.x && point.x <= max.x) &&
                   (point.y >= min.y && point.y <= max.y);
        }

        public void SetMinMax(Vector2 min, Vector2 max)
        {
            size = (max - min);
            center = min + extents;
        }

        public void Expand(Vector2 point)
        {
            SetMinMax(Vector2.Min(point, min), Vector2.Max(point, max));
        }

        public void Expand(AABBStruct aabb)
        {
            SetMinMax(Vector2.Min(aabb.min, min), Vector2.Max(aabb.max, max));
        }

        public void Draw(Color color, float width = 0.05f)
        {
            //< use Debug.DrawLine to draw four lines of the AABB>
            Debug.DrawLine(min, max, color, width);
        }

    }
}
