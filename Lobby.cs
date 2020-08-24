///<name>Lobby class</name>
/// <summary>
/// The users chooses from different options and sends the total number of players, 
/// number of human players, number of computer players, difficulty level of computer players, 
/// and game time to the Game class.
/// </summary>
/// <author>Daisy Watson</author>
/// <date>5/15/20</date>

using System;
using System.Windows.Forms;

namespace DialogGame
{
    public partial class Lobby : Form
    {
        private int m_numPlayers = 2;
        private int m_numHumanPlayers = 1;
        private int m_numCompPlayers = 1;
        private bool m_easyDifficulty = true; //difficulty of the computer player(s) only
        private int m_gameTime = 0;
     
        public Lobby()
        {
            InitializeComponent();      
        }

        /// <summary>
        /// Default no warning when starting the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SetWarningText("");
        }

        /// <summary>
        ///Warn user if number of computer/human players doesn't add up to the total
        ///number of players selected
        /// </summary>
        /// <param name="a_text"></param>
        /// <remarks> a_text displays the warning text to the user</remarks>
        private void SetWarningText(string a_text)
        {
            warningLabel.Text = a_text;
        }

        /// <summary>
        /// Set total number of players
        /// Check that all the players add up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoLabel_CheckedChanged(object sender, EventArgs e)
        {
            SetNumPlayers(2);
            ReadyToPlay();
        }

        /// <summary>
        /// Set total number of players
        /// Check that all the players add up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeLabel_CheckedChanged(object sender, EventArgs e)
        {
            SetNumPlayers(3);
            ReadyToPlay();
        }

        /// <summary>
        /// Set total number of players
        /// Check that all the players add up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FourLabel_CheckedChanged(object sender, EventArgs e)
        {
            SetNumPlayers(4);
            ReadyToPlay();
        }

        /// <summary>
        /// Set the total number of players
        /// </summary>
        /// <param name="a_num"></param>
        /// <remarks> a_num is the number of players the player input </remarks>
        private void SetNumPlayers (int a_num)
        {
            m_numPlayers = a_num;
        }

        /// <summary>
        /// Check to see that number of computer/players matches the total number of players
        /// </summary>
        public void ReadyToPlay()
        {
            if (CheckNumPlayers() == true)
            {
                SetWarningText("");
                PlayButton.Visible = true;
            }
            else
            {
                PlayButton.Visible = false;
                if (GetHumanPlayers() + GetCompPlayers() < GetNumPlayers())
                {
                    warningLabel.Text = "Warning: too few selected players";
                }
                else
                {
                    warningLabel.Text = "Warning: too many selected players";
                }
            }
        }

        /// <summary>
        /// Get the number of human players
        /// </summary>
        /// <returns>m_numHumanPlayers</returns>
        private int GetHumanPlayers()
        {
            return m_numHumanPlayers;
        }

        /// <summary>
        /// Get the number of computer players
        /// </summary>
        /// <returns>m_numCompPlayers</returns>
        private int GetCompPlayers()
        {
            return m_numCompPlayers;
        }

        /// <summary>
        /// Get total number of players
        /// </summary>
        /// <returns>m_numPlayers</returns>
        private int GetNumPlayers()
        {
            return m_numPlayers;
        }

