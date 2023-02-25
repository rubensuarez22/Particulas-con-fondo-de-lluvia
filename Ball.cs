using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;



namespace Particulas_con_fondo
{
    public class Ball
    {
        public float x;
        public float y;
        public float vx;
        public float vy;
        public float ay;
        public float radio;
        public int vida;
        public System.Drawing.Image image;
        public bool ToRemove { get; private set; }
        public event EventHandler BallRemoved;

        private Size space;

        public Ball(Random rand, Size size, int index)
        {
            this.vida = rand.Next(100, 300);
            this.radio = rand.Next(20, 35);
            this.x = rand.Next(0, size.Width);
            this.y = 0;
            this.image = Properties.Resources.Lluvia;

            this.vx = 0;
            this.vy = 100;

            this.ay = 500;

            this.ToRemove = false;
            this.space = size;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(image, x - radio, y - radio, radio * 2, radio * 2);
        }

        public void Update(float deltaTime)
        {
            if (y + radio >= space.Height)
            {
                ToRemove = true;
            }

            x += vx * deltaTime;
            y += vy * deltaTime;

            vy += ay * deltaTime;

            if (x + radio < 0 || x - radio > space.Width || y + radio < 0 || y - radio > space.Height)
            {
                BallRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
