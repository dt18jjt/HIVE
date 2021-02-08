using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColliderSpawner: MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public int numberRandomPositions = 10;
    public GameObject Room;
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        Spawn();
    }

    private Vector3 RandomPointInBounds(Bounds bounds, float scale)
    {
        return new Vector3(
            Random.Range(bounds.min.x * scale, bounds.max.x * scale),
            Random.Range(bounds.min.y * scale, bounds.max.y * scale),
            Random.Range(bounds.min.z * scale, bounds.max.z * scale)
        );
    }
    void Spawn()
    {
        int i = 0;
        while (i < numberRandomPositions)
        {
            Vector3 rndPoint3D = RandomPointInBounds(polygonCollider.bounds, 1f);
            Vector2 rndPoint2D = new Vector2(rndPoint3D.x, rndPoint3D.y);
            Vector2 rndPointInside = polygonCollider.ClosestPoint(new Vector2(rndPoint2D.x, rndPoint2D.y));
            if (rndPointInside.x == rndPoint2D.x && rndPointInside.y == rndPoint2D.y)
            {
                GameObject rndCube = GameObject.CreatePrimitive(PrimitiveType.Plane);
                rndCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                rndCube.transform.position = rndPoint2D;
                rndCube.transform.parent = this.transform;
                Room.GetComponent<RoomTypes>().iSpawnPoints.Add(rndCube.transform);
                //Destroy(gameObject, 3f);
                i++;
            }
        }
    }
    
}
