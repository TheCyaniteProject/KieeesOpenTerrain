using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public float height { get { return heights[0]; } set { heights[0] = value; } }
    public World.Tiles type = World.Tiles.Empty;
    public int[] position;
    public float[] heights = new float[] { 1, 0, 0, 0, 0 };
    public int[] sides = new int[] { 0, 0, 0, 0, 0, 0 };
    public Chunk chunk;

    public void SetNeedsUpdate()
    {
        chunk.needsUpdate = true;
    }

    public Mesh Render()
    {
        World world = World.Instance;
        if (height <= 0) type = World.Tiles.Empty;
        if (type != World.Tiles.Empty)
        {

            if (world.GetTile(new int[] { position[0], position[1], position[2] + 1 }) == null || world.GetTile(new int[] { position[0], position[1], position[2] + 1 }).type == World.Tiles.Empty)
            { sides[0] = 1; }
            else
            { // Has valid tile beside it
                sides[0] = 0;
                //Tile neighbour = world.GetTile(new int[] { position[0], position[1], position[2] + 1 });

                //float h = ((height -1) + (neighbour.height -1));


                //heights[3] = h;
                //heights[4] = h;
            }
            if (world.GetTile(new int[] { position[0], position[1], position[2] - 1 }) == null || world.GetTile(new int[] { position[0], position[1], position[2] - 1 }).type == World.Tiles.Empty)
            { sides[1] = 1; }
            else
            {
                sides[1] = 0;
            }
            
            if (world.GetTile(new int[] { position[0] , position[1] + 1, position[2] }) == null || world.GetTile(new int[] { position[0], position[1] + 1, position[2] }).type == World.Tiles.Empty)
            { sides[2] = 1; }
            else
            {
                sides[2] = 0;
            }
            if (world.GetTile(new int[] { position[0], position[1] - 1, position[2] }) == null || world.GetTile(new int[] { position[0], position[1] - 1, position[2] }).type == World.Tiles.Empty)
            { sides[3] = 1; }
            else
            {
                sides[3] = 0;
            }

            if (world.GetTile(new int[] { position[0] + 1, position[1], position[2] }) == null || world.GetTile(new int[] { position[0] + 1, position[1], position[2] }).type == World.Tiles.Empty)
            { sides[4] = 1; }
            else
            {
                sides[4] = 0;
            }
            if (world.GetTile(new int[] { position[0] - 1, position[1], position[2] }) == null || world.GetTile(new int[] { position[0] - 1, position[1], position[2] }).type == World.Tiles.Empty)
            { sides[5] = 1; }
            else
            {
                sides[5] = 0;
            }
            return TileRenderer.MakeCube(heights, sides);
        }

        return null;
    }
}
