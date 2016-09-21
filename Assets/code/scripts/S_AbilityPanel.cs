using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class S_AbilityPanel : MonoBehaviour{
    public int width, height;
	public List<S_Ability> abilities = new List<S_Ability>();
	public S_Ability activeAbility;
	public void Setup(int[] headerData) {
		List<S_Button> buttons = new List<S_Button>();
        name = "Ability Panel";
		int numAbilities = Util.ProperSign(headerData[0]) + Util.ProperSign(headerData[1]) + Util.ProperSign(headerData[2]);

		//
		for (int i= 0;i< headerData.Length;i++) {
			int datum = headerData[i];
            if (datum == 0) continue;
            S_Button buttonScrip = S_Button.CreateButton(Sprites.ability_border);
            S_Ability ability = null;
            switch (datum & 3) {
                case 1: ability = buttonScrip.gameObject.AddComponent<S_Ability_Move3>(); break;
                case 2: ability = buttonScrip.gameObject.AddComponent<S_Ability_Eye>(); break;
				case 3: ability = buttonScrip.gameObject.AddComponent<S_Ability_Swap>(); break;
			}
			abilities.Add(ability);
            ability.init((datum & 28)>>2);
            buttonScrip.SetDownAction(() => {
				if (Game.IsPaused()) return;
                ability.Click();
            });
            GameObject button = buttonScrip.gameObject;
            button.name = "Button";
            Util.SetColour(button, Colours.RED);
            buttons.Add(button.GetComponent<S_Button>());
            S_Camera.SetupScale(buttonScrip.transform);
            GameObject image = Primitives.CreateActor(ability.GetSprite(), 1, 9);
            image.name = "image";
            Util.SetLayer(image, Util.LayerName.Tiles, 0);
            image.transform.SetParent(buttonScrip.gameObject.transform, false);
			if (Game.KEYBOARD) { 
				GameObject text = Primitives.CreateText("["+ability.GetKey()+"]", 0, 0);
				S_Follower follower = text.AddComponent<S_Follower>();
				follower.Follow(button, -20*S_Camera.scale, (int)((Sprites.GetBounds(Sprites.ability_border).y * S_Camera.scale /2f - 7*S_Camera.scale)));
				ability.SetText(text);
			}
			Util.SetZ(button, Util.ZLayer.Gameplay);
		}

        int gap = 10 * S_Camera.scale;
        int currentY = -gap;
        foreach (S_Button butt in buttons) {
            currentY += gap;
            butt.transform.position = new Vector2(0, currentY);
            currentY += (int)(butt.GetBounds().size.y);
        }

        width = 37 * S_Camera.scale;;
        height = currentY;
        foreach (S_Button butt in buttons) {
            butt.transform.SetParent(transform, false);
        }
    }

	public void Cleanup() {
		foreach (S_Ability ability in abilities) {
			ability.ClearText();
		}
	}

	public void ActivateAbilityFromKeypress(KeyCode key) {
		foreach (S_Ability a in abilities) {
			if (a.GetKey() == key) {
				a.Click();
				return;
			}
		}
	}

	internal bool DeselectAbility(S_Ability s_Ability, bool sound = true) {
		if (activeAbility == null) return true;
		bool same = s_Ability == activeAbility;
		activeAbility.Toggle(s_Ability == activeAbility && sound);
		return !same;
	}

	internal void ActivateAbility(S_Ability ability, bool active) {
		if (active) {
			activeAbility = ability;
		}
		else {
			activeAbility = null;
		}
        Level.Get(gameObject).UpdateGridHighlightedness();
	}

	internal void Flash(bool flash) {
		if (abilities.Count == 1) abilities[0].GetComponent<S_Button>().SetFlashing(flash);
	}
}
