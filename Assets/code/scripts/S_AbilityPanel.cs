using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_AbilityPanel : MonoBehaviour{

    public int width, height;
    List<S_Button> buttons = new List<S_Button>();
    public void Setup(int headerData) {

        name = "Ability Panel";
        GameObject button= null;
        button = S_Button.CreateButton(Sprites.ability_border, ()=> {
            Game.Get().level.ActivateAbility();
            Util.SetColour(button, Colours.LIGHT);
        });
        
        button.name = "Button";
        Util.SetColour(button, Colours.GREEN);
        buttons.Add(button.GetComponent<S_Button>());

        int currentY = 0;
        int gap = 10 * S_Camera.scale;
        currentY += gap;
        foreach (S_Button butt in buttons) {
            S_Camera.SetupScale(butt.transform);
            butt.transform.position = new Vector2(gap, currentY);
            currentY += (int)(butt.GetBounds().size.y);
            currentY += gap;
            GameObject image = Primitives.CreateActor(Sprites.ability_dash, 1, 9);
            int pips = 2;
            int pipGap = (int)((Sprites.GetBounds(Sprites.ability_border).x-2 - Sprites.GetBounds(Sprites.ability_pip).x*pips) /(pips+1));
            Debug.Log(pipGap);
            int currentX = S_Camera.scale+pipGap;
            for(int i = 0; i < pips; i++) {
                GameObject pip = Primitives.CreateActor(Sprites.ability_pip, currentX, S_Camera.scale);
                Util.SetLayer(pip, Util.LayerName.UI, 5);
                currentX += (int)(Sprites.GetBounds(Sprites.ability_pip).x);
                currentX += pipGap;
                pip.transform.SetParent(button.transform, false);
            }
            image.name = "image";
            Util.SetLayer(image, Util.LayerName.UI, 10);
            image.transform.SetParent(butt.gameObject.transform, false);

        }

        width = (int)(gap * 2 + buttons[0].GetBounds().size.x);
        height = currentY;

        /*
        int border = S_Camera.scale;
        GameObject outerRectangle = Primitives.CreateRectangle(width, height, Colours.GREEN);
        outerRectangle.transform.SetParent(transform, false);
        outerRectangle.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        outerRectangle.GetComponent<SpriteRenderer>().sortingOrder = 5;

        GameObject innerRectangle = Primitives.CreateRectangle(width-border*2, height - border * 2, Colours.DARK);
        innerRectangle.transform.position = new Vector2(border, border);
        innerRectangle.transform.SetParent(transform, false);
        innerRectangle.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        innerRectangle.GetComponent<SpriteRenderer>().sortingOrder = 6;
        */

        foreach (S_Button butt in buttons) {
            butt.transform.SetParent(transform, false);
        }

        
    }
}
