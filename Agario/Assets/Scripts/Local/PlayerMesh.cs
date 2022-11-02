using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using AgarioShared.AgarioShared.Enums;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMesh : MonoBehaviour
{
    Mesh playerMesh;
    MeshCollider meshCollider;
    public PlayerCounter PlayerCounter;
    public string playerName;
    private TextMeshProUGUI nameText;
    private SphereCollider Collider;
    public Material theMaterial;
    [NonSerialized] List<Vector3> vertices = new(); 
    [NonSerialized] private List<int> triangles= new();
    [NonSerialized] private List<Color> _colors= new();
 
   
    
    // Start is called before the first frame update
    void Start()
    {

        PlayerLink.Instance.KillOponent += KillThis;
        nameText = gameObject.AddComponent<TextMeshProUGUI>();
        
        Collider = gameObject.AddComponent<SphereCollider>();
        Collider.radius = 1f;

    }

    private void KillThis(PlayerCounter count)
    {
        if (count != PlayerCounter) return;
        
        Destroy(gameObject);
    }

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = playerMesh = new Mesh();
    
        BuildAMesh(12, 1f);
        SetMaterial(theMaterial);
    }

    private void SetPlayerName(string thePlayerName, PlayerCounter counter)
    {
        if (PlayerLink.Instance.PlayerName != playerName) return;
        PlayerCounter = counter;
    }
    
    
    private void BuildAMesh(int numTris, float startExtent)
    {
        PlayerLink.Instance.SizeUpdated += SizeUp;
        var center = Vector3.zero;
        var degreeInc = 360 / numTris;
        var degrees = 0f;
        
        for (var i = 0; i < numTris; i++)
        {
            Vector3 extent2 = GetCircleEdge(degrees, center, startExtent);
            Vector3 extent1 = GetCircleEdge(degrees + degreeInc, center, startExtent);
            AddTriangle(extent2,center, extent1);
            degrees += degreeInc;
        }
    }

    private void OnDisable()
    {
        PlayerLink.Instance.KillOponent += KillThis;
        PlayerLink.Instance.SizeUpdated -= SizeUp;
    }

    public void SizeUp(float size, PlayerCounter playerCounter)
    {
        if (PlayerCounter != playerCounter) return;
        gameObject.transform.localScale += new Vector3(0.1f, 0, 0.1f);
        Collider.radius += 0.1f;
    }
    
    public Vector3 GetCircleEdge(float degree, Vector3 center, float extent)
    {
        var cos = Math.Cos(Mathf.PI / 180f * degree) * extent;
        var sin = Math.Sin(Mathf.PI / 180f * degree) * extent;
        
        return center + new Vector3((float)cos, 0, (float) sin);
    }
    
    public void SetMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
        UpdateMesh();
    }
    
    public Vector3 GetEdgePosition(int vertOne, int vertTwo, float alpha)
    {
        return Vector3.Lerp(vertices[vertOne], vertices[vertTwo], alpha);
    }
    
    public void UpdateMesh()
    {
        playerMesh.Clear();
        playerMesh.SetVertices(vertices);
        playerMesh.SetTriangles(triangles, 0);
        playerMesh.RecalculateNormals();
    }
    
    private void AddTriangle(Vector3 v1,Vector3 v2, Vector3 v3)
    {
        var vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        
        UpdateMesh();
    }


    public int AddQuadWithMeshPanel(List<Vector3> pointList)
    {
        int vertexIndex = vertices.Count;
        
        vertices.Add(pointList[0]);
        vertices.Add(pointList[1]);
        vertices.Add(pointList[2]);
        vertices.Add(pointList[3]);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
        UpdateMesh();

        return vertexIndex;
    }
    
    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) 
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }
}
