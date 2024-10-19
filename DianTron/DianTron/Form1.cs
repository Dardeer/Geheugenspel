using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Media; // Nieuw Functie -1
using System.Linq;
using System.Windows.Forms;
using WMPLib; // WindowsMediaPlayer // Nieuw Functie -2


namespace DianTron
{
    public partial class Form1 : Form
    {
        // Variables

        List<PictureBox> pictureBoxes;
        List<Image> images;
        PictureBox firstClicked = null, secondClicked = null;
        Random random = new Random();
        int pairsFound = 0;  // Counter for pairs 
        SoundPlayer player; // palayer als een variable // Nieuw Functie -1
        WindowsMediaPlayer backgroundPlayer; // Achtergrondmuziek speler // Nieuw Functie -2
        public Form1()
        {
            InitializeComponent();
            LoadImages();  
            AssignImagesToPictureBoxes();  // random images 
            player = new SoundPlayer(@"D:\DianTronRepo\DianTron\DianTron\Geluid_Effect\geluid_pair_found.wav"); // geluid laden // Nieuw Functie -1


            // Achtergrondmuziek instellen functie -2
            backgroundPlayer = new WindowsMediaPlayer();
            backgroundPlayer.URL = @"D:\DianTronRepo\DianTron\DianTron\Achtergrondmuziek\Butterfly-chosic.com_.mp3\"; // Pas het pad aan naar je achtergrondmuziek bestand
            backgroundPlayer.settings.setMode("loop", true); // Zet de muziek op loop
            backgroundPlayer.controls.play(); // Speel de achtergrondmuziek af  // Nieuw Functie -2
        }



        private void LoadImages()
        {
            images = new List<Image>
            {
                Image.FromFile("Images/1.jpg"),
                Image.FromFile("Images/2.jpg"),
                Image.FromFile("Images/3.jpg"),
                Image.FromFile("Images/4.jpg"),


            };

            // Duplicate flashcards
            images.AddRange(images);  //  creates pairs of each flashcard

        }

        //Assign flashcards randomly to the PictureBox 
        private void AssignImagesToPictureBoxes()
        {
            pictureBoxes = new List<PictureBox>
            {
                   pictureBox5, pictureBox6, pictureBox11,
                    pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox2
            };

            if (images.Count < pictureBoxes.Count)
            {
                MessageBox.Show("Geen fotos om te tonen");
                return;
            }

            foreach (PictureBox pictureBox in pictureBoxes)
            {
                if (images.Count > 0)  
                {
                    int randomIndex = random.Next(images.Count);  
                    pictureBox.Tag = images[randomIndex];  
                    pictureBox.Click += PictureBox_Click;  
                    images.RemoveAt(randomIndex);  
                }
                else
                {
                    MessageBox.Show("Geen foto's meer te toonen!");
                    break;
                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goed Gedaan!");
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Good job! Click to play again.");
        }


        
        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (firstClicked != null && secondClicked != null)
                return;  

            PictureBox clickedBox = sender as PictureBox;

            if (clickedBox == null || clickedBox.Image != null)
                return;

           
            clickedBox.Image = (Image)clickedBox.Tag;

            
            if (firstClicked == null)
            {
                firstClicked = clickedBox;
                return;
            }

            secondClicked = clickedBox;

            // Check if flashcards match
            if (firstClicked.Tag == secondClicked.Tag)
            {
                pairsFound++;
                player.Play(); // geluid afspelen // Nieuw Functie -1


                if (pairsFound == pictureBoxes.Count / 2)
                {
                    
                    lblMessage.Visible = true;  
                    btnPlayAgain.Visible = true;  
                }

              // Reset
                firstClicked = null;
                secondClicked = null;
            }

            else
            {
                // dely in case 2 pairs did not match
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;  
                timer.Tick += (s, args) =>
                {
                    firstClicked.Image = null;  
                    secondClicked.Image = null; 
                    firstClicked = null;  
                    secondClicked = null;  
                    timer.Stop();  
                };

                timer.Start();
            }
        }

        // PlayAgain Btn
        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            
            ResetGame();
            lblMessage.Visible = false; 
            btnPlayAgain.Visible = false;  
            MessageBox.Show("Klaar om opnieuw te spelen! Press OK!");
        }

        private void ResetGame()
        {
            
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                pictureBox.Image = null;  
            }

          
            firstClicked = null;
            secondClicked = null;
            pairsFound = 0; 
            LoadImages();  // Reload images
            AssignImagesToPictureBoxes();
            // Herstart de achtergrondmuziek indien nodig // nieuw funtie -2
            backgroundPlayer.controls.play();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            backgroundPlayer.controls.stop(); // Stop de muziek bij het sluiten van het spel
            base.OnFormClosing(e);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

