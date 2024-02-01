using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Utilities
{
	public static class TileMapHelper
	{
		public static void ChangeTile(Tilemap tilemap, Tile oldTile , Tile newTile)
		{
			BoundsInt bounds = tilemap.cellBounds;

			for (int x = bounds.min.x; x < bounds.max.x; x++)
			{
				for (int y = bounds.min.y; y < bounds.max.y; y++)
				{
					Vector3Int cellPosition = new Vector3Int(x, y, 0);
					Tile tile = tilemap.GetTile<Tile>(cellPosition);
					if (tile != null)
					{
						if (oldTile.Equals(tile)) tilemap.SetTile(cellPosition, newTile);
					}
				}
			}
			tilemap.RefreshAllTiles();
		}
	}
}
