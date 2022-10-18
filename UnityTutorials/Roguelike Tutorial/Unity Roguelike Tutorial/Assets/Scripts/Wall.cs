using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;

    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    public AudioClip chopSound1,
        chopSound2;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
