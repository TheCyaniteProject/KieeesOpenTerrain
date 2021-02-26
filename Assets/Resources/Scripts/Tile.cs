using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public World.Tiles type = World.Tiles.Empty;
    public int[] position;


    public static Mesh Render(byte type, int[] position)
    {
        return Render(type, new Vector3(position[0], position[1], position[2]));
    }
    public static Mesh Render(byte type, Vector3 position)
    {
        World world = World.Instance;
		float size = 0.5f;



		Vector3 _TL = new Vector3(0, 0.5f, 0);
		Vector3 _TR = new Vector3(0, 0.5f, 0);
		Vector3 _TF = new Vector3(0, 0.5f, 0);
		Vector3 _TB = new Vector3(0, 0.5f, 0);

		Vector3 _BL = new Vector3(0, -0.5f, 0);
		Vector3 _BR = new Vector3(0, -0.5f, 0);
		Vector3 _BF = new Vector3(0, -0.5f, 0);
		Vector3 _BB = new Vector3(0, -0.5f, 0);

		Vector3 _TFL = new Vector3(-0.25f, 0.5f, -0.25f);
		Vector3 _TFR = new Vector3(0.25f, 0.5f, -0.25f);
		Vector3 _TBL = new Vector3(-0.25f, 0.5f, 0.25f);
		Vector3 _TBR = new Vector3(0.25f, 0.5f, 0.25f);

		Vector3 _BFL = new Vector3(-0.25f, -0.5f, -0.25f);
		Vector3 _BFR = new Vector3(0.25f, -0.5f, -0.25f);
		Vector3 _BBL = new Vector3(-0.25f, -0.5f, 0.25f);
		Vector3 _BBR = new Vector3(0.25f, -0.5f, 0.25f);

		Vector3 _LB = new Vector3(-0.25f, 0, 0.25f);
		Vector3 _RB = new Vector3(0.25f, 0, 0.25f);
		Vector3 _LF = new Vector3(-0.25f, 0, -0.25f);
		Vector3 _RF = new Vector3(0.25f, 0, -0.25f);

		if (type != (byte)World.Tiles.Empty)
        {
            int[] sides = new int[] { 1, 1, 1, 1, 1, 1 };

            if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Back
            { // Has valid tile beside it
                sides[0] = 0;

				_TB = new Vector3(0, 0f, 0);
				_BB = new Vector3(0, 0f, 0);
				_TBL = new Vector3(0f, 0.5f, 0f);
				_TBR = new Vector3(0f, 0.5f, 0f);

				_BBL = new Vector3(0f, -0.5f, 0f);
				_BBR = new Vector3(0f, -0.5f, 0f);
			}

            if (world.GetTile(position - new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Front
            {
                sides[1] = 0;

				_TF = new Vector3(0, 0f, 0);
				_BF = new Vector3(0, 0f, 0);
				_TFL = new Vector3(0f, 0.5f, 0f);
				_TFR = new Vector3(0f, 0.5f, 0f);

				_BFL = new Vector3(0f, -0.5f, 0f);
				_BFR = new Vector3(0f, -0.5f, 0f);
			}

            if (world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Right
            {
                sides[4] = 0;

				_TR = new Vector3(0, 0f, 0);
				_BR = new Vector3(0, 0f, 0);
				_TFR = new Vector3(0f, 0.5f, 0f);
				_TBR = new Vector3(0f, 0.5f, 0f);

				_BFR = new Vector3(0f, -0.5f, 0f);
				_BBR = new Vector3(0f, -0.5f, 0f);

			}

            if (world.GetTile(position - new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Left
            {
                sides[5] = 0;
				_TL = new Vector3(0, 0f, 0);
				_BL = new Vector3(0, 0f, 0);
				_TFL = new Vector3(0f, 0.5f, 0f);
				_TBL = new Vector3(0f, 0.5f, -0f);

				_BFL = new Vector3(0f, -0.5f, 0f);
				_BBL = new Vector3(0f, -0.5f, -0f);

				_LB = new Vector3(0, 0, 0);
				_LF = new Vector3(0, 0, 0);
			}

			if (world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty) // Top
			{
				sides[2] = 0;

				_TL = new Vector3(0, 0, 0);
				_TR = new Vector3(0, 0, 0);
				_TF = new Vector3(0, 0, 0);
				_TB = new Vector3(0, 0, 0);

				_TFL = new Vector3(-0.25f, 0, -0.25f);
				_TFR = new Vector3(0.25f, 0, -0.25f);
				_TBL = new Vector3(-0.25f, 0, 0.25f);
				_TBR = new Vector3(0.25f, 0, 0.25f);

				if (world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Right
				{
					_RB = new Vector3(0, 0, 0);
					_RF = new Vector3(0, 0, 0);

					_TBR = new Vector3(0f, 0f, 0.5f);
					_TFR = new Vector3(0f, 0f, -0.5f);
				}
				if (world.GetTile(position - new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Left
				{
					_LB = new Vector3(0, 0, 0);
					_LF = new Vector3(0, 0, 0);

					_TBL = new Vector3(0f, 0f, 0.5f);
					_TFL = new Vector3(0f, 0f, -0.5f);
				}


				if (world.GetTile(position - new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Front
				{
					_RF = new Vector3(0, 0, 0);
					_LF = new Vector3(0, 0, 0);

					_TFR = new Vector3(0.5f, 0f, 0f);
					_TFL = new Vector3(-0.5f, 0f, 0f);
				}

				if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Back
				{
					_RB = new Vector3(0, 0, 0);
					_LB = new Vector3(0, 0, 0);

					_TBR = new Vector3(0.5f, 0f, 0f);
					_TBL = new Vector3(-0.5f, 0f, 0f);
				}
			}

			if (world.GetTile(position - new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty) // Bottom
			{
				sides[3] = 0;

				_BL = new Vector3(0, 0, 0);
				_BR = new Vector3(0, 0, 0);
				_BF = new Vector3(0, 0, 0);
				_BB = new Vector3(0, 0, 0);

				_BFL = new Vector3(-0.25f, 0, -0.25f);
				_BFR = new Vector3(0.25f, 0, -0.25f);
				_BBL = new Vector3(-0.25f, 0, 0.25f);
				_BBR = new Vector3(0.25f, 0, 0.25f);

				if (world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Right
				{
					_RB = new Vector3(0, 0, 0);
					_RF = new Vector3(0, 0, 0);

					_BBR = new Vector3(0f, 0f, 0.5f);
					_BFR = new Vector3(0f, 0f, -0.5f);
				}
				if (world.GetTile(position - new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Left
				{
					_LB = new Vector3(0, 0, 0);
					_LF = new Vector3(0, 0, 0);

					_BBL = new Vector3(0f, 0f, 0.5f);
					_BFL = new Vector3(0f, 0f, -0.5f);
				}


				if (world.GetTile(position - new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Front
				{
					_RF = new Vector3(0, 0, 0);
					_LF = new Vector3(0, 0, 0);

					_BFR = new Vector3(0.5f, 0f, 0f);
					_BFL = new Vector3(-0.5f, 0f, 0f);
				}

				if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Back
				{
					_RB = new Vector3(0, 0, 0);
					_LB = new Vector3(0, 0, 0);

					_BBR = new Vector3(0.5f, 0f, 0f);
					_BBL = new Vector3(-0.5f, 0f, 0f);
				}
			}

			if (world.GetTile(position + new Vector3(1, 0, 1)) != (byte)World.Tiles.Empty) // Back Right Corner
			{
				if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Back
				{
					_TBR = new Vector3(0f, 0f, 0f);
					_BBR = new Vector3(0f, 0f, 0f);
				}
			}

			if (world.GetTile(position + new Vector3(-1, 0, 1)) != (byte)World.Tiles.Empty) // Back Left Corners
			{
				if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty)
				{
					_TBL = new Vector3(0f, 0f, 0f);
					_BBL = new Vector3(0f, 0f, 0f);
				}
			}

			if (world.GetTile(position + new Vector3(-1, 0, -1)) != (byte)World.Tiles.Empty) // Front Left Corners
			{
				if (world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty)
				{
					_TFL = new Vector3(0f, 0f, 0f);
					_BFL = new Vector3(0f, 0f, 0f);
				}
			}

			if (world.GetTile(position + new Vector3(1, 0, -1)) != (byte)World.Tiles.Empty) // Front Right Corners
			{

				if (world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty)
				{
					_TFR = new Vector3(0f, 0f, 0f);
					_BFR = new Vector3(0f, 0f, 0f);
				}
			}


			if (world.GetTile(position + new Vector3(1, 1, 0)) != (byte)World.Tiles.Empty) // Top Right Corner
			{
				if (world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Back
				{
					_TBR = new Vector3(0f, 0f, 0f);
					_TFR = new Vector3(0f, 0f, 0f);
				}
			}
			if (world.GetTile(position + new Vector3(1, -1, 0)) != (byte)World.Tiles.Empty) // Bottom Right Corner
			{
				if (world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty) // Back
				{
					_BBR = new Vector3(0f, 0f, 0f);
					_BFR = new Vector3(0f, 0f, 0f);
				}
			}

			if (world.GetTile(position + new Vector3(-1, 1, 0)) != (byte)World.Tiles.Empty) // Top Left Corner
			{
				if (world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty) // Back
				{
					_TBL = new Vector3(0f, 0f, 0f);
					_TFL = new Vector3(0f, 0f, 0f);
				}
			}
			if (world.GetTile(position + new Vector3(-1, -1, 0)) != (byte)World.Tiles.Empty) // Bottom Left Corner
			{
				if (world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty) // Back
				{
					_BBL = new Vector3(0f, 0f, 0f);
					_BFL = new Vector3(0f, 0f, 0f);
				}
			}


			if (world.GetTile(position + new Vector3(0, 1, 1)) != (byte)World.Tiles.Empty) // Top Back Corner
			{
				if (world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Back
				{
					_TBR = new Vector3(0f, 0f, 0f);
					_TBL = new Vector3(0f, 0f, 0f);
				}
			}
			if (world.GetTile(position + new Vector3(0, -1, 1)) != (byte)World.Tiles.Empty) // Bottom Back Corner
			{
				if (world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty) // Back
				{
					_BBR = new Vector3(0f, 0f, 0f);
					_BBL = new Vector3(0f, 0f, 0f);
				}
			}

			if (world.GetTile(position + new Vector3(0, 1, -1)) != (byte)World.Tiles.Empty) // Top Front Corner
			{
				if (world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty) // Back
				{
					_TFR = new Vector3(0f, 0f, 0f);
					_TFL = new Vector3(0f, 0f, 0f);
				}
			}
			if (world.GetTile(position + new Vector3(0, -1, -1)) != (byte)World.Tiles.Empty) // Bottom Front Corner
			{
				if (world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty) // Back
				{
					_BFR = new Vector3(0f, 0f, 0f);
					_BFL = new Vector3(0f, 0f, 0f);
				}
			}
			

			List<Vector3> vertices = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			float xSize = 1f / 8f;
			float ySize = 1f / 6f;

			if (sides[1] == 1) // Front
			{
				vertices.AddRange(new Vector3[] { new Vector3(0, 0, 0) - _BFL, new Vector3(0, size, 0) - _LF, new Vector3(size, size, 0), new Vector3(size, 0, 0) - _BF });
				vertices.AddRange(new Vector3[] { new Vector3(0, size, 0) - _LF, new Vector3(0, size * 2, 0) - _TFL, new Vector3(size, size * 2, 0) - _TF, new Vector3(size, size, 0) });
				vertices.AddRange(new Vector3[] { new Vector3(size, size, 0), new Vector3(size, size * 2, 0) - _TF, new Vector3(size * 2, size * 2, 0) - _TFR, new Vector3(size * 2, size, 0) - _RF });
				vertices.AddRange(new Vector3[] { new Vector3(size, 0, 0) - _BF, new Vector3(size, size, 0), new Vector3(size * 2, size, 0) - _RF, new Vector3(size * 2, 0, 0) - _BFR });

				uvs.AddRange(new Vector2[] { new Vector2(xSize * 4, ySize * 6), new Vector2(xSize * 4, ySize * 5), new Vector2(xSize * 3, ySize * 5), new Vector2(xSize * 3, ySize * 6) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 4, ySize * 5), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 3, ySize * 5) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 5), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 2, ySize * 5) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 6), new Vector2(xSize * 3, ySize * 5), new Vector2(xSize * 2, ySize * 5), new Vector2(xSize * 2, ySize * 6) });
			}
			if (sides[0] == 1) // Back
			{
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, size * 2) - _BBR, new Vector3(size * 2, size, size * 2) - _RB, new Vector3(size, size, size * 2), new Vector3(size, 0, size * 2) - _BB });
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, size, size * 2) - _RB, new Vector3(size * 2, size * 2, size * 2) - _TBR, new Vector3(size, size * 2, size * 2) - _TB, new Vector3(size, size, size * 2) });
				vertices.AddRange(new Vector3[] { new Vector3(size, size, size * 2), new Vector3(size, size * 2, size * 2) - _TB, new Vector3(0, size * 2, size * 2) - _TBL, new Vector3(0, size, size * 2) - _LB });
				vertices.AddRange(new Vector3[] { new Vector3(size, 0, size * 2) - _BB, new Vector3(size, size, size * 2), new Vector3(0, size, size * 2) - _LB, new Vector3(0, 0, size * 2) - _BBL });

				uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 0), new Vector2(xSize * 2, ySize * 1), new Vector2(xSize * 3, ySize * 1), new Vector2(xSize * 3, ySize * 0) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 1), new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 3, ySize * 1) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 1), new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 4, ySize * 2), new Vector2(xSize * 4, ySize * 1) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 0), new Vector2(xSize * 3, ySize * 1), new Vector2(xSize * 4, ySize * 1), new Vector2(xSize * 4, ySize * 0) });
			}
			if (sides[2] == 1) // Top
			{
				vertices.AddRange(new Vector3[] { new Vector3(0, size * 2, size) - _TL, new Vector3(size, size * 2, size), new Vector3(size, size * 2, 0) - _TF, new Vector3(0, size * 2, 0) - _TFL });
				vertices.AddRange(new Vector3[] { new Vector3(size, size * 2, size), new Vector3(size * 2, size * 2, size) - _TR, new Vector3(size * 2, size * 2, 0) - _TFR, new Vector3(size, size * 2, 0) - _TF });
				vertices.AddRange(new Vector3[] { new Vector3(0, size * 2, size * 2) - _TBL, new Vector3(size, size * 2, size * 2) - _TB, new Vector3(size, size * 2, size), new Vector3(0, size * 2, size) - _TL });
				vertices.AddRange(new Vector3[] { new Vector3(size, size * 2, size * 2) - _TB, new Vector3(size * 2, size * 2, size * 2) - _TBR, new Vector3(size * 2, size * 2, size) - _TR, new Vector3(size, size * 2, size) });

				uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 3, ySize * 3), new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 4, ySize * 2) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 3), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 4, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 3, ySize * 3), new Vector2(xSize * 3, ySize * 2) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 3, ySize * 3) });
			}
			if (sides[3] == 1) // Bottom
			{
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, size * 2) - _BBR, new Vector3(size, 0, size * 2) - _BB, new Vector3(size, 0, size), new Vector3(size * 2, 0, size) - _BR });
				vertices.AddRange(new Vector3[] { new Vector3(size, 0, size * 2) - _BB, new Vector3(0, 0, size * 2) - _BBL, new Vector3(0, 0, size) - _BL, new Vector3(size, 0, size) });
				vertices.AddRange(new Vector3[] { new Vector3(size, 0, size), new Vector3(0, 0, size) - _BL, new Vector3(0, 0, 0) - _BFL, new Vector3(size, 0, 0) - _BF });
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, size) - _BR, new Vector3(size, 0, size), new Vector3(size, 0, 0) - _BF, new Vector3(size * 2, 0, 0) - _BFR });

				uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 2), new Vector2(xSize * 6, ySize * 3), new Vector2(xSize * 7, ySize * 3), new Vector2(xSize * 7, ySize * 2) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 3), new Vector2(xSize * 6, ySize * 4), new Vector2(xSize * 7, ySize * 4), new Vector2(xSize * 7, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 7, ySize * 3), new Vector2(xSize * 7, ySize * 4), new Vector2(xSize * 8, ySize * 4), new Vector2(xSize * 8, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 7, ySize * 2), new Vector2(xSize * 7, ySize * 3), new Vector2(xSize * 8, ySize * 3), new Vector2(xSize * 8, ySize * 2) });
			}
			if (sides[4] == 1) // Right
			{
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, 0) - _BFR, new Vector3(size * 2, size, 0) - _RF, new Vector3(size * 2, size, size), new Vector3(size * 2, 0, size) - _BR });
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, size, 0) - _RF, new Vector3(size * 2, size * 2, 0) - _TFR, new Vector3(size * 2, size * 2, size) - _TR, new Vector3(size * 2, size, size) });
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, size, size), new Vector3(size * 2, size * 2, size) - _TR, new Vector3(size * 2, size * 2, size * 2) - _TBR, new Vector3(size * 2, size, size * 2) - _RB });
				vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, size) - _BR, new Vector3(size * 2, size, size), new Vector3(size * 2, size, size * 2) - _RB, new Vector3(size * 2, 0, size * 2) - _BBR });

				uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 2), new Vector2(xSize * 5, ySize * 2), new Vector2(xSize * 5, ySize * 3), new Vector2(xSize * 6, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 5, ySize * 2), new Vector2(xSize * 4, ySize * 2), new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 5, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 5, ySize * 3), new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 5, ySize * 4) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 3), new Vector2(xSize * 5, ySize * 3), new Vector2(xSize * 5, ySize * 4), new Vector2(xSize * 6, ySize * 4) });
			}
			if (sides[5] == 1) // Left
			{
				vertices.AddRange(new Vector3[] { new Vector3(0, 0, size * 2) - _BBL, new Vector3(0, size, size * 2) - _LB, new Vector3(0, size, size), new Vector3(0, 0, size) - _BL });
				vertices.AddRange(new Vector3[] { new Vector3(0, size, size * 2) - _LB, new Vector3(0, size * 2, size * 2) - _TBL, new Vector3(0, size * 2, size) - _TL, new Vector3(0, size, size) });
				vertices.AddRange(new Vector3[] { new Vector3(0, size, size), new Vector3(0, size * 2, size) - _TL, new Vector3(0, size * 2, 0) - _TFL, new Vector3(0, size, 0) - _LF });
				vertices.AddRange(new Vector3[] { new Vector3(0, 0, size) - _BL, new Vector3(0, size, size), new Vector3(0, size, 0) - _LF, new Vector3(0, 0, 0) - _BFL });

				uvs.AddRange(new Vector2[] { new Vector2(xSize * 0, ySize * 4), new Vector2(xSize * 1, ySize * 4), new Vector2(xSize * 1, ySize * 3), new Vector2(xSize * 0, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 1, ySize * 4), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 1, ySize * 3) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 1, ySize * 3), new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 1, ySize * 2) });
				uvs.AddRange(new Vector2[] { new Vector2(xSize * 0, ySize * 3), new Vector2(xSize * 1, ySize * 3), new Vector2(xSize * 1, ySize * 2), new Vector2(xSize * 0, ySize * 2) });
			}

			List<int> triangles = new List<int>();
			for (int i = 0; i < vertices.Count; i += 4)
			{
				triangles.Add(i);
				triangles.Add(i + 1);
				triangles.Add(i + 2);
				triangles.Add(i);
				triangles.Add(i + 2);
				triangles.Add(i + 3);
			}

			List<Vector3> vertices2 = new List<Vector3>();
			if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty) // E - Top Back Left
			{
				if (world.GetTile(position + new Vector3(0, 1, 1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, 1, 0)) == (byte)World.Tiles.Empty)
                {
					if (world.GetTile(position + new Vector3(-1, 0, 1)) != (byte)World.Tiles.Empty) // if back/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(0, size * 2, size), new Vector3(0, size * 2, size * 2), new Vector3(size, size * 2, size * 2) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(0, size * 2, size), new Vector3(0, size, size * 2), new Vector3(size, size * 2, size * 2) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 4, ySize * 2), new Vector2(xSize * 4, ySize * 3) });
				}
			}
			if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty) // E - Top Back Right
			{
				if (world.GetTile(position + new Vector3(0, 1, 1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, 1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(1, 0, 1)) != (byte)World.Tiles.Empty) // if front/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(size, size * 2, size * 2), new Vector3(size * 2, size * 2, size * 2), new Vector3(size * 2, size * 2, size) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(size, size * 2, size * 2), new Vector3(size * 2, size, size * 2), new Vector3(size * 2, size * 2, size) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 3, ySize * 4) });
				}
			}
			if (world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty) // E - Top Front Left
			{
				if (world.GetTile(position + new Vector3(0, 1, -1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, 1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(-1, 0, -1)) != (byte)World.Tiles.Empty) // if front/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(size, size * 2, 0), new Vector3(0, size * 2, 0), new Vector3(0, size * 2, size) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(size, size * 2, 0), new Vector3(0, size, 0), new Vector3(0, size * 2, size) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 3, ySize * 2) });
				}
			}
			if (world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, 1, 0)) != (byte)World.Tiles.Empty) // E - Top Front Right
			{
				if (world.GetTile(position + new Vector3(0, 1, -1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, 1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(1, 0, -1)) != (byte)World.Tiles.Empty) // if front/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(size * 2, size * 2, size), new Vector3(size * 2, size * 2, 0), new Vector3(size, size * 2, 0) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(size * 2, size * 2, size), new Vector3(size * 2, size, 0), new Vector3(size, size * 2, 0) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 2, ySize * 3) });
				}
			}

			if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty) // E - Top Back Left
			{
				if (world.GetTile(position + new Vector3(0, -1, 1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, -1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(-1, 0, 1)) != (byte)World.Tiles.Empty) // if back/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(size, 0, size * 2), new Vector3(0, 0, size * 2), new Vector3(0, 0, size) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(size, 0, size * 2), new Vector3(0, size, size * 2), new Vector3(0, 0, size) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 4, ySize * 2), new Vector2(xSize * 4, ySize * 3) });
				}
			}
			if (world.GetTile(position + new Vector3(0, 0, 1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty) // E - Top Back Right
			{
				if (world.GetTile(position + new Vector3(0, -1, 1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, -1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(1, 0, 1)) != (byte)World.Tiles.Empty) // if front/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(size * 2, 0, size), new Vector3(size * 2, 0, size * 2), new Vector3(size, 0, size * 2) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(size * 2, 0, size), new Vector3(size * 2, size, size * 2), new Vector3(size, 0, size * 2) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 3, ySize * 4) });
				}
			}
			if (world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(-1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty) // E - Top Front Left
			{
				if (world.GetTile(position + new Vector3(0, -1, -1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(-1, -1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(-1, 0, -1)) != (byte)World.Tiles.Empty) // if front/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(0, 0, size), new Vector3(0, 0, 0), new Vector3(size, 0, 0) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(0, 0, size), new Vector3(0, size, 0), new Vector3(size, 0, 0) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 3, ySize * 2) });
				}
			}
			if (world.GetTile(position + new Vector3(0, 0, -1)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(1, 0, 0)) != (byte)World.Tiles.Empty
				&& world.GetTile(position + new Vector3(0, -1, 0)) != (byte)World.Tiles.Empty) // E - Top Front Right
			{
				if (world.GetTile(position + new Vector3(0, -1, -1)) == (byte)World.Tiles.Empty && world.GetTile(position + new Vector3(1, -1, 0)) == (byte)World.Tiles.Empty)
				{
					if (world.GetTile(position + new Vector3(1, 0, -1)) != (byte)World.Tiles.Empty) // if front/left are connected
						vertices2.AddRange(new Vector3[] { new Vector3(size, 0, 0), new Vector3(size * 2, 0, 0), new Vector3(size * 2, 0, size) });
					else
						vertices2.AddRange(new Vector3[] { new Vector3(size, 0, 0), new Vector3(size * 2, size, 0), new Vector3(size * 2, 0, size) });
					uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 2, ySize * 3) });
				}
			}


			for (int i = vertices.Count; i < vertices2.Count + vertices.Count; i += 3)
			{
				triangles.Add(i);
				triangles.Add(i + 1);
				triangles.Add(i + 2);
			}
			vertices.AddRange(vertices2);


			Mesh mesh = new Mesh();
			mesh.Clear();
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.Optimize();
			mesh.RecalculateNormals();
			return mesh;
		}

        return null;
    }


}
