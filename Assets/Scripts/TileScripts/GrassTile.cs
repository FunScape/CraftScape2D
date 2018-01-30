using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrassTile : Tile {

	public Sprite[] grassSprites;
	public Sprite preview;

	public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
		base.RefreshTile(position, tilemap);
	}

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
		base.GetTileData(position, tilemap, ref tileData);
		// 15% chance of tile being a flower tile
		if (Random.Range(0f, 100f) < 15f) {
			// tileData.sprite = grassSprites[1];
			bool hasFlowerNeighbor = false;
			for (int y = -1; y <= 1; y++) {
				for (int x = -1; x <= 1; x++) {
					Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
					if (tilemap.GetSprite(nPos) == grassSprites[1])
						hasFlowerNeighbor = true;
				}
			}
			if (!hasFlowerNeighbor) {
				tileData.sprite = grassSprites[1];
			}
		}
	}

	private bool isFlowerTile(ref TileData tileData) {
		return tileData.sprite == grassSprites[1];
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/GrassTile")]
	public static void CreateGrassTile() {
		string path = EditorUtility.SaveFilePanelInProject("Save Grass Tile", "New Grass Tile", "Asset", "Save Grass Tile", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GrassTile>(), path);
	}
#endif

}
