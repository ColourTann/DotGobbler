using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_AbilityPanel : MonoBehaviour{

    public int width, height;
    List<S_Button> buttons = new List<S_Button>();
    public void Setup(int headerData) {

        name = "Ability Panel";
        int[] abilityDatums = new int[] { headerData & 15, (headerData & 240) >> 4 };
        List<S_Ability> abilities = new List<S_Ability>();
        foreach (int datum in abilityDatums) {
            if (datum == 0) continue;
            Sprite sprite = null;
            Debug.Log(datum & 3);
            switch(datum & 3) {
                case 1: sprite = Sprites.ability_dash; break;
                case 2: sprite = Sprites.ability_eye; break;
            }
            S_Button buttonScrip = S_Button.CreateButton(Sprites.ability_border);
            S_Ability ability = buttonScrip.gameObject.AddComponent<S_Ability>();
            ability.init(2, S_Ability.AbilityType.Move3);
            buttonScrip.SetAction(() => {
                ability.Toggle(true);
            });
            GameObject button = buttonScrip.gameObject;
            button.name = "Button";
            Util.SetColour(button, Colours.RED);
            buttons.Add(button.GetComponent<S_Button>());
            S_Camera.SetupScale(buttonScrip.transform);
            GameObject image = Primitives.CreateActor(sprite, 1, 9);
            image.name = "image";
            Util.SetLayer(image, Util.LayerName.UI, 10);
            image.transform.SetParent(buttonScrip.gameObject.transform, false);
        }

        int gap = 10 * S_Camera.scale;
        int currentY = -gap;
        foreach (S_Button butt in buttons) {
            currentY += gap;
            butt.transform.position = new Vector2(gap, currentY);
            currentY += (int)(butt.GetBounds().size.y);
        }

        width = (int)(gap * 2 + buttons[0].GetBounds().size.x);
        height = currentY;
        foreach (S_Button butt in buttons) {
            butt.transform.SetParent(transform, false);
        }

        
    }
}
