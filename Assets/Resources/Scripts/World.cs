﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class World : MonoBehaviour
{
    public static World Instance;
    public int chunkSize = 16;
    public Vector2 maxWorldSize = new Vector2(3, 3); // in chunks
    public Chunk[,,] chunks;
    public bool isRunning = false;
    public int renderDistance = 2; // TODO ----------

    public TilePreset[] tilePresets;

    public enum Tiles { // Add new tiles here
        Empty, // Air
        Grass,
        Dirt,
        Stone
    }

    [System.Serializable]
    public class TilePreset
    {
        public World.Tiles tile;
        public Material material;
    }

    void Awake()
    {
        Instance = this;
        chunks = new Chunk[(int)maxWorldSize.x, (int)maxWorldSize.y, (int)maxWorldSize.x];
    }

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        if (!isRunning)
            StartCoroutine(Generate());
    }

    IEnumerator Generate() // Generates the entire world. This shouldn't be used to refresh a world, a seperate function will be needed for that
    {
        isRunning = true;
        ClearChunks();

        for (int x = 0; x <= (int)maxWorldSize.x - 1; x++)
        {
            for (int y = 0; y <= (int)maxWorldSize.y - 1; y++)
            {
                for (int z = 0; z <= (int)maxWorldSize.x - 1; z++)
                {
                    GameObject chunk = new GameObject();
                    chunk.name = "Chunk";
                    chunk.transform.SetParent(transform);
                    chunk.transform.localPosition = new Vector3( (x) * chunkSize, (y) * chunkSize, (z) * chunkSize);
                    chunk.AddComponent(typeof(Chunk));
                    chunks[x, y, z] = chunk.GetComponent<Chunk>();
                    chunks[x, y, z].size = chunkSize;
                    chunks[x, y, z].position = new int[] { x, y, z };
                    chunks[x, y, z].Generate();
                    yield return null;
                }
            }
        }

        for (int x = 0; x <= (int)maxWorldSize.x - 1; x++)
        {
            for (int y = 0; y <= (int)maxWorldSize.y - 1; y++)
            {
                for (int z = 0; z <= (int)maxWorldSize.x - 1; z++)
                {
                    if (!chunks[x, y, z].isEmpty)
                        chunks[x, y, z].SetNeedsLiteUpdate();
                    yield return null;
                }
            }
        }


        isRunning = false;
    }

    public void ClearChunks()
    {
        foreach (Chunk chunk in chunks)
        {
            DestroyImmediate(chunk);
        }
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public Tile GetTile(Vector3 position) { return GetTile( new int[] { (int)position.x, (int)position.y, (int)position.z } ); }
    public Tile GetTile(int[] position)
    {

        if (GetChunkFromWorldPosition(position) != null)
        {
            return GetChunkFromWorldPosition(position).GetTile(GetPositionInChunk(position));
        }
        else
        {
            return null;
        }
    }

    public int[] GetPositionInChunk(int[] worldPosition)
    {
        int[] chunkId = new int[] { worldPosition[0] / chunkSize, worldPosition[1] / chunkSize, worldPosition[2] / chunkSize };
        return new int[] { worldPosition[0] - (chunkId[0] * chunkSize), worldPosition[1] - (chunkId[1] * chunkSize), worldPosition[2] - (chunkId[2] * chunkSize) };
    }
    
    public Chunk GetChunkFromWorldPosition(int[] tilePosition)
    {
        try
        {
            int[] chunkId = new int[] { tilePosition[0] / chunkSize, tilePosition[1] / chunkSize, tilePosition[2] / chunkSize };

            if (chunks != null && chunks[chunkId[0], chunkId[1], chunkId[2]] != null)
            {
                return chunks[chunkId[0], chunkId[1], chunkId[2]];
            }
            else
            {
                return null;
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            return null;
        }
    }

    public Chunk GetChunk(int[] chunkPosition)
    {
        try
        {
            if (chunks != null && chunks[chunkPosition[0], chunkPosition[1], chunkPosition[2]] != null)
            {
                return chunks[chunkPosition[0], chunkPosition[1], chunkPosition[2]];
            }
            else
            {
                return null;
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            return null;
        }
    }

    
}
