using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

class Hyvis : PhysicsObject
{
    private IntMeter elamaLaskuri = new IntMeter(3, 0, 3);
    public IntMeter ElamaLaskuri { get { return elamaLaskuri; } }

    private AssaultRifle ase2 = new AssaultRifle(10, 10);
    public AssaultRifle Ase2
    {
        get
        {
            return ase2;
        }
        set
        {
            ase2 = value;
        }
    }

    private AssaultRifle ase = new AssaultRifle(10, 10);
    public AssaultRifle Ase
    {
        get
        {
            return ase;
        }
        set
        {
            ase = value;
        }
    }

    private AssaultRifle ase3 = new AssaultRifle(10, 10);
    public AssaultRifle Ase3
    {
        get
        {
            return ase3;
        }
        set
        {
            ase3 = value;
        }
    }

    /*private List<Weapon> aseet = new List<Weapon>();
    public List<Weapon> Aseet
    {
        get
        {
            return aseet;
        }
        set
        {
            if (value != null)
            {
                aseet = value;
            }
        }
    }*/

    public Hyvis(double leveys, double korkeus)
        : base(leveys, korkeus)
    {
        elamaLaskuri.LowerLimit += delegate
        {
            this.Ase2.Destroy();
            this.Ase.Destroy();
            this.Ase3.Destroy();
            this.Destroy();
        };
    }
}
