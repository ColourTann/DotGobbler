using UnityEngine;
using System.Collections.Generic;



public class Level {
    List<S_Entity> entities = new List<S_Entity>();
    GameObject map;
    S_Slider slider;
    public S_Tile[,] tiles;
    public S_Player player;
    int tilesAcross;
    int tilesDown;
    int pickups = 0;
    Texture2D levelData;
    GameObject grid;
    public Level(Texture2D levelData) {
        this.levelData = levelData;
    }

    public void Init() {
        InitLayoutStuff();
        InitTiles();
        foreach (S_Entity entity in entities) {
            entity.ChooseMove();
        }
    }

    void InitLayoutStuff() {
        //initialise slider
        GameObject parent = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/slider")));
        slider = parent.GetComponent<S_Slider>();
        slider.name = "level";

        //initialise map and parent it to slider
        map = new GameObject();
        map.name = "map";
        map.transform.SetParent(slider.transform, false);

        //initialise grid and parent it to map
        grid = new GameObject();
        grid.name = "grid";
        grid.transform.SetParent(map.transform);
    }

    void InitTiles() {
        //init tiles array
        tilesAcross = levelData.width;
        tilesDown = levelData.height - 1;
        tiles = new S_Tile[tilesAcross, tilesDown];

        int gridWidth = (int)(tiles.GetLength(0) * S_Tile.width) + S_Camera.scale;
        int gridHeight = (int)(tiles.GetLength(1) * S_Tile.height) + S_Camera.scale;

        //header stuff for extra data
        Color header = (levelData.GetPixel(0, levelData.height));
        int headerData = (int)(header.r * 255);
        bool hasAbility = (headerData & 1) > 0;
        int currentX = 0;
        int gap = 0;
        if (hasAbility) {
            GameObject abilityObject = new GameObject();
            S_AbilityPanel abilityPanel = abilityObject.AddComponent<S_AbilityPanel>();
            abilityPanel.Setup();
            abilityPanel.transform.SetParent(slider.transform);
            gap = (Screen.width - (gridWidth + S_AbilityPanel.WIDTH)) / 3;
            currentX += gap;
            abilityPanel.transform.position = new Vector2(currentX, (int)(Screen.height / 2 - S_AbilityPanel.HEIGHT / 2));
            currentX += S_AbilityPanel.WIDTH;
            currentX += gap;
        }
        else {
            gap = (Screen.width - gridWidth) / 2;
            currentX += gap;
        }

        //put map in the center based on tiles
      
        
        map.transform.position = new Vector2(currentX, (int)(Screen.height / 2 - gridHeight / 2));

       

        //use colours in leveldata to setup entities
        for (int x = 0; x < tilesAcross; x++) {
            for (int y = 0; y < tilesDown; y++) {
                S_Tile tile;
                switch (FromColour(levelData.GetPixel(x, y))) {
                    case LevelContent.wall:
                        break;
                    case LevelContent.blank:
                        tile = MakeTile(x, y);
                        break;
                    case LevelContent.food:
                        tile = MakeTile(x, y);
                        tile.AddPickup();
                        pickups++;
                        break;
                    case LevelContent.player:
                        tile = MakeTile(x, y);
                        GameObject playerObject = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/player")));
                        player = playerObject.GetComponent<S_Player>();
                        player.Init();
                        entities.Add(player);
                        player.MoveToTile(tile, true);
                        break;
                    case LevelContent.enemy:
                        tile = MakeTile(x, y);
                        GameObject pincerObject = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/pincer")));
                        S_Entity enemy = pincerObject.GetComponent<S_Pincer>();
                        enemy.Init();
                        entities.Add(enemy);
                        enemy.MoveToTile(tile, true);
                        break;
                }
            }
        }

        //hierarchy object for entities
        GameObject entityParent = new GameObject("entities");
        entityParent.transform.SetParent(map.transform, false);
        foreach (S_Entity e in entities) {
            e.PositionSetter.transform.SetParent(entityParent.transform, false);
        }

        //setup map border
        GameObject rect = Primitives.CreateRectangle(tilesAcross * S_Tile.width + S_Camera.scale, tilesDown * S_Tile.height + S_Camera.scale, Colours.RED);
        rect.transform.SetParent(map.transform, false);
        rect.GetComponent<SpriteRenderer>().sortingLayerName = "Tiles";
        rect.GetComponent<SpriteRenderer>().sortingOrder = 0;
        rect.name = "level_background";

    }

    public S_Tile MakeTile(int x, int y) {
        GameObject tile = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/tile")));
        S_Tile tileScript = tile.GetComponent<S_Tile>();
        tiles[x, y] = tileScript;
        tileScript.SetPosition(x, y);
        tileScript.transform.SetParent(grid.transform, false);
        tile.name = "tile " + x + ":" + y;
        return tileScript;
    }

    public void SlideIn() {

        slider.transform.position = new Vector3(Screen.width, 0, 0);
        slider.SlideTo(0, 0, .3f);
    }

    public void DeleteSelf() {
        GameObject.Destroy(slider.gameObject);
    }

    public void SlideAway() {
        foreach (S_Entity e in entities) {
            e.Deactivate();
        }
        slider.SlideTo(-Screen.width, (int)slider.transform.position.y, .3f, DeleteSelf);
    }

    public enum LevelContent {
        blank, player, wall, enemy, food
    }

    LevelContent FromColour(Color c) {
        if (c.a == 0) return LevelContent.blank;
        if (c == Colours.RED) return LevelContent.wall;
        if (c == Colours.LIGHT) return LevelContent.player;
        if (c == Colours.DARK) return LevelContent.wall;
        if (c == Colours.GREEN) return LevelContent.enemy;
        if (c == Colours.zWHITE) return LevelContent.food;
        Debug.Log(c);
        return LevelContent.blank;
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
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1)) {
            return null;
        }
        return tiles[x, y];
    }
}
