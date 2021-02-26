using System.Collections;
using System.Collections.Specialized;
using UnityEngine;

//[ExecuteInEditMode]
public class World : MonoBehaviour
{
    public static World Instance;
    public int surfaceHeight = 20;
    public int chunkSize = 16;
    public Vector2 maxWorldSize = new Vector2(3, 3); // in chunks
    public Chunk[,,] chunks;
    //public int renderDistance = 2; // TODO ----------

    public TilePreset[] tilePresets;

    public WorldTypes worldPreset = WorldTypes.Landscape;

    public Generation generate = Generation.Select;

    public bool isRunning = false;

    public enum WorldTypes
    { // Types of Generation (Mainly for testing)
        Empty,
        LoneGrassTile,
        StoneStar,
        RandomTiles,
        RandomFilledChunk,
        StoneFilledChunk,
        Landscape
    }

    public enum Generation
    { // Generate
        Select,
        Generate,
        ClearChunks
    }

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

    private void Start()
    {
        Instance = this;

        OrderedDictionary orderedDictionary = new OrderedDictionary();

        int index = 0;
        for (int x = 0; x <= chunkSize - 1; x++)
        {
            for (int y = 0; y <= chunkSize - 1; y++)
            {
                for (int z = 0; z <= chunkSize - 1; z++)
                {
                    //Debug.Log(Mathf.Sqrt(x*x+y*y+z*z));
                    orderedDictionary.Add(Mathf.Sqrt(x*x+y*y+z*z).ToString(), index.ToString());
                }
            }
        }
    }

    void Update()
    {
        if (generate == World.Generation.Generate)
        {
            generate = World.Generation.Select;
            PreGenerate();
        }
        else if (generate == World.Generation.ClearChunks)
        {
            generate = World.Generation.Select;
            ClearChunks();
        }
    }

    private void PreGenerate()
    {
        Instance = this;
        chunks = new Chunk[(int)maxWorldSize.x, (int)maxWorldSize.y, (int)maxWorldSize.x];
        if (!isRunning)
        {
            if (worldPreset == WorldTypes.Landscape)
            {
                StartCoroutine(GenerateTerrain());
            }
            else if (worldPreset == WorldTypes.Empty)
            {
                StartCoroutine(GenerateEmpty());
            }
            else if (worldPreset == WorldTypes.LoneGrassTile)
            {
                StartCoroutine(GenerateSingleTile());
            }
            else if (worldPreset == WorldTypes.StoneFilledChunk)
            {
                StartCoroutine(StoneFilledChunk());
            }
            else if (worldPreset == WorldTypes.RandomFilledChunk)
            {
                StartCoroutine(RandomFilledChunk());
            }
            else if (worldPreset == WorldTypes.RandomTiles)
            {
                StartCoroutine(RandomTiles());
            }
            else if (worldPreset == WorldTypes.StoneStar)
            {
                StartCoroutine(StoneStar());
            }
        }
    }

    IEnumerator StoneStar() // Generates a star shape made of stone
    {
        isRunning = true;
        ClearChunks();

        GameObject chunk = new GameObject();
        chunk.name = "Chunk";
        chunk.transform.SetParent(transform);
        chunk.transform.localPosition = Vector3.zero;
        chunk.AddComponent(typeof(Chunk));
        chunks[0, 0, 0] = chunk.GetComponent<Chunk>();
        chunk.GetComponent<Chunk>().size = chunkSize;
        chunk.GetComponent<Chunk>().position = new int[] { 0, 0, 0 };
        chunk.GetComponent<Chunk>().Populate();

        int[] pos = new int[] { (int)chunkSize / 2, (int)chunkSize / 2, (int)chunkSize / 2 };

        chunk.GetComponent<Chunk>().isEmpty = false;
        chunk.GetComponent<Chunk>().tiles[pos[0], pos[1], pos[2]] = (byte)Tiles.Stone;
        chunk.GetComponent<Chunk>().tiles[pos[0], pos[1]+1, pos[2]] = (byte)Tiles.Stone;
        chunk.GetComponent<Chunk>().tiles[pos[0], pos[1]-1, pos[2]] = (byte)Tiles.Stone;
        chunk.GetComponent<Chunk>().tiles[pos[0], pos[1], pos[2]+1] = (byte)Tiles.Stone;
        chunk.GetComponent<Chunk>().tiles[pos[0], pos[1], pos[2]-1] = (byte)Tiles.Stone;
        chunk.GetComponent<Chunk>().tiles[pos[0]+1, pos[1], pos[2]] = (byte)Tiles.Stone;
        chunk.GetComponent<Chunk>().tiles[pos[0]-1, pos[1], pos[2]] = (byte)Tiles.Stone;

        chunk.GetComponent<Chunk>().SetNeedsLiteUpdate();

        isRunning = false;
        yield return null;
    }

