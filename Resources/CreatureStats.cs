using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class CreatureStats : Resource
{
    int hp_backing = 1;
    int max_hp_backing = 1;
    int attack_backing = 1;

    [Export]
    public int Hp { get => hp_backing; set { 
            hp_backing = Math.Max(value, 0);
        } }
    [Export]
    public int MaxHp { get => max_hp_backing; set { 
            max_hp_backing = Math.Max(value, 1);
        } }
    [Export]
    public int Attack { get => attack_backing; set { 
            attack_backing = Math.Max(value, 0);
        } }
}

