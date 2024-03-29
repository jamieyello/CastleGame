﻿using Castle.Scenes.Entities.Creatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreatureSelectData : IEnumerable<ICreature>
{
    List<ICreature> selected_creatures { get; } = new();

    public void Add(ICreature creature) {
        if (!selected_creatures.Contains(creature)) selected_creatures.Add(creature);
    }

    public void Remove(ICreature creature) {
        if (selected_creatures.Contains(creature)) selected_creatures.Remove(creature);
    }

    public void Clear() => selected_creatures.Clear();

    public bool IsSelected(ICreature creature) => selected_creatures.Contains(creature);

    public IEnumerator<ICreature> GetEnumerator()
    {
        return ((IEnumerable<ICreature>)selected_creatures).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)selected_creatures).GetEnumerator();
    }
}