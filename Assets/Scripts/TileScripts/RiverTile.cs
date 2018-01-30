using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RiverTile : Tile {

	[SerializeField]
	public Sprite[] riverSprites;
	[SerializeField]
	public Sprite preview;

	public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				if (x == 0 && y == 0)
					continue;
				Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
				if (HasRiverTile(tilemap, nPos))
					tilemap.RefreshTile(position);
			}
		}
	}

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
		base.GetTileData(position, tilemap, ref tileData);
		string composition = string.Empty;

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {

				if (x == 0 && y == 0)
					continue;
				
				// if (HasRiverTile(tilemap, new Vector3Int(position.x + x, position.y + y, position.z))) {
				// 	composition += 'R';
				// } else {
				// 	composition += 'E';
				// }
				composition += HasRiverTile(
					tilemap, new Vector3Int(position.x + x, position.y + y, position.z)
					) ? 'R' : 'E';

			}
		}

		// Debug.Log(composition);

		Sprite intersectionSprite = riverSprites[0];
		Sprite horizontalSprite = riverSprites[1];
		Sprite verticalSprite = riverSprites[Random.Range(2,3)];
		// Sprite verticalSprite = riverSprites[3];
		Sprite leftDownSprite = riverSprites[4];
		Sprite rightDownSprite = riverSprites[5];
		Sprite leftUpSprite = riverSprites[6];
		Sprite rightUpSprite = riverSprites[7];

		bool right = composition[6] == 'R';
		bool left = composition[1] == 'R';
		bool down = composition[3] == 'R';
		bool up = composition[4] == 'R';
		// bool bottomLeft = composition[0] == 'R';
		// bool topLeft = composition[2] == 'R';
		// bool bottomRight = composition[5] == 'R';
		// bool topRight = composition[7] == 'R';

		Sprite sprite;
		if (right && left && !up && !down) 
		{
			sprite = horizontalSprite;
		}
		else if (right && !left && !up && !down)
		{
			sprite = intersectionSprite;
		}
		else if (left && !right && !up && !down)
		{
			sprite = intersectionSprite;
		}
		else if (left && right && up && down)
		{
			sprite = intersectionSprite;
		}
		else if (up && down && !left && !right)
		{
			sprite = verticalSprite;
		}
		else if (left && up && !right && !down)
		{
			sprite = leftUpSprite;
		}
		else if (right && up && !left && !down)
		{
			sprite = rightUpSprite;
		}
		else if (right && down && !left && !up)
		{	
			sprite = rightDownSprite;
		}
		else if (left && down && !right && !up)
		{
			sprite = leftDownSprite;
		}
		else
		{
			sprite = intersectionSprite;
		}

		tileData.sprite = sprite;
		
	}

	private bool HasRiverTile(ITilemap tilemap, Vector3Int position) {
		return tilemap.GetTile(position) == this;
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/RiverTile")]
	public static void CreateRiverTile() {
		string path = EditorUtility.SaveFilePanelInProject("Save River Tile", "New River Tile", "Asset", "Save River Tile", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RiverTile>(), path);
	}
#endif

}
