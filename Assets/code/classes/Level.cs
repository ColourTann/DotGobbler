using UnityEngine;
using System.Collections.Generic;
using System;

public class Level : MonoBehaviour{
	public S_Slider slider;
	GameObject mapObject;
	GameObject grid;
	int gridWidth, gridHeight;

	Texture2D levelData;
	public S_Tile[,] tiles;
	List<S_Tile> allTiles = new List<S_Tile>();
	List<S_Entity> entities = new List<S_Entity>();
	public S_Player player;

	public int pickupsRemaining = 0;
	public int totalPickups = 0;

	public S_AbilityPanel abilityPanel;
	GameObject tutorialAnimation;


	public void Init(Texture2D levelData) {
        this.levelData = levelData;
        InitLayoutStuff();
		InitTilesAndEntities();
		InitAbilities();
		InitTutorial();
		EnemySetup();
	}

	void InitLayoutStuff() {
		//initialise slider
		GameObject parent = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/slider")));
		slider = parent.GetComponent<S_Slider>();
		slider.name = "level";
        slider.transform.SetParent(transform, false);

		//initialise map and parent it to slider
		mapObject = new GameObject();
		mapObject.name = "map";
		mapObject.transform.SetParent(slider.transform, false);

		//initialise grid and parent it to map
		grid = new GameObject();
		grid.name = "grid";
		grid.transform.SetParent(mapObject.transform);
	}

	void InitTilesAndEntities() {
		//init tiles array
		int tilesAcross = levelData.width;
		int tilesDown = levelData.height - 1;
		tiles = new S_Tile[tilesAcross, tilesDown];
		gridWidth = (int)(tilesAcross * (S_Tile.BASE_TILE_SIZE + 1) * S_Camera.scale + S_Camera.scale);
		gridHeight = (int)(tilesDown * (S_Tile.BASE_TILE_SIZE + 1) * S_Camera.scale + S_Camera.scale);

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
						pickupsRemaining++;
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
					case LevelContent.charger:
						tile = MakeTile(x, y);
						GameObject chargerObject = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/charger")));
						S_Entity charger = chargerObject.GetComponent<S_Charger>();
						charger.Init();
						entities.Add(charger);
						charger.MoveToTile(tile, true);
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
		entityParent.transform.SetParent(mapObject.transform, false);
		foreach (S_Entity e in entities) {
			e.positionSetter.transform.SetParent(entityParent.transform, false);
		}

		//setup map border
		GameObject rect = Primitives.CreateRectangle(tilesAcross * S_Tile.width + S_Camera.scale, tilesDown * S_Tile.height + S_Camera.scale, Colours.RED);
		rect.transform.SetParent(mapObject.transform, false);
		rect.GetComponent<SpriteRenderer>().sortingLayerName = "Tiles";
		rect.GetComponent<SpriteRenderer>().sortingOrder = 0;
		rect.name = "level_background";

		//add level number (rethink into something symbolic maybe?)
		levelNumberObject = Primitives.CreateText(("level " + (Game.Get().levelNumber)));
		S_Follower follow = levelNumberObject.AddComponent<S_Follower>();
		follow.Follow(slider.gameObject, 2 * S_Camera.scale, Screen.height - 28 * S_Camera.scale);
	}

