using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableUIObject))]
[Serializable]
public class Card : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected string title;
    [SerializeField] protected GameObject weaponPrefab = default;
    [SerializeField] protected GameObject weaponOnFloorPrefab = default;

    public GameObject WeaponOnFloor { get { return weaponOnFloorPrefab; } }
    public GameObject WeaponPrefab { get { return weaponPrefab; } }

    public string GetTitle()
    {
        return title;
    }


    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            Card c = (Card)obj;
            return this == c;
        }
    }

    public override int GetHashCode()
    {
        var hashCode = -1114142817;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
        return hashCode;
    }

    public static bool operator ==(Card c1, Card c2)
    {
        if (c1 && c2)
            return c1.GetTitle().Equals(c2.GetTitle());
        else if (!c1)
            return !c2;
        else if (!c2)
            return !c1;
        else
            return false;
    }

    public static bool operator !=(Card c1, Card c2) => !(c1 == c2);


}
