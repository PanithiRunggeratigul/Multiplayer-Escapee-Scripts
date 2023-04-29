using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemAssets : MonoBehaviour
{
    public Sprite dudeSprite;
    public Sprite whiteSprite;

    public Transform pfItemWorld;
    public static itemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
