using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile {

	public Sprite[] roadSprites;
	public Sprite preview {
		get {
			return roadSprites[6];
		}	
	}

	public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				if (x == 0 && y == 0)
					continue;
				Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
				if (HasRoadTile(tilemap, nPos))
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
				
				if (HasRoadTile(tilemap, new Vector3Int(position.x + x, position.y + y, position.z))) {
					composition += 'R';
				} else {
					composition += 'E';
				}
				// composition += HasRoadTile(
				// 	tilemap, new Vector3Int(position.x + x, position.y + y, position.z)
				// 	) ? 'R' : 'E';

			}
		}

		// Debug.Log(composition);

		Sprite intersection = roadSprites[6];
		Sprite closedLeft = roadSprites[4];
		Sprite closedRight = roadSprites[5];
		Sprite closedUp = roadSprites[10];
		Sprite closedDown = roadSprites[9];
		// Sprite horizontal = roadSprites[0];
		// Sprite vertical = roadSprites[7];
		Sprite horizontal = roadSprites[Random.Range(0, 3)];
		Sprite vertical = roadSprites[Random.Range(7, 8)];

		bool right = composition[6] == 'R';
		bool left = composition[1] == 'R';
		bool down = composition[3] == 'R';
		bool up = composition[4] == 'R';

		// string debugString = "----\n|";
		// debugString += composition[2];
		// debugString += composition[4];
		// debugString += composition[7];
		// debugString += "|\n|";
		// debugString += composition[1];
		// debugString += '#';
		// debugString += composition[6];
		// debugString += "|\n|";
		// debugString += composition[0];
		// debugString += composition[3];
		// debugString += composition[5];
		// debugString += "|\n----\n";
        // System.Console.WriteLine(debugString);


        if (right && left && !up && !down) {
			tileData.sprite = horizontal;
		} else if (up && down && !left && !right) {
			tileData.sprite = vertical;
		} else if (right && !left && !up && !down) {
			tileData.sprite = closedLeft;
		} else if (left && !right && !up && !down) {
			tileData.sprite = closedRight;
		} else if (up && !down && !right && !left) {
			tileData.sprite = closedDown;
		} else if (down && !up && !right && !left) {
			tileData.sprite = closedUp;
		} else if ((up || down) && (right || left)) {
			tileData.sprite = intersection;
		// } else if ()
		} else {
			tileData.sprite = intersection;
		}
		
	}

	private bool HasRoadTile(ITilemap tilemap, Vector3Int position) {
		return tilemap.GetTile(position) == this;
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/RoadTile")]
	public static void CreateRoadTile() {
		string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RoadTile>(), path);
	}
#endif

}
