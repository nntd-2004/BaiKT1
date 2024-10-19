using System;
using System.Windows.Forms;

namespace game1
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;

        public Form1()
        {
            InitializeComponent();
            gameTimer.Start(); // Bắt đầu bộ đếm thời gian khi khởi động trò chơi
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            player.Top += jumpSpeed;

            if (goLeft)
                player.Left -= playerSpeed;
            if (goRight)
                player.Left += playerSpeed;

            // Kiểm tra trạng thái nhảy
            if (jumping)
            {
                if (force < 0)
                {
                    jumping = false; // Ngừng nhảy nếu lực nhỏ hơn 0
                }
                else
                {
                    jumpSpeed = -8; // Lực nhảy
                    force--; // Giảm lực nhảy
                }
            }
            else
            {
                jumpSpeed = 10; // Trọng lực rơi xuống
            }

            // Kiểm tra va chạm với các đối tượng
            CheckCollisions();

            // Di chuyển các đối tượng
            MovePlatforms();
            MoveEnemies();

            // Kiểm tra nếu player rơi ra ngoài màn hình
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                EndGame("You fell to your death!");
            }

            // Kiểm tra nếu player chạm vào cửa
            if (player.Bounds.IntersectsWith(door.Bounds))
            {
                if (score == 27) // Giả sử 27 là số điểm cần thiết để hoàn thành trò chơi
                {
                    EndGame("Cuộc phiêu lưu của bạn đã hoàn thành! ");
                }
                else
                {
                    txtScore.Text = "Bạn cần thu thập tất cả đồng xu trước!";
                }
            }
            else
            {
                txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all the coins";
            }
        }

        private void CheckCollisions()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8; // Thiết lập lại lực nhảy
                            player.Top = x.Top - player.Height; // Đặt player lên trên platform
                        }
                    }
                    if ((string)x.Tag == "coin" && x.Visible)
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            x.Visible = false; // Ẩn coin khi player lấy
                            score++;
                        }
                    }
                    if ((string)x.Tag == "enemy" && player.Bounds.IntersectsWith(x.Bounds))
                    {
                        EndGame("You were killed in your journey!!");
                    }
                }
            }
        }

        private void MovePlatforms()
        {
            horizontalPlatfrom.Left -= horizontalSpeed;
            if (horizontalPlatfrom.Left < 0 || horizontalPlatfrom.Left + horizontalPlatfrom.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed; // Đảo chiều di chuyển
            }

            verticalPlatfrom.Top += verticalSpeed;
            if (verticalPlatfrom.Top < 195 || verticalPlatfrom.Top > 581)
            {
                verticalSpeed = -verticalSpeed; // Đảo chiều di chuyển
            }
        }

        private void MoveEnemies()
        {
            enemyOne.Left -= enemyOneSpeed;
            if (enemyOne.Left < 0 || enemyOne.Left + enemyOne.Width > this.ClientSize.Width)
            {
                enemyOneSpeed = -enemyOneSpeed; // Đảo chiều di chuyển
            }

            enemyTwo.Left += enemyTwoSpeed;
            if (enemyTwo.Left < 0 || enemyTwo.Left + enemyTwo.Width > this.ClientSize.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed; // Đảo chiều di chuyển
            }
        }

        private void EndGame(string message)
        {
            gameTimer.Stop();
            isGameOver = true;
            txtScore.Text = "Score: " + score + Environment.NewLine + message;
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = true;
            if (e.KeyCode == Keys.Right)
                goRight = true;
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
                force = 2; // Khởi động lực nhảy
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = false;
            if (e.KeyCode == Keys.Right)
                goRight = false;
            if (e.KeyCode == Keys.Space)
                jumping = false; 

            if (e.KeyCode == Keys.Enter && isGameOver)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && !x.Visible)
                {
                    x.Visible = true; // Hiện lại các đối tượng
                }
            }

            // Reset vị trí của player, platform và enemies
            player.Left = 72;
            player.Top = 656;
            enemyOne.Left = 471;
            enemyTwo.Left = 360;
            horizontalPlatfrom.Left = 275;
            verticalPlatfrom.Top = 581;
            gameTimer.Start();
        }
    }
}
