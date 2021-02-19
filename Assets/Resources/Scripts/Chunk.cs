using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    [HideInInspector]
    public int size = 16;

    public Tile[,,] tiles;

    public int[] position;

    public bool needsUpdate = false;
    public bool needsLiteUpdate = false;

    private GameObject grassLayer;
    private GameObject dirtLayer;

    private void Start()
    {
        grassLayer = new GameObject();
        grassLayer.transform.SetParent(transform);
        grassLayer.transform.localPosition = Vector3.zero;
        grassLayer.AddComponent<MeshFilter>();
        grassLayer.AddComponent<MeshRenderer>();
        grassLayer.AddComponent<MeshCollider>();
        grassLayer.GetComponent<Renderer>().material = World.Instance.grassMaterial;
        grassLayer.name = "GrassLayer";

        dirtLayer = new GameObject();
        dirtLayer.transform.SetParent(transform);
        dirtLayer.transform.localPosition = Vector3.zero;
        dirtLayer.AddComponent<MeshFilter>();
        dirtLayer.AddComponent<MeshRenderer>();
        dirtLayer.AddComponent<MeshCollider>();
        dirtLayer.GetComponent<Renderer>().material = World.Instance.dirtMaterial;
        dirtLayer.name = "DirtLayer";
    }

    private void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
            RenderNeighbours();
        }
        if (needsLiteUpdate)
        {
            needsLiteUpdate = false;
            RenderChunk();
        }
    }

    public void Generate()
    {
        Populate(); // Initializes tiles


        // Terrain Generation code

        float thresh = 0.45f;

        for (int x = 0; x <= size - 1; x++)
        {
            for (int y = 0; y <= size - 1; y++)
            {
                for (int z = 0; z <= size - 1; z++)
                {
                    float value = WorldGen.Noise3D(tiles[x, y, z].position);
                    bool hasTile = value > thresh;
                    //Debug.Log(tiles[x, y, z].position[0] + ", " + tiles[x, y, z].position[1] + ", " + tiles[x, y, z].position[2]);
                    //Debug.Log(value);
                    if (hasTile)
                    {
                        if (tiles[x, y, z].position[1] == 20)
                        {
                            tiles[x, y, z].type = World.Tiles.Grass;
                        }
                        else if (tiles[x, y, z].position[1] < 20)
                        {
                            tiles[x, y, z].type = World.Tiles.Dirt;
                        }
                    }
                }
            }
        }
    }

    public void Populate()
    {
        tiles = new Tile[size, size, size];
        for (int x = 0; x <= size-1; x++)
        {
            for (int y = 0; y <= size - 1; y++)
            {
                for (int z = 0; z <= size - 1; z++)
                {
                    tiles[x, y, z] = new Tile();
                    tiles[x, y, z].position = new int[] { x + (position[0] * size), y + (position[1] * size), z + (position[2] * size) };
                    tiles[x, y, z].chunk = this;
                }
            }
        }
    }
    public void SetNeedsUpdate()
    {
        this.needsUpdate = true;
    }

    public void SetNeedsLiteUpdate()
    {
        this.needsLiteUpdate = true;
    }


    public void RenderNeighbours()
    {
        RenderChunk();

        Chunk chunk;

        chunk = World.Instance.GetChunk(new int[] { position[0] + 1, position[1], position[1] });
        if (chunk != null && !chunk.needsUpdate) // if it already needs an update, it will re-render itself anyways
        {
            chunk.SetNeedsLiteUpdate();
        }
        chunk = World.Instance.GetChunk(new int[] { position[0] - 1, position[1], position[1] });
        if (chunk != null && !chunk.needsUpdate) // if it already needs an update, it will re-render itself anyways
        {
            chunk.SetNeedsLiteUpdate();
        }
        chunk = World.Instance.GetChunk(new int[] { position[0], position[1] + 1, position[1] });
        if (chunk != null && !chunk.needsUpdate) // if it already needs an update, it will re-render itself anyways
        {
            chunk.SetNeedsLiteUpdate();
        }
        chunk = World.Instance.GetChunk(new int[] { position[0], position[1] - 1, position[1] });
        if (chunk != null && !chunk.needsUpdate) // if it already needs an update, it will re-render itself anyways
        {
            chunk.SetNeedsLiteUpdate();
        }
        chunk = World.Instance.GetChunk(new int[] { position[0], position[1], position[1] + 1 });
        if (chunk != null && !chunk.needsUpdate) // if it already needs an update, it will re-render itself anyways
        {
            chunk.SetNeedsLiteUpdate();
        }
        chunk = World.Instance.GetChunk(new int[] { position[0], position[1], position[1] - 1 });
        if (chunk != null && !chunk.needsUpdate) // if it already needs an update, it will re-render itself anyways
        {
            chunk.SetNeedsLiteUpdate();
        }
        
    }
    public void RenderChunk()
    {
        needsUpdate = false;
        //if (GetComponent<MeshFilter>().sharedMesh != null)
            //DestroyImmediate(GetComponent<MeshFilter>().sharedMesh);
        CombineInstance[] grassTiles = new CombineInstance[size*size*size];
        CombineInstance[] dirtTiles = new CombineInstance[size*size*size];
        int index = 0;
        for (int x = 0; x <= size-1; x++)
        {
            for (int y = 0; y <= size - 1; y++)
            {
                for (int z = 0; z <= size - 1; z++)
                {
                    if (tiles[x, y, z].type == World.Tiles.Empty) continue;
                    GameObject tile = new GameObject();
                    tile.AddComponent<MeshFilter>();
                    tile.transform.position = new Vector3(x, y, z);
                    tile.GetComponent<MeshFilter>().sharedMesh = tiles[x, y, z].Render();
                    switch(tiles[x, y, z].type)
                    {
                        case World.Tiles.Grass:
                            grassTiles[index].mesh = tile.GetComponent<MeshFilter>().sharedMesh;
                            grassTiles[index].transform = tile.transform.localToWorldMatrix;
                            break;
                        case World.Tiles.Dirt:
                            dirtTiles[index].mesh = tile.GetComponent<MeshFilter>().sharedMesh;
                            dirtTiles[index].transform = tile.transform.localToWorldMatrix;
                            break;
                    }
                    DestroyImmediate(tile);
                    index++;
                }
            }
        }


        Mesh grassMesh = new Mesh();
        // Throws overflow error if all tiles in a chunk render all of their sides
        grassMesh.CombineMeshes(grassTiles);
        grassLayer.GetComponent<MeshFilter>().sharedMesh = grassMesh;
        grassLayer.GetComponent<MeshCollider>().sharedMesh = grassMesh;

        Mesh dirtMesh = new Mesh();
        dirtMesh.CombineMeshes(dirtTiles);
        dirtLayer.GetComponent<MeshFilter>().sharedMesh = dirtMesh;
        dirtLayer.GetComponent<MeshCollider>().sharedMesh = dirtMesh;
    }

    public Tile GetTile(Vector3 position) { return GetTile(new int[] { (int)position.x, (int)position.y, (int)position.z }); }
    public Tile GetTile(int[] position)
    {
        if ( position[0] < 0 || position[1] < 0 || position[2] < 0)
        {
            return null;
        }
        return tiles[position[0], position[1], position[2]];
    }
}
