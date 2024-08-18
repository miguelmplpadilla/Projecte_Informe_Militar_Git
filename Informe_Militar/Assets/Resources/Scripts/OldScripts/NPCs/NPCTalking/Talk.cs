using System;
using System.Collections.Generic;

[Serializable]
public class Talk
{
    public List<Dialoge> dialoge;
}

[Serializable]
public class Dialoge
{
    public int npc;
    public string text;
    public string animation = "idle";
}