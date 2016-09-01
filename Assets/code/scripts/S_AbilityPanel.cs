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
            S_Button buttonScrip = S_Button.CreateButton(Sprites.ability_border);
            S_Ability ability = null;
            switch (datum & 3) {
                case 1: ability = buttonScrip.gameObject.AddComponent<S_Ability_Move3>(); break;
                case 2: ability = buttonScrip.gameObject.AddComponent<S_Ability_Eye>(); break;
            }
            ability.init(2);
            buttonScrip.SetAction(() => {
                ability.Click();
            });
            GameObject button = buttonScrip.gameObject;
            button.name = "Button";
            Util.SetColour(button, Colours.RED);
            buttons.Add(button.GetComponent<S_Button>());
            S_Camera.SetupScale(buttonScrip.transform);
            GameObject image = Primitives.CreateActor(ability.GetSprite(), 1, 9);
            image.name = "image";
            Util.SetLayer(image, Util.LayerName.UI, 10);
            image.transform.SetParent(buttonScrip.gameObject.transform, false);
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
}