	private void InitAbilities() {
		//header stuff for extra data
		Color header = (levelData.GetPixel(0, levelData.height - 1));
		int[] abilityHeaderData = new int[] { (int)(header.r * 255), (int)(header.g * 255), (int)(header.b * 255) };
		int currentX = 0;
		int gap = 0;
		bool hasAbility = (abilityHeaderData[0]) > 0;
		GameObject abilityPanelObject = new GameObject();
		abilityPanel = abilityPanelObject.AddComponent<S_AbilityPanel>();
		abilityPanel.transform.SetParent(slider.transform);
		if (hasAbility) {
			abilityPanel.Setup(abilityHeaderData);
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
		mapObject.transform.position = new Vector2(currentX, (int)(Screen.height / 2 - gridHeight / 2));
	}

	private void InitTutorial() {
		Color tutorialColour = (levelData.GetPixel(1, levelData.height - 1));
		int tutorialHeaderData = (int)(tutorialColour.r * 255);
		if (tutorialHeaderData == 1) {
			AddTutorial(TutorialType.Move, true);
		}
		if (tutorialHeaderData == 2) {
			AddTutorial(TutorialType.Ability, true);
		}
	}

	void EnemySetup() {
		foreach (S_Entity entity in entities) {
			entity.ChooseMove();
		}
	}

	GameObject levelNumberObject;

	public enum TutorialType { NOT, Move, Ability }

	public enum ActionType { SelectAbility, DeselectAbility, UseAbility }
	TutorialType currentTutorial = TutorialType.NOT;

	public void ActionCompleted(ActionType type) {
		if (currentTutorial == TutorialType.NOT) return;
		if (tutorialAnimation == null) return;
		switch (currentTutorial) {
			case TutorialType.Move: break;
			case TutorialType.Ability:
				switch (type) {
					case ActionType.SelectAbility:
						AddTutorial(TutorialType.Move, false);
						break;
					case ActionType.DeselectAbility:
						AddTutorial(TutorialType.Ability, false);
						break;
					case ActionType.UseAbility:
						currentTutorial = TutorialType.NOT;
						GameObject.Destroy(tutorialAnimation);
						tutorialAnimation = null;
						break;
				}
				break;
		}
	}

	public void AddTutorial(TutorialType type, bool initial) {
		if (initial) this.currentTutorial = type;
		if (tutorialAnimation != null) GameObject.Destroy(tutorialAnimation);
		Sprite[] sprites = null;
		switch (type) {
			case TutorialType.Move: sprites = Game.KEYBOARD ? Sprites.tutorial_0_keyboard : Sprites.tutorial_0_touch; break;
			case TutorialType.Ability: sprites = Game.KEYBOARD ? Sprites.tutorial_1_keyboard : Sprites.tutorial_1_touch; break;
		}
		int x = (int)(mapObject.transform.position.x + gridWidth / 2 - Sprites.GetBounds(sprites[0]).x / 2 * S_Camera.scale);
		if (type == TutorialType.Ability) {
			x = (int)(abilityPanel.gameObject.transform.position.x + Sprites.GetBounds(Sprites.ability_border).x * S_Camera.scale / 2 - Sprites.GetBounds(sprites[0]).x / 2 * S_Camera.scale);
		}
		int y = (int)(Screen.height / 2 + gridHeight / 2 + (Screen.height - gridHeight) / 4 - Sprites.GetBounds(sprites[0]).y / 2 * S_Camera.scale);
		tutorialAnimation = S_Animation.CreateAnimation(sprites, .45f, x, y);
		Util.SetLayer(tutorialAnimation, Util.LayerName.UI, 0);
		S_Camera.SetupScale(tutorialAnimation.transform);
		tutorialAnimation.transform.SetParent(slider.transform, false);
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

	public void Cleanup() {
		foreach (S_Ability ability in abilityPanel.abilities) {
			ability.ClearText();
		}
		GameObject.Destroy(slider.gameObject.transform.parent.gameObject);
		GameObject.Destroy(levelNumberObject);
	}

	public void SlideIn() {
		slider.SetPosition(Screen.width, 0);
		slider.SlideTo(0, 0, .3f);
	}

	public void SlideAway() {
		foreach (S_Entity e in entities) {
			e.Deactivate();
		}
		slider.SlideTo(-Screen.width, (int)slider.transform.position.y, .3f, Interpolation.InterpolationType.Pow2Out, Cleanup);
	}

	public enum LevelContent {
		blank, player, wall, enemy, food, charger
	}

	LevelContent FromColour(Color c) {
		if (c.a == 0) return LevelContent.blank;
		if (c == Colours.RED) return LevelContent.wall;
		if (c == Colours.LIGHT) return LevelContent.player;
		if (c == Colours.DARK) return LevelContent.wall;
		if (c == Colours.GREEN) return LevelContent.enemy;
		if (c == Colours.zWHITE) return LevelContent.food;
		if (c.r == 1 / 255f * 177) return LevelContent.charger; //extra pink colour added for chargers. Not part of the palette
		Debug.Log(c);
		return LevelContent.blank;
	}

	internal void Pickup(S_Pickup pickup) {
		pickupsRemaining--;
		GameObject.Destroy(pickup.gameObject);
		
		
		if (totalPickups > Sounds.nicePitches.Length) {
			Sounds.PlaySound(Sounds.pip, .9f, Mathf.Pow(1.05946f, ((float)(totalPickups - pickupsRemaining) / (totalPickups)) * 12));
		}
		else {
			Sounds.PlaySound(Sounds.pip, .9f, Mathf.Pow(1.05946f, Sounds.nicePitches[totalPickups - 1][totalPickups - pickupsRemaining - 1]));
		}
		if (pickupsRemaining == 0) {
			Game.Get().Victory();
		}
	}

	public void EnemyTurn() {
		//take the planned move
		foreach (S_Entity ent in entities) {
			ent.TakeTurn();
		}
		//plan the next turn
		foreach (S_Entity ent in entities) {
			ent.ChooseMove();
		}
		if (entities.Count == 1) Game.Get().CheckForEndOfLevel();
		//Game.Get().CheckForEndOfLevel();
	}

	public S_Tile GetTile(int x, int y) {
		if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1)) {
			return null;
		}
		return tiles[x, y];
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
		S_Ability activeAbility = abilityPanel.activeAbility;
		if (activeAbility != null) {
			//bunch of long codey bits because it's paperwork to do this sort of thing in unity it seems
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


    private static Dictionary<GameObject, Level> dict = new Dictionary<GameObject, Level>();
    public static Level Get(GameObject go) {
        if (dict.ContainsKey(go)) return dict[go];
        Level level = go.GetComponent<Level>();
        if (level == null) {
            level = Get(go.transform.parent.gameObject);
        }
        dict.Add(go, level);
        return level;
    }
}