    IEnumerator RandomTiles() // Fills available chunks with random tiles (including air)
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
                    chunk.transform.localPosition = new Vector3((x) * chunkSize, (y) * chunkSize, (z) * chunkSize);
                    chunk.AddComponent(typeof(Chunk));
                    chunks[x, y, z] = chunk.GetComponent<Chunk>();
                    chunks[x, y, z].size = chunkSize;
                    chunks[x, y, z].position = new int[] { x, y, z };
                    chunks[x, y, z].Populate();
                    for (int x1 = 0; x1 <= chunkSize - 1; x1++)
                    {
                        for (int y1 = 0; y1 <= chunkSize - 1; y1++)
                        {
                            for (int z1 = 0; z1 <= chunkSize - 1; z1++)
                            {
                                chunk.GetComponent<Chunk>().tiles[x1, y1, z1] = (byte)Random.Range(0, tilePresets.Length + 1);
                            }
                        }
                    }
                    chunk.GetComponent<Chunk>().isEmpty = false;
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
                    chunks[x, y, z].SetNeedsLiteUpdate();
                    yield return null;
                }
            }
        }

        isRunning = false;
        yield return null;
    }

    IEnumerator RandomFilledChunk() // Fills available chunks with random tiles (discluding air)
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
                    chunk.transform.localPosition = new Vector3((x) * chunkSize, (y) * chunkSize, (z) * chunkSize);
                    chunk.AddComponent(typeof(Chunk));
                    chunks[x, y, z] = chunk.GetComponent<Chunk>();
                    chunks[x, y, z].size = chunkSize;
                    chunks[x, y, z].position = new int[] { x, y, z };
                    chunks[x, y, z].Populate();
                    for (int x1 = 0; x1 <= chunkSize - 1; x1++)
                    {
                        for (int y1 = 0; y1 <= chunkSize - 1; y1++)
                        {
                            for (int z1 = 0; z1 <= chunkSize - 1; z1++)
                            {
                                chunk.GetComponent<Chunk>().tiles[x1, y1, z1] = (byte)Random.Range(1, tilePresets.Length + 1);
                            }
                        }
                    }
                    chunk.GetComponent<Chunk>().isEmpty = false;
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
                    chunks[x, y, z].SetNeedsLiteUpdate();
                    yield return null;
                }
            }
        }

        isRunning = false;
        yield return null;
    }

    IEnumerator StoneFilledChunk() // Fills available chunks with stone tiles
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
                    chunk.transform.localPosition = new Vector3((x) * chunkSize, (y) * chunkSize, (z) * chunkSize);
                    chunk.AddComponent(typeof(Chunk));
                    chunks[x, y, z] = chunk.GetComponent<Chunk>();
                    chunks[x, y, z].size = chunkSize;
                    chunks[x, y, z].position = new int[] { x, y, z };
                    chunks[x, y, z].Populate();
                    for (int x1 = 0; x1 <= chunkSize - 1; x1++)
                    {
                        for (int y1 = 0; y1 <= chunkSize - 1; y1++)
                        {
                            for (int z1 = 0; z1 <= chunkSize - 1; z1++)
                            {
                                chunk.GetComponent<Chunk>().tiles[x1, y1, z1] = (byte)Tiles.Stone;
                            }
                        }
                    }
                    chunk.GetComponent<Chunk>().isEmpty = false;
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
                    chunks[x, y, z].SetNeedsLiteUpdate();
                    yield return null;
                }
            }
        }

        isRunning = false;
        yield return null;
    }

    IEnumerator GenerateSingleTile() // Generates a single chunk and sets the middle tile to grass
    {
        isRunning = true;
        ClearChunks();

        GameObject chunk = new GameObject();
        chunk.name = "Chunk";
        chunk.transform.SetParent(transform);
        chunk.transform.localPosition = Vector3.zero;
        chunk.AddComponent(typeof(Chunk));
        chunks[0,0,0] = chunk.GetComponent<Chunk>();
        chunk.GetComponent<Chunk>().size = chunkSize;
        chunk.GetComponent<Chunk>().position = new int[] { 0, 0, 0 };
        chunk.GetComponent<Chunk>().Populate();

        chunk.GetComponent<Chunk>().tiles[(int)chunkSize / 2, (int)chunkSize / 2, (int)chunkSize / 2] = (byte)Tiles.Grass;
        chunk.GetComponent<Chunk>().isEmpty = false;

        chunk.GetComponent<Chunk>().SetNeedsLiteUpdate();

        isRunning = false;
        yield return null;
    }

    IEnumerator GenerateEmpty() // Generates avaiable chunks but doesn't add anything to them
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
                    chunk.transform.localPosition = new Vector3((x) * chunkSize, (y) * chunkSize, (z) * chunkSize);
                    chunk.AddComponent(typeof(Chunk));
                    chunks[x, y, z] = chunk.GetComponent<Chunk>();
                    chunks[x, y, z].size = chunkSize;
                    chunks[x, y, z].position = new int[] { x, y, z };
                    chunks[x, y, z].Populate();
                    yield return null;
                }
            }
        }


        isRunning = false;
    }

    IEnumerator GenerateTerrain() // Generates the entire world.
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
                }
            }
        }

        for (int x = 0; x <= (int)maxWorldSize.x - 1; x++)
        {
            for (int y = 0; y <= (int)maxWorldSize.y - 1; y++)
            {
                for (int z = 0; z <= (int)maxWorldSize.x - 1; z++)
                {
                    yield return new WaitForEndOfFrame();
                    if (chunks != null && chunks[x, y, z] != null && !chunks[x, y, z].isEmpty && chunks[x, y, z].needsUpdate)
                        chunks[x, y, z].RenderChunk();
                }
            }
        }

        isRunning = false;
    }

    public void ClearChunks()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public byte GetTile(Vector3 position) { return GetTile( new int[] { (int)position.x, (int)position.y, (int)position.z } ); }
    public byte GetTile(int[] position)
    {

        if (GetChunkFromWorldPosition(position) != null)
        {
            return GetChunkFromWorldPosition(position).GetTile(GetPositionInChunk(position));
        }
        else
        {
            return 0;
        }
    }

    public void SetTile(Vector3 position, byte value) { SetTile(new int[] { (int)position.x, (int)position.y, (int)position.z }, value); }
    public void SetTile(int[] position, byte value)
    {

        if (GetChunkFromWorldPosition(position) != null)
        {
            GetChunkFromWorldPosition(position).SetTile(GetPositionInChunk(position), value);
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
