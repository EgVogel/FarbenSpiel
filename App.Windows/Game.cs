using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;



namespace App
{
    class Game
    {
        #region variables
        public const double RADIUS = 200;
        public const double RADIUSSPIELER = 25;

        public int blau = 1, rot = 2, gelb = 3, grün = 4;
        public double currentwinkel;
        public int angle = 3;
      

        public double width;
        public double height;

        #endregion

        public Game(double width, double height )
        {
            this.width = width;
            this.height = height;
            

            Kreis = new Point(width/2,-500);
            Velocity = new Point(100, 100);
            Spieler = new Point(width / 2, height / 2);
            SpielerVelocity = new Point(100,400);
            Dead = false;
            Spielerfarbe = 1;
        }

        public void Update(float ms)
        {

            double seconds = (double)ms / 1000;
            Kreis = new Point(Kreis.X + (Velocity.X*seconds), Kreis.Y + (Velocity.Y*seconds));
            Rotation += angle;

            if (Kreis.Y +  RADIUS > Spieler.Y)
            {
                currentwinkel = Rotation % 360;
                if  (Spielerfarbe == grün)
                {
                    if (currentwinkel >= 0 && currentwinkel < 90)
                    {
                        Kreis = new Point(Kreis.X, -400);
                        Score += 10;
                    }
                    else
                    {
                        Dead = true;
                    }
                } else
                if (Spielerfarbe == blau)
                {
                    if  (currentwinkel >= 90 && currentwinkel < 180)
                    {
                        Kreis = new Point(Kreis.X, -400);
                        Score += 10;
                    }
                    else
                    {
                        Dead = true;
                    }
                }else
                if (Spielerfarbe == rot)
                {
                    if (currentwinkel >= 180 && currentwinkel < 270)
                    {
                        Kreis = new Point(Kreis.X, -400);
                        Score += 10;
                    }
                    else
                    {
                        Dead = true;
                    }
                }else
                if (Spielerfarbe == gelb)
                {
                    if (currentwinkel >= 270 && currentwinkel < 360)
                    {
                        Kreis = new Point(Kreis.X, -400);
                        Score += 10;
                    }
                    else
                    {
                        Dead = true;
                    }
                }
                if (Score >= 50)
                {
                    Velocity = new Point(100,200);
                    angle = 5;
                    
                }
                if (Score >= 100)
                {
                    Velocity = new Point(100,300);
                    angle = 7;
                }
                if (Score >= 150)
                {
                    Velocity = new Point(100,500);
                    angle = 10;
                }
              
            }
            

        }

        #region SpielerBewegung
        public void Jumping(float ms)
        {
            double seconds = (double)ms / 1000;

            Spieler = new Point(Spieler.X,Spieler.Y-(SpielerVelocity.Y*seconds));
            
        }
        public void Falling ( float ms)
        {
            double seconds = (double)ms / 1000;
            Spieler = new Point(Spieler.X, Spieler.Y + (SpielerVelocity.Y * seconds));
        }
        #endregion

        #region props
        public Point Kreis { get; set; }
        public Point Velocity { get; set; }
        public int Rotation { get; set; }
        public Point Spieler { get; set; }
        public int Score { get; set; }
        public Point SpielerVelocity { get; set; }
        public int Spielerfarbe { get; set; }
        public bool Dead { get; set; }
        #endregion
    }
    
}
