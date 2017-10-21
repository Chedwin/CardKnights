using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    WATER = 0,
    FIRE = 1,
    EARTH = 2,
    WIND = 4,
    ELECTRIC = 5
}

[System.Serializable]
public sealed class MagicCard {

    public MagicCard(string _name, ElementType eType) {
        name = _name;
        elementType = eType;
    }

    public string name {
        get; private set;
    }
	
    public ElementType elementType {
        get; private set;
    }

    public float atk {
        get; private set;
    }

    public float def {
        get; private set;
    }

} // end class MagicCard