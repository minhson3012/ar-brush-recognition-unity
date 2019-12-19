using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public NavMeshSurface surface;
    public GameObject playZone;
    List<GameObject> items = new List<GameObject>();
    GameObject anchor;
    GameObject plane;
    void Awake()
    {
        //Find anchor
        anchor = GameObject.FindGameObjectWithTag("Anchor");

        //Attach playZone to anchor
        playZone.transform.parent = anchor.transform;
        playZone.transform.position = anchor.transform.position;
        playZone.transform.rotation = anchor.transform.rotation;

        //Bake Navmesh
        surface.BuildNavMesh();
    }
}
