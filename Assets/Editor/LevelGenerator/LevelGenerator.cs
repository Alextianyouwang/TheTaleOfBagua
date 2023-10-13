

using UnityEngine;

public class LevelGenerator
{
    public Camera Cam { get; set; }

    public Vector3[] GetScreenInWorldSpace(float depth)
    {
        Vector3[] corners = new Vector3[4];
        if (Cam == null)
        {
            Debug.LogError("Camera reference is null!");
            return corners;
        }
        corners[0] = Cam.ScreenToWorldPoint(new Vector3(0, 0, depth));
        corners[1] = Cam.ScreenToWorldPoint(new Vector3(Cam.pixelWidth, 0, depth));
        corners[2] = Cam.ScreenToWorldPoint(new Vector3(Cam.pixelWidth, Cam.pixelHeight, depth));
        corners[3] = Cam.ScreenToWorldPoint(new Vector3(0, Cam.pixelHeight, depth));
        return corners;
    }

    public GameObject CreatePlaneLevel(string name, Mesh mesh, Material mat)
    {
        GameObject g = new GameObject(name);
        MeshFilter mf = g.AddComponent<MeshFilter>();
        MeshRenderer mr = g.AddComponent<MeshRenderer>();
        BoxCollider mc = g.AddComponent<BoxCollider>();
        mr.sharedMaterial = mat;
        mf.sharedMesh = mesh;
        mc.size = mesh.bounds.size;
        mc.center = mesh.bounds.center;
        return g;
    }

    public void AdjustQuadDepth(Mesh target, float depth)
    {
        target.vertices = GetScreenInWorldSpace(depth);
        target.RecalculateBounds();
    }
    public Mesh CreateQuad(Vector3[] corners)
    {

        Mesh m = new Mesh();

        m.vertices = new Vector3[] { corners[0], corners[1], corners[2], corners[3] };
        m.triangles = new int[] { 0, 3, 2, 2, 1, 0 };
        m.uv = new Vector2[] { Vector2.zero, Vector2.right, Vector2.one, Vector2.up };

        m.RecalculateNormals();
        m.RecalculateBounds();

        return m;
    }

    public Cell[] CreateChunks(Mesh platform,int x, int y, float height) 
    {
        Cell[] cells = new Cell[x*y];
        float xSegment = 1f / x;
        float ySegment = 1f / y;
        float xLength = platform.bounds.size.x / x;
        float yLength = platform.bounds.size.z / y;
        Vector3 offset = -new Vector3(platform.bounds.size.x - xLength, 0, platform.bounds.size.z - yLength)/2; 
        for (int i = 0; i < x; i++) 
        {
            for (int j = 0; j < y; j++) 
            {
                Vector3 pos = offset + new Vector3(i * xLength, 0, j*yLength);
                cells[i * y + j] = new Cell(pos,new Vector3 (xLength,height,yLength));
                cells[i * y + j].SetTexSpaceInfo(new Vector2(xSegment * i + xSegment / 2f, ySegment * j + ySegment / 2f), new Vector2(xSegment, ySegment));

                //Checker Pattern
                cells[i * y + j].SetActive(j % 2 == 0 ? true : false);
                cells[i * y + j].SetActive(i % 2 == 0 ? cells[i * y + j].isActive : cells[i * y + j].isActive ? false : true);

            } 
        }
        return cells;
    }

    public Cell[] AdjustCellData(Cell[] input,int x, int y, Mesh platform , float height) 
    {
        float xSegment = 1f / x;
        float ySegment = 1f / y;
        float xLength = platform.bounds.size.x / x;
        float yLength = platform.bounds.size.z / y;
        Vector3 offset = -new Vector3(platform.bounds.size.x - xLength, 0, platform.bounds.size.z - yLength) / 2;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector3 pos = offset + new Vector3(i * xLength, 0, j * yLength);
                input[i * y + j].position = pos;
                input[i * y + j].size = new Vector3(xLength, height, yLength);
                input[i * y + j].SetTexSpaceInfo(new Vector2(xSegment * i + xSegment / 2f, ySegment * j + ySegment / 2f), new Vector2(xSegment, ySegment));
            }
        }
        return input;
    }

    public Cell GetSelectedCell(Cell[] cells, Vector3 pos) 
    {
        Cell closestCell = null;
        float shortestDist = float.MaxValue;
        foreach (Cell c in cells) 
        {
            float dist = Vector3.Distance(pos, c.position);
            if (dist <= shortestDist) 
            {
                shortestDist = dist;
                closestCell = c;
            }
        }
        return closestCell;
    }
}


public class Cell
{
    public Vector3 position = Vector3.zero;
    public Vector3 size = Vector3.zero;
    public bool isActive = true;

    public Vector2 texSpacePos = Vector2.zero;
    public Vector2 texSpaceSize = Vector2.zero;
    public CellUV cellStruct { get { return new CellUV(texSpacePos, texSpaceSize, isActive?1f:0f); } }

    public struct CellUV
    {
        public Vector2 position;
        public Vector2 size;
        public float isActive;

        public CellUV(Vector2 position, Vector2 size, float isActive) 
        {
             this.position= position;
             this.size= size;
             this.isActive = isActive;
        }
    }

    public void SetTexSpaceInfo(Vector2 texSpacePos, Vector2 texSpaceSize ) 
    {
        this.texSpacePos = texSpacePos;
        this.texSpaceSize = texSpaceSize;
    }

    public Cell(Vector3 position, Vector3 size)
    {
        this.position = position;
        this.size = size;
    }

    public void SetActive(bool value) 
    {
        isActive = value;
    }

}