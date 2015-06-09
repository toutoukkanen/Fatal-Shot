using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class HotlineVihti : PhysicsGame
{
    Hyvis pelaaja1;
    Image pelaajawithkalashnikov = LoadImage ("pelaajawithkalashnikov");

    IntMeter haulit;

    //SoundEffect kalashnikovsound = LoadSoundEffect("kalashnikovsound");

    SoundEffect aksound = LoadSoundEffect ("aksound");
    SoundEffect mp5sound = LoadSoundEffect ("mp5sound");
    SoundEffect m3sound = LoadSoundEffect ("m3sound");
    SoundEffect vaihto = LoadSoundEffect ("vaihto");
    SoundEffect m3vaihto = LoadSoundEffect("m3vaihto");


    Image pelaajanKuva = LoadImage ("pelaajav1");
    Image poliisiAmpuuKuva = LoadImage ("pelaajav1");
	Image poliisinKuva = LoadImage ("pelaajav1");
    Image mp5 = LoadImage ("mp5");
    Image pistooli = LoadImage ("pistooli");
    Image haulikko = LoadImage ("shotgun");
    Image kalashnikov = LoadImage ("kalashnikov");
    Image veriLantti = LoadImage ("verilantti");
    Vector nopeusYlos = new Vector(0, 500);
    Vector nopeusAlas = new Vector(0, -500);
    Vector nopeusOikea = new Vector(500, 0);
    Vector nopeusVasen = new Vector(-500, 0);
    AssaultRifle pelaajan1Ase;
    AssaultRifle pelaajan1Ase2;
    AssaultRifle pelaajan1Ase3;
    bool pelikaynnissa = true;
    bool rekyyliOn = true;
    //bool akkadessa = true;
    Timer rekyylitimer = new Timer();
    PhysicsObject asetrigger;

    //bool vaihdettujo = false;

    int ammokalashnikov = 24;
    int ammomp5 = 30;
    //int ammo2value = 150;
    bool haulikkovalmiina = false;
    bool haulikkorekyyli = false;
    int ruudunKoko = 40;

    public override void Begin()
    {

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");

        TileMap kentta = TileMap.FromLevelAsset("kentta");
        Level.Background.Color = Color.Gray;
        kentta.SetTileMethod('%', luoPelaaja);
        kentta.SetTileMethod('#', lisaaTaso1);
        
        kentta.Execute(ruudunKoko, ruudunKoko);

        TileMap kentta2 = TileMap.FromLevelAsset("kentta");
        kentta2.SetTileMethod('&', luoPahis);
        kentta2.SetTileMethod('H', VaihdaAse);
        kentta2.SetTileMethod('/', VaihdaAse2);
        kentta2.SetTileMethod('¤', VaihdaAse3);
        kentta2.Execute(ruudunKoko, ruudunKoko);

        Mouse.Listen(MouseButton.Left, ButtonState.Down, Ammu, "Ammu", pelaajan1Ase);
        Mouse.Listen(MouseButton.Left, ButtonState.Down, Ammu3, null, pelaajan1Ase3);
        Mouse.Listen(MouseButton.Left, ButtonState.Down, Ammu2, null, pelaajan1Ase2);
        Mouse.Listen(MouseButton.Left, ButtonState.Released, delegate { Camera.FollowOffset = new Vector(0.0, 0.0); }, "");
        Mouse.Listen(MouseButton.Left, ButtonState.Down, rekyyli, "");

        //Mouse.Listen(MouseButton.Middle, ButtonState.Pressed, VaihdaAse, "Vaihda ase");
        //delegate { { rekyylitimer.Start(); } }

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.W, ButtonState.Down, liikutaP, "Liiku ylos", pelaaja1, nopeusYlos);
        Keyboard.Listen(Key.S, ButtonState.Down, liikutaP, "Liiku alas", pelaaja1, nopeusAlas);
        Keyboard.Listen(Key.W, ButtonState.Released, liikutaP, null, pelaaja1, Vector.Zero);
        Keyboard.Listen(Key.S, ButtonState.Released, liikutaP, null, pelaaja1, Vector.Zero);

        Keyboard.Listen(Key.D, ButtonState.Down, liikutaP, "Liiku oikealle", pelaaja1, nopeusOikea);
        Keyboard.Listen(Key.D, ButtonState.Released, liikutaP, null, pelaaja1, Vector.Zero);

        Keyboard.Listen(Key.A, ButtonState.Down, liikutaP, "Liiku vasemmalle", pelaaja1, nopeusVasen);
        Keyboard.Listen(Key.A, ButtonState.Released, liikutaP, null, pelaaja1, Vector.Zero);

        Keyboard.Listen(Key.R, ButtonState.Pressed, aloitaAlusta, "Aloita alusta");

        haulit = new IntMeter(56);

        Timer hauliajastin = new Timer();
        hauliajastin.Interval = 0.9;
        hauliajastin.Timeout += delegate()
        {
            haulikkovalmiina = true;
        };

        hauliajastin.Start();

        rekyylitimer.Interval = 0.1;
        rekyylitimer.Timeout += rekyyli; 
            //delegate()
        //{
            //if (pelaaja1.Ase2.IsAddedToGame && pelaajan1Ase2.Ammo > 1)
            //{
            //    Camera.FollowOffset = Vector.FromLengthAndAngle(10.0, -pelaajan1Ase2.Angle);

            //    Timer rekyylivaihtotimer = new Timer();
            //    rekyylivaihtotimer.Interval = 0.1;
            //    rekyylivaihtotimer.Timeout += delegate()
            //    {
            //        Camera.FollowOffset = new Vector(0.0, 0.0);
            //    };
            //    rekyylivaihtotimer.Start(1);
            //}

            //if (pelaaja1.Ase3.IsAddedToGame && pelaajan1Ase3.Ammo > 1)
            //{
            //    Camera.X += 2;
            //    Camera.Y += 2;
            //}

            //if (pelaaja1.Ase.IsAddedToGame && pelaajan1Ase.Ammo > 1)
            //{

            //}
        //};

        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää");
        Camera.ZoomFactor = 1.4;
        Mouse.IsCursorVisible = true;
        Camera.Follow(pelaaja1);
    }

    void rekyyli()
    {
        if (pelaaja1.Ase2.IsAddedToGame && pelaajan1Ase2.Ammo > 1)
        {
            if (rekyyliOn)
            {
                //Camera.FollowOffset = Vector.FromLengthAndAngle(50.0, -pelaajan1Ase2.Angle);
                Camera.Position = pelaaja1.Position + Vector.FromLengthAndAngle(60.0, -pelaajan1Ase2.Angle);
                rekyyliOn = false;

                Timer rekyylivaihtotimer = new Timer();
                rekyylivaihtotimer.Interval = 0.1;
                rekyylivaihtotimer.Timeout += delegate()
                {
                    //Camera.FollowOffset = Vector.FromLengthAndAngle(25.0, pelaajan1Ase2.Angle);
                    Camera.Position = pelaaja1.Position + Vector.FromLengthAndAngle(25.0, pelaajan1Ase2.Angle);
                    rekyyliOn = true;
                };
                rekyylivaihtotimer.Start(1);
            }
        }

        if (pelaaja1.Ase3.IsAddedToGame && pelaajan1Ase3.Ammo > 1)
        {
            if (rekyyliOn)
            {
                //Camera.FollowOffset = Vector.FromLengthAndAngle(50.0, -pelaajan1Ase2.Angle);
                Camera.Position = pelaaja1.Position + Vector.FromLengthAndAngle(20.0, -pelaajan1Ase2.Angle);
                rekyyliOn = false;

                Timer rekyylivaihtotimer = new Timer();
                rekyylivaihtotimer.Interval = 0.1;
                rekyylivaihtotimer.Timeout += delegate()
                {
                    //Camera.FollowOffset = Vector.FromLengthAndAngle(25.0, pelaajan1Ase2.Angle);
                    Camera.Position = pelaaja1.Position + Vector.FromLengthAndAngle(15.0, pelaajan1Ase2.Angle);
                    rekyyliOn = true;
                };
                rekyylivaihtotimer.Start(1);
            }
        }

        if (pelaaja1.Ase.IsAddedToGame && haulit.Value > 1)
        {
            if (rekyyliOn && haulikkorekyyli)
            {
                //Camera.FollowOffset = Vector.FromLengthAndAngle(50.0, -pelaajan1Ase2.Angle);
                Camera.Position = pelaaja1.Position + Vector.FromLengthAndAngle(60.0, -pelaajan1Ase2.Angle);
                rekyyliOn = false;

                Timer rekyylivaihtotimer = new Timer();
                rekyylivaihtotimer.Interval = 0.1;
                rekyylivaihtotimer.Timeout += delegate()
                {
                    //Camera.FollowOffset = Vector.FromLengthAndAngle(25.0, pelaajan1Ase2.Angle);
                    Camera.Position = pelaaja1.Position + Vector.FromLengthAndAngle(25.0, pelaajan1Ase2.Angle);
                    rekyyliOn = true;
                    haulikkorekyyli = false;
                };
                rekyylivaihtotimer.Start(1);
            }
        }
    }
    void VaihdaAse(Vector paikka, double leveys, double korkeus)
    {
        LisaaAseTrigger(paikka, leveys, korkeus, "ase", haulikko);
    }

    void VaihdaAse2(Vector paikka, double leveys, double korkeus)
    {
        LisaaAseTrigger(paikka, leveys * 2, korkeus * 2, "ase2", kalashnikov);
    }

    void VaihdaAse3(Vector paikka, double leveys, double korkeus)
    {
        LisaaAseTrigger(paikka, leveys, korkeus, "ase3", mp5);
    }

    void LisaaAseTrigger(Vector paikka, double leveys, double korkeus, string tagiNimi, Image kuva)
    {
        asetrigger = PhysicsObject.CreateStaticObject(20, 20);
        asetrigger.IgnoresCollisionResponse = true;
        asetrigger.IgnoresPhysicsLogics = true;
        asetrigger.CollisionIgnoreGroup = 0;
        asetrigger.Tag = tagiNimi;
        asetrigger.Position = paikka;
        asetrigger.Image = kuva;
        Add(asetrigger, -3);


       
    }

    void aloitaAlusta()
    {
        pelikaynnissa = true;
        ClearAll();
        Begin();
        
        //vaihdettujo = false;
    }

    /*void rekyyli()
    {
        rekyylitimer = new Timer();
        rekyylitimer.Interval = 0.1;
        rekyylitimer.Timeout += delegate()
        {
            Camera.X += 1;
            Camera.Y += 1;
            rekyylitimer.Stop();
        };

        rekyylitimer.Start();
    }*/

    void Ammu3(AssaultRifle ase3)
    {
        if (pelaajan1Ase3.IsAddedToGame)
        {
            ase3.AttackSound = mp5sound;
            PhysicsObject ammus = ase3.Shoot();

            //for (int i = 0; i < 10; i++)
            //{
            //    rekyyli();
            //}


            if (ammus != null)
            {
                ammus.CollisionIgnoreGroup = 3;
                ase3.Power.DefaultValue = 450;
                ase3.FireRate = 10;
                ammus.Size *= 0.45;
            }
        }
    }



    void Ammu(AssaultRifle ase)
    {
        if (haulikkovalmiina && pelikaynnissa && haulit.Value > 7 && pelaajan1Ase.IsAddedToGame)
        {
            m3sound.Play();
            for (int i = 0; i < 9; i++)
            {

                PhysicsObject hauli = new PhysicsObject(2, 2);
                hauli.Shape = Shape.Ellipse;
                hauli.Color = Color.Yellow;
                hauli.Position = pelaaja1.Position;
                //Vector suunta = (Mouse.PositionOnWorld - pelaajan1Ase.AbsolutePosition);
                Vector impulssi = Vector.FromLengthAndAngle(70000, (pelaajan1Ase.Angle + (RandomGen.NextAngle(Angle.FromDegrees(-25), Angle.FromDegrees(25)))));
                //hauli.Angle = RandomGen.NextAngle(Angle.FromDegrees(-45), Angle.FromDegrees(45));
                Add(hauli);

                hauli.MaximumLifetime = TimeSpan.FromSeconds(0.3);
                AddCollisionHandler(hauli, "paha", AmmusOsui2);
                AddCollisionHandler(hauli, "seina", CollisionHandler.DestroyObject);

                hauli.CollisionIgnoreGroup = 3;
                hauli.Push(impulssi);
                haulikkovalmiina = false;
                haulikkorekyyli = true;
                haulit.Value -= 1;
            }
        }

    }

    void Ammu2(AssaultRifle ase2)
    {
        if (pelaajan1Ase2.IsAddedToGame)
        {
            ase2.AttackSound = aksound;
            PhysicsObject ammus = ase2.Shoot();
            //kalashnikovsound.Play();
            //for (int i = 0; i < 10; i++)
            //{
            //    rekyyli();
            //}

            if (ammus != null)
            {
                ase2.Power.DefaultValue = 250;
                ase2.FireRate = 7;
                ammus.Size *= 0.65;
                ammus.CollisionIgnoreGroup = 3;
            }
        }
    }

    void lisaaTaso1(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Cyan;
        taso.KineticFriction = 0;
        taso.Tag = "seina";
        Add(taso);
    }

    void luoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new Hyvis(leveys * 0.5, korkeus * 0.5);
        pelaaja1.Position = paikka;
        pelaaja1.Image = pelaajanKuva;
        pelaaja1.Shape = Shape.Octagon;
        pelaaja1.Restitution = 0;
        pelaaja1.CanRotate = false; ;
        pelaaja1.Tag = "hyva";

        pelaajan1Ase2 = new AssaultRifle(30, 10);
        pelaajan1Ase2.Ammo.Value = ammokalashnikov;
        pelaajan1Ase2.ProjectileCollision = AmmusOsui;
        pelaajan1Ase2.Image = kalashnikov;
        //pelaaja1.Aseet.Add(pelaajan1Ase2);
        
        

        pelaajan1Ase = new AssaultRifle(30, 10);
        pelaajan1Ase.ProjectileCollision = AmmusOsui2;
        pelaajan1Ase.Image = haulikko;
        //pelaaja1.Aseet.Add(pelaajan1Ase);
        //pelaajan1Ase.X -= 8;

        pelaajan1Ase3 = new AssaultRifle(32, 12);
        pelaajan1Ase3.Ammo.Value = ammomp5;
        pelaajan1Ase3.ProjectileCollision = AmmusOsui3;
        pelaajan1Ase3.Image = mp5;


        pelaaja1.CollisionIgnoreGroup = 3;
        //pelaaja1.Aseet.Add(pelaajan1Ase3);

        AddCollisionHandler(pelaaja1, "ase", ase);

        AddCollisionHandler(pelaaja1, "ase2", ase2);

        AddCollisionHandler(pelaaja1, "ase3", ase3);

        pelaaja1.Add(pelaajan1Ase3);

        pelikaynnissa = false;

        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää");

        pelaaja1.Ase = pelaajan1Ase;
        pelaaja1.Ase2 = pelaajan1Ase2;
        pelaaja1.Ase3 = pelaajan1Ase3;

        Add(pelaaja1);

    }

    void ase(PhysicsObject pelaaja1, PhysicsObject ase)
    {
        pelaaja1.Remove(pelaajan1Ase3);
        pelaaja1.Remove(pelaajan1Ase2);

        pelaaja1.Add(pelaajan1Ase);

        haulit.Value = 56;

        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää");

        m3vaihto.Play();

        ase.Destroy();

        haulikkovalmiina = false;
        pelikaynnissa = true;
    }

    void ase2(PhysicsObject pelaaja1, PhysicsObject ase2)
    {
        pelaaja1.Remove(pelaajan1Ase);
        pelaaja1.Remove(pelaajan1Ase3); 
        pelaaja1.Remove(pelaajan1Ase2);

        pelaaja1.Add(pelaajan1Ase2);

        pelaajan1Ase2.Ammo.Value = ammokalashnikov;

        vaihto.Play();

        ase2.Destroy();
        

        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää");
    }

    void ase3(PhysicsObject pelaaja1, PhysicsObject ase3)
    {
        pelaaja1.Remove(pelaajan1Ase);
        pelaaja1.Remove(pelaajan1Ase2);
        pelaaja1.Remove(pelaajan1Ase3);

        pelaaja1.Add(pelaajan1Ase3);

        pelikaynnissa = false;

        pelaajan1Ase3.Ammo.Value = 30;

        vaihto.Play();

        ase3.Destroy();

        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää");
    }

    void luoPahis(Vector paikka, double leveys, double korkeus)
    {
        Pahis pahis = new Pahis(leveys * 0.6, korkeus * 0.6, veriLantti, mp5);
        pahis.Position = paikka;
        pahis.Image = pelaajanKuva;
        pahis.Shape = Shape.Octagon;

        FollowerBrain seuraajaAivot = new FollowerBrain(pelaaja1);
        RandomMoverBrain randomAivot = new RandomMoverBrain();
        randomAivot.ChangeMovementSeconds = 3;

        AssaultRifle pahisAse = new AssaultRifle(30, 10);
        pahisAse.Ammo.Value = 100;
        pahisAse.ProjectileCollision = PahisAmmusOsui;
        pahisAse.InfiniteAmmo = true;
        pahis.Ase = pahisAse;
        pahis.Tag = "paha";
        pahisAse.AttackSound = null;
        pahisAse.Image = mp5;
        pahis.Add(pahisAse);

        AddCollisionHandler(pahis, seinatormays);

        pahis.Brain = seuraajaAivot;
        seuraajaAivot.Active = true;
        seuraajaAivot.Speed = 500;
        seuraajaAivot.DistanceClose = 150;
        seuraajaAivot.DistanceFar = 250;

        randomAivot.Speed = 500;

        LabyrinthWandererBrain labyrinttiAivot = new LabyrinthWandererBrain(ruudunKoko);
        labyrinttiAivot.Speed = 100.0;
        labyrinttiAivot.LabyrinthWallTag = "seina";

        seuraajaAivot.FarBrain = labyrinttiAivot;

        seuraajaAivot.TargetClose += delegate { if (pahis.SeesObject(pelaaja1)) { pahisAmpuu(pahisAse, pahis); } else { } };

        seuraajaAivot.StopWhenTargetClose = true;
        pahis.CollisionIgnoreGroup = 1;

        Add(pahis);

    }

    void seinatormays(PhysicsObject pahis, PhysicsObject kohde)
    {
        //Vector impulssi = new Vector(500.0, 500.0);
        //pahis.Hit(impulssi);
    }

    void pahisAmpuu(AssaultRifle ase, Pahis pahis)
    {
        Vector suunta = (pelaaja1.Position - pahis.Position).Normalize();

        if (pelaaja1.Velocity == Vector.Zero)
        {
            pahis.Angle = suunta.Angle;
        }
        else
        { 
            pahis.Angle = suunta.Angle * 1.1;
        }


        ase.AttackSound = mp5sound;
        PhysicsObject ammus = ase.Shoot();

        if (ammus != null)
        {
            ase.Power.DefaultValue = 300;
            ase.FireRate = 4;
            ase.CanHitOwner = false;
            ammus.Size *= 0.45;
            ammus.MaximumLifetime = TimeSpan.FromSeconds(4);
            ammus.CollisionIgnoreGroup = 1;
        }
    }

    void Tahtaa(AnalogState hiirenLiike)
    {
        
        Vector suunta = (Mouse.PositionOnWorld - pelaajan1Ase.AbsolutePosition).Normalize();
        pelaajan1Ase.Angle = suunta.Angle;

        Vector suunta2 = (Mouse.PositionOnWorld - pelaajan1Ase2.AbsolutePosition).Normalize();
        pelaajan1Ase2.Angle = suunta2.Angle;

        Vector suunta3 = (Mouse.PositionOnWorld - pelaajan1Ase3.AbsolutePosition).Normalize();
        pelaajan1Ase3.Angle = suunta3.Angle; 
        
        /*foreach (Weapon ase in pelaaja1.Aseet)
        {
            Vector suunta = (Mouse.PositionOnWorld - ase.AbsolutePosition).Normalize();
            ase.Angle = suunta.Angle;
        }*/
    }

    void AmmusOsui2(PhysicsObject hauli, PhysicsObject kohde)
    {
        if (kohde.Tag.Equals("paha") || kohde.Tag.Equals("seina"))
        {
            hauli.Destroy();
        }
        if (kohde.Tag.Equals("paha"))
        {
            LisaaAseTrigger(kohde.Position, ruudunKoko, ruudunKoko, "ase3", mp5);

            

            GameObject verilantti = new GameObject(30, 30);
            verilantti.Position = kohde.Position;
            verilantti.Image = veriLantti;
            Add(verilantti, -3);

            //verilantti.MaxVelocity = 0;
            //verilantti.CollisionIgnoreGroup = 1;
            //verilantti.IgnoresCollisionResponse = true;

            (kohde as Pahis).ElamaLaskuri.Value -= 3;
            
        }

        if (kohde.Tag.Equals("hyva"))
        {
            (kohde as Hyvis).Ase.Destroy();
            (kohde as Hyvis).Ase2.Destroy();
            (kohde as Hyvis).Ase3.Destroy();
            (kohde as Hyvis).Destroy();
            pelikaynnissa = false;
        }
        
    }

    void AmmusOsui3( PhysicsObject ammus, PhysicsObject kohde)
    {
        if (kohde.Tag.Equals("paha") || kohde.Tag.Equals("seina"))
        {
            ammus.Destroy();
        }
        
        if (kohde.Tag.Equals("paha"))
        {

            //LisaaAseTrigger(kohde.Position, ruudunKoko, ruudunKoko, "ase3", mp5);

            Explosion rajahdys = new Explosion(5000);
            rajahdys.Image = veriLantti;
            rajahdys.MaxRadius = 70;
            rajahdys.Force = 100;
            rajahdys.Speed = 100;
            rajahdys.Position = kohde.Position + new Vector(3, 0);
            rajahdys.ShockwaveColor = new Color(10, 0, 0, 0);
            Add(rajahdys);

            GameObject verilantti = new GameObject(30, 30);
            verilantti.Position = kohde.Position;
            verilantti.Image = veriLantti;
            Add(verilantti, -3);

            //verilantti.MaxVelocity = 0;
            //verilantti.CollisionIgnoreGroup = 1;
            //verilantti.IgnoresCollisionResponse = true;

            (kohde as Pahis).ElamaLaskuri.Value -= 2;
        }

    }

    void PahisAmmusOsui(PhysicsObject ammus, PhysicsObject kohde)
    {
        if (kohde.Tag.Equals("hyva") || kohde.Tag.Equals("seina"))
        {
            ammus.Destroy();
        }
        if (kohde.Tag.Equals("hyva"))
        {
            (kohde as Hyvis).Ase.Destroy();
            (kohde as Hyvis).Ase2.Destroy();
            (kohde as Hyvis).Ase3.Destroy();
            (kohde as Hyvis).Destroy();
            pelikaynnissa = false;
        }
    }

    void AmmusOsui( PhysicsObject ammus, PhysicsObject kohde)
    {
        if (kohde.Tag.Equals("paha") || kohde.Tag.Equals("seina"))
        {
            ammus.Destroy();
        }
        if (kohde.Tag.Equals("paha"))
        {

            //LisaaAseTrigger(kohde.Position, ruudunKoko, ruudunKoko, "ase3", mp5);

            Explosion rajahdys = new Explosion(5000);
            rajahdys.Image = veriLantti;
            rajahdys.MaxRadius = 70;
            rajahdys.Force = 100;
            rajahdys.Speed = 100;
            rajahdys.Position = kohde.Position + new Vector(3, 0);
            rajahdys.ShockwaveColor = new Color(10, 0, 0, 0);
            Add(rajahdys);

            GameObject verilantti = new GameObject(30, 30);
            verilantti.Position = kohde.Position;
            verilantti.Image = veriLantti;
            Add(verilantti, -3);

            //verilantti.MaxVelocity = 0;
            //verilantti.CollisionIgnoreGroup = 1;
            //verilantti.IgnoresCollisionResponse = true;

            (kohde as Pahis).ElamaLaskuri.Value -= 3;
        }

    }

    void liikutaP(PhysicsObject pelaaja1, Vector nopeus)
    {
        if ((nopeus.Y < 0) && (pelaaja1.Bottom < Level.Bottom))
        {
            pelaaja1.Velocity = Vector.Zero;
            return;
        }

        if ((nopeus.Y > 0) && (pelaaja1.Top > Level.Top))
        {
            pelaaja1.Velocity = Vector.Zero;
            return;
        }
        pelaaja1.Velocity = nopeus;
    }
}