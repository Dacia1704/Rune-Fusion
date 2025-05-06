using System;
using System.Collections;
using UnityEngine;

public class FloatingTextObjectPooling: ObjectPooling
{
    // private void Start()
    // {
    //     StartCoroutine(TestFloatingText(100));
    // }

    // private IEnumerator TestFloatingText(int dam)
    // {
    //     while (true)
    //     {
    //         ShowDamage(dam);
    //         yield return new WaitForSeconds(2f);
    //     }
    // }
    
    
    public void ShowDamage(int damageAmount)
    {
        GameObject instance = GetObject("FloatingText");
        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.anchoredPosition =Vector2.zero;
        UIFloatingText floating = instance.GetComponent<UIFloatingText>();
        floating.Show(this,$"{Mathf.Abs(damageAmount)}", damageAmount >0 ? Color.red : Color.green);
    }
}