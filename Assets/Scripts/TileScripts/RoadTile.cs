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

    private Sprite intersection {
        get {
            return roadSprites[6];
        }
    }

    private Sprite closedLeft { get { return roadSprites[4]; } }
    private Sprite closedRight { get { return roadSprites[5]; } }
    private Sprite closedUp { get { return roadSprites[10]; } }
    private Sprite closedDown { get { return roadSprites[9]; } }
    private Sprite horizontal { get { return GetRandomHorizontalSprite(); } }
    private Sprite vertical { get { return GetRandomVerticalSprite(); } }

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
				
				//if (HasRoadTile(tilemap, new Vector3Int(position.x + x, position.y + y, position.z))) {
				//	composition += 'R';
				//} else {
				//	composition += 'E';
				//}
				 composition += HasRoadTile(
				 	tilemap, new Vector3Int(position.x + x, position.y + y, position.z)
				 	) ? 'R' : 'E';

			}
		}

        // Debug.Log(composition);

		bool right = composition[6] == 'R';
		bool left = composition[1] == 'R';
		bool down = composition[3] == 'R';
		bool up = composition[4] == 'R';

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
		} else {
			tileData.sprite = intersection;
		}
		
	}

	private bool HasRoadTile(ITilemap tilemap, Vector3Int position) {
		return tilemap.GetTile(position) == this;
	}

    private Sprite GetRandomHorizontalSprite()
    {
        int random = Random.Range(0, 100);
        if (random <= 5)
            return roadSprites[3];
        else if (random > 5 && random <= 10)
            return roadSprites[2];
        else if (random > 10 && random <= 15)
            return roadSprites[1];
        else
            return roadSprites[0];
    }

    private Sprite GetRandomVerticalSprite()
    {
        int random = Random.Range(0, 100);
        if (random < 30)
            return roadSprites[8];
        return roadSprites[7];
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
