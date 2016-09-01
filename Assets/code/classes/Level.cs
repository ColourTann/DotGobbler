﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class Level {
	List<S_Entity> entities = new List<S_Entity>();
	GameObject map;
	S_Slider slider;
	public S_Tile[,] tiles;
	List<S_Tile> allTiles = new List<S_Tile>();
	public S_Player player;
	int tilesAcross;
	int tilesDown;
	public int pickups = 0;
	public int totalPickups = 0;
	Texture2D levelData;
	GameObject grid;
	public S_Ability activeAbility;
	public static int[][] pitches =
		{
		new int[] {12},
		new int[] {0,12},
		new int[] {0,7,12},
		new int[] {0,4,7,12},
		new int[] {0,4,7,10,12},
		new int[] {0,2,5,7,10,12},
		new int[] {0,2,4,7,9,11,12},
		new int[] {0,2,4,5,7,9,11,12},
		new int[] {0,2,4,5,7,8,9,11,12},
		new int[] {0,2,4,5,7,8,9,10,11,12},
		new int[] {0,2,4,5,6,7,8,9,10,11,12},
		new int[] {0,2,3,4,5,6,7,8,9,10,11,12},
		new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12},
		};

	public Level(Texture2D levelData) {
		this.levelData = levelData;
	}

	public void Init() {
		InitLayoutStuff();
		InitTiles();
		foreach (S_Entity entity in entities) {
			entity.ChooseMove();
			entity.InstantFace();
		}
	}

	internal void DeselectAbility(S_Ability s_Ability) {
		if (activeAbility == null || activeAbility == s_Ability) return;
		activeAbility.Toggle(false);
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

		int gridWidth = (int)(tiles.GetLength(0) * (S_Tile.BASE_TILE_SIZE + 1) * S_Camera.scale + S_Camera.scale);
		int gridHeight = (int)(tiles.GetLength(1) * (S_Tile.BASE_TILE_SIZE + 1) * S_Camera.scale + S_Camera.scale);

		//header stuff for extra data
		Color header = (levelData.GetPixel(0, levelData.height - 1));
		int headerData = (int)(header.r * 255);
		bool hasAbility = (headerData & 1) > 0;
		int currentX = 0;
		int gap = 0;
		if (hasAbility) {
			GameObject abilityObject = new GameObject();
			S_AbilityPanel abilityPanel = abilityObject.AddComponent<S_AbilityPanel>();
			abilityPanel.Setup(headerData);
			abilityPanel.transform.SetParent(slider.transform);
			gap = (Screen.width - (gridWidth + abilityPanel.width)) / 3;
			currentX += gap;
			abilityPanel.transform.position = new Vector2(currentX, (int)(Screen.height / 2 - abilityPanel.height / 2));
			currentX += abilityPanel.width;
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
				//Color32[] data = levelData.GetPixels32();

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
						totalPickups++;
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
		allTiles.Add(tileScript);
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
			Debug.Log("next");
			Game.Get().NextLevel();
		}
	}

	public void Turn() {
		foreach (S_Entity ent in entities) {
			ent.TakeTurn();
		}
		foreach (S_Entity ent in entities) {
			ent.ChooseMove();
		}
		Game.Get().EndOfTurn();
	}

	public S_Tile GetTile(int x, int y) {
		if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1)) {
			return null;
		}
		return tiles[x, y];
	}

	internal void ActivateAbility(S_Ability ability, bool active) {
		if (active) {
			activeAbility = ability;
		}
		else {
			activeAbility = null;
		}
		UpdateGridHighlightedness();
	}

	List<GameObject> highlightRectangles = new List<GameObject>();

	internal void UpdateGridHighlightedness() {
		foreach (GameObject go in highlightRectangles) {
			GameObject.Destroy(go);
		}
		highlightRectangles.Clear();
		foreach (S_Tile tile in allTiles) {
			tile.SetHighlight(false);
		}
		if (activeAbility != null) {
			switch (activeAbility.GetTargetingType()) {
				case S_Ability.TargetingType.Line:
					foreach (S_Tile tile in activeAbility.GetValidTiles(player.currentTile)) {
						int baseWidth = tile.x - player.currentTile.x;
						int baseHeight = tile.y - player.currentTile.y;
						int rWidth = baseWidth == 0 ? 1 : baseWidth;
						int rHeight = baseHeight == 0 ? 1 : baseHeight;
						rWidth *= S_Tile.height;
						rHeight *= S_Tile.width;
						rWidth += -S_Camera.scale * Util.ProperSign(rWidth);
						rHeight += -S_Camera.scale * Util.ProperSign(rHeight);
						GameObject go = Primitives.CreateRectangle(rWidth, rHeight, S_Camera.scale, Colours.LIGHT);
						Util.SetLayerContainer(go, Util.LayerName.Entities, 5);
						S_Tile origin = player.currentTile;
						if (baseWidth > 0) {
							origin = origin.GetTile(1, 0);
						}
						else if (baseHeight > 0) {
							origin = origin.GetTile(0, 1);
						}
						go.transform.position = new Vector2(origin.transform.position.x + (baseWidth < 0 ? -S_Camera.scale : 0), origin.transform.position.y + (baseHeight < 0 ? -S_Camera.scale : 0));
						highlightRectangles.Add(go);
						go.transform.SetParent(Game.GetMisc("effects").transform);

					}
					break;

				case S_Ability.TargetingType.SingleTile:
					foreach (S_Tile tile in activeAbility.GetValidTiles(player.currentTile)) {
						tile.SetHighlight(true);
					}
					break;
			}
		}
	}
}
