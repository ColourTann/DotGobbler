using UnityEngine;
using System.Collections.Generic;



public class Level {

    List<S_Entity> entities = new List<S_Entity>();

    GameObject parent;

    S_Slider slider;
    public S_Tile[,] grid;
    public S_Player player;
    int tilesAcross;
    int tilesDown;
    int pickups = 0;
    string[] levelData;
    public Level(int levelNumber) {
        levelData = System.IO.File.ReadAllText("Assets/Resources/data/1.level").Split('\n');
        tilesAcross = levelData[0].Length-1;
        tilesDown = levelData.Length;
        parent = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/slider")));
        slider = parent.GetComponent<S_Slider>();
        grid = new S_Tile[tilesAcross, tilesDown];

        
        Debug.Log(tilesAcross + ":" + S_Tile.width);

       
    }

    public S_Tile MakeTile(int x, int y) {
        GameObject tile = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/tile")));
        S_Tile tileScript = tile.GetComponent<S_Tile>();
        grid[x, y] = tileScript;
        tileScript.SetPosition(x, y);
        tileScript.transform.SetParent(parent.transform);
        return tileScript;
    }

    public void SlideIn() {
        int gridWidth = (int)(grid.GetLength(0) * S_Tile.width) + S_Camera.scale;
        int gridHeight = (int)(grid.GetLength(1) * S_Tile.height) + S_Camera.scale;
        int goodX = (int)(Screen.width / 2 - gridWidth / 2);
        int goodY = (int)(Screen.height / 2 - gridHeight / 2);
        slider.transform.position = new Vector3(Screen.width, goodY, 0);
        slider.SlideTo(goodX, goodY, .3f);
    }

    public void DeleteSelf() {
        GameObject.Destroy(parent);
    }

    public void SlideAway() {
        slider.SlideTo(-Screen.width, (int)slider.transform.position.y, .3f, DeleteSelf);
    }

    public void Init() {
        for (int x = 0; x < tilesAcross; x++) {
            for (int y = 0; y < tilesDown; y++) {
                S_Tile tile;
                switch (levelData[tilesDown-y-1][x]) {
                    case '0':
                        break;
                    case '1':
                        tile = MakeTile(x, y);
                        break;
                    case '2':
                        tile = MakeTile(x, y);
                        tile.AddPickup();
                        pickups++;
                        break;
                    case '3':
                        tile = MakeTile(x, y);
                        GameObject playerObject = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/player")));
                        player = playerObject.GetComponent<S_Player>();
                        player.Init();
                        player.PositionSetter.transform.SetParent(parent.transform);
                        entities.Add(player);
                        player.MoveToTile(tile, true);
                        break;
                    case '4':
                        tile = MakeTile(x, y);
                        GameObject pincerObject = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/pincer")));
                        S_Entity enemy = pincerObject.GetComponent<S_Pincer>();
                        enemy.Init();
                        enemy.PositionSetter.transform.SetParent(parent.transform);
                        entities.Add(enemy);
                        enemy.MoveToTile(tile, true);
                        break;
                }
            }
        }
        GameObject rect = Primitives.CreateRectangle(tilesAcross * S_Tile.width + S_Camera.scale, tilesDown * S_Tile.height + S_Camera.scale, Colours.RED);
        rect.transform.SetParent(parent.transform);
        rect.GetComponent<SpriteRenderer>().sortingLayerName = "Tiles";
        rect.GetComponent<SpriteRenderer>().sortingOrder = 0;

    }

    internal void Pickup(S_Pickup pickup) {
        pickups--;
        if (pickups == 0) {
            Game.Get().NextLevel();
        }
    }

    public void Turn() {
        foreach (S_Entity ent in entities) {
            ent.TakeTurn();
        }
    }

    public S_Tile GetTile(int x, int y) {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) {
            return null;
        }
        return grid[x, y];
    }
}
