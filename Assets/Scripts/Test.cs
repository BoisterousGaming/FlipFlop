using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;
    public CardFlipper cardFlipper;
    public CardGridHandler cardGridHandler;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            cardGridHandler.GenerateGrid(null);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            cardFlipper.DoFlip(backSprite, () => Debug.Log("Flipped to back side"));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            cardFlipper.DoFlip(backSprite, () => Debug.Log("Flipped to front side"));
        }
    }
}
