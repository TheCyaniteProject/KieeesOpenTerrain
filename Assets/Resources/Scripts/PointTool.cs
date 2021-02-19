using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTool : MonoBehaviour
{
    public int[] tilePosition;
    public int[] chunkPosition;
    [SerializeField]
    public Tile currentTile;

    private World.Tiles type = World.Tiles.Empty;
    private float[] heights = new float[] { 1, 0, 0, 0, 0 };
    private int[] sides = new int[] { 0, 0, 0, 0, 0, 0 };

    private void Update()
    {
        this.tilePosition = new int[] { (int)transform.position.x, (int)transform.position.y, (int)transform.position.z };
        Chunk chunk = World.Instance.GetChunkFromWorldPosition(this.tilePosition);

        if (chunk != null)
        {
            this.chunkPosition = World.Instance.GetChunkFromWorldPosition(this.tilePosition).position;
            if (World.Instance.GetChunkFromWorldPosition(this.tilePosition))
            {
                int[] pos = World.Instance.GetPositionInChunk(this.tilePosition);

                Tile chunkTile = World.Instance.GetTile(this.tilePosition);
                this.currentTile = chunkTile;

                if (currentTile != null && (currentTile.heights != this.heights || currentTile.sides != this.sides || currentTile.type != this.type))
                { // If any values have changed, update terrain
                    UpdateTile();
                }
            }
        }
    }

    void UpdateTile()
    {
        if (currentTile != null)
        {
            this.heights = currentTile.heights;
            this.sides = currentTile.sides;
            this.type = currentTile.type;
            currentTile.SetNeedsUpdate();
        }
    }
}
