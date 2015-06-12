using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

    class Pahis : PhysicsObject
    {

        private IntMeter elamaLaskuri = new IntMeter(3, 0, 3);
        public IntMeter ElamaLaskuri { get { return elamaLaskuri; } }

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

        public Pahis(double leveys, double korkeus, Image kuva, Image aseenkuva)
            : base(leveys, korkeus)
        {
            elamaLaskuri.LowerLimit += delegate { this.Tapa(kuva, aseenkuva); };
        }

        public void Tapa(Image kuva, Image aseenkuva)
        {
            //Explosion rajahdys = new Explosion(5000);
            //rajahdys.Image = kuva;
            //rajahdys.MaxRadius = 70;
            //rajahdys.Force = 100;
            //rajahdys.Speed = 100;
            //rajahdys.Position = this.Position + new Vector(3, 0);
            //rajahdys.ShockwaveColor = new Color(10, 0, 0, 0);
            //Game.Add(rajahdys);

            PhysicsObject asetrigger = PhysicsObject.CreateStaticObject(20, 20);
            asetrigger.CollisionIgnoreGroup = 0;
            asetrigger.IgnoresCollisionResponse = true;
            if (this.ase.IsAddedToGame)
            {
                asetrigger.Tag = "ase3";
            }
            if (this.ase2.IsAddedToGame)
            {
                asetrigger.Tag = "ase2";
            }
            asetrigger.Position = this.Position;
            asetrigger.Image = aseenkuva;
            Game.Add(asetrigger,1);

            this.Ase.Destroy();
            this.Destroy();
        }

        /*void VaihdaAse3(Vector paikka, double leveys, double korkeus)
        {
            
        }

        void LisaaAseTrigger(Vector paikka, double leveys, double korkeus, string tagiNimi, Image kuva)
        {
            PhysicsObject asetrigger = PhysicsObject.CreateStaticObject(20, 20);
            asetrigger.IgnoresCollisionResponse = true;
            asetrigger.Tag = tagiNimi;
            asetrigger.Position = paikka;
            asetrigger.Image = kuva;
            Add(asetrigger);



        }*/

    }
