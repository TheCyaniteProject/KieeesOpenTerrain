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

    public byte[,,] tiles;

    public int[] position;

    public bool isEmpty = true;

    public bool needsUpdate = false;
    public bool needsLiteUpdate = false;

    private Dictionary<byte, GameObject> layers = new Dictionary<byte, GameObject>();

    private void Start()
    {
        foreach (World.TilePreset tilePreset in World.Instance.tilePresets)
        {
            GameObject layer = new GameObject();
            layers[(byte)tilePreset.tile] = layer;
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
                    int[] position = GetGlobalPosition(new int[] { x, y, z });

                    float caves = WorldGen.Noise3D(position[0], position[1], position[2], 15f, 0);

                    float surfaceHeight = WorldGen.Noise(position[0], position[2], 45f, 0f);
                    if (position[1] == topLevel + (int)(surfaceHeight * 10))
                    {
                        tiles[x, y, z] = (int)World.Tiles.Grass;
                        this.isEmpty = false;
                    } else if (position[1] < topLevel + (int)(surfaceHeight * 10) && position[1] >= topLevel + (int)(surfaceHeight * 10) - 3)
                    {
                        tiles[x, y, z] = (int)World.Tiles.Dirt;
                        this.isEmpty = false;
                    } else if (position[1] < topLevel + (int)(surfaceHeight * 10) - 3)
                    {
                        tiles[x, y, z] = (int)World.Tiles.Stone;
                        this.isEmpty = false;
                    }
                    if (caves < thresh)
                    {
                        if (position[1] < topLevel + (int)(surfaceHeight * 10) - 3 && position[1] > 1 + (int)(surfaceHeight * 9.64 ))
                        {
                            tiles[x, y, z] = (int)World.Tiles.Empty;
                        }
                    }
                }
            }
        }
    }

    public void Populate()
    {
        tiles = new byte[size, size, size];
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
        Dictionary<byte, CombineInstance[]> combines = new Dictionary<byte, CombineInstance[]>();
        foreach (byte tile in layers.Keys)
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
                    if (tiles[x, y, z] == (byte)World.Tiles.Empty) continue;
                    Mesh tileMesh = Tile.Render(tiles[x, y, z], new int[] { x, y, z });
                    if (tileMesh == null) continue;

                    GameObject tile = new GameObject();
                    tile.transform.position = new Vector3(x, y, z);
                    
                    combines[tiles[x, y, z]][index].mesh = tileMesh;
                    combines[tiles[x, y, z]][index].transform = tile.transform.localToWorldMatrix;

                    Destroy(tile);
                    index++;
                }
            }
        }
        
        foreach (byte layer in combines.Keys)
        {
            // Throws overflow error if all tiles in a chunk render all of their sides
            Mesh layerMesh = new Mesh();
            layerMesh.CombineMeshes(combines[layer]);
            layers[layer].GetComponent<MeshFilter>().sharedMesh = layerMesh;
            layers[layer].GetComponent<MeshCollider>().sharedMesh = layerMesh;
        }
    }

    public byte GetTile(Vector3 position) { return GetTile(new int[] { (int)position.x, (int)position.y, (int)position.z }); }
    public byte GetTile(int[] position)
    {
        if (!(position[0] < 0 || position[1] < 0 || position[2] < 0) && (position[0] < size - 1 || position[1] < size - 1 || position[2] < size - 1))
        {
            return tiles[position[0], position[1], position[2]];
        }
        return 0;
    }

    public void SetTile(Vector3 position, byte value) { SetTile(new int[] { (int)position.x, (int)position.y, (int)position.z }, value); }
    public void SetTile(int[] position, byte value)
    {

        if (!(position[0] < 0 || position[1] < 0 || position[2] < 0) && (position[0] < size-1 || position[1] < size - 1 || position[2] < size - 1))
        {
            tiles[position[0], position[1], position[2]] = value;
        }
    }

    public int[] GetGlobalPosition(int[] localPosition)
    {
        return new int[] { localPosition[0] + (position[0] * size), localPosition[1] + (position[1] * size), localPosition[2] + (position[2] * size) };
    }
}
