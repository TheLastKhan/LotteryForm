using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private List<int> lottery = new List<int>();
        private List<int> lucky = new List<int>();
        private List<int> completedPlayers = new List<int>(); // Stores the indices of completed players
        private Dictionary<int, string> playerNames = new Dictionary<int, string>(); // Stores player names
        private Dictionary<int, int> playerMatches = new Dictionary<int, int>(); // Stores the match counts for players
        bool button1Clicked = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Happy New Year !!! :) :D");

            comboBox1.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                comboBox1.Items.Add(i + 1 + " Player");
            }

            for (int i = 1; i <= 50; i++)
            {
                lottery.Add(i);
            }

            panel1.AutoScroll = true; // Added scroll feature
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var choice = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (choice == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Have a nice day!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetGame();
            button1Clicked = true;

            // Perform error check
            if (!ErrorCheck())
            {
                return;
            }

            // Handle game mode selection
            if (checkBox1.Checked)
            {
                MessageBox.Show("Player mode selected...");
            }
            else if (checkBox2.Checked)
            {
                MessageBox.Show("System mode selected...");
            }

            // Get player count and initialize game
            int playerCount = comboBox1.SelectedIndex + 1;

            InitializeLotteryNumbers();
            InitializeGameUI(playerCount);
        }

        private bool ErrorCheck()
        {
            // Check if both checkboxes are selected
            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("You cannot select both options. Please choose only one...");
                return false;
            }

            // Check if neither checkbox is selected
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                MessageBox.Show("Please select at least one game mode...");
                return false;
            }

            // Check if player count is selected
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the number of players...");
                return false;
            }

            return true;
        }


        private void InitializeLotteryNumbers()
        {
            if (lottery.Count == 0)
            {
                for (int i = 1; i <= 50; i++)
                {
                    lottery.Add(i);
                }
            }

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            if (listBox2.Items.Count == 0)
            {
                for (int i = 1; i <= 50; i++)
                {
                    listBox2.Items.Add(i.ToString());
                }
            }
        }

        private void InitializeGameUI(int playerCount)
        {
            string formattedLotteryNumbers = "";
            for (int i = 0; i < lottery.Count; i++)
            {
                formattedLotteryNumbers += lottery[i] + " ";

                if ((i + 1) % 10 == 0 || i == lottery.Count - 1)
                {
                    formattedLotteryNumbers += "\n";
                }
            }

            panel1.Controls.Clear();
            MessageBox.Show("New game started!");

            int playersPerColumn = 5; // Maximum of 5 players per column
            CreatePlayerPanels(playerCount, playersPerColumn);
        }

        private void CreatePlayerPanels(int playerCount, int playersPerColumn)
        {
            for (int playerIndex = 0; playerIndex < playerCount; playerIndex++)
            {
                Panel playerPanel = new Panel();
                playerPanel.BorderStyle = BorderStyle.FixedSingle;
                playerPanel.Size = new Size(300, 100);

                int column = playerIndex / playersPerColumn;
                int row = playerIndex % playersPerColumn;
                playerPanel.Location = new Point(column * 310, row * 110);

                TextBox playerTextBox = new TextBox();
                playerTextBox.Size = new Size(90, 20);
                playerTextBox.Location = new Point(10, 20);
                playerTextBox.BorderStyle = BorderStyle.FixedSingle;
                playerTextBox.Name = $"Player_{playerIndex}_Name";
                playerPanel.Controls.Add(playerTextBox);

                if (checkBox1.Checked)
                {
                    AddPlayerNumbers(playerPanel, playerIndex);
                }
                else if (checkBox2.Checked)
                {
                    AddRandomSystemNumbers(playerPanel);
                }

                panel1.Controls.Add(playerPanel);
            }
        }

        private void AddPlayerNumbers(Panel playerPanel, int playerIndex)
        {
            for (int i = 0; i < 5; i++)
            {
                TextBox numberBox = new TextBox();
                numberBox.Size = new Size(40, 20);
                numberBox.Location = new Point(10 + i * 50, 50);
                numberBox.BorderStyle = BorderStyle.FixedSingle;
                numberBox.Tag = $"Player_{playerIndex}_Number_{i}";
                playerPanel.Controls.Add(numberBox);
            }
        }

        private void AddRandomSystemNumbers(Panel playerPanel)
        {
            HashSet<int> generatedNumbers = new HashSet<int>(); // To avoid duplicate numbers

            for (int i = 0; i < 5; i++)
            {
                int randomNumber;

                // First generate a random number
                randomNumber = random.Next(1, 51); // Random number between 1 and 50

                // Keep generating a new random number while it already exists in the HashSet
                while (generatedNumbers.Contains(randomNumber))
                {
                    randomNumber = random.Next(1, 51); // Generate a new random number
                }

                generatedNumbers.Add(randomNumber); // Add the new unique number to the HashSet

                Label numberLabel = new Label();
                numberLabel.Text = randomNumber.ToString(); // Random number between 1 and 50
                numberLabel.Size = new Size(40, 20);
                numberLabel.TextAlign = ContentAlignment.MiddleCenter;
                numberLabel.BorderStyle = BorderStyle.FixedSingle;
                numberLabel.Location = new Point(10 + i * 50, 50);
                playerPanel.Controls.Add(numberLabel);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button1Clicked == false)
            {
                MessageBox.Show("Please start the game first...");
                return;
            }

            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("You cannot select both options. Please choose only one...");
                return;
            }
            else if (!checkBox1.Checked && !checkBox2.Checked)
            {
                MessageBox.Show("Please select at least one game mode...");
                return;
            }

            if (lottery.Count == 0)
            {
                MessageBox.Show("All numbers have been drawn from the bag. The game is over!");
                ShowFinalResults();
                return;
            }

            DrawLotteryNumber();
        }

        private void DrawLotteryNumber()
        {
            int index = random.Next(0, lottery.Count);
            int drawnNumber = lottery[index];
            lottery.RemoveAt(index);
            lucky.Add(drawnNumber);

            listBox1.Items.Add(drawnNumber);
            listBox2.Items.Clear();
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            foreach (int number in lottery)
            {
                listBox2.Items.Add(number);
            }

            CheckPlayerMatches(drawnNumber);
        }

        private void CheckPlayerMatches(int drawnNumber)
        {
            foreach (Control panel in panel1.Controls)
            {
                if (panel is Panel playerPanel)
                {
                    int matchedCount = 0;
                    int playerIndex = panel1.Controls.OfType<Panel>().ToList().IndexOf(playerPanel) + 1;
                    TextBox nameBox = playerPanel.Controls.OfType<TextBox>().FirstOrDefault();
                    if (nameBox != null && !playerNames.ContainsKey(playerIndex))
                    {
                        playerNames[playerIndex] = nameBox.Text;
                    }

                    foreach (Control control in playerPanel.Controls)
                    {
                        if (control is Label numberLabel)
                        {
                            if (int.TryParse(numberLabel.Text, out int number) && lucky.Contains(number))
                            {
                                matchedCount++;
                                numberLabel.BackColor = Color.LightGreen;
                            }
                        }
                        else if (control is TextBox numberBox)
                        {
                            if (int.TryParse(numberBox.Text, out int number) && lucky.Contains(number))
                            {
                                matchedCount++;
                                numberBox.BackColor = Color.LightGreen;
                            }
                        }
                    }

                    playerMatches[playerIndex] = matchedCount;

                    if (matchedCount == 5 && !completedPlayers.Contains(playerIndex))
                    {
                        completedPlayers.Add(playerIndex);
                        MessageBox.Show($"({playerNames[playerIndex]}) matched all numbers!");
                    }
                }
            }

            MessageBox.Show($"Drawn number from the bag: {drawnNumber}\nRemaining numbers in the bag: {lottery.Count}");
        }

        // Show the final ranking of players
        private void ShowFinalResults()
        {
            var sortedPlayers = completedPlayers
                .OrderByDescending(playerIndex => playerMatches[playerIndex])
                .ThenBy(playerIndex => completedPlayers.IndexOf(playerIndex))
                .ToList();

            string resultMessage = "Player Rankings:\n";
            int rank = 1;
            foreach (var playerIndex in sortedPlayers)
            {
                resultMessage += $"{rank++}. Player ({playerNames[playerIndex]}) \n";
            }

            MessageBox.Show(resultMessage, "Happy New Year !!! :) :D");
        }

        private void ResetGame()
        {
            // Clear all player panels
            foreach (Control panel in panel1.Controls)
            {
                if (panel is Panel playerPanel)
                {
                    foreach (Control control in playerPanel.Controls)
                    {
                        if (control is Label numberLabel)
                        {
                            numberLabel.BackColor = Color.Transparent;
                        }
                        else if (control is TextBox numberBox)
                        {
                            numberBox.BackColor = Color.White;
                            numberBox.Clear(); // Clear the value of TextBox
                        }
                    }
                }
            }

            // Clear collections
            lottery.Clear();
            lucky.Clear();
            playerMatches.Clear();
            completedPlayers.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            playerNames.Clear(); // Clear player names

            // Create new lottery bag
            for (int i = 1; i <= 50; i++)
            {
                lottery.Add(i);
            }

        }
    }
}