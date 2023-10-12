using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class CreatureStats : Resource
{
    int hp_backing;
    int max_hp_backing;
    int attack_backing;

    [Export]
    public int Hp { get => hp_backing; set { 
            hp_backing = Math.Max(value, 0);
        } }
    [Export]
    public int MaxHp { get => max_hp_backing; set { 
            max_hp_backing = Math.Max(value, 0);
        } }
    [Export]
    public int Attack { get => attack_backing; set { 
            attack_backing = Math.Max(value, 0);
        } }
}

