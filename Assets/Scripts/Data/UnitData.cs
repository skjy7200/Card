using UnityEngine;

[CreateAssetMenu(menuName = "Unit/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int maxHP = 20;
    public Sprite unitSprite;
}