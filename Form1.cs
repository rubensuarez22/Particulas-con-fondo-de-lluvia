using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Particulas_con_fondo
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;
        Ball ball;
        public List<Ball> balls;
        SolidBrush brush;
        static float deltaTime;
        private Bitmap pelotaImagen;




        Timer timer = new Timer();
        Timer addBallTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            pelotaImagen = Properties.Resources.Lluvia;


            bmp = new Bitmap(PCT_CANVAS.Width, PCT_CANVAS.Height);
            g = Graphics.FromImage(bmp);

            brush = new SolidBrush(Color.White);

            Random rand = new Random(); // declare and initialize the 'rand' variable

            balls = new List<Ball>();
            for (int b = 0; b < 50; b++)
            {
                balls.Add(new Ball(rand, PCT_CANVAS.Size, b));
            }

            // Suscribirse al evento BallRemoved
            foreach (Ball ball in balls)
            {
                ball.BallRemoved += new EventHandler(BallRemoved);
            }

            addBallTimer.Interval = 1000; // Establecer el intervalo a 500 milisegundos (medio segundo)
            addBallTimer.Tick += new EventHandler(AddNewBalls);
            addBallTimer.Start();

            PCT_CANVAS.Image = bmp;
            timer.Interval = 5;
            timer.Tick += new EventHandler(TIMER_Tick);
            timer.Start();
        }
        public void AddNewBalls(object sender, EventArgs e)
        {
            Random rand = new Random();
            lock (lockObject)
            {
                for (int b = 0; b < 50; b++)
                {
                    balls.Add(new Ball(rand, PCT_CANVAS.Size, balls.Count));
                }
            }
        }




        // Método para agregar una nueva partícula
        public void AddNewBall()
        {
            Random rand = new Random();
            Ball newBall = new Ball(rand, PCT_CANVAS.Size, balls.Count);
            newBall.BallRemoved += new EventHandler(BallRemoved);
            balls.Add(newBall);
        }

        // Método para manejar el evento BallRemoved
        private void BallRemoved(object sender, EventArgs e)
        {
            Ball ball = (Ball)sender;
            RemoveBall(ball);
            AddNewBall();
        }

        public void RemoveBall(Ball ball)
        {
            balls.Remove(ball);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            AddNewBall();
        }




        private object lockObject = new object(); // Objeto para bloqueo

        private void TIMER_Tick(object sender, EventArgs e)
        {
            g.Clear(Color.Transparent);

            lock (lockObject)
            {
                for (int i = balls.Count - 1; i >= 0; i--)
                {
                    balls[i].Update(0.1f);

                    if (balls[i].ToRemove) // Verificar si la partícula debe eliminarse
                    {
                        balls.RemoveAt(i);
                    }
                }
            }

            // Pintamos cada partícula en secuencia
            lock (lockObject)
            {
                foreach (Ball p in balls)
                {
                    p.Draw(g);
                }
            }

            PCT_CANVAS.Invalidate();
        }
    }
}
