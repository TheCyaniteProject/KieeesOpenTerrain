using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTool : MonoBehaviour
{
    public int[] tilePosition;
    private int[] _tilePosition;
    public int[] chunkPosition;
    [Space]
    [SerializeField]
    public World.Tiles currentTile;

    private World.Tiles type = World.Tiles.Empty;

    private void Start()
    {
        _tilePosition = new int[3];
    }

    private void Update()
    {
        if (World.Instance == null) return;
        this.tilePosition = new int[] { (int)transform.position.x, (int)transform.position.y, (int)transform.position.z };
        Chunk chunk = World.Instance.GetChunkFromWorldPosition(this.tilePosition);

        if (chunk != null)
        {
            this.chunkPosition = World.Instance.GetChunkFromWorldPosition(this.tilePosition).position;
            if (World.Instance.GetChunkFromWorldPosition(this.tilePosition))
            {
                if (_tilePosition[0] != tilePosition[0] || _tilePosition[01] != tilePosition[1] || _tilePosition[2] != tilePosition[2])
                {
                    _tilePosition = tilePosition;
                    byte chunkTile = World.Instance.GetChunkFromWorldPosition(this.tilePosition).GetTile(this.tilePosition);
                    this.currentTile = (World.Tiles)chunkTile;
                    this.type = currentTile;
                }

                if (currentTile != this.type)
                { // If any values have changed, update terrain
                    UpdateTile();
                }
            }
        }
    }

    void UpdateTile()
    {
        if (World.Instance.GetChunkFromWorldPosition(this.tilePosition) != null)
        {
            this.type = currentTile;
            World.Instance.GetChunkFromWorldPosition(this.tilePosition).SetTile(World.Instance.GetPositionInChunk(this.tilePosition), (byte)this.type);
            World.Instance.GetChunkFromWorldPosition(this.tilePosition).SetNeedsUpdate();
        }
    }
}
