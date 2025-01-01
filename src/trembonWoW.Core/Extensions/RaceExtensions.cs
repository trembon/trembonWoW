using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Models.Enums;

namespace trembonWoW.Core.Extensions;

public static class RaceExtensions
{
    public static bool IsAlliance(this Race race)
    {
        switch (race)
        {
            case Race.Draenei:
            case Race.Dwarf:
            case Race.Gnome:
            case Race.Human:
            case Race.NightElf:
                return true;

            default: return false;
        }
    }

    public static bool IsHorde(this Race race)
    {
        switch (race)
        {
            case Race.BloodElf:
            case Race.Goblin:
            case Race.Orc:
            case Race.Tauren:
            case Race.Troll:
            case Race.Undead:
                return true;

            default: return false;
        }
    }
}
