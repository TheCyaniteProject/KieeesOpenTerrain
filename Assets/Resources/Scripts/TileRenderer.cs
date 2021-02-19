using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TileRenderer
{
	public static Mesh MakeCube(float[] heights, int[] sides)
	{
		float size = 0.5f;

		List<Vector3> vertices = new List<Vector3>();
		if (sides[1] == 1) // Front
        {
			vertices.AddRange(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, size, 0), new Vector3(size, size, 0), new Vector3(size, 0, 0)});
			vertices.AddRange(new Vector3[] { new Vector3(0, size, 0), new Vector3(0, size * 2, 0), new Vector3(size, size * 2, 0), new Vector3(size, size, 0) });
			vertices.AddRange(new Vector3[] { new Vector3(size, size, 0), new Vector3(size, size * 2, 0), new Vector3(size * 2, size * 2, 0), new Vector3(size * 2, size, 0) });
			vertices.AddRange(new Vector3[] { new Vector3(size, 0, 0), new Vector3(size, size, 0), new Vector3(size * 2, size, 0), new Vector3(size * 2, 0, 0) });
		}
		if (sides[0] == 1) // Back
		{
			vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, size * 2), new Vector3(size * 2, size, size * 2), new Vector3(size, size, size * 2), new Vector3(size, 0, size * 2) });
			vertices.AddRange(new Vector3[] { new Vector3(size * 2, size, size * 2), new Vector3(size * 2, size * 2, size * 2), new Vector3(size, size * 2, size * 2), new Vector3(size, size, size * 2) });
			vertices.AddRange(new Vector3[] { new Vector3(size, size, size * 2), new Vector3(size, size * 2, size * 2), new Vector3(0, size * 2, size * 2), new Vector3(0, size, size * 2) });
			vertices.AddRange(new Vector3[] { new Vector3(size, 0, size * 2), new Vector3(size, size, size * 2), new Vector3(0, size, size * 2), new Vector3(0, 0, size * 2) });
		}
		if (sides[2] == 1) // Top
		{
			vertices.AddRange(new Vector3[] { new Vector3(0, (size * 2) + heights[1], size), new Vector3(size, size * 2, size), new Vector3(size, size * 2, 0), new Vector3(0, size * 2, 0) });
			vertices.AddRange(new Vector3[] { new Vector3(size, size * 2, size), new Vector3(size * 2, size * 2, size), new Vector3(size * 2, size * 2, 0), new Vector3(size, size * 2, 0) });
			vertices.AddRange(new Vector3[] { new Vector3(0, size * 2, size * 2), new Vector3(size, size * 2, size * 2), new Vector3(size, size * 2, size), new Vector3(0, (size * 2) + heights[1], size) });
			vertices.AddRange(new Vector3[] { new Vector3(size, size * 2, size * 2), new Vector3(size * 2, size * 2, size * 2), new Vector3(size * 2, size * 2, size), new Vector3(size, size * 2, size) });
		}
		if (sides[3] == 1) // Bottom
		{
			vertices.AddRange(new Vector3[] { new Vector3(size * 2,  0, size * 2), new Vector3(size,  0, size * 2), new Vector3(size, 0,  size), new Vector3(size * 2, 0,  size) });
			vertices.AddRange(new Vector3[] { new Vector3(size,  0, size * 2), new Vector3(0,  0, size * 2), new Vector3(0, 0,  size), new Vector3(size, 0,  size) });
			vertices.AddRange(new Vector3[] { new Vector3(size,  0, size), new Vector3(0,  0, size), new Vector3(0, 0,  0), new Vector3(size, 0,  0) });
			vertices.AddRange(new Vector3[] { new Vector3(size * 2,  0, size), new Vector3(size,  0, size), new Vector3(size, 0,  0), new Vector3(size * 2, 0,  0) });
		}
		if (sides[4] == 1) // Left
		{
			vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, 0), new Vector3(size * 2, size, 0), new Vector3(size * 2, size, size), new Vector3(size * 2, 0, size) });
			vertices.AddRange(new Vector3[] { new Vector3(size * 2, size, 0), new Vector3(size * 2, size * 2, 0), new Vector3(size * 2, size * 2, size), new Vector3(size * 2, size, size) });
			vertices.AddRange(new Vector3[] { new Vector3(size * 2, size, size), new Vector3(size * 2, size * 2, size), new Vector3(size * 2, size * 2, size * 2), new Vector3(size * 2, size, size * 2) });
			vertices.AddRange(new Vector3[] { new Vector3(size * 2, 0, size), new Vector3(size * 2, size, size), new Vector3(size * 2, size, size * 2), new Vector3(size * 2, 0, size * 2) });
		}
		if (sides[5] == 1) // Right
		{

			vertices.AddRange(new Vector3[] { new Vector3(0, 0, size * 2), new Vector3(0, size, size * 2), new Vector3(0, size, size), new Vector3(0, 0, size) });
			vertices.AddRange(new Vector3[] { new Vector3(0, size, size * 2), new Vector3(0, size * 2, size * 2), new Vector3(0, (size * 2) + heights[1], size), new Vector3(0, size, size) });
			vertices.AddRange(new Vector3[] { new Vector3(0, size, size), new Vector3(0, (size * 2) + heights[1], size), new Vector3(0, size * 2, 0), new Vector3(0, size, 0) });
			vertices.AddRange(new Vector3[] { new Vector3(0, 0, size), new Vector3(0, size, size), new Vector3(0, size, 0), new Vector3(0, 0, 0) });
		}

		int[] triangles = new int[vertices.Count / 4 * 2 * 3];
		int iPos = 0;
		for (int i = 0; i < vertices.Count; i = i + 4)
		{
			triangles[iPos++] = i;
			triangles[iPos++] = i + 1;
			triangles[iPos++] = i + 2;
			triangles[iPos++] = i;
			triangles[iPos++] = i + 2;
			triangles[iPos++] = i + 3;
		}

		float xSize = 1f / 8f;
		float ySize = 1f / 6f;

		List<Vector2> uvs = new List<Vector2>();
		if (sides[0] == 1) // Front
		{
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 4, ySize * 6), new Vector2(xSize * 4, ySize * 5), new Vector2(xSize * 3, ySize * 5), new Vector2(xSize * 3, ySize * 6) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 4, ySize * 5), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 3, ySize * 5) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 5), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 2, ySize * 5) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 6), new Vector2(xSize * 3, ySize * 5), new Vector2(xSize * 2, ySize * 5), new Vector2(xSize * 2, ySize * 6) });
		}
		if (sides[1] == 1) // Back
		{
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 0), new Vector2(xSize * 2, ySize * 1), new Vector2(xSize * 3, ySize * 1), new Vector2(xSize * 3, ySize * 0) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 1), new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 3, ySize * 1) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 1), new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 4, ySize * 2), new Vector2(xSize * 4, ySize * 1) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 0), new Vector2(xSize * 3, ySize * 1), new Vector2(xSize * 4, ySize * 1), new Vector2(xSize * 4, ySize * 0) });
		}
		if (sides[2] == 1) // Top
		{
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 2), new Vector2(xSize * 3, ySize * 3), new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 4, ySize * 2) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 3, ySize * 3), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 4, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 3, ySize * 3), new Vector2(xSize * 3, ySize * 2) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 3, ySize * 4), new Vector2(xSize * 3, ySize * 3) });
		}
		if (sides[3] == 1) // Bottom
		{
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 2), new Vector2(xSize * 6, ySize * 3), new Vector2(xSize * 7, ySize * 3), new Vector2(xSize * 7, ySize * 2) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 3), new Vector2(xSize * 6, ySize * 4), new Vector2(xSize * 7, ySize * 4), new Vector2(xSize * 7, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 7, ySize * 3), new Vector2(xSize * 7, ySize * 4), new Vector2(xSize * 8, ySize * 4), new Vector2(xSize * 8, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 7, ySize * 2), new Vector2(xSize * 7, ySize * 3), new Vector2(xSize * 8, ySize * 3), new Vector2(xSize * 8, ySize * 2) });
		}
		if (sides[4] == 1) // Left
		{
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 2), new Vector2(xSize * 5, ySize * 2), new Vector2(xSize * 5, ySize * 3), new Vector2(xSize * 6, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 5, ySize * 2), new Vector2(xSize * 4, ySize * 2), new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 5, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 5, ySize * 3), new Vector2(xSize * 4, ySize * 3), new Vector2(xSize * 4, ySize * 4), new Vector2(xSize * 5, ySize * 4) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 6, ySize * 3), new Vector2(xSize * 5, ySize * 3), new Vector2(xSize * 5, ySize * 4), new Vector2(xSize * 6, ySize * 4) });
		}
		if (sides[5] == 1) // Right
		{
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 0, ySize * 4), new Vector2(xSize * 1, ySize * 4), new Vector2(xSize * 1, ySize * 3), new Vector2(xSize * 0, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 1, ySize * 4), new Vector2(xSize * 2, ySize * 4), new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 1, ySize * 3) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 1, ySize * 3), new Vector2(xSize * 2, ySize * 3), new Vector2(xSize * 2, ySize * 2), new Vector2(xSize * 1, ySize * 2) });
			uvs.AddRange(new Vector2[] { new Vector2(xSize * 0, ySize * 3), new Vector2(xSize * 1, ySize * 3), new Vector2(xSize * 1, ySize * 2), new Vector2(xSize * 0, ySize * 2) });
		}

		
		
		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles;
		mesh.uv = uvs.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();
		return mesh;
	}
}
