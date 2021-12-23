using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace HanoiTowers
{
    public partial class Form : System.Windows.Forms.Form
    {
        const int amountOfRings = 12;

        int levelRingsAmount;

        PictureBox[] towers = new PictureBox[4];
        PictureBox[] rings = new PictureBox[amountOfRings + 1];
        int[,] array = new int[4, amountOfRings + 1];
        int[] size = new int[4];

        Random rand = new Random();

        Button[] buttons = new Button[5];

        int tick = 0;

        int from;
        int to;

        bool firstClick;

        PictureBox loadingBar;
        Label loadingLabel;
        Label timerLabel;

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            CreateMenu();
        }        

        private void CreateMenu()
        {
            System.Media.SoundPlayer menuPlayer = new System.Media.SoundPlayer(@"D:\Учёба\HanoiTowers\HanoiTowers\Menu.wav");
            menuPlayer.PlayLooping();

            this.BackColor = Color.FromArgb(197, 142, 135);
            PictureBox pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Size = new Size(950, 400);
            pictureBox.Image = Image.FromFile(@"D:\Учёба\HanoiTowers\HanoiTowers\Decision.gif");
            pictureBox.Location = new Point(0, 0);
            Controls.Add(pictureBox);

            for (int i = 0; i < 5; ++i)
            {
                Button button = new Button();

                if (i == 0)
                    button.Location = new Point(30 + 350, 460);
                else
                    button.Location = new Point(30 + 230 * (i - 1), 400);

                button.Size = new Size(200, 40);
                button.Font = new Font("Consolas", 12, FontStyle.Bold);
                button.FlatStyle = FlatStyle.Flat;
                Controls.Add(button);
                buttons[i] = button;
            }

            buttons[0].Text = "Exit";
            buttons[1].Text = "Easy (6)";
            buttons[2].Text = "Medium (8)";
            buttons[3].Text = "Hard (10)";
            buttons[4].Text = "Insane (12)";

            buttons[0].BackColor = Color.Silver;
            buttons[1].BackColor = Color.Green;
            buttons[2].BackColor = Color.Yellow;
            buttons[3].BackColor = Color.Red;
            buttons[4].BackColor = Color.Purple;

            this.buttons[0].Click += new System.EventHandler(this.Exit_Click);
            this.buttons[1].Click += new System.EventHandler(this.EasyButton_Click);
            this.buttons[2].Click += new System.EventHandler(this.MediumButton_Click);
            this.buttons[3].Click += new System.EventHandler(this.HardButton_Click);
            this.buttons[4].Click += new System.EventHandler(this.InsaneButton_Click);

            
        }

        private void CreateTowers()
        {
            for (int i = 1; i <= 3; ++i)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(260, 20);
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(40 * i + 260 * (i - 1), 500);
                Controls.Add(pictureBox);
                towers[i] = pictureBox;
            }

            for (int i = 1; i <= 3; ++i)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(20, 200);
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(towers[i].Location.X + 120, 300);
                Controls.Add(pictureBox);
            }

            this.towers[1].Click += new System.EventHandler(this.Tower1_Click);
            this.towers[2].Click += new System.EventHandler(this.Tower2_Click);
            this.towers[3].Click += new System.EventHandler(this.Tower3_Click);
        }

        private void CreateRings()
        {
            
            int ringHeight = Convert.ToInt32(Math.Round(200.0 / levelRingsAmount));

            for (int i = 1; i <= levelRingsAmount; ++i)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(260 - (i - 1) * 20, ringHeight);
                pictureBox.BackColor = Color.FromArgb(255, Color.FromArgb(Convert.ToInt32(rand.Next(0x1000000))));
                pictureBox.Location = new Point(40 + (i - 1) * 10, 500 - i * ringHeight);
                Controls.Add(pictureBox);
                rings[levelRingsAmount - i + 1] = pictureBox;
            }

            size[1] = levelRingsAmount;
            size[2] = 0;
            size[3] = 0;

            for (int i = 1; i <= levelRingsAmount; ++i)
                array[1, i] = levelRingsAmount - i + 1;

            for (int i = 1; i <= levelRingsAmount; ++i)
            {
                array[2, i] = 0;
                array[3, i] = 0;
            }
        }

        private void LoadingScreen()
        {
            Controls.Clear();

            this.BackColor = Color.Black;

            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(400, 20);
                pictureBox.BackColor = Color.White;
                pictureBox.Location = new Point(280, 300);
                Controls.Add(pictureBox);
            }

            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(4, 20);
                pictureBox.BackColor = Color.Green;
                pictureBox.Location = new Point(280, 300);
                Controls.Add(pictureBox);
                loadingBar = pictureBox;
                loadingBar.BringToFront();
            }

            {
                Label label = new Label();
                label.Font = new Font("Consolas", 12, FontStyle.Bold);
                label.ForeColor = Color.White;
                label.Size = new Size(200, 20);
                label.Location = new Point(430, 280);
                Controls.Add(label);
                loadingLabel = label;
            }

            loadingTimer.Start();
        }

        private void Rules()
        {
            {
                String s1 = "How to play: \n";
                String s2 = "Move all disks from first to second tower. \n";
                String s3 = "But you cannot place a larger disk onto a smaller disk. \n";
                String s4 = "How to move disk:\n";
                String s5 = "1. Click on the base of the tower from where you want to move the disc.\n";
                String s6 = "2. Click on the base of the tower where you want to place the disc.";

                Label label = new Label();
                label.Size = new Size(600, 200);
                label.Text = s1 + s2 + s3 + s4 + s5 +s6;
                label.ForeColor = Color.Black;
                label.Font = new Font("Consolas", 12, FontStyle.Bold);
                label.Location = new Point(0, 0);
                Controls.Add(label);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {   
            levelRingsAmount = 6;
            LoadingScreen();
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            levelRingsAmount = 8;
            LoadingScreen();
        }

        private void HardButton_Click(object sender, EventArgs e)
        {
            levelRingsAmount = 10;
            LoadingScreen();
        }

        private void InsaneButton_Click(object sender, EventArgs e)
        {
            levelRingsAmount = 12;
            LoadingScreen();
        }

        private void Tower1_Click(object sender, EventArgs e)
        {
            if (!firstClick)
            {
                to = 1;
                firstClick = true;
                Movement(from, to);
                return;
            }

            from = 1;
            firstClick = false;
        }

        private void Tower2_Click(object sender, EventArgs e)
        {
            if (!firstClick)
            {
                to = 2;
                firstClick = true;
                Movement(from, to);
                return;
            }

            from = 2;
            firstClick = false;
        }

        private void Tower3_Click(object sender, EventArgs e)
        {
            if (!firstClick)
            {
                to = 3;
                firstClick = true;
                Movement(from, to);
                return;
            }

            from = 3;
            firstClick = false;
        }

        void Movement(int from, int to)
        {
            int ringHeight = Convert.ToInt32(Math.Round(200.0 / levelRingsAmount));

            if (size[to] == 0 ||
                array[from, size[from]] < array[to, size[to]])
            {
                rings[array[from, size[from]]].Location = new Point(towers[to].Location.X + (260 - rings[array[from, size[from]]].Size.Width) / 2, towers[to].Location.Y - (size[to] + 1) * ringHeight);
                ++size[to]; 
                array[to, size[to]] = array[from, size[from]];
                array[from, size[from]] = 0;
                --size[from];

                return;
            }

            MessageBox.Show("Wrong movement!");
        }

        private void loadingTimer_Tick(object sender, EventArgs e)
        {
            if (tick == 100)
            {
                Controls.Clear();
                loadingTimer.Stop();

                switch(levelRingsAmount)
                {
                    case 6:
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\Учёба\HanoiTowers\HanoiTowers\Easy.wav");
                            player.PlayLooping();
                            break;
                        }
                    case 8:
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\Учёба\HanoiTowers\HanoiTowers\Medium.wav");
                            player.PlayLooping();
                            break;
                        }
                    case 10:
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\Учёба\HanoiTowers\HanoiTowers\Hard.wav");
                            player.PlayLooping();
                            break;
                        }
                    case 12:
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\Учёба\HanoiTowers\HanoiTowers\Insane.wav");
                            player.PlayLooping();
                            break;
                        }
                }

                CreateTowers();
                CreateRings();
                Rules();

                {
                    Label label = new Label();
                    label.Size = new Size(200, 40);
                    label.ForeColor = Color.Black;
                    label.Font = new Font("Consolas", 20, FontStyle.Bold);
                    label.Location = new Point(700, 0);
                    Controls.Add(label);
                    timerLabel = label;
                }

                tick = 0;

                clock.Start();

                firstClick = true;

                this.BackColor = Color.FromArgb(197, 142, 135);

                return;
            }

            loadingBar.Size = new Size(4 * tick, 20);

            ++tick;

            loadingLabel.Text = "Loading " + Convert.ToString(tick) + "%"; 
        }

        private void clock_Tick(object sender, EventArgs e)
        {
            timerLabel.Text = "Time: " + Convert.ToString(tick);
            ++tick;

            if (size[2] == levelRingsAmount)
            {
                clock.Stop();
                MessageBox.Show("Congratulations!!!\n You Won!");
                this.Close();
            }
        }
    }
}
