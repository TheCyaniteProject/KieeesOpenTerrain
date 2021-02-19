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

    public bool isEmpty = true;

    public bool needsUpdate = false;
    public bool needsLiteUpdate = false;

    private Dictionary<World.Tiles, GameObject> layers = new Dictionary<World.Tiles, GameObject>();

    private void Start()
    {
        foreach (World.TilePreset tilePreset in World.Instance.tilePresets)
        {
            GameObject layer = new GameObject();
            layers[tilePreset.tile] = layer;
            layer.gameObject.transform.SetParent(transform);
            layer.gameObject.transform.localPosition = Vector3.zero;
            layer.gameObject.AddComponent<MeshFilter>();
            layer.gameObject.AddComponent<MeshRenderer>();
            layer.gameObject.AddComponent<MeshCollider>();
            layer.gameObject.GetComponent<Renderer>().material = tilePreset.material;
            layer.gameObject.name = tilePreset.tile.ToString();
        }
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

        float thresh = 0.35f;

        int topLevel = 25;

        for (int x = 0; x <= size - 1; x++)
        {
            for (int y = 0; y <= size - 1; y++)
            {
                for (int z = 0; z <= size - 1; z++)
                {
                    int[] position = tiles[x, y, z].position;
                    float caves = WorldGen.Noise3D(position[0], position[1], position[2], 15f, 0);
                    bool removeTile = caves < thresh;

                    float surfaceHeight = WorldGen.Noise(position[0], position[2], 45f, 0f);
                    if (tiles[x, y, z].position[1] == topLevel + (int)(surfaceHeight * 10))
                    {
                        tiles[x, y, z].type = World.Tiles.Grass;
                        this.isEmpty = false;
                    } else if (tiles[x, y, z].position[1] < topLevel + (int)(surfaceHeight * 10) && tiles[x, y, z].position[1] >= topLevel + (int)(surfaceHeight * 10) - 3)
                    {
                        tiles[x, y, z].type = World.Tiles.Dirt;
                        this.isEmpty = false;
                    } else if (tiles[x, y, z].position[1] < topLevel + (int)(surfaceHeight * 10) - 3)
                    {
                        tiles[x, y, z].type = World.Tiles.Stone;
                        this.isEmpty = false;
                    }
                    if (removeTile)
                    {
                        if (tiles[x, y, z].position[1] < topLevel + (int)(surfaceHeight * 10) - 3 && tiles[x, y, z].position[1] > 1 + (int)(surfaceHeight * 9.64 ))
                        {
                            tiles[x, y, z].type = World.Tiles.Empty;
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
        Dictionary<World.Tiles, CombineInstance[]> combines = new Dictionary<World.Tiles, CombineInstance[]>();
        foreach (World.Tiles tile in layers.Keys)
        {
            combines[tile] = new CombineInstance[size*size*size];
        }
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
                    
                    combines[tiles[x, y, z].type][index].mesh = tile.GetComponent<MeshFilter>().sharedMesh;
                    combines[tiles[x, y, z].type][index].transform = tile.transform.localToWorldMatrix;

                    DestroyImmediate(tile);
                    index++;
                }
            }
        }
        
        foreach (World.Tiles layer in combines.Keys)
        {
            // Throws overflow error if all tiles in a chunk render all of their sides
            Mesh layerMesh = new Mesh();
            layerMesh.CombineMeshes(combines[layer]);
            layers[layer].GetComponent<MeshFilter>().sharedMesh = layerMesh;
            layers[layer].GetComponent<MeshCollider>().sharedMesh = layerMesh;
        }
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