        /// <summary>
        /// If the combination of player types adds up to total number of players
        /// </summary>
        /// <returns>true or false</returns>
        public bool CheckNumPlayers()
        {
            if (GetHumanPlayers() + GetCompPlayers() == GetNumPlayers())
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Set num human players
        /// Check that number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZeroHumanButton_CheckedChanged(object sender, EventArgs e)
        {
            SetHumanPlayers(0);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num human players
        /// Check that number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneHumanButton_CheckedChanged(object sender, EventArgs e)
        {
            SetHumanPlayers(1);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num human players
        /// Check that number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoHumanButton_CheckedChanged(object sender, EventArgs e)
        {
            SetHumanPlayers(2);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num human players
        /// Check that number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeHumanButton_CheckedChanged(object sender, EventArgs e)
        {
            SetHumanPlayers(3);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num human players
        /// Check that number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FourHumanButton_CheckedChanged(object sender, EventArgs e)
        {
            SetHumanPlayers(4);
            ReadyToPlay();
        }

        /// <summary>
        /// Set number of human players equal to what user picked
        /// </summary>
        /// <param name="a_num"></param>
        /// <remarks> a_num is the number the user picked </remarks>
        private void SetHumanPlayers(int a_num)
        {
            m_numHumanPlayers = a_num;
        }

        /// <summary>
        /// Set num comp players
        /// Turn off difficulty if all human players
        /// Check if total number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZeroComputerButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCompPlayers(0);
            ToggleDifficultyUI(false);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num comp players
        /// Turn on difficulty for computer player
        /// Check if total number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneComputerButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCompPlayers(1);
            ToggleDifficultyUI(true);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num comp players
        /// Turn on difficulty for computer players
        /// Check if total number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoComputerButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCompPlayers(2);
            ToggleDifficultyUI(true);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num comp players
        /// Turn on difficulty for computer players
        /// Check if total number of players adds up 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeComputerButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCompPlayers(3);
            ReadyToPlay();
        }

        /// <summary>
        /// Set num comp players
        /// Turn on difficulty for computer players
        /// Check if total number of players adds up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FourComputerButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCompPlayers(4);
            ToggleDifficultyUI(true);
            ReadyToPlay();
        }

        /// <summary>
        /// Set number of computer players equal to what user picked
        /// </summary>
        /// <param name="a_num"></param>
        /// <remarks> a_num is the number the user picked </remarks>
        private void SetCompPlayers (int a_num)
        {
            m_numCompPlayers = a_num;
        }

        /// <summary>
        /// Toggle the visibility of the easy/hard buttons and difficulty label
        /// </summary>
        /// <param name="turnOn"></param>
        /// <remarks> turnOn the difficulty options if user chooses > 1 computer players</remarks>
        private void ToggleDifficultyUI(bool a_turnOn)
        {
            if (a_turnOn)
            {
                difficultyLabel.Visible = true;
                EasyButton.Visible = true;
                HardButton.Visible = true;
            }
            if (!a_turnOn)
            {
                difficultyLabel.Visible = false;
                EasyButton.Visible = false;
                HardButton.Visible = false;
            }
        }

        /// <summary>
        /// Set to easy difficulty (true)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyButton_CheckedChanged(object sender, EventArgs e)
        {
            SetDifficulty(true);
        }

        /// <summary>
        /// set to easy or hard mode
        /// </summary>
        /// <param name="a_easy"></param>
        /// <remarks> a_easy = true is easy mode t</remarks>
        private void SetDifficulty(bool a_easy)
        {
            m_easyDifficulty = a_easy;
        }

        /// <summary>
        /// Set to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardButton_CheckedChanged(object sender, EventArgs e)
        {
            SetDifficulty(false);
        }

        /// <summary>
        /// Turn off time limit if not timed game
        /// Set time to 0 minutes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassicLabel_CheckedChanged(object sender, EventArgs e)
        {
            TimeLimitBox.Visible = false;
            SetGameTime(0);
        }

        /// <summary>
        /// Set time limit to 5 minutes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeLimitLabel_CheckedChanged(object sender, EventArgs e)
        {
            TimeLimitBox.Visible = true;
            SetGameTime(5);
        }

        /// <summary>
        /// How long a timed game will last
        /// </summary>
        /// <param name="a_num"></param>
        /// <remarks> a_num is the length the user picked in minutes </remarks>
        private void SetGameTime (int a_num)
        {
            m_gameTime = a_num;
        }

        /// <summary>
        ///5 minutes, 10 minutes, or 30 minutes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeLimitBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string amtTime = TimeLimitBox.Text;
            string[] splitTime = amtTime.Split(' ');

            SetGameTime(Convert.ToInt32(splitTime[0]));
        }

        /// <summary>
        /// Pass on values, close this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            //Pass number of players (human/computer) and mode to next view, the game view  
            Game startGame = new Game(GetNumPlayers(), GetHumanPlayers(), GetCompPlayers(), 
                GetGameTime(), GetCompDifficulty());
            this.Hide();
            startGame.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Return the chosen game time, 0 if classic game
        /// </summary>
        /// <returns>m_gameTime</returns>
        private int GetGameTime()
        {
            return m_gameTime;
        }

        /// <summary>
        /// Return easy or hard difficulty
        /// True for easy, false for hard
        /// </summary>
        /// <returns>m_easyDifficulty</returns>
        private bool GetCompDifficulty()
        {
            return m_easyDifficulty;
        }
    }
}
