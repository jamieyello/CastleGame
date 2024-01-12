using Castle.Scenes.Entities.Creatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreatureSelectData : IEnumerable<Creature>
{
    List<Creature> selected_creatures { get; } = new();

    public void Add(Creature creature) {
        if (!selected_creatures.Contains(creature)) selected_creatures.Add(creature);
    }

    public void Remove(Creature creature) {
        if (selected_creatures.Contains(creature)) selected_creatures.Remove(creature);
    }

    public void Clear() => selected_creatures.Clear();

    public bool IsSelected(Creature creature) => selected_creatures.Contains(creature);

    public IEnumerator<Creature> GetEnumerator()
    {
        return ((IEnumerable<Creature>)selected_creatures).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)selected_creatures).GetEnumerator();
    }
}