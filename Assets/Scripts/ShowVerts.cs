using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowVerts : MonoBehaviour
{
    private Matrix4x4 localToWorld;

    private MeshFilter mf;

    private Vector3[] verts;
    [SerializeField] private GameObject obj;
    private List<GameObject> spheres;
    // Start is called before the first frame update
    void Start()
    {
        mf = this.gameObject.GetComponent<MeshFilter>();
        localToWorld = transform.localToWorldMatrix;
        
        spheres = new List<GameObject>();

        GenerateInteractableVerts();
    }

    void GenerateInteractableVerts()
    {
        float threshold = 0.1f;
        
        foreach (Vector3 t in mf.mesh.vertices)
        {
            // Get the vertex's world transform
            Vector3 vert = localToWorld.MultiplyPoint3x4( t);
            
            // Stops duplicate objects from being made
            if (spheres.Any(sph => (sph.transform.position == vert))) continue;
            
            GameObject sphere = Instantiate(obj, vert, Quaternion.identity);

            sphere.transform.position = vert;
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            spheres.Add(sphere);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
