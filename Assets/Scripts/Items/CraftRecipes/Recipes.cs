using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Scriptable Objects/Recipes")]
public class Recipes : ScriptableObject
{
    public List<Item> ingredients;

    public Item result;
}
