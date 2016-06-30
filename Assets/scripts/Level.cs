using UnityEngine;
using System.Collections;
using System;

public class Level{
    public S_Tile[,] grid;
    public S_Player player;
    int tilesAcross = 3;
    int tilesDown = 3;
    int pickups = 6;
    public Level() {
        GameObject parent = new GameObject("wowee");

        grid = new S_Tile[tilesAcross, tilesDown];
        for(int x = 0; x < grid.GetLength(0); x++) {
            for(int y = 0; y < grid.GetLength(1); y++) {
                GameObject tile = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/tile")));
                S_Tile tileScript = tile.GetComponent<S_Tile>();
                grid[x, y] = tileScript;
                tileScript.SetPosition(x, y);
                tileScript.transform.SetParent(parent.transform);
                if(x!=1 || y!=1) {
                    tileScript.AddPickup();
                }
            }
        }
        GameObject rect = Primitives.CreateRectangle(tilesAcross * S_Tile.width + S_Camera.scale, tilesDown * S_Tile.height + S_Camera.scale, Colours.RED);
        rect.transform.SetParent(parent.transform);
        rect.GetComponent<SpriteRenderer>().sortingLayerName = "Tiles";

        GameObject playerObject = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/player")));
        player = playerObject.GetComponent<S_Player>();
        player.transform.SetParent(parent.transform);
        player.MoveToTile(grid[0, 1], true);

       
        int gridWidth = (int)(grid.GetLength(0) * S_Tile.width) + S_Camera.scale;
        int gridHeight = (int)(grid.GetLength(1) * S_Tile.height) + S_Camera.scale;
        parent.transform.position = new Vector3((int)(Screen.width / 2 - gridWidth / 2), (int)(Screen.height / 2 - gridHeight / 2), 0);


    }

    internal void Pickup(S_Pickup pickup) {
        pickups--;
    }
}
