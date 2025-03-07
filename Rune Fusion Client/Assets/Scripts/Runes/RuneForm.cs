public enum RuneForm
{
        Base =0,  // only need 1 times to break;
        Protected, // need 1 times to transform to Base;
        Poison, // when swipe get dam to monster;
        Vertical, // when break can break 1 column;
        Horizontal, // when break can break 1 row;
        Explosive, // can break all surrounding  rune;
        Special, // can break all rune in map;
}