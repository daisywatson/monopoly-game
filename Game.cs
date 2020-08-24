/// <summary>
/// Directly gets user input and holds the main game loop and UI for the game. 
/// All the buttons and controls respond in this class, the timer counts down if it's a timed game. 
/// </summary>
/// <author>Daisy Watson</author>
/// <date>5/15/20</date>

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogGame
{
    public partial class Game : Form
    {
        private int m_gameTime = 0;
        private int m_TimeLeft = 0;
        private Controller m_myController = new Controller(); //game controller
        private Board m_gameBoard = new Board();
        private Computer m_compStrat; //computer player(s) strategy, easy or hard
        private bool m_playerOrderNotified = false; //if the player has been informed of the order players will go
        TaskCompletionSource<bool> auctionFinished = null;

        private void ClassicGame_Load(object sender, EventArgs e) {}

        /// <summary>
        /// Set class variables 
        /// Set timer if applicable
        /// Create Computer object if applicable 
        /// </summary>
        /// <param name="a_numPlayers"></param>
        /// <param name="a_numHumanPlayers"></param>
        /// <param name="a_numCompPlayers"></param>
        /// <param name="a_timeLimit"></param>
        /// <param name="a_easyDifficulty"></param>
        public Game(int a_numPlayers, int a_numHumanPlayers, int a_numCompPlayers, 
            int a_timeLimit, bool a_easyDifficulty)
        {
            InitializeComponent();

            m_myController.SetNumPlayers(a_numPlayers);
            m_myController.SetHumanPlayers(a_numHumanPlayers);
            m_myController.SetCompPlayers(a_numCompPlayers);
            if (m_myController.GetNumComp() > 0)
            {
                m_compStrat = new Computer();
                m_compStrat.SetDifficulty(a_easyDifficulty);
                if (m_gameTime != 0)
                {
                    m_compStrat.SetTimeLimit(true);
                }
            }
            m_myController.SetPlayerTypeGame();
 
            m_gameTime = a_timeLimit;
            if (m_gameTime != 0)
            {
                SetTimeLimit(m_gameTime);
            }

            //start rolling dice to determine who goes first:
            PlayersDeterminePlayerOrder();        
        }

        /// <summary>
        /// If the player closes the game window,
        /// double chekc before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = "Are you sure you want to close the window?";
            string caption = "Exiting the game";
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            // Displays the MessageBox.
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (result == DialogResult.Cancel) e.Cancel = true;
        }

        /// <summary>
        /// Set the amount of time the game will go for
        /// </summary>
        /// <param name="a_timeLimit"></param>
        private void SetTimeLimit(int a_timeLimit)
        {
            m_TimeLeft = (a_timeLimit * 60);
            timeLabel.Visible = true;
            timer1.Start();
        }

        /// <summary>
        /// The timer counting down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (m_TimeLeft > 0)
            {
                // Display the new time left
                // by updating the time label.
                m_TimeLeft = m_TimeLeft - 1;
                timeLabel.Text = StopwatchDisplay();
            }
            else
            {
                //Times up
                timer1.Stop();
                timeLabel.Text = "Time's up!";
                TurnOffAllButtons();
                PropertiesBox.Visible = false;
                TimeGameOver();
            }
        }

        /// <summary>
        /// Display the time counting  down on the screen
        /// </summary>
        /// <returns>how much time is left</returns>
        private string StopwatchDisplay()
        {
            int numMins = m_TimeLeft / 60;
            int numSecs = m_TimeLeft % 60;

            //if TimeLeft < 60
            string countdownNum = "";
            if (numMins > 1)
            {
                countdownNum = Convert.ToString(numMins) + " mins. ";
            }
            if (numMins == 1)
            {
                countdownNum = Convert.ToString(numMins) + " min. ";
            }

            if (numSecs != 0)
            {
                if (numSecs == 1)
                {
                    countdownNum += Convert.ToString(numSecs) + " sec.";
                }
                else
                {
                    countdownNum += Convert.ToString(numSecs) + " secs.";
                }
            }

            return (countdownNum);
        }

        /// <summary>
        /// The players roll the dice to determine what order each player plays
        /// User must press the RollDice button if not an all computer game
        /// </summary>
        private void PlayersDeterminePlayerOrder()
        {
            dialogBox.Text += "The players must roll the dice to determine who goes first." + Environment.NewLine;
            string playerTypeGame = m_myController.GetPlayerTypeGame();
            if (playerTypeGame == "mixed")
            {
                dialogBox.Text += "This is a mixed game with both human and computer players." + Environment.NewLine;
                dialogBox.Text += "The human player(s) roll first." + Environment.NewLine;
                dialogBox.Text += "Player 1(human) rolls first." + Environment.NewLine;
                dialogBox.Text += "There are " + Convert.ToString(m_myController.GetNumHumans()) + 
                    " human player(s) and "
                    + Convert.ToString(m_myController.GetNumComp()) + " computer player(s)." + Environment.NewLine;

                RollDiceButton.Visible = true;       
            }
            if (playerTypeGame == "human")
            {
                dialogBox.Text += "This is a game with only human players." + Environment.NewLine;
                dialogBox.Text += "Player 1 rolls first." + Environment.NewLine; ;
                dialogBox.Text += "There are " + Convert.ToString(m_myController.GetNumPlayers()) + 
                    " players." + Environment.NewLine;

                RollDiceButton.Visible = true; 
            }
            if (playerTypeGame == "computer")
            {
                dialogBox.Text += "This is a game with only computer players." + Environment.NewLine;
                dialogBox.Text += "Player 1 rolls first." + Environment.NewLine;
                dialogBox.Text += "There are " + Convert.ToString(m_myController.GetNumPlayers()) + 
                    " players." + Environment.NewLine;
                dialogBox.Text += m_myController.ComputerRollsDice();

                OkButton.Visible = true;
            }
        }

        /// <summary>
        /// Only visible in a mixed/human only game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollDiceButton_Click(object sender, EventArgs e)
        {
            dialogBox.Text += m_myController.DiceOutput(); //what the player rolled
            dialogBox.Text += m_myController.RerollOrNext(); //whose turn is next or if player needs to reroll

            if(m_myController.FinishedRolling())
            {
                //human players finished rolling dice
                RollDiceButton.Visible = false;
                dialogBox.Text += m_myController.ComputerRollsDice(); //if there are any comoputer players
                OkButton.Visible = true;
            }
        }

        /// <summary>
        /// 1. player clicks once: display the player order 
        /// 1. player clicks again: 
        /// move to next screen to choose game piece color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (m_playerOrderNotified == false)
            {
                //Display the order of the players
                dialogBox.Text += m_myController.DisplayPlayerOrder();
                m_playerOrderNotified = true;
            }
            else
            {
                //After the player(s) sees the player order, the player presses 
                //"ok" again and the players will choose their game piece color next
                OkButton.Visible = false;
                ChooseColor();
            }
        }

        /// <summary>
        /// All computer game chooses pieces automatically
        /// Human players choose by pressing color buttons
        /// In a mixed game, the human players choose first,
        /// then the computer players choose the remaining colors 
        /// </summary>
        private void ChooseColor()
        {        
            dialogBox.Text = "Now the players pick the color of their game piece." + Environment.NewLine;
           
            //check if all computer game here, if so, computer players pick colors then start game
            if (m_myController.CompOnlyGame())
            {
                //output of computer chosen piece colors
                dialogBox.Text += m_myController.ComputerPlayersChooseColor() + Environment.NewLine;
                StartGame();            
            }
            if (m_myController.HumanOnlyGame()) //human players choose piece colors
            {
                dialogBox.Text += "It's " + m_myController.GetCurrentPlayerNameAndType() + 
                    "'s turn to pick a color: " + Environment.NewLine;
                DisplayPieceColorButtons(true);
                EnableColorButtons(true);
            }
            if (m_myController.MixedGame())
            {
                dialogBox.Text += "It's " + m_myController.GetCurrentPlayerNameAndType() + 
                    "'s turn to pick a color: " + Environment.NewLine;      
                DisplayPieceColorButtons(true);
                if (m_myController.HumanTurn())
                {
                    //If human turn, enable color buttons to choose from
                    //By default, the buttons are disabled/can't be clicked
                    EnableColorButtons(true);
                }
                else
                {
                    //else let the computer choose 
                    CompPlayerChoosesColor();
                }
            }
        }

        /// <summary>
        /// turn on or off the buttons for choosing the colors of the players' game pieces
        /// </summary>
        /// <param name="a_turnOn"></param>
        private void DisplayPieceColorButtons(bool a_turnOn)
        {
            if (a_turnOn)
            {
                YellowButton.Visible = true;
                BlueButton.Visible = true;
                OrangeButton.Visible = true;
                PurpleButton.Visible = true;
            }
            if (!a_turnOn)
            {
                YellowButton.Visible = false;
                BlueButton.Visible = false;
                OrangeButton.Visible = false;
                PurpleButton.Visible = false;
            }
        }

        /// <summary>
        /// Enable buttons to be clicked
        /// a_enableButtons = true to be clickable
        /// Buttons are set to not clickable to prevent
        /// human from clicking during computer's turn
        /// </summary>
        /// <param name="a_enableButtons"></param>
        private void EnableColorButtons(bool a_enableButtons)
        {
            if (a_enableButtons)
            {
                YellowButton.Enabled = true;
                BlueButton.Enabled = true;
                OrangeButton.Enabled = true;
                PurpleButton.Enabled = true;
            }
            if (!a_enableButtons)
            {
                YellowButton.Enabled = false;
                BlueButton.Enabled = false;
                OrangeButton.Enabled = false;
                PurpleButton.Enabled = false;
            }
        }

        /// <summary>
        /// Computer player chooses a color. The button is turned off
        /// The color is set for the player
        /// </summary>
        private void CompPlayerChoosesColor()
        {
            //if computer player, output the computer's choice and turn off the applicable button
            string computerChoice = m_myController.ComputerPlayerColorChoice();
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " picked " 
                + computerChoice + "." + Environment.NewLine;
            TurnOffColorButton(computerChoice);
            ChooseColor2(computerChoice);        
        }

        /// <summary>
        /// turn off individual buttons, used by computer player
        /// </summary>
        /// <param name="a_color"></param>
        private void TurnOffColorButton (string a_color)
        {
            if (a_color == "yellow")
            {
                YellowButton.Visible = false;
            }
            if (a_color == "blue")
            {
                BlueButton.Visible = false;
            }
            if (a_color == "orange")
            {
                OrangeButton.Visible = false;

            }
            if (a_color == "purple")
            {
                PurpleButton.Visible = false;
            }
        }

        /// <summary>
        /// Human player picked this color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YellowButton_Click(object sender, EventArgs e)
        {
            YellowButton.Visible = false;
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " picked yellow." + Environment.NewLine;

            ChooseColor2("yellow");
        }

        /// <summary>
        /// Human player picked this color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlueButton_Click(object sender, EventArgs e)
        {
            BlueButton.Visible = false;
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " picked blue." + Environment.NewLine;

            ChooseColor2("blue");
        }

        /// <summary>
        /// Human player picked this color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrangeButton_Click(object sender, EventArgs e)
        {
            OrangeButton.Visible = false;
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " picked orange." + Environment.NewLine;

            ChooseColor2("orange");
        }

        /// <summary>
        /// Human player picked this color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PurpleButton_Click(object sender, EventArgs e)
        {
            PurpleButton.Visible = false;
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " picked purple." + Environment.NewLine;

            ChooseColor2("purple");
        }

        /// <summary>
        /// Set the color for player, increment to next player, check if finished
        /// </summary>
        /// <param name="a_color"></param>
        /// <remarks>  a_color is the color the human player chose </remarks>
        private void ChooseColor2(string a_color)
        {
            //if human turn, set color, increment to next player
            if (m_myController.HumanTurn())
            {
                m_myController.SetHumanPlayerColor(a_color);
                EnableColorButtons(false);
            }
            else 
            {
                //eles computer turn, increment to next player
                m_myController.IncrementCurrentPlayerPosition();             
            }

            //check if finished choosing colors
            if (m_myController.FinishedColorPicking())
            {
                //turn off any remaining color piecce buttons
                DisplayPieceColorButtons(false);
                StartGame();
            }
            else
            {
                dialogBox.Text += "It's " + m_myController.GetCurrentPlayerNameAndType() + 
                    "'s turn to pick a color: " + Environment.NewLine;
                //Human player chooses from available color buttons
                if (m_myController.HumanTurn()) { EnableColorButtons(true); }
                if (!m_myController.HumanTurn()) { CompPlayerChoosesColor(); }
            }
        }

        /// <summary>
        /// Start Game
        /// 1. Turn off visibility of current player piece on board
        /// 2. Roll the dice
        /// 3. Store which new square landed on
        /// 4. Turn on player piece on new square
        /// 5. Show the player's funds
        /// 6. Go the player's options menu
        /// </summary>
        private void StartGame()
        {
            StartGameSetup();
            dialogBox.Text += Environment.NewLine + "A new game begins." + Environment.NewLine;

            //Turn off player's current position
            TogglePlayerSquare(0, m_myController.GetCurrentPlayerColor(), false);

            dialogBox.Text += "It's " + m_myController.GetCurrentPlayerNameAndType() + "'s turn. " 
                + m_myController.GetCurrentPlayerName() + " rolls the dice." + Environment.NewLine;
            int[] diceArray = m_myController.RollDice();
            dialogBox.Text += m_myController.GetCurrentPlayerName() + " rolled a " + diceArray[0] + " and a " 
                + diceArray[1] + "." + Environment.NewLine;
            int diceSum = diceArray[0] + diceArray[1];

            //Store which square the player landed on:
            m_myController.SetCurrentPlayerSquare(diceSum);

            int currentPlayerSquare = m_myController.GetCurrentPlayerSquare();
            dialogBox.Text += m_myController.GetCurrentPlayerName() + " moves " + Convert.ToString(diceSum) +
                " spaces forward to square " + Convert.ToString(currentPlayerSquare) + "." + Environment.NewLine;
            //turn square on
            TogglePlayerSquare(currentPlayerSquare, m_myController.GetCurrentPlayerColor(), true);

            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            FundsLabel.Visible = true;

            //Go to options menu if human player, a separate function if computer player
            if (m_myController.HumanTurn()) OptionsMenu(currentPlayerSquare);

            if (!m_myController.HumanTurn()) ComputerTurn(currentPlayerSquare);
        }

        /// <summary>
        /// 1. Set the first player
        /// 2. Turn on the labels that show the order of all the players
        /// 3. Display the player's pieces in their initial places on Go square
        /// </summary>
        private void StartGameSetup()
        {
            //Set the current player to the first player in the player order 
            m_myController.SetFirstPlayer();
            TurnOnPlayersLabels();
            UpdatePlayersLabel(m_myController.GetCurrentPlayerName()); //show whose turn it is
            DisplayInitialPlayerPieces();
            PropertiesBox.Visible = true; //properties a player owns
        }

        /// <summary>
        /// Make player labels visible at beginning of the game
        /// This set them/turns them on
        /// </summary>
        private void TurnOnPlayersLabels()
        {
            for (int playerNum = 1; playerNum <= m_myController.GetNumPlayers(); playerNum++)
            {         
                //Find the label by name and make it visible
                string labelName = "player" + Convert.ToString(playerNum) + "Label";
                Label playerLabel = Controls.Find(labelName, true).FirstOrDefault() as Label;
                playerLabel.Visible = true;           
            }
        }

        /// <summary>
        /// Turn on the colors of the the different plaeyr
        /// Highlight which player's turn it is
        /// each turn.
        /// Loop through array to display all colors and
        /// find whose turn it is.
        /// </summary>
        /// <param name="a_playerToUpdate"></param>
        private void UpdatePlayersLabel(string a_playerToUpdate)
        {
            Player1Label.Text = "";
            int playerCount = 1;
            Player[] pArray = m_myController.GetPlayerArray();
            foreach (Player player in pArray)
            {
                string labelName = "player" + Convert.ToString(playerCount) + "Label";
                Label playerLabel = Controls.Find(labelName, true).FirstOrDefault() as Label;
                playerLabel.Text = player.GetPlayerName();
                if (player.GetPlayerColor() == "yellow")
                {
                    playerLabel.BackColor = Color.Yellow;
                }
                if (player.GetPlayerColor() == "blue")
                {
                    playerLabel.BackColor = Color.PaleTurquoise;
                }
                if (player.GetPlayerColor() == "orange")
                {
                    playerLabel.BackColor = Color.Orange;
                }
                if (player.GetPlayerColor() == "purple")
                {
                    playerLabel.BackColor = Color.MediumPurple;
                }
                if (player.GetPlayerName() == a_playerToUpdate)
                {
                    playerLabel.Text += "'s turn";
                }
                playerCount++;
            }
        }

        /// <summary>
        /// Display initial pieces n the game board
        /// On the Go square 
        /// </summary>
        private void DisplayInitialPlayerPieces()
        {
            Player[] pArray = m_myController.GetPlayerArray();
            for (int j = 0; j < m_myController.GetNumPlayers(); j++)
            {
                if (pArray[j].GetPlayerColor() == "yellow")
                {
                    square0y.Visible = true;
                }
                if (pArray[j].GetPlayerColor() == "blue")
                {
                    square0b.Visible = true;
                }
                if (pArray[j].GetPlayerColor() == "orange")
                {
                    square0o.Visible = true;
                }
                if (pArray[j].GetPlayerColor() == "purple")
                {
                    square0p.Visible = true;
                }
            }
        }

        /// <summary>
        /// turn on/off player's piece on the square
        /// by searching for it on the board
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_playerColor"></param>
        /// <param name="a_turnOn"></param>
        private void TogglePlayerSquare(int a_squareNum, string a_playerColor, bool a_turnOn)
        {
            string playerPieceName = "square" + Convert.ToString(a_squareNum) + a_playerColor[0];

            Label playerPiece = Controls.Find(playerPieceName, true).FirstOrDefault() as Label;
            if (a_turnOn == true)
            {
                playerPiece.Visible = true;
            }
            else
            {
                playerPiece.Visible = false;
            }
        }

        /// <summary>
        /// If it's the computer's turn, go through computer's
        /// options menu for each different type of square.
        /// 
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void ComputerTurn(int a_currentPlayerSquare)
        {
            //Property options dropdown menu is locked during computer's turn
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "go")
            {
                PlayerLandedGoSquare();
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                    " collects $200. " + Environment.NewLine;
                CompFinishedTurn();
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "parking")
            {
                PlayerLandedParkingSquare();
                CompFinishedTurn();
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "tax")
            {
                PlayerLandedTaxSquare(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "chance")
            {
                PlayerLandedChanceSquare(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "chest")
            {
                PlayerLandedChestSquare(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "jail")
            {
                JailOptions(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "utilities" ||
                m_gameBoard.GetSquareType(a_currentPlayerSquare) == "railroad" ||
                m_gameBoard.GetSquareType(a_currentPlayerSquare) == "property")
            {
                //else it's a property square
                PropertySquare(a_currentPlayerSquare);
            }
        }

        /// <summary>
        /// At the end of the computer's turn,
        /// computer has option to sell jail cards
        /// if any are owned as well as open up
        /// propetery options.
        /// If neither are owned, show the end turn button
        /// </summary>
        private void CompFinishedTurn()
        {
            //Whether or not to sell get out of jail cards on hand, if any owned
            if (m_myController.GetCurrentPlayerNumJailCards() > 0) CompCanSellJailCards();
            //Computer's property options (if any properties owned)     
            else if (m_myController.GetCurrentPlayerTotalProperties() > 0) CompPropertyOptions();

            if (m_myController.GetCurrentPlayerNumJailCards() == 0 &&
                m_myController.GetCurrentPlayerTotalProperties() == 0)
            { EndTurnButton.Visible = true; }
        }

        /// <summary>
        /// If the computer has jail cards to jail, 
        /// the computer will decide whether or not to 
        /// sell them.
        /// The computer will loop through them to sell them
        /// and wait for an auction to finish before moving on
        /// to the next card.
        /// At the end of this, if the player owns any properties,
        /// the computer player will then open up property options. 
        /// </summary>
        private async void CompCanSellJailCards()
        {
            int numCards = m_myController.GetCurrentPlayerNumJailCards();
            //If player decides to sell jail card
            if (m_compStrat.SellJailCard(numCards, m_myController.GetCurrentPlayerFunds()))
            {
                for (int i = 0; i < numCards; i++)
                {
                    m_myController.SetSellCardBool(true);
                    m_myController.SetCompSellJailCard(true);
                    auctionFinished = new TaskCompletionSource<bool>();
                    Auction();
                    await auctionFinished.Task;
                }
                m_myController.SetSellCardBool(false);
                m_myController.SetCompSellJailCard(false);
            }
            if (m_myController.GetCurrentPlayerTotalProperties() > 0) CompPropertyOptions();
        }

        /// <summary>
        /// If the player owns any properties, the computer
        /// player can decide what to do with each property:
        /// Buy a house/hotel, sell a house/hotel, mortgage it, 
        /// unmortgage it.
        /// Afterwards, the player can decide whether or not to sell
        /// the property.
        /// </summary>
        private void CompPropertyOptions()
        {
            EndTurnButton.Visible = false;
            int[] ownedProperties = m_myController.GetCurrentPlayerPropertiesList().ToArray();
            //Loop through each property on the list
            for (int i = 0; i < ownedProperties.Length; i++)
            {
                int squareNum = ownedProperties[i];
                m_myController.SetPropertyName(m_gameBoard.GetSquareMessage(squareNum));

                //Whether or not to build a house (if possible)
                //Buy a house (if all colors owned, is an even build, house available, 4 houses/a hotel haven't been built
                //and player has enough funds
                if (m_gameBoard.HouseBuildable(squareNum) && !m_myController.CurrentPlayerFourHousesBuilt(squareNum)
                    && !m_myController.CurrentPlayerHotelBuilt(squareNum) && m_gameBoard.HouseAvailable() &&
                        m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(squareNum), 
                        squareNum, true)
                        && (m_gameBoard.GetHouseCost(squareNum) <= m_myController.GetCurrentPlayerFunds()))
                {
                    if (m_compStrat.BuyHouse()) BuyHouse();
                }
                //Whether or not to purchase a hotel (if possible): if four houses have been built, a hotel is available,
                //it's an even build, and player has enough funds. A hotel costs the same as a house
                if (m_myController.CurrentPlayerFourHousesBuilt(squareNum) && m_gameBoard.HotelAvailable() &&
                        m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(squareNum), 
                        squareNum, true)
                         && (m_gameBoard.GetHouseCost(squareNum) <= m_myController.GetCurrentPlayerFunds()))
                {
                    if (m_compStrat.BuyHotel()) BuyHotel();
                }
                //Whether or not to unmortgage a property
                if (m_myController.IsCurrentPlayerPropertyMortgaged(squareNum) &&
                    (m_gameBoard.GetUnmortgageCost(squareNum) <= m_myController.GetCurrentPlayerFunds()))
                {
                    if (m_compStrat.UnmortgageProperty()) PlayerUnmortgagedProperty();
                }
                else if (!m_myController.IsCurrentPlayerPropertyMortgaged(squareNum))
                {
                    //Do not mortgage immediately after unmortgaging
                    //Whether or not to mortgage a property
                    if (m_compStrat.MortgageProperty(m_myController.GetCurrentPlayerFunds())) MortgageProperty();
                }
                //Whether or not to sell a hotel
                if (m_myController.CurrentPlayerHotelBuilt(squareNum) && m_gameBoard.HotelSellable() &&
                         m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(squareNum), squareNum,
                         false))
                {
                    if (m_compStrat.SellHouseHotel(m_myController.GetCurrentPlayerFunds())) SellAHotel();
                }
                //Whether or not to sell a house
                //Sell a house (to bank) if it can be sold evenly and a hotel has not been built on the property     
                if (!m_myController.CurrentPlayerPropertyUnimproved(squareNum) && 
                    m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(squareNum), 
                    squareNum, false)
                        && !m_myController.CurrentPlayerHotelBuilt(squareNum))
                {
                    if (m_compStrat.SellHouseHotel(m_myController.GetCurrentPlayerFunds())) SellAHouse();
                }         
            }
            if (ownedProperties.Length > 0) CompPropOptionsSellProp();
            else EndTurnButton.Visible = true;
        }

        /// <summary>
        /// Whether or not computer player should sell a property
        /// After the computer decides to sell a property to raise some funds,
        /// it will wait for each auction to finish before moving onto the next property.
        /// Then the computer's turn will be done after finishing with property options 
        /// </summary>
        private async void CompPropOptionsSellProp()
        {  
            //For the remainder properties, sell the highest value properties first to 
            //raise some money (if the computer deems it necessary).
            //Having more properties left on the board equates to more chances to collect rent and fewer chances
            //to pay rent.
            int[] orderedProperties = m_myController.GetCurrentPlayerPropertiesList().ToArray().
                OrderByDescending(x => x).ToList().ToArray();
            for (int i = 0; i < orderedProperties.Length; i++)
            {
                if (m_myController.CurrentPlayerPropertyUnimproved(orderedProperties[i]))
                {
                    if (m_compStrat.SellPlayerProperty(m_myController.GetCurrentPlayerFunds()))
                    {
                        m_myController.SetBuyPropertyBool(true);
                        m_myController.SetAuctionPropertySquare(orderedProperties[i]);
                        m_myController.SetPlayerSale(true);
                        m_myController.SetCompPropOptionsSale(true);

                        auctionFinished = new TaskCompletionSource<bool>();
                        Auction();
                        await auctionFinished.Task;
                    }
                }
            }
      
            //Reset after finished
            m_myController.SetCompPropOptionsSale(false);
            m_myController.SetPlayerSale(false);
            m_myController.SetBuyPropertyBool(false);
            EndTurnButton.Visible = true;
        }

        /// <summary>
        /// Collect $200, update player's funds on-screen
        /// End turn button to signify ending turn
        /// </summary>
        private void PlayerLandedGoSquare()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " has landed on Go." + Environment.NewLine;
            m_myController.CurrentPlayerPassedGo();
            UpdateFunds(m_myController.GetCurrentPlayerFunds()); //update funds label in the UI
            EndTurnButton.Visible = true;
        }

        /// <summary>
        /// Nothing happens.
        /// </summary>
        private void PlayerLandedParkingSquare()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " has landed on free parking." + Environment.NewLine;
            EndTurnButton.Visible = true;
        }

        /// <summary>
        /// Player has to pay tax.
        /// If not enough funds to pay, player will go to InsufficientFunds()
        /// to raise some funds to pay the outstanding payment.
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void PlayerLandedTaxSquare (int a_currentPlayerSquare)
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has landed on a tax square, " +
                   m_gameBoard.GetSquareMessage(a_currentPlayerSquare) + "." + Environment.NewLine;
            m_myController.SetTaxBool(true); //Prevent player from opening property options before paying tax
            if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), 
                m_gameBoard.GetTaxAmount(a_currentPlayerSquare)))
            {
                if (m_gameBoard.GetTaxAmount(a_currentPlayerSquare) == 200)
                {
                    IncomeTaxSquare(a_currentPlayerSquare);
                }
                if (m_gameBoard.GetTaxAmount(a_currentPlayerSquare) == 100)
                {
                    LuxuryTaxSquare(a_currentPlayerSquare);
                }
            }
            else
            {
                if (m_myController.HumanTurn()) dialogBox.Text += "You don't";
                if (!m_myController.HumanTurn()) dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                        " doesn't";

                dialogBox.Text += " have sufficient funds to pay the tax." + Environment.NewLine;
                //income tax amount assumed to be $200 even if 10% of player's assets is lower than that amount
                m_myController.SetOutstandingPayment(m_gameBoard.GetTaxAmount(a_currentPlayerSquare));
                m_myController.SetTaxBool(false);
                InsufficientFunds();
            }
           
        }

        /// <summary>
        /// Player must pay $200 or 10% of all assets 
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void IncomeTaxSquare(int a_currentPlayerSquare)
        {
            //The player has two different options to pay if landing on the Income Tax square
            if (m_myController.HumanTurn()) HumanPaysTax(a_currentPlayerSquare);
            else { ComputerPaysTax(a_currentPlayerSquare); }
        }

        /// <summary>
        /// Player pays luxury tax amount of $100
        /// Turn off taxBool, signifying that player has finished
        /// paying tax and is now free to access property options
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void LuxuryTaxSquare(int a_currentPlayerSquare)
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " pays $" +
                   Convert.ToString(m_gameBoard.GetTaxAmount(a_currentPlayerSquare)) + "." + Environment.NewLine;
            m_myController.CurrentPlayerPaysMoney(m_gameBoard.GetTaxAmount(a_currentPlayerSquare));
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            m_myController.SetTaxBool(false);         
            if (m_myController.HumanTurn())
            {
                ShowJailCards();
                EndTurnButton.Visible = true;
            }
            if (!m_myController.HumanTurn()) CompFinishedTurn();
        }

        /// <summary>
        /// The computer decides which income tax to pay 
        /// Turn off taxBool, finished paying tax
        /// Go to CompFinishedTurn() for remainder of computer's turn
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void ComputerPaysTax (int a_currentPlayerSquare)
        {
            //The player has two different options to pay if landing on the Income Tax square
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has the option to pay $" +
                Convert.ToString(m_gameBoard.GetTaxAmount(a_currentPlayerSquare)) +
                " or pay 10% of their assets." + Environment.NewLine;
            //get computer decisions here
            if (m_compStrat.PayFlatIncomeTax(m_myController.GetCurrentPlayerTotalAssets()))
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has chosen to pay $200."
                    + Environment.NewLine;
            }
            else
            {
                int tenPercent = (m_myController.GetCurrentPlayerTotalAssets() / 10);
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() +
                    " has chosen to pay 10% of their assets, $" + Convert.ToString(tenPercent) + "." + 
                    Environment.NewLine;
            }
            m_myController.SetTaxBool(false);
            CompFinishedTurn();
        }

        /// <summary>
        /// Human player must decide which amount of tax to pay 
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void HumanPaysTax (int a_currentPlayerSquare)
        {
            //The player has two different options to pay if landing on the Income Tax square
            dialogBox.Text += "You can pay $" + Convert.ToString(m_gameBoard.GetTaxAmount(a_currentPlayerSquare))
                + " or you can pay 10% of your assets." + Environment.NewLine;
            TwoHundredButton.Visible = true;
            TenPercentButton.Visible = true;
        }

        /// <summary>
        /// The player draws a chance card and must follow the message on it
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void PlayerLandedChanceSquare(int a_currentPlayerSquare)
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " has landed on chance." + Environment.NewLine;
            dialogBox.Text += "The card says: " + Environment.NewLine;
            dialogBox.Text += m_gameBoard.GetChanceMessage() + Environment.NewLine;
            ChestChance("chance", a_currentPlayerSquare);
        }

        /// <summary>
        /// The player draws a community chest card and must follow the message on it
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void PlayerLandedChestSquare (int a_currentPlayerSquare)
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " has landed on a community chest." + Environment.NewLine;
            dialogBox.Text += "The card says: " + Environment.NewLine; ;
            dialogBox.Text += m_gameBoard.GetChestMessage() + Environment.NewLine; ;
            ChestChance("chest", a_currentPlayerSquare);
        }

        /// <summary>
        /// The options menu during the human's turn
        /// What the player must do depends on what kind of square
        /// was landed on.
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void OptionsMenu(int a_currentPlayerSquare)
        {
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "go")
            {
                PlayerLandedGoSquare();
                if (m_myController.HumanTurn()) dialogBox.Text += "You collect";
                else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " collects";
                dialogBox.Text += " $200. " + Environment.NewLine;
                ShowJailCards();
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "parking")
            {
                PlayerLandedParkingSquare();
                ShowJailCards();
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "tax")
            {
                PlayerLandedTaxSquare(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "chance")
            {
                PlayerLandedChanceSquare(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "chest")
            {
                PlayerLandedChestSquare(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "jail")
            {
                JailOptions(a_currentPlayerSquare);
            }
            if (m_gameBoard.GetSquareType(a_currentPlayerSquare) == "utilities" ||
               m_gameBoard.GetSquareType(a_currentPlayerSquare) == "railroad" ||
               m_gameBoard.GetSquareType(a_currentPlayerSquare) == "property")
            {
                
                //else it's a property square
                PropertySquare(a_currentPlayerSquare);
            }
        }

        /// <summary>
        /// Update the amount of funds the player has on-screen
        /// </summary>
        /// <param name="a_playerFunds"></param>
        private void UpdateFunds(int a_playerFunds)
        {
            FundsLabel.Text = "$" + Convert.ToString(a_playerFunds);
        }

        /// <summary>
        /// End of the player's turn. Move onto the next player's turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndTurnButton_Click(object sender, EventArgs e)
        {
            //move on to next player's turn
            dialogBox.Text = "";
            m_myController.SetPropertyName("");
            PlayGame();
        }

        /// <summary>
        /// If the human player has any get-out-of-jail-free cards
        /// on hand, show them on-screen
        /// </summary>
        private void ShowJailCards()
        {
            //Show jail cards in hand
            if (m_myController.GetCurrentPlayerNumJailCards() != 0)
            {
                HandButton.Visible = true;
            }
            if (m_myController.GetCurrentPlayerNumJailCards() == 0)
            {
                HandButton.Visible = false;
            }
        }

        /// <summary>
        /// If player have enough funds to pay for something
        /// </summary>
        /// <param name="funds"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        private bool MoneySufficient(int funds, int cost)
        {
            if (funds > cost || funds == cost)
            {
                return true;
            }
            return false; 
        }

        /// <summary>
        /// If the player can't pay an outstanding amount/fee,
        /// the player must sell some assets to cover the costs
        /// or declare bankruptcy if the costs can't be covered 
        /// </summary>
        private void InsufficientFunds()
        {
            if (m_myController.HumanTurn())
            {
                dialogBox.Text += "You owe $" + Convert.ToString(m_myController.GetOutstandingPayment()) +
                    ". Sell your assets or get mortgages to cover the costs. " +
                    "If you have sold all your assets and/or mortgaged all your " +
                      "properties, you must declare bankruptcy." + Environment.NewLine;
                BankruptButton.Visible = true;
                PayButton.Visible = true;
                ShowJailCards();
            }
            else { ComputerInsufficientFunds(); }           
        }

        /// <summary>
        /// If the player can't pay an outstanding amount/fee,
        /// the player must sell some assets to cover the costs
        /// or declare bankruptcy if the costs can't be covered.
        /// This is the computer player's version of InsufficientFunds()
        /// in which the computer must decide what to do
        /// </summary>
        private void ComputerInsufficientFunds()
        {
            int outstandingPayment = m_myController.GetOutstandingPayment();
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " owes $" + Convert.ToString(outstandingPayment) + ". " +
                m_myController.GetCurrentPlayerNameAndType() +
                   " must sell their assets or get mortgages to cover the costs " + 
                   "or else declare bankruptcy if all assets have been sold " + "" +
                   "and properties have been mortgaged." + Environment.NewLine;

            dialogBox.Text += "Player has " + m_myController.GetCurrentPlayerNumJailCards() + " cards." + 
                Environment.NewLine;

            //Sell hotel(s)
            if (!m_myController.CurrentPlayerEnoughFunds(outstandingPayment) &&
                m_myController.GetCurrentPlayerTotalHotels() > 0)
            {
                CompInsufficientFundsSellHotels();
            }
            //Sell houses
            if (!m_myController.CurrentPlayerEnoughFunds(outstandingPayment) &&
                m_myController.GetCurrentPlayerTotalHouses() > 0)
            {
                CompInsufficientFundsSellHouses();
            }
            //Mortgage properties (if they haven't been mortgaged)
            if (!m_myController.CurrentPlayerEnoughFunds(outstandingPayment) &&
                m_myController.CurrentPlayerMortgagesAvailable())
            {
                CompInsufficientFundsMortgageProperties();
            }
            //Sell extra jail cards
            if (m_myController.GetCurrentPlayerNumJailCards() > 0 && 
                !m_myController.CurrentPlayerEnoughFunds(outstandingPayment))
            {
                CompInsufficientFundsSellJailCards();
            }
            //Sell properties after jail cards, unless no jail cards
            if (!m_myController.CurrentPlayerEnoughFunds(outstandingPayment) &&
                m_myController.GetCurrentPlayerTotalProperties() > 0 
                && m_myController.GetCurrentPlayerNumJailCards() == 0)
            {
                CompInsufficientFundsSellProperties();
            }

            if (m_myController.GetCurrentPlayerNumJailCards() == 0 &&
                m_myController.GetCurrentPlayerTotalProperties() == 0)
            {
                PayOrBankrupt();
            }
        }

        /// <summary>
        /// After the player has sold assets, the player will either
        /// pay off the fee or declare bankruptcy
        /// </summary>
        private void PayOrBankrupt()
        {
            //Pay the fee
            if (m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment()))
            {
                PayOutstandingPayment();
            }
            else
            {
                //Declare bankruptcy if still can't pay the fee
                PlayerBankrupt();
            }
        }

        /// <summary>
        /// Computer player sells hotels in a loop
        /// to raise funds to pay fees
        /// </summary>
        private void CompInsufficientFundsSellHotels()
        {
            int [] ownedHotels = m_myController.GetCurrentPlayerHotels().ToArray();
            for (int i = 0; i < ownedHotels.Count(); i++)
            {
                m_myController.SetPropertyName(m_gameBoard.GetSquareMessage(ownedHotels[i]));
                SellAHotel();
                if (m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment())) break;
            }
        }

        /// <summary>
        /// Computer player sells houses in a loop
        /// to raise funds to pay fees
        /// </summary>
        private void CompInsufficientFundsSellHouses()
        {
            int [] ownedHouses = m_myController.GetCurrentPlayerHouses().ToArray();
            for (int i = 0; i < ownedHouses.Count(); i++)
            {
                m_myController.SetPropertyName(m_gameBoard.GetSquareMessage(ownedHouses[i]));
                SellAHouse();
                if (m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment())) break;
            }
        }

        /// <summary>
        /// Computer player mortgages properties in a loop
        /// to raise funds to pay fees
        /// </summary>
        private void CompInsufficientFundsMortgageProperties()
        {
            int [] unmortgagedProperties = m_myController.GetCurrentPlayerUnmortgagedProperties().ToArray();
            for (int i = 0; i < unmortgagedProperties.Count(); i++)
            {
                m_myController.SetPropertyName(m_gameBoard.GetSquareMessage(unmortgagedProperties[i]));
                MortgageProperty();
                if (m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment())) break;
            }
        }

        /// <summary>
        /// Computer player sells jail cards to raise funds
        /// to pay off fee.
        /// Waits for auction to finish before continuing on to next
        /// card.
        /// </summary>
        private async void CompInsufficientFundsSellJailCards()
        {
            //try to sell jail cards to other players via auction
            if (m_myController.GetCurrentPlayerNumJailCards() > 0 &&
                !m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment()))
            {
                for (int i = 0; i < m_myController.GetCurrentPlayerNumJailCards(); i++)
                {
                    m_myController.SetSellCardBool(true);
                    m_myController.SetCompSellJailCard(true);
                    auctionFinished = new TaskCompletionSource<bool>();
                    Auction();
                    await auctionFinished.Task;
                }
                m_myController.SetSellCardBool(false);
                m_myController.SetCompSellJailCard(false);           
            }

            //sell properties after jail cards
            if (!m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment()) &&
              m_myController.GetCurrentPlayerTotalProperties() > 0)
            {
                CompInsufficientFundsSellProperties();
            }
        }

        /// <summary>
        /// Computer player sells properties to raise funds
        /// to pay off fee.
        /// Waits for auction to finish before continuing on to next
        /// property.
        /// </summary>
        private async void CompInsufficientFundsSellProperties()
        {
            //If player owns all properties in a color set, prioritize selling other properties first
            int[] ownedProperties = m_myController.GetCurrentPlayerPropertiesList().ToArray();
            if (!m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment()))
            {
                for (int i = 0; i < ownedProperties.Length; i++)
                {
                    //Not all colors held, haven't tried to sell the property already
                    if (!m_gameBoard.AllColorsHeld(ownedProperties[0]))
                    {
                        m_myController.SetBuyPropertyBool(true);
                        m_myController.SetAuctionPropertySquare(ownedProperties[i]);
                        m_myController.SetPlayerSale(true);
                        m_myController.SetCompPropertySale(true);
                        auctionFinished = new TaskCompletionSource<bool>();
                        Auction();
                        await auctionFinished.Task;
                    }
                }
            }

            //If still not enough funds after trying to sell all properties owned not in a color set,
            //try to sell remaining properties in a color set
            if (!m_myController.CurrentPlayerEnoughFunds(m_myController.GetOutstandingPayment()))
            {
                //For the remainder properties, sell the highest value properties first to raise the most funds
                //Having more properties left on the board equates to more chances to collect rent and fewer chances
                //to pay rent.
                int[] orderedProperties = ownedProperties.OrderByDescending(x => x).ToList().ToArray();
                for (int i = 0; i < orderedProperties.Length; i++)
                {
                    if (m_gameBoard.AllColorsHeld(orderedProperties[i]))
                    {
                        m_myController.SetBuyPropertyBool(true);
                        m_myController.SetAuctionPropertySquare(orderedProperties[i]);
                        m_myController.SetPlayerSale(true);
                        m_myController.SetCompPropertySale(true);
                        auctionFinished = new TaskCompletionSource<bool>();
                        Auction();
                        await auctionFinished.Task;
                    }

                }
            }

            m_myController.SetBuyPropertyBool(false);
            m_myController.SetPlayerSale(false);
            m_myController.SetCompPropertySale(false);
            PayOrBankrupt();
        }

        /// <summary>
        /// What the player must do if drawing a community chest/chance card
        /// Move squares, collect money, pay fees, get jail cards.
        /// Put card at the bottom of deck at the end and end the player's turn
        /// by showing end turn button
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_currentPlayerSquare"></param>
        private void ChestChance(string a_whichDeck, int a_currentPlayerSquare)
        {
            int formerSquare = a_currentPlayerSquare;
            if (m_gameBoard.ChestChanceMovement(a_whichDeck) == true)
            {
                // move squares
                ChestChanceMoveSquares(a_whichDeck, a_currentPlayerSquare);
            }
            if (m_gameBoard.ChestChanceCollectMoney(a_whichDeck) == true)
            {
                int amountCollected = m_gameBoard.ChestChanceGetMoney(
                    a_whichDeck, m_myController.GetNumPlayers());
                m_myController.AddCurrentPlayerFunds(amountCollected);
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
                dialogBox.Text += "Player received $" + Convert.ToString(amountCollected) + "." + Environment.NewLine;
            }
            if (m_gameBoard.ChestChancePayMoney(a_whichDeck) == true)
            {
                ChestChancePayMoney(a_whichDeck, a_currentPlayerSquare);
            }
            if (m_gameBoard.ChestChanceJailCard(a_whichDeck) == true)
            {
                m_myController.AddCurrentPlayerJailCards();
                if (m_myController.HumanTurn()) HandButton.Visible = true;
            }

            //Do not collect $200 if landing in jail

            //Call options menu if the player has moved
            if (m_gameBoard.ChestChanceMovement(a_whichDeck) == true)
            {
                ChestChancePlayerMoved(a_whichDeck, a_currentPlayerSquare, formerSquare);
            }
            else
            {
                //else just put used card at bottom of deck (move the card index)
                m_gameBoard.SetCardIndex(a_whichDeck);
                if (m_myController.GetOutstandingPayment() == 0)
                {                  
                    if (m_myController.HumanTurn())
                    {
                        EndTurnButton.Visible = true;
                        ShowJailCards();
                    }
                    if (!m_myController.HumanTurn()) CompFinishedTurn();
                }
            }   
        }

        /// <summary>
        /// Display new location of player's piece on board
        ///  if moved because of drawing a chest/chance card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_currentPlayerSquare"></param>
        private void ChestChanceMoveSquares(string a_whichDeck, int a_currentPlayerSquare)
        {
            //turn off former square
            dialogBox.Text += "The former player square was " + Convert.ToString(a_currentPlayerSquare)
                + "." + Environment.NewLine;
            TogglePlayerSquare(a_currentPlayerSquare, m_myController.GetCurrentPlayerColor(), false);

            //set player's current square
            m_myController.SetCurrentPlayerSquare(m_gameBoard.ChestChanceMoveSquares(a_whichDeck, a_currentPlayerSquare));
            dialogBox.Text += "The current player square is " +
                Convert.ToString(m_myController.GetCurrentPlayerSquare()) + "." + Environment.NewLine;
            //turn on current square
            TogglePlayerSquare(m_myController.GetCurrentPlayerSquare(), m_myController.GetCurrentPlayerColor(), true);
        }

        /// <summary>
        /// Player pays fees triggered by drawing chest/chance cards.
        /// if the player can't pay, InsufficientFunds() is triggered
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_currentPlayerSquare"></param>
        private void ChestChancePayMoney(string a_whichDeck, int a_currentPlayerSquare)
        {
            int fees = m_gameBoard.ChestChancePayFees(a_whichDeck, m_myController.GetCurrentPlayerTotalHouses(),
                   m_myController.GetCurrentPlayerTotalHotels(), m_myController.GetNumPlayers());
            if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), fees))
            {
                m_myController.CurrentPlayerPaysMoney(fees);
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() +
                    " paid $" + Convert.ToString(fees) + "." + Environment.NewLine;
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
            }
            else
            {
                if (m_myController.HumanTurn()) dialogBox.Text += "You don't";
                if (!m_myController.HumanTurn()) dialogBox.Text += 
                        m_myController.GetCurrentPlayerNameAndType() + " doesn't";

                dialogBox.Text += " have sufficient funds to pay the fees." + Environment.NewLine;
                m_myController.SetOutstandingPayment(fees);
                InsufficientFunds();
            }
        }

        /// <summary>
        /// If the player moved squares from drawing a card,
        /// the player must respond to events triggered by that. 
        /// The player will either go to jail, pay a special rent,
        /// and finally open optionsMenu to respond to landing on a new square.
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_currentPlayerSquare"></param>
        /// <param name="a_formerSquare"></param>
        private void ChestChancePlayerMoved(string a_whichDeck, int a_currentPlayerSquare, int a_formerSquare)
        {
            if (m_gameBoard.ChestChanceJail(a_whichDeck))
            {
                //Go to jail
                TogglePlayerSquare(30, m_myController.GetCurrentPlayerColor(), false);
                TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), true);
                m_myController.SetCurrentPlayerSquare(40);
                m_myController.SendCurrentPlayerToJail();
            }
            if (m_gameBoard.ChestChancePassGo(a_whichDeck, a_formerSquare))
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has passed Go." + Environment.NewLine;
                if (m_myController.HumanTurn()) dialogBox.Text += "You collect";
                if (m_myController.HumanTurn()) dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() 
                        + " collects";

                dialogBox.Text += " $200." + Environment.NewLine;
                m_myController.AddCurrentPlayerFunds(200);
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
            }
            if (m_gameBoard.SpecialRentPay(a_whichDeck))
            {
                m_myController.SetSpecialRentPay(true);
            }
            m_gameBoard.SetCardIndex(a_whichDeck); //move used card to bottom of deck
            if (m_myController.HumanTurn()) OptionsMenu(m_myController.GetCurrentPlayerSquare());

            if (!m_myController.HumanTurn()) ComputerTurn(m_myController.GetCurrentPlayerSquare());
        }

        /// <summary>
        /// When a player lands in jail:
        /// 1. either the player is just visiting
        /// 2. Must pay fee or roll the dice
        /// 3. Or has already rolled for 3 turns and 
        /// must pay fee this turn 
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void JailOptions(int a_currentPlayerSquare)
        {
            if (a_currentPlayerSquare == 10)
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType()  + 
                    " has landed on the jail square." + Environment.NewLine;
                dialogBox.Text += "But is just visiting." + Environment.NewLine;                
                if (m_myController.HumanTurn())
                {
                    EndTurnButton.Visible = true;
                    ShowJailCards();
                }
                if (!m_myController.HumanTurn()) CompFinishedTurn();
            }
            else if (a_currentPlayerSquare == 30)
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                    " has landed on the 'Go To Jail' square." + Environment.NewLine;
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                    " is now in jail." + Environment.NewLine;
                TogglePlayerSquare(30, m_myController.GetCurrentPlayerColor(), false);
                TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), true);
                m_myController.SetCurrentPlayerSquare(40);
                m_myController.SendCurrentPlayerToJail();

                JailPayOrRoll();
            }
            else if (a_currentPlayerSquare == 40)
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                    " is in jail." + Environment.NewLine;
                if (m_myController.CurrentPlayerJailedThreeTurns())
                {
                    FourthJailTurn();
                }
                else
                {
                    JailPayOrRoll();
                }
            }
        }

        /// <summary>
        /// the human player is prompted to decide whether to pay money
        /// to get out of jail or roll dice to get out of jail.
        /// Can also use jail card if available
        /// </summary>
        private void JailPayOrRoll()
        {
            if (m_myController.HumanTurn())
            {
                //Player must respond to this prompt before doing anything else
                m_myController.SetPlayerJailRoll(true);
                dialogBox.Text += "To get out of jail, pay $50 or roll the dice." + Environment.NewLine +
                          "If you roll doubles, you get out of jail. " + Environment.NewLine +
                         "You may roll the dice for 3 turns." + Environment.NewLine;
                dialogBox.Text += "Would you like to pay $50 to get out of jail? Choose 'No' to roll the dice."
                    + Environment.NewLine;
                YesButton.Visible = true;
                NoButton.Visible = true;
                if (m_myController.GetCurrentPlayerNumJailCards() != 0)
                {
                    HandButton.Visible = false;
                    UseCardButton.Visible = true;
                    dialogBox.Text += "You have " + Convert.ToString(m_myController.GetCurrentPlayerNumJailCards()) +
                        " Get Out of Jail Free Card(s) on hand." + Environment.NewLine;
                    dialogBox.Text += "You can also use a get out of jail free card to get out jail." +
                        " Press 'use card' to use your card." + Environment.NewLine;
                }
            }
            else { ComputerPlayerJail(); }
        }

        /// <summary>
        /// Computer player is prompted to decide whether to pay money
        /// to get out of jail or roll dice to get out of jail
        /// Can also use jail card if available
        /// </summary>
        private void ComputerPlayerJail ()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() +
                " must pay $50 or roll the dice to get out of jail." + Environment.NewLine +
                          "Rolling doubles will get " + m_myController.GetCurrentPlayerNameAndType() +
                          " out of jail. " + Environment.NewLine + m_myController.GetCurrentPlayerNameAndType()
                         + " may roll the dice for 3 turns." + Environment.NewLine;

            //If jail cards available, always use to avoid penalty
            if (m_myController.GetCurrentPlayerNumJailCards() != 0)
            {
                CompPlayerUsesJailCard();
            }
            if (m_myController.GetCurrentPlayerNumJailCards() == 0)
            {
                //Get the computer's strategy
                if (m_compStrat.RollOutOfJail(m_myController.GetCurrentPlayerFunds(), m_gameBoard.GetNumHouses(), 
                    m_gameBoard.NumOccupiedProperties()))
                {
                    //Roll the dice
                    PlayerRollsDiceInJail();
                }
                else
                {
                    //Pay to get out of jail
                    PlayerPaysOutOfJail();
                }
            }
         
        }

        /// <summary>
        /// If the computer player uses a card to get out of jail
        /// Set player out of jail
        /// Use the card
        /// </summary>
        private void CompPlayerUsesJailCard ()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has " +
                   Convert.ToString(m_myController.GetCurrentPlayerNumJailCards()) +
                   " get out of jail free card(s) on hand." + Environment.NewLine;
            dialogBox.Text += "Get out of jail free cards may also be used to get out jail."
                + Environment.NewLine + m_myController.GetCurrentPlayerNameAndType()
                + " chose to use a get out of jail free card." + Environment.NewLine;

            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " is now out of jail." + Environment.NewLine;
            TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), false);
            TogglePlayerSquare(10, m_myController.GetCurrentPlayerColor(), true);

            m_myController.CurrentPlayerLeavesJail();
            m_myController.SetCurrentPlayerSquare(10);
            m_myController.CurrentPlayerUsesJailCard();

            if (!m_myController.HumanTurn()) CompFinishedTurn();
        }

        /// <summary>
        /// Number of turns in incremented.
        /// If the player decides to roll dice in jail
        /// If not a doubles roll, the player will have to roll
        /// again next turn. 
        /// If success, player is out of jail.
        /// 
        /// </summary>
        private void PlayerRollsDiceInJail ()
        {
            //Player doesn't want to pay money to get out of jail
            m_myController.CurrentPlayerAddJailTurns();
            //Roll dice:
            int[] diceArray = m_myController.RollDice();
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " rolled a " + diceArray[0] +
                " and a " + diceArray[1] + "." + Environment.NewLine;
            if (diceArray[0] == diceArray[1])
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " rolled doubles, "
                    + m_myController.GetCurrentPlayerNameAndType() + " is now out of jail." + Environment.NewLine;
                m_myController.CurrentPlayerLeavesJail();
                //Turn off current square
                TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), false);
                TogglePlayerSquare(10, m_myController.GetCurrentPlayerColor(), true);
                //Set player's current position to just visiting jail square
                m_myController.SetCurrentPlayerSquare(10);
            }
            if (diceArray[0] != diceArray[1])
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() +
                    " did not roll doubles." + Environment.NewLine;
                if (m_myController.HumanTurn()) dialogBox.Text += "You are";
                else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " is";

                dialogBox.Text += " allowed to roll for doubles for three turns." + Environment.NewLine +
                   m_myController.GetCurrentPlayerNameAndType() + " is on turn " +
                   Convert.ToString(m_myController.GetCurrentPlayerNumJailTurns()) + "." + Environment.NewLine;           
            }

            m_myController.SetPlayerJailRoll(false);
            UseCardButton.Visible = false;
            if (m_myController.HumanTurn())
            {
                EndTurnButton.Visible = true;
                ShowJailCards();
            }
            if (!m_myController.HumanTurn()) CompFinishedTurn();
        }

        /// <summary>
        /// Player chose to pay $50 to get out of jail
        /// Move player out of jail
        /// Subtract fees
        /// </summary>
        private void PlayerPaysOutOfJail()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has paid $50. "
                       + m_myController.GetCurrentPlayerNameAndType() + 
                       " is now out of jail." + Environment.NewLine;

            TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), false);
            TogglePlayerSquare(10, m_myController.GetCurrentPlayerColor(), true);

            m_myController.CurrentPlayerLeavesJail();
            m_myController.CurrentPlayerPaysMoney(50);
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            //Set player's current position to jail square
            m_myController.SetCurrentPlayerSquare(10);

            if (m_myController.HumanTurn())
            {
                NoButton.Visible = false;
                YesButton.Visible = false;
                UseCardButton.Visible = false;
                m_myController.SetPlayerJailRoll(false);
                EndTurnButton.Visible = true;
                ShowJailCards();
            }

            if (!m_myController.HumanTurn()) CompFinishedTurn();
        }

        /// <summary>
        /// If the player has already been in jail for three turns previously
        /// Now the player must pay the fee or go to InsufficientFunds()
        /// if not enough funds are available
        /// </summary>
        private void FourthJailTurn()
        {
            dialogBox.Text += Environment.NewLine + m_myController.GetCurrentPlayerNameAndType() 
                + " has unsuccessfully tried to roll doubles for 3 turns." +
                Environment.NewLine + "Now a fee of $50 must be paid to get out of jail." + Environment.NewLine;

            m_myController.SetPlayerJailRoll(false);
            TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), false);
            TogglePlayerSquare(10, m_myController.GetCurrentPlayerColor(), true);
            m_myController.CurrentPlayerLeavesJail();
            //Set player's current position to visiting jail square
            m_myController.SetCurrentPlayerSquare(10);
            if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), 50))
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " pays $50. " 
                    + m_myController.GetCurrentPlayerNameAndType() + " is now out of jail.";
             
                m_myController.CurrentPlayerPaysMoney(50);
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
           
                if (m_myController.HumanTurn()) ShowJailCards();
                EndTurnButton.Visible = true;
            }
            else
            {
                if (m_myController.HumanTurn()) dialogBox.Text += "You don't";
                if (!m_myController.HumanTurn()) dialogBox.Text += 
                        m_myController.GetCurrentPlayerNameAndType() + " doesn't";

                dialogBox.Text += " have sufficient funds to get out of jail.";
                m_myController.SetOutstandingPayment(50);

                InsufficientFunds();
            }        
        }

        /// <summary>
        /// Number of get of jail free cards the player has on hand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandButton_Click(object sender, EventArgs e)
        {
            if (m_myController.GetCurrentPlayerNumJailCards() != 0)
            {
                dialogBox.Text += "You have " +
                    Convert.ToString(m_myController.GetCurrentPlayerNumJailCards()) +
                    " get out of jail free card(s) on hand." + Environment.NewLine;
                SellCardButton.Visible = true;
                ExitButton.Visible = true;
                EndTurnButton.Visible = false;
            }
            if (m_myController.GetCurrentPlayerNumJailCards() == 0)
            {
                dialogBox.Text += "You don't have any jail cards on hand." + Environment.NewLine;
            }
        }

        /// <summary>
        /// Human uses get out of jail free card
        /// To get out of jail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseCardButton_Click(object sender, EventArgs e)
        {
            YesButton.Visible = false;
            NoButton.Visible = false;
            UseCardButton.Visible = false;
            EndTurnButton.Visible = true;

            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " is now out of jail." + Environment.NewLine;
            TogglePlayerSquare(40, m_myController.GetCurrentPlayerColor(), false);
            TogglePlayerSquare(10, m_myController.GetCurrentPlayerColor(), true);

            m_myController.SetPlayerJailRoll(false);
            m_myController.CurrentPlayerLeavesJail();
            m_myController.SetCurrentPlayerSquare(10);
            m_myController.CurrentPlayerUsesJailCard();

            if (m_myController.HumanTurn()) ShowJailCards();
        }

        /// <summary>
        /// If the player landed on a property square,
        /// then either pay rent or have the option to buy
        /// the property. 
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void PropertySquare(int a_currentPlayerSquare)
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has landed on a " 
                + m_gameBoard.GetSquareType(a_currentPlayerSquare) + " square, " + 
                m_gameBoard.GetSquareMessage(a_currentPlayerSquare) + "." + Environment.NewLine;
            //check if property occupied, if it is and unmortgaged, pay rent
            if (m_gameBoard.IsOccupied(a_currentPlayerSquare) == true)
            {
                PropertySquarePayRent(a_currentPlayerSquare);
            }
            if (!m_gameBoard.IsOccupied(a_currentPlayerSquare) == true)
            {
                //else offer option to buy property
                BuyProperty(a_currentPlayerSquare);
            }
        }

        /// <summary>
        /// Pay rent if owned by other player and not mortgaged
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void PropertySquarePayRent(int a_currentPlayerSquare)
        {
            if (m_gameBoard.WhoOwns(a_currentPlayerSquare) != m_myController.GetCurrentPlayerName())
            {
                string propertyOwner = m_gameBoard.WhoOwns(a_currentPlayerSquare);
                if (!m_myController.IsPropertyMortgaged(propertyOwner, a_currentPlayerSquare))
                {
                    PayRent(a_currentPlayerSquare);
                }
                if (m_myController.IsPropertyMortgaged(propertyOwner, a_currentPlayerSquare))
                {
                    dialogBox.Text += propertyOwner + "(" + m_myController.GetPlayerType(propertyOwner) +
                        ") owns this property." + Environment.NewLine;
                    dialogBox.Text += "This property is mortgaged, ";
                    if (m_myController.HumanTurn()) dialogBox.Text += "you don't";
                    else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " doesn't";
                    dialogBox.Text += " have to pay rent." + Environment.NewLine;
                    EndTurnButton.Visible = true;
                    if (m_myController.HumanTurn()) ShowJailCards();
                }
            }
            if (m_gameBoard.WhoOwns(a_currentPlayerSquare) == m_myController.GetCurrentPlayerName())
            {
                if (m_myController.HumanTurn()) dialogBox.Text += "You own";
                else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " owns";
                dialogBox.Text += " this property." + Environment.NewLine;
                EndTurnButton.Visible = true;
                if (m_myController.HumanTurn()) ShowJailCards();
            }
        }

        /// <summary>
        /// The player has the option to buy property the player
        /// landed on.
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void BuyProperty(int a_currentPlayerSquare)
        {
            m_myController.SetAuctionPropertySquare(a_currentPlayerSquare);
            m_myController.SetBuyPropertyBool(true);
            if (m_myController.HumanTurn())
            { dialogBox.Text += "Would you like to buy this property? ";
                dialogBox.Text += "It costs $" +
             Convert.ToString(m_gameBoard.GetPropertyCost(a_currentPlayerSquare)) + 
             "." + Environment.NewLine;
                YesButton.Visible = true;
                NoButton.Visible = true;
            }
            if (!m_myController.HumanTurn())
            {
                CompPlayerBuyProperty(a_currentPlayerSquare);
            }
        }

        /// <summary>
        /// The computer player has the option to buy property the player
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void CompPlayerBuyProperty(int a_currentPlayerSquare)
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " has the option to buy this property." 
                + Environment.NewLine;
            //If the computer decides whether to buy the property
            if (m_compStrat.BuyProperty(m_gameBoard.GetPropertyCost(a_currentPlayerSquare), 
                m_myController.GetCurrentPlayerFunds()))
            {
                //Buy the property
                BuyProperty2();
            }
            else
            {
                CompPropSquareAuction();
            }
        }

        /// <summary>
        /// If the computer player chose not to buy the property
        /// </summary>
        public void CompPropSquareAuction()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() +
               " did not buy this property." + Environment.NewLine;
            //Else go to auction
            Auction();
        }

        /// <summary>
        /// If the player landed on another player's square,
        /// the player must pay rent
        /// </summary>
        /// <param name="a_currentPlayerSquare"></param>
        private void PayRent(int a_currentPlayerSquare)
        {
            string propertyOwner = m_gameBoard.WhoOwns(a_currentPlayerSquare);
            int numHouses = 0;
            if (m_myController.PlayerPropertyUnimproved(propertyOwner, a_currentPlayerSquare) == false)
            {
                //if property owner owns houses on this property, get the number of houses
                numHouses = m_myController.GetNumHouses(propertyOwner, a_currentPlayerSquare);
            }
            int rentCost = m_gameBoard.GetRentCost(a_currentPlayerSquare, numHouses, propertyOwner);
            if (m_myController.GetSpecialRentPay())
            {
                //special cases from drawing chance cards
                if (m_gameBoard.NoDiceRoll())
                {
                    rentCost = m_gameBoard.GetSpecialRent(rentCost);
                }
                else
                {
                    dialogBox.Text += "The player rolls the dice." + Environment.NewLine;
                    int[] diceResults = m_myController.RollDice();
                    dialogBox.Text += "The player rolled a " + Convert.ToString(diceResults[0]) +
                        " and a " + Convert.ToString(diceResults[1]) + "." + Environment.NewLine;
                    dialogBox.Text += "The rent is 10 times this amount." + Environment.NewLine;
                    int diceSum = diceResults[0] + diceResults[1];
                    rentCost = (10 * diceSum);
                }
                m_myController.SetSpecialRentPay(false); //turn it off
            }
            dialogBox.Text += propertyOwner + "(" + m_myController.GetPlayerType(propertyOwner) +
                ") owns this property. ";
            if (m_myController.HumanTurn()) dialogBox.Text += "You";
            else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType();
            dialogBox.Text += " must pay rent." + Environment.NewLine;

            if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), rentCost))
            {
                //subtract money from player's funds
                m_myController.CurrentPlayerPaysMoney(rentCost);
                //update funds on screen
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
                //add money to property owner's funds
                m_myController.AddPlayerFunds(propertyOwner, rentCost);
                if (m_myController.HumanTurn()) dialogBox.Text += "You";
                else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType();
                dialogBox.Text += " paid " + propertyOwner + "(" + m_myController.GetPlayerType(propertyOwner) + ")" + 
                    " $" + rentCost + "." + Environment.NewLine;
                EndTurnButton.Visible = true;
                if (m_myController.HumanTurn()) ShowJailCards();
            }
            else
            {
                dialogBox.Text += "Uh-oh! ";
                if (m_myController.HumanTurn()) dialogBox.Text += "You";
                if (!m_myController.HumanTurn()) dialogBox.Text += m_myController.GetCurrentPlayerNameAndType();
                dialogBox.Text += " don't have sufficient funds to pay rent." + Environment.NewLine;
                m_myController.SetOutstandingPayment(rentCost);
                InsufficientFunds();
            }
        }

        /// <summary>
        /// YesButton used for multiple purposes:
        /// 1. If the player agrees "yes" to buying a property
        /// the player landed on
        /// 2. A player is selling owned property to another player,
        /// if the buying player pressed the "yes" button to buying it
        /// 3. A player is selling a jail card to another player,
        /// if the buying player pressed the "yes" button to buying it
        /// 4. If the player pressed "yes" to pay a fee to get out of jail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesButton_Click(object sender, EventArgs e)
        {
            if (m_myController.GetBuyPropertyBool() == true)
            {
                //Current player wants to buy the property landed on
                BuyProperty2();
            }
            else if (m_myController.GetSelectPlayerBool())
            {
                //Buyer confirmed buying the property
                NoButton.Visible = false;
                YesButton.Visible = false;
                m_myController.SetSellPropertyBool(true);
                //finish player's attempt to sell property to another player
                CompletePlayerSale();
            }
            else if (m_myController.GetSellCardBool())
            {
                //sell get out of jail card
                YesButton.Visible = false;
                NoButton.Visible = false;
                m_myController.SetJailCardBought(true);
                SoldJailCard();
            }
            else 
            {
                if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), 50))
                {
                    //Player paid money to get out of jail
                    PlayerPaysOutOfJail();
                }
                else
                {
                    dialogBox.Text += "You don't have sufficient funds to get out of jail." + Environment.NewLine;
                    //Player must roll to get out of jail
                }
            }
        }

        /// <summary>
        /// Multiple uses for NoButton:
        /// 1. In a player to player property sale, the buying player
        /// chose "no" and didn't buy the property
        /// 2. The buying player chose "no" and didn't buy the other player's
        /// jail card
        /// 3.The player chose "no" and decided to roll dice to get out of jail
        /// 4. The player chose "no" and didn't want to buy the property landed on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoButton_Click(object sender, EventArgs e)
        {
            NoButton.Visible = false;
            YesButton.Visible = false;

            if (m_myController.GetSelectPlayerBool())
            {
                NoButton.Visible = false;
                YesButton.Visible = false;
                //finish player's attempt to sell property to another player
                CompletePlayerSale();
            }
            else if (m_myController.GetSellCardBool())
            {
                SoldJailCard();
            }
            else if (m_myController.GetCurrentPlayerJailStatus())
            {
                PlayerRollsDiceInJail();
            }
            else
            {
                //Current player doesn't want to buy property, go to auction
                Auction();
            }

        }

        /// <summary>
        /// Player chose to buy the property landed on
        /// If money adequate, add property to player's list of owned properties
        /// If not adequate, go to auction
        /// </summary>
        private void BuyProperty2()
        {
            int propertyCost = m_gameBoard.GetPropertyCost(m_myController.GetCurrentPlayerSquare());
            //check if money is sufficient
            if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), propertyCost) == true)
            {
                m_myController.CurrentPlayerBuysProperty(propertyCost);
                //add property to list of occupied properties in board class
                m_gameBoard.AddProperty(m_myController.GetCurrentPlayerSquare(), m_myController.GetCurrentPlayerName());

                UpdateFunds(m_myController.GetCurrentPlayerFunds());

                if (m_myController.HumanTurn()) dialogBox.Text += "You now own ";
                if (!m_myController.HumanTurn())
                { 
                    dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " purchased ";
                }
                dialogBox.Text += m_gameBoard.GetSquareMessage(m_myController.GetCurrentPlayerSquare()) + 
                    "." + Environment.NewLine;
                //Add new property to player's properties list
                PropertiesBox.Items.Add(m_gameBoard.GetSquareMessage(m_myController.GetCurrentPlayerSquare()));

                m_myController.SetBuyPropertyBool(false);
                m_myController.SetAuctionPropertySquare(0);
                if (m_myController.HumanTurn())
                {
                    NoButton.Visible = false;
                    YesButton.Visible = false;
                    ShowJailCards();
                }
                EndTurnButton.Visible = true;               
            }
            else
            {
                if (m_myController.HumanTurn()) dialogBox.Text += "You don't";
                else dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " doesn't";
                dialogBox.Text += " have sufficient funds to buy that property." + Environment.NewLine;
                if (m_myController.HumanTurn())
                {
                    NoButton.Visible = false;
                    YesButton.Visible = false;
                }
                Auction();
            }
        }

        /// <summary>
        /// Begin an auction for a property a player landed on,
        /// a jail card, or a player to player sale. 
        /// Set the first bid and tell users if the property is mortgaged. 
        /// </summary>
        private void Auction()
        {
            dialogBox.Text += Environment.NewLine + "The other players have the opportunity to buy ";
            if (m_myController.GetSellCardBool())
            {
                dialogBox.Text += "a jail card from " + m_myController.GetCurrentPlayerNameAndType() +
                    "." + Environment.NewLine;
            }
            else
            {
                //else a property
                dialogBox.Text += m_gameBoard.GetSquareMessage(m_myController.GetAuctionPropertySquare()) +
                    "." + Environment.NewLine;
            }
            dialogBox.Text += "Bidding begins at $10. Bids may be increased by as little as $1." + 
                Environment.NewLine;
            if (m_myController.GetPlayerSale()) //if player to player sale
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " is selling this property."
                    + Environment.NewLine;
                if (m_myController.IsCurrentPlayerPropertyMortgaged(
                    m_gameBoard.GetSquareNum(m_myController.GetPropertyName())))
                {
                    dialogBox.Text += "This property is mortgaged." + Environment.NewLine;
                }
            }
            m_myController.SetAuctionPlayer(m_myController.GetCurrentPlayerName());           
            EndTurnButton.Visible = false;
            
            AuctionSetup();
            AuctionMenu();
        }

        /// <summary>
        /// UI changes:
        /// Show whose turn it is during an auction.
        /// Update their funds and properties.
        /// </summary>
        private void AuctionSetup()
        {
            //Show whoever's turn it is during the auction and their matching funds/properties
            UpdatePlayersLabel(m_myController.GetAuctionPlayer());

            UpdateFunds(m_myController.GetPlayerFunds(m_myController.GetAuctionPlayer()));

            RefreshPropertiesBox(m_myController.GetAuctionPlayer());
        }

        /// <summary>
        /// The menu for whoever's turn it is
        /// If player isn't bankrupt, the player has
        /// the opportunity to bid.
        /// </summary>
        private void AuctionMenu()
        {    
            if (!m_myController.IsPlayerBankrupt(m_myController.GetAuctionPlayer()))
            {
                dialogBox.Text += "It's " + m_myController.GetAuctionPlayer() + "(" 
                    + m_myController.GetPlayerType(m_myController.GetAuctionPlayer()) +
                   ")" + "'s turn." + Environment.NewLine;
                dialogBox.Text += "The current bid is $" + m_myController.GetBidValue() + 
                    ". Bids may be increased by as little as $1." + Environment.NewLine;
                int startingBid = (m_myController.GetBidValue() + 1);
                dialogBox.Text += "The starting bid is $" + Convert.ToString(startingBid) + Environment.NewLine;
                if (m_myController.GetPlayerType(m_myController.GetAuctionPlayer()) == "human")
                {
                    BidButton.Visible = true;
                    PassButton.Visible = true;
                    TurnOnPriceBox();
                }
                else
                {
                    CompPlayerBidOrPass();
                }                 
            }
            else //if bankrupt, continue
            {
                Auction2();
            }
        }

        /// <summary>
        /// The computer player decides whether to
        /// bid in an auction or not.
        /// </summary>
        private void CompPlayerBidOrPass ()
        {
           //Computer makes a decision
            if (CompBidDecision()) 
            {                           
                //Make a minimum bid
                PlayerBid(m_myController.GetBidValue() + 1);               
            }
            else
            { 
                //Otherwise pass 
                dialogBox.Text += m_myController.GetAuctionPlayer() + " passed." +
                    Environment.NewLine + Environment.NewLine;              
                PlayerPassed();
            }
        }

        /// <summary>
        /// If the computer is the player selling, the
        /// computer must ensure to not to sell to themselves.
        /// 
        /// The computer's decision will be different if
        /// buying a jail card or a property.
        /// </summary>
        /// <returns></returns>
        private bool CompBidDecision()
        {         
            //don't sell to self when selling property
            if (m_myController.GetPlayerSale() &&
                (m_myController.GetAuctionPlayer() == m_myController.GetCurrentPlayerName()))
            {
                return false;
            }
            else if (m_myController.GetCompSellJailCard() &&
                (m_myController.GetAuctionPlayer() == m_myController.GetCurrentPlayerName()))
            {
                //don't sell to self when selling jail card
                return false;
            }
            else
            {
                //If jail card being sold
                if (m_myController.GetSellCardBool())
                {
                    return m_compStrat.BuyJailCard(m_myController.GetBidValue(),
                        m_myController.GetCurrentPlayerFunds());
                }
                else
                {
                    //else decide whether or not to buy property 
                    return m_compStrat.BidProperty(m_myController.GetBidValue(),
                    m_gameBoard.GetPropertyCost(m_myController.GetAuctionPropertySquare()),
                    m_myController.GetPlayerFunds(m_myController.GetAuctionPlayer()),
                    m_gameBoard.GetColorGroup(m_myController.GetAuctionPropertySquare()));
                }
            }           
        }

        /// <summary>
        /// If the player passed during their turn, 
        /// turn on their auction pass
        /// </summary>
        private void PlayerPassed()
        {
            m_myController.TurnOnPlayerAuctionPass(m_myController.GetAuctionPlayer());
            Auction2();
        }

        /// <summary>
        /// If the player bid on the property in the auction,
        /// set the the bid value and turn off player's auction pass.
        /// </summary>
        /// <param name="a_PlayerBidValue"></param>
        private void PlayerBid(int a_PlayerBidValue)
        {
            dialogBox.Text += m_myController.GetAuctionPlayer() + "(" + 
                m_myController.GetPlayerType(m_myController.GetAuctionPlayer()) +
                 ") bid $" + Convert.ToString(a_PlayerBidValue) + "." + Environment.NewLine + Environment.NewLine;
            m_myController.SetBidValue(a_PlayerBidValue);
            //Player didn't pass this turn:    
            m_myController.TurnOffPlayerAuctionPass(m_myController.GetAuctionPlayer());
            m_myController.SetAuctionWinner(m_myController.GetAuctionPlayer());
            Auction2();
        }

        /// <summary>
        /// The human player must enter a valid number to bid with.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BidButton_Click(object sender, EventArgs e)
        {
            try
            {
                int playerInput = Convert.ToInt32(InputPriceBox.Text);
                int playerFunds = m_myController.GetPlayerFunds(m_myController.GetAuctionPlayer());
                if (playerInput > m_myController.GetBidValue() && playerInput <= playerFunds)
                {
                    PlayerBid(playerInput);
                }
                else if (playerInput < m_myController.GetBidValue() || playerInput > playerFunds)
                {
                    if (playerInput <= m_myController.GetBidValue())
                    {
                        dialogBox.Text += "The amount you input is not a high enough bid." + Environment.NewLine;
                    }
                    if (playerInput > playerFunds)
                    {
                        dialogBox.Text += "You don't have enough funds for that bid." + Environment.NewLine;
                    }
                }
            }
            catch (FormatException)
            {
                dialogBox.Text += "Invalid input." + Environment.NewLine;
            }
            catch (OverflowException)
            {
                dialogBox.Text += "Value invalid, system overflow." + Environment.NewLine;
            }
        }

        /// <summary>
        /// The human player passed on their turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassButton_Click(object sender, EventArgs e)
        {
            dialogBox.Text += m_myController.GetAuctionPlayer() + " passed." +
                 Environment.NewLine + Environment.NewLine;
            PlayerPassed();
        }

        /// <summary>
        /// Prices human player can input to bid
        /// This is turned on for the human player
        /// </summary>
        private void TurnOnPriceBox()
        {
            MoneyLabel.Visible = true;
            InputPriceBox.Visible = true;
            InputPriceBox.Text = "";
        }
        /// <summary>
        /// This function checks if the auction is over or not.
        /// It counts how many players have passed and if anyone bid.
        /// Then either the auction continues or the auction ends.
        /// </summary>
        private void Auction2()
        {
            BidButton.Visible = false; 
            PassButton.Visible = false;
            InputPriceBox.Visible = false;
            MoneyLabel.Visible = false;
            m_myController.SetAuctionPlayer(m_myController.WhoseTurn(m_myController.GetAuctionPlayer()));
            AuctionSetup();

            //check if all players have passed once, sell to player with last bid
            int numPlayers = m_myController.NotBankruptPlayers();
            int auctionWinCounter = 0;
            int passCounter = 0;
            //Get index of the last player
            int tempIndex = m_myController.PreviousPlayerPosition(m_myController.GetPlayerPosition(
                m_myController.GetAuctionPlayer()));

            for (int i = 0; i < numPlayers; i++)
            {
                //Get the index in the player array of the previous player before the current one
                tempIndex = m_myController.PreviousPlayerPosition(tempIndex);
                if (m_myController.GetPlayerAuctionPass(tempIndex)) //if the player passed on their turn
                {
                    //count how many players have passed
                    passCounter++;
                    if (i != (numPlayers - 1)) //not including the current player
                    {
                        //The number of players that need to have passed for there to be a winner in an auction
                        //This is always (# of players - 1)
                        auctionWinCounter++;
                    }
                }
            }

            //If all but one player passed and there was a bid, this auction has a winner
            if (passCounter == (numPlayers - 1) && m_myController.GetBidValue() > 10)
            {
                //Auction is over, property goes to highest bidder
                AuctionWinner();
            }
            else if (passCounter == (numPlayers))
            {
                dialogBox.Text += "No one bid in the auction." + Environment.NewLine;
    
                EndAuction();
            }
            else
            {
                //Continue with auction if not over yet
                AuctionMenu();
            }

        }

        /// <summary>
        /// If the auction had a winner, the winner is announced and gets 
        /// the purchased property/jail card.
        /// Assets and cash are added/subtracted for the buying player
        /// as well as for the selling player if applicable. 
        /// </summary>
        private void AuctionWinner()
        {
            string auctionWinnerName = m_myController.GetAuctionWinner();
            dialogBox.Text += Environment.NewLine + "The auction is over. "
                + auctionWinnerName + "(" + m_myController.GetPlayerType(auctionWinnerName) + ")";
            if(m_myController.GetSellCardBool())
            {
                //If sold a get out of jail free card
                AuctionWinnerCard();
            }
            else
            {
                //else sold a property
                dialogBox.Text += " has purchased the property";
                dialogBox.Text += " for $" + Convert.ToString(m_myController.GetBidValue()) +
              "." + Environment.NewLine;

                int auctionPropertySquare = m_myController.GetAuctionPropertySquare();
                //Subtract bid value from funds and add property to list of winner player's properites
                m_myController.PlayerWinsPropertyAuction(auctionWinnerName, m_myController.GetBidValue(),
                    auctionPropertySquare);
                //If current player sells owned property to another player 
                if (m_myController.GetPlayerSale())
                {
                    AuctionWinnerPlayerProperty();
                }
                else
                {
                    //If just a regular auction, add property to the assets of the buying player
                    m_myController.PlayerAddsAssets(auctionWinnerName, 
                        m_gameBoard.GetPropertyCost(auctionPropertySquare));
                }

                //update board properties
                m_gameBoard.AddProperty(auctionPropertySquare, auctionWinnerName);
            }
         
            EndAuction();
        }

        /// <summary>
        /// If the player purchased a jail card.
        /// </summary>
        private void AuctionWinnerCard()
        {
            dialogBox.Text += " has purchased the jail card for $" + Convert.ToString(m_myController.GetBidValue()) + 
                "." + Environment.NewLine;
            m_myController.PlayerWinsCardAuction(m_myController.GetAuctionWinner(), m_myController.GetBidValue());
        }

        /// <summary>
        /// If selling property from one player to another.
        /// If not a bankrupt selling player, the selling player
        /// gets what the buying player pays and buying payer
        /// gets what the sellig player sold
        /// </summary>
        private void AuctionWinnerPlayerProperty()
        {
            if (m_myController.GetBankruptcySaleBool())
            {
                AuctionBankruptProperty();
            }
            else
            {
                int auctionPropertySquare = m_myController.GetAuctionPropertySquare();
                //Add bid value to selling palyer
                m_myController.AddCurrentPlayerFunds(m_myController.GetBidValue());
                if (m_myController.IsCurrentPlayerPropertyMortgaged(auctionPropertySquare))
                {
                    //Add assets/mortgage to buying player, subtract mortgage/assets from selling player
                    m_myController.MortgagedPropertySale(m_myController.GetAuctionWinner(),
                        auctionPropertySquare, m_gameBoard.GetMortgageAmount(auctionPropertySquare));
                }
                else
                {
                    //not mortgaged
                    //Subtract assets from selling player
                    m_myController.CurrentPlayerSubstractsAssets(m_gameBoard.GetPropertyCost(auctionPropertySquare));
                    //add assets to winning player
                    m_myController.PlayerAddsAssets(m_myController.GetAuctionWinner(), 
                        m_gameBoard.GetPropertyCost(auctionPropertySquare));
                }
                //Remove property from selling player
                m_myController.RemoveCurrentPlayerProperty(auctionPropertySquare);
                m_gameBoard.RemoveProperty(auctionPropertySquare);
            }
        }

        /// <summary>
        /// If the selling Player is already bankrupt
        /// then only the buying player gets assets/cash
        /// added and subtracted
        /// </summary>
        private void AuctionBankruptProperty()
        {
            int auctionPropertySquare = m_myController.GetAuctionPropertySquare();
            if (m_myController.IsCurrentPlayerPropertyMortgaged(auctionPropertySquare))
            {
                //Add assets/mortgage to buying player
                m_myController.BankruptMortgagedPropertySale(m_myController.GetAuctionWinner(),
                    auctionPropertySquare, m_gameBoard.GetMortgageAmount(auctionPropertySquare));
            }
            else
            {
                //Only add assets
                m_myController.PlayerAddsAssets(m_myController.GetAuctionWinner(), 
                    m_gameBoard.GetPropertyCost(auctionPropertySquare));
            }
        }

        /// <summary>
        /// End of auction cleans up all variables
        /// and buttons. 
        /// If a thread was waiting, alert it and 
        /// go back to player's sales.
        /// Otherwise end turn.
        /// </summary>
        private void EndAuction()
        {
            BidButton.Visible = false; ;
            PassButton.Visible = false;
            InputPriceBox.Visible = false;
            MoneyLabel.Visible = false;
            PropertiesBox.Visible = false;
            m_myController.SetBuyPropertyBool(false);
            m_myController.SetBidValue(10);
            UpdatePlayersLabel(m_myController.GetCurrentPlayerName());
            //reset all players' auction pass booleans
            m_myController.ResetPlayersAuctionPass(m_myController.GetAuctionPlayer());
            m_myController.SetAuctionPlayer("");
            m_myController.SetAuctionWinner("");
            m_myController.SetAuctionPropertySquare(0);

            RefreshPropertiesBox(m_myController.GetCurrentPlayerName());
            UpdateFunds(m_myController.GetCurrentPlayerFunds());

            if (m_myController.HumanTurn())
            {               
                ClosePropertyOptions();
            }

            //Current player should press end turn button here unless bankruptcy sale is happening
            //or selling cards/properties to raise funds
            if (m_myController.GetBankruptcySaleBool())
            {
                //BankruptcySale();
                auctionFinished.SetResult(true);
            }
            else if (m_myController.GetCompSellJailCard())
            {
                //Return to computer sells jail cards or insufficient funds
                auctionFinished.SetResult(true);               
            }
            else if (m_myController.GetCompPropOptionsSale())
            {
                //return to computer player's property options
                auctionFinished.SetResult(true);
            }
            else if (m_myController.GetCompPropertySale())
            {
                auctionFinished.SetResult(true);
            }
            else
            {
                if (m_myController.HumanTurn()) ShowJailCards();
                if (!m_myController.HumanTurn()) CompFinishedTurn();
                if (m_myController.GetOutstandingPayment() == 0)
                { EndTurnButton.Visible = true; }
            }

        }

        /// <summary>
        /// The main game loop, part 1.
        /// Set the next player and start player's turn
        /// if player is not bankrupt.
        /// End the game if all other players are bankrupt
        /// </summary>
        private void PlayGame()
        {
            TurnOffAllButtons();

            if (m_myController.CheckIfGameover() == false)
            {
                //1. Set current player to the next player in the player order
                m_myController.SetNextPlayer();

                //If current player isn't bankrupt
                if (!m_myController.IsPlayerBankrupt(m_myController.GetCurrentPlayerName()))
                {
                    PlayerTurn();
                }
                else
                {
                    //If current player is bankrupt, move onto the next player
                    PlayGame();
                }
            }
            else
            {
                GameOverMessage();
            }
        }

        /// <summary>
        /// Update funds, properties box, and players labels for new player's turn.
        /// Roll dice and move piece on board
        /// Check if passing go to collect $200 if applicable
        /// Then go to optionsMenu to determine what player will do on the square
        /// the player landed on.
        /// </summary>
        private void PlayerTurn()
        {
            UpdatePlayersLabel(m_myController.GetCurrentPlayerName());
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            RefreshPropertiesBox(m_myController.GetCurrentPlayerName());
            if (m_myController.GetCurrentPlayerJailStatus() == false)
            {
                int currentPlayerSquare = m_myController.GetCurrentPlayerSquare();
                //2. Turn off player's current position
                TogglePlayerSquare(currentPlayerSquare, m_myController.GetCurrentPlayerColor(), false);

                //3. Roll dice
                dialogBox.Text = "It's " + m_myController.GetCurrentPlayerNameAndType() + "'s turn. "
                    + m_myController.GetCurrentPlayerName() + " rolls the dice." + Environment.NewLine;
                int[] diceArray = m_myController.RollDice();
                dialogBox.Text += m_myController.GetCurrentPlayerName() + " rolled a " + diceArray[0] + 
                    " and a " + diceArray[1] + "." + Environment.NewLine;
                int diceSum = diceArray[0] + diceArray[1];

                //4. Check if passing Go and update player's current square
                if (m_myController.WrapAround(currentPlayerSquare, diceSum) == true)
                {
                    m_myController.SetCurrentPlayerSquare(m_myController.WrapSquare(currentPlayerSquare, diceSum));
                    if (m_myController.GetCurrentPlayerSquare() != 0)
                    {
                        dialogBox.Text += "You have passed Go, collect $200." + Environment.NewLine;
                        m_myController.CurrentPlayerPassedGo();
                        UpdateFunds(m_myController.GetCurrentPlayerFunds());
                    }
                }
                if (!m_myController.WrapAround(currentPlayerSquare, diceSum))
                {
                    m_myController.SetCurrentPlayerSquare(diceSum + currentPlayerSquare);
                }

                //5. Output new position
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() +
                    " moves " + Convert.ToString(diceSum) +
                    " spaces forward to square " +
                    Convert.ToString(m_myController.GetCurrentPlayerSquare()) + "."
                    + Environment.NewLine;
                //6. Turn square on
                TogglePlayerSquare(m_myController.GetCurrentPlayerSquare(),
                    m_myController.GetCurrentPlayerColor(), true);
            }

            //7. Player chooses what to do this turn
            if (m_myController.HumanTurn()) OptionsMenu(m_myController.GetCurrentPlayerSquare());

            if (!m_myController.HumanTurn()) ComputerTurn(m_myController.GetCurrentPlayerSquare());
        }

        /// <summary>
        /// Turn off all buttons to prevent on-screen clutter
        /// where called. 
        /// </summary>
        private void TurnOffAllButtons()
        {
            EndTurnButton.Visible = false;
            YesButton.Visible = false;
            NoButton.Visible = false;
            SellButton.Visible = false;
            SellCardButton.Visible = false;
            BuyButton.Visible = false;
            BidButton.Visible = false;
            PassButton.Visible = false;
            HandButton.Visible = false;
            RollDiceButton.Visible = false;
            CancelButton.Visible = false;
            MortgageButton.Visible = false;
            UnmortgageButton.Visible = false;
            InputPriceBox.Visible = false;
            MoneyLabel.Visible = false;
            OneButton.Visible = false;
            TwoButton.Visible = false;
            ThreeButton.Visible = false;
            FourButton.Visible = false;
            FiveButton.Visible = false;
            SixButton.Visible = false;
            SevenButton.Visible = false;
        }

        /// <summary>
        /// Display the house on the board
        /// a_turnOn = true means display the
        /// house, false to turn it off.
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_numHouses"></param>
        /// <param name="a_turnOn"></param>
        private void ToggleHouse(int a_squareNum, int a_numHouses, bool a_turnOn)
        {
            string houseName = "square" + Convert.ToString(a_squareNum) + "house";
            Label houseLabel = Controls.Find(houseName, true).FirstOrDefault() as Label;
            if (a_turnOn == true)
            {
                houseLabel.Visible = true;
                houseLabel.Text = Convert.ToString(a_numHouses);
            }
            if (a_turnOn == false)
            {
                houseLabel.Visible = false;
                houseLabel.Text = "";
            }
        }

        /// <summary>
        /// Display the hotel on the board.
        /// a_turnOn = true means turn the hotel on,
        /// false means make the hotel invisible/turned off
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_turnOn"></param>
        private void ToggleHotel(int a_squareNum, bool a_turnOn)
        {
            string hotelName = "square" + Convert.ToString(a_squareNum) + "hotel";
            Label hotelLabel = Controls.Find(hotelName, true).FirstOrDefault() as Label;
            if (a_turnOn == true)
            {
                hotelLabel.Visible = true;
            }
            if (a_turnOn != true)
            {
                hotelLabel.Visible = false;
            }
        }
        /// <summary>
        /// If a player purchased a property from another player
        /// If purchased, remove property from seller, add to buyer
        /// If not purchased, inform players 
        /// </summary>
        private void CompletePlayerSale()
        {
            UpdatePlayersLabel(m_myController.GetCurrentPlayerName());
            RefreshPropertiesBox(m_myController.GetCurrentPlayerName());
            if (m_myController.GetSellPropertyBool())
            {
                dialogBox.Text += m_myController.GetSelectedPlayer() + "(" + 
                    m_myController.GetPlayerType(m_myController.GetSelectedPlayer()) +
                    ") purchased the property" + " for $" + Convert.ToString(m_myController.GetSaleAmount()) + 
                    "." + Environment.NewLine;
           
                int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
                m_myController.CurrentPlayerPropertySold(squareNum, m_myController.GetSaleAmount(), 
                    m_gameBoard.GetPropertyCost(squareNum), 
                    m_gameBoard.GetMortgageAmount(squareNum), m_myController.GetSelectedPlayer());
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
                //remove property from selling player             
                m_gameBoard.RemoveProperty(squareNum);
                //add property to buying player
                m_gameBoard.AddProperty(squareNum, m_myController.GetSelectedPlayer());
            }
            if (!m_myController.GetSellPropertyBool())
            {
                dialogBox.Text += m_myController.GetSelectedPlayer() + "(" + 
                    m_myController.GetPlayerType(m_myController.GetSelectedPlayer()) +
                     ") did not purchase the property." + Environment.NewLine;
            }
            if (m_myController.HumanTurn()) ClosePropertyOptions();
        }

       
        /// <summary>
        /// Clear the dialog box.
        /// Optional for user to clear away too much text in box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            dialogBox.Text = "";
        }

        private void RefreshPropertiesBox(string a_playerName)
        {
            PropertiesBox.Visible = true;
            PropertiesBox.Text = "Properties";
            PropertiesBox.Items.Clear();

            List<int> propertiesList = m_myController.GetPlayerPropertiesList(a_playerName);
            foreach (int squareNum in propertiesList)
            {
                PropertiesBox.Items.Add(m_gameBoard.GetSquareMessage(squareNum));
            }
        }

        /// <summary>
        /// The human player can view their own properties during
        /// their turn and take actions with their own properties.
        /// This function checks to ensure that the player has first responded
        /// to other in-game events before trying to open PropertyOptions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertiesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string propertyName = PropertiesBox.Text;          
            //The player can't open Property Options when buying/auctioning a property, selling a jail card, 
            //selling property to another player, paying taxes, is a computer player, is having a 
            //bankruptcy sale, is in jail but hasn't rolled/paid fee yet
            if (propertyName != "Properties"  && propertyName != "" 
                && m_myController.GetBuyPropertyBool() == false && 
                m_myController.GetSellCardBool() == false && !m_myController.GetBankruptcySaleBool() &&
                m_myController.GetSellPlayerProperty() == false && m_myController.HumanTurn()
                && !m_myController.GetTaxBool() && !m_myController.GetPlayerJailRoll())
            {
                m_myController.SetPropertyName(propertyName);
                EndTurnButton.Visible = false;
                HandButton.Visible = false;
                PropertyOptions();
            }
        }

        /// <summary>
        /// The human player's property options.
        /// The player can: Mortgage property,
        /// sell it, buy/sell house, buy/sell hotel,
        /// unmortgage property. 
        /// </summary>
        private void PropertyOptions()
        {
            TurnOffNumberButtons(); //clear out buttons from previous property selections 

            dialogBox.Text += "What would you like to do with " + m_myController.GetPropertyName() + 
            "?" + Environment.NewLine;
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            if (m_myController.CurrentPlayerPropertyUnimproved(squareNum))
            {
                SellOrMortgageProperty(squareNum);
            }
            //Buy a house (if all colors owned)
            if (m_gameBoard.HouseBuildable(squareNum) && !m_myController.CurrentPlayerFourHousesBuilt(squareNum) 
                && !m_myController.CurrentPlayerHotelBuilt(squareNum))
            {
                BuyHouseOnProperty(squareNum);
            }
            if (!m_myController.CurrentPlayerPropertyUnimproved(squareNum))
            {
                SellHouseOnProperty(squareNum);
            }
            if (m_myController.CurrentPlayerFourHousesBuilt(squareNum))
            {
                BuildHotelOnProperty(squareNum);
            }
            if (m_myController.CurrentPlayerHotelBuilt(squareNum))
            {
                SellHotelOnProperty(squareNum);
            }
            if (m_myController.IsCurrentPlayerPropertyMortgaged(squareNum))
            {
                dialogBox.Text += "7. Unmortgage property." + Environment.NewLine;
                SevenButton.Visible = true;
            }
            CancelButton.Visible = true;
        }

        /// <summary>
        /// Clear out buttons from screen of previous property selections.
        /// </summary>
        private void TurnOffNumberButtons()
        {
            OneButton.Visible = false;
            TwoButton.Visible = false;
            ThreeButton.Visible = false;
            FourButton.Visible = false;
            FiveButton.Visible = false;
            SixButton.Visible = false;
            SevenButton.Visible = false;
        }

        /// <summary>
        /// Player can choose whether to sell or mortgage the 
        /// selected property
        /// </summary>
        /// <param name="a_squareNum"></param>
        private void SellOrMortgageProperty(int a_squareNum)
        {
            //sell property to another player
            dialogBox.Text += "1. Sell the property." + Environment.NewLine;
            OneButton.Visible = true;
            //mortgage the property
            if (!m_myController.IsCurrentPlayerPropertyMortgaged(a_squareNum))
            {
                dialogBox.Text += "2. Mortgage property." + Environment.NewLine;
                TwoButton.Visible = true;
            }
        }

        /// <summary>
        /// Player is prompted about whether to buy house
        /// on property (if it's possible to buy one)
        /// </summary>
        /// <param name="a_squareNum"></param>
        private void BuyHouseOnProperty(int a_squareNum)
        {
            if (m_gameBoard.HouseAvailable() &&
                   m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(a_squareNum), 
                   a_squareNum, true))
            {
                dialogBox.Text += "3. Buy a house." + Environment.NewLine;
                ThreeButton.Visible = true;
            }
            if (!m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(a_squareNum), 
                a_squareNum, true))
            {
                dialogBox.Text += "3. Buy a house." + Environment.NewLine;
                dialogBox.Text += "You cannot build another house evenly." + Environment.NewLine;
            }
            if (!m_gameBoard.HouseAvailable())
            {
                dialogBox.Text += "3. Buy a house." + Environment.NewLine;
                dialogBox.Text += "There are no houses available to purchase." + Environment.NewLine;
            }
        }

        /// <summary>
        /// Player is prompted with option to sell house on property
        /// if it's an available option.
        /// </summary>
        /// <param name="a_squareNum"></param>
        private void SellHouseOnProperty(int a_squareNum)
        {
            //Sell a house (to bank) if it can be sold evenly and a hotel has not been built on the property          
            if (m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(a_squareNum), 
                a_squareNum, false)
                && !m_myController.CurrentPlayerHotelBuilt(a_squareNum))
            {
                dialogBox.Text += "4. Sell a house." + Environment.NewLine;
                FourButton.Visible = true;
            }
            if (!m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(a_squareNum), a_squareNum, false))
            {
                dialogBox.Text += "4. Sell a house." + Environment.NewLine;
                dialogBox.Text += "You cannot sell another house evenly." + Environment.NewLine;
            }
            //Don't display any text if a hotel is built
        }

        /// <summary>
        /// Player is prompted with option to build hotel on property
        /// if it's an available option.
        /// </summary>
        /// <param name="a_squareNum"></param>
        private void BuildHotelOnProperty(int a_squareNum)
        {
            //buy a hotel (if 4 houses owned)
            if (m_gameBoard.HotelAvailable() &&
                m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(a_squareNum), 
                a_squareNum, true))
            {
                dialogBox.Text += "5. Buy a hotel." + Environment.NewLine;
                FiveButton.Visible = true;
            }
            if (!m_gameBoard.HotelAvailable())
            {
                dialogBox.Text += "5. Buy a hotel." + Environment.NewLine;
                dialogBox.Text += "There are no hotels available to purchase." + Environment.NewLine;
            }
            //Don't display text to player if it's not an even build
        }

        /// <summary>
        /// Player is prompted with option to sell hotel on property
        /// if it's an available option.
        /// </summary>
        /// <param name="a_squareNum"></param>
        private void SellHotelOnProperty(int a_squareNum)
        {
            if (m_gameBoard.HotelSellable() &&
                    m_myController.CurrentPlayerIsEvenBuild(m_gameBoard.GetColorGroupSquareNums(a_squareNum), 
                    a_squareNum, false))
            {
                dialogBox.Text += "6. Sell a hotel." + Environment.NewLine;
                SixButton.Visible = true;
            }
            if (!m_gameBoard.HotelSellable())
            {
                dialogBox.Text += "6. Sell a hotel." + Environment.NewLine;
                dialogBox.Text += "There are not enough houses available to sell a hotel." + Environment.NewLine;
            }
            //Don't display text to player if the hotel can't be sold evenly
        }

        /// <summary>
        /// The one button has two uses:
        /// 1. Ask the player again to confirm selling a property
        /// 2. Sell the property to other players auction style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Sell selected property
        private void OneButton_Click(object sender, EventArgs e)
        {
            TurnOffNumberButtons();
            if (!m_myController.GetSellPlayerProperty())
            {
                //ask player to confirm they want to sell the property
                SellProperty();
            }
            else
            {
                //sell to other players auction style
                m_myController.SetBuyPropertyBool(true);
                m_myController.SetAuctionPropertySquare(m_gameBoard.GetSquareNum(
                    m_myController.GetPropertyName()));
                ClosePropertyOptions(); //lock the other buttons
                m_myController.SetPlayerSale(true);
                Auction();
            }
        }

        /// <summary>
        /// Double check that the user wants to sell the proeprty
        /// </summary>
        private void SellProperty()
        {
            dialogBox.Text += "Are you sure you want to sell the property?" + Environment.NewLine;
            SellButton.Visible = true;
            CancelButton.Visible = true;
        }

        /// <summary>
        /// Cancelling purchasing a hotel/a house, 
        /// selling a hotel/house/property, closing property options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClosePropertyOptions();
        }

        /// <summary>
        /// After the player cancels or finishes a transaction
        /// with a property in property options.
        /// Clear/reset everything. 
        /// </summary>
        private void ClosePropertyOptions()
        {
            SellCardButton.Visible = false;
            TurnOffPropertyButtons();
            m_myController.SetSellCardBool(false);
            m_myController.SetHotelPurchase(false);
            m_myController.SetSellHouse(false);
            m_myController.SetSellHotel(false);
            m_myController.SetSelectPlayerBool(false);
            m_myController.SetSellPropertyBool(false);
            m_myController.SetSellPlayerProperty(false);
            m_myController.SetPropertyName("");
            m_myController.SetSelectedPlayer("");
            InputPriceBox.Visible = false;
            MoneyLabel.Visible = false;
            RefreshPropertiesBox(m_myController.GetCurrentPlayerName());
            if (m_myController.GetOutstandingPayment() == 0) EndTurnButton.Visible = true;
        }

        /// <summary>
        /// Turn off property related buttons
        /// </summary>
        private void TurnOffPropertyButtons()
        {
            TurnOffNumberButtons();
            SellButton.Visible = false;
            BuyButton.Visible = false;
            MortgageButton.Visible = false;
            UnmortgageButton.Visible = false;
            CancelButton.Visible = false;
        }

        /// <summary>
        /// The sell button has several uses:
        /// 1. The player has decided to sell a house
        /// 2. The player has decided to sell a hotel
        /// 3. Player has decided to sell property to another
        /// selected player
        /// 4. Confirmation that the player wants to sell the property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SellButton_Click(object sender, EventArgs e)
        {
            if (m_myController.GetSellHouse())
            {
                SellButton.Visible = false;
                CancelButton.Visible = false;
                m_myController.SetSellHouse(false);
                SellAHouse();
            }
            else if (m_myController.GetSellHotel())
            {
                SellButton.Visible = false;
                CancelButton.Visible = false;
                m_myController.SetSellHotel(false);
                SellAHotel();
            }
            else if (m_myController.GetSelectPlayerBool())
            {
                //Second step after player has decided whether
                //to sell property through auction or individual player
                //Sell property from one player to another
                //Enter a sell price
                SellHumanPlayerProperty2();
            }
            else
            {
                //First step after player has confirmed to sell property
                //sell player's property, make sure player wants to sell
                SellHumanPlayerProperty();
            }
        }

        /// <summary>
        /// Prompt asking how the player wants to sell the property
        /// </summary>
        private void SellHumanPlayerProperty()
        {
            m_myController.SetSellPlayerProperty(true);
            SellButton.Visible = false;
            CancelButton.Visible = false;
            dialogBox.Text += "How would you like to sell the property?" + Environment.NewLine;
            dialogBox.Text += "1. Auction to all other players" + Environment.NewLine;
            OneButton.Visible = true;
            dialogBox.Text += "2. Sell to one player" + Environment.NewLine;
            TwoButton.Visible = true;
        }

        /// <summary>
        /// The player must enter a valid selling price for their property
        /// </summary>
        private void SellHumanPlayerProperty2()
        {
            //Allow selected player to agree to purchasing property 
            try
            {
                int playerInput = Convert.ToInt32(InputPriceBox.Text);
                if (playerInput >= 0 && playerInput <= 15140)
                {
                    m_myController.SetSaleAmount(playerInput);
                    InputPriceBox.Visible = false;
                    MoneyLabel.Visible = false;
                    SellButton.Visible = false;
                    CancelButton.Visible = false;
                    PlayerPropertySale();
                }
                if (playerInput < 0 || playerInput > 15140)
                {
                    if (playerInput < 0)
                    {
                        dialogBox.Text += "The amount you input is too small." + Environment.NewLine;
                    }
                    if (playerInput > 154140)
                    {
                        dialogBox.Text += "The amount you input is too large." + Environment.NewLine;
                    }
                }
            }
            catch (FormatException)
            {
                dialogBox.Text += "Invalid input." + Environment.NewLine;
            }
            catch(OverflowException)
            {
                dialogBox.Text += "Invalid input, system overflow." + Environment.NewLine;
            }
        }

        /// <summary>
        /// The TwoButton has two uses:
        /// 1. The player presses this button when the player
        /// wants to mortgage a property
        /// 2. The player selects to sell their property to an individual player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoButton_Click(object sender, EventArgs e)
        {
            TurnOffNumberButtons();
            if (!m_myController.GetSellPlayerProperty())
            {
                //Mortgage property
                int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
                int mortgageAmount = m_gameBoard.GetMortgageAmount(squareNum);
                dialogBox.Text += "You can mortgage " + m_myController.GetPropertyName() +
                    " for $" + Convert.ToString(mortgageAmount) + "." + Environment.NewLine;
                MortgageButton.Visible = true;
                //or press cancel button
            }
            else 
            {
                dialogBox.Text += "Which player would you like to sell the property to?" +
                  " Select from the list of players." + Environment.NewLine;
                m_myController.SetSelectPlayerBool(true);
            }
        }

        /// <summary>
        /// The player mortgages the property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MortgageButton_Click(object sender, EventArgs e)
        {
            MortgageProperty();
            ClosePropertyOptions(); 
        }

        /// <summary>
        /// Add funds and mortgage to player
        /// Update the player's funds on the screen
        /// </summary>
        private void MortgageProperty ()
        {
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int mortgageAmount = m_gameBoard.GetMortgageAmount(squareNum);
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has mortgaged " + 
                m_myController.GetPropertyName() +
                 " for $" + Convert.ToString(mortgageAmount) + "." + Environment.NewLine;
            m_myController.CurrentPlayerMortgageProperty(squareNum, mortgageAmount, 
                m_gameBoard.GetPropertyCost(squareNum));

            UpdateFunds(m_myController.GetCurrentPlayerFunds());
        }

        /// <summary>
        /// The selected player the player decided to sell their 
        /// property to has the option to purchase the property.
        /// The player is informed if it's mortgaged. 
        /// </summary>
        private void PlayerPropertySale()
        {
            UpdatePlayersLabel(m_myController.GetSelectedPlayer());
            RefreshPropertiesBox(m_myController.GetSelectedPlayer());
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " is selling " + m_myController.GetPropertyName() +
                " for $" + Convert.ToString(m_myController.GetSaleAmount()) + 
                ". " + Environment.NewLine;
            if (m_myController.IsCurrentPlayerPropertyMortgaged(m_gameBoard.GetSquareNum(
                m_myController.GetPropertyName())))
            {
                dialogBox.Text += "This property is mortgaged." + Environment.NewLine;
            }
            if (m_myController.GetPlayerType(m_myController.GetSelectedPlayer()) == "human")
            {
                if (m_myController.GetPlayerFunds(m_myController.GetSelectedPlayer()) < 
                    m_myController.GetSaleAmount())
                {
                    dialogBox.Text += "You do not have enough funds to purchase " + m_myController.GetPropertyName() + 
                        ". "
                        + "Press 'No' to continue." + Environment.NewLine;
                }
                if (m_myController.GetPlayerFunds(m_myController.GetSelectedPlayer()) >= 
                    m_myController.GetSaleAmount())
                {
                    dialogBox.Text += "Would you like to purchase " + m_myController.GetPropertyName() + "?" + 
                        Environment.NewLine;
                    YesButton.Visible = true;
                }
                NoButton.Visible = true;
            }
            if (m_myController.GetPlayerType(m_myController.GetSelectedPlayer()) == "computer")
            {
                CompPlayerPropertySale();
            }
        }

        /// <summary>
        /// The computer player dedices whether to buy 
        /// a property being sold by another player 
        /// </summary>
        private void CompPlayerPropertySale ()
        {
            if (m_myController.GetPlayerFunds(m_myController.GetSelectedPlayer()) < m_myController.GetSaleAmount())
            {
                dialogBox.Text += m_myController.GetSelectedPlayer() + "(" + 
                    m_myController.GetPlayerType(m_myController.GetSelectedPlayer()) +
                    ") does not have enough funds to purchase " + m_myController.GetPropertyName() + ". "
                    + Environment.NewLine;
            }
            else
            {
                if (m_compStrat.BuyPlayerProperty(m_myController.GetSaleAmount(), 
                    m_gameBoard.GetPropertyCost(m_myController.GetPropertyName()),
                    m_myController.GetCurrentPlayerFunds(), 
                    m_gameBoard.GetColorGroup(m_gameBoard.GetSquareNum(m_myController.GetPropertyName()))))
                    { m_myController.SetSellPropertyBool(true); }              
            }
            CompletePlayerSale();
        }
    
        /// <summary>
        /// The ThreeButton is 
        /// for the player to press when the player
        /// wants to buy a house.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Buy a house
        private void ThreeButton_Click(object sender, EventArgs e)
        {
            TurnOffNumberButtons();

            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int propertyCost = m_gameBoard.GetHouseCost(squareNum);
            dialogBox.Text += "A house costs $" + Convert.ToString(propertyCost) + Environment.NewLine;
            if (propertyCost > m_myController.GetCurrentPlayerFunds())
            {
                dialogBox.Text += "You don't have sufficient funds to purchase a house." + Environment.NewLine;
            }
            if (propertyCost <= m_myController.GetCurrentPlayerFunds())
            {
                dialogBox.Text += "Are you sure you want to buy a house?" + Environment.NewLine;
                BuyButton.Visible = true;
            }
            CancelButton.Visible = true;
        }

        /// <summary>
        /// The buy button has two uses:
        /// 1. For the player to buy a house
        /// 2. Or for the player to buy a hotel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyButton_Click(object sender, EventArgs e)
        {
            BuyButton.Visible = false;
            CancelButton.Visible = false;
            if (m_myController.GetHotelPurchase() == false)
            {
                BuyHouse();
            }
            else
            {
                BuyHotel();
            }
        }

        /// <summary>
        /// The player purchased a house.
        /// Subtract funds and display the house on the board
        /// </summary>
        private void BuyHouse()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has purchased a house on " 
                + m_myController.GetPropertyName() + "." + Environment.NewLine;
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            m_myController.CurrentPlayerBoughtHouse(m_gameBoard.GetHouseCost(squareNum), squareNum);
            m_gameBoard.SubtractHouses(1);
            //toggle on house and number
            ToggleHouse(squareNum, m_myController.GetCurrentPlayerNumHouses(squareNum), true);
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            if (m_myController.HumanTurn()) ClosePropertyOptions();
        }

        /// <summary>
        /// The player purchased a hotel.
        /// Subtract funds, turn off houses,
        /// and display the hotel on the board
        /// </summary>
        private void BuyHotel()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + 
                " has purchased a hotel on " + m_myController.GetPropertyName() + "." + Environment.NewLine;
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            m_myController.CurrentPlayerBoughtHotel(m_gameBoard.GetHouseCost(squareNum), squareNum);
            //turn off house square and erase text
            ToggleHouse(squareNum, 0, false);
            ToggleHotel(squareNum, true);
            m_gameBoard.SubtractHotels(1);
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            if (m_myController.HumanTurn()) ClosePropertyOptions();
        }

        /// <summary>
        /// The player purchased a hotel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Purchase hotel
        private void FiveButton_Click(object sender, EventArgs e)
        {
            TurnOffNumberButtons();

            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int hotelCost = m_gameBoard.GetHouseCost(squareNum); //hotels cost same as a house
            dialogBox.Text += "A hotel costs $" + Convert.ToString(hotelCost) + Environment.NewLine;
            if (hotelCost > m_myController.GetCurrentPlayerFunds())
            {
                dialogBox.Text += "You don't have sufficient funds to purchase a hotel." + Environment.NewLine;
            }
            else
            {
                dialogBox.Text += "Are you sure you want to buy a hotel?" + Environment.NewLine;
                m_myController.SetHotelPurchase(true);
                BuyButton.Visible = true;
            }
            CancelButton.Visible = true;
        }

        /// <summary>
        /// The player has the option to sell a house.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Sell a house
        private void FourButton_Click(object sender, EventArgs e)
        {
            //Sell a house back to the bank for half the price it was purchased for
            TurnOffNumberButtons();

            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int sellPrice = m_gameBoard.GetSellHousePrice(squareNum);
            dialogBox.Text += "A house can be sold back to the bank for half its cost." +
                "This house can sell for $" + Convert.ToString(sellPrice) + Environment.NewLine;

            dialogBox.Text += "Are you sure you want to sell a house?" + Environment.NewLine;
            m_myController.SetSellHouse(true);
            SellButton.Visible = true;
            CancelButton.Visible = true;
        }

        /// <summary>
        /// The player sold a house.
        /// Turn off/subtract the house from visibility on the board
        /// and add funds to player.
        /// </summary>
        private void SellAHouse()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has sold a house on " 
                + m_myController.GetPropertyName() + "." + Environment.NewLine;
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            m_myController.CurrentPlayerSoldHouse(m_gameBoard.GetSellHousePrice(squareNum), 
                squareNum, m_gameBoard.GetHouseCost(squareNum));

            if (m_myController.CurrentPlayerPropertyUnimproved(squareNum))
            {
                //turn off house if no more houses on property
                ToggleHouse(squareNum, 0, false);
            }
            if (!m_myController.CurrentPlayerPropertyUnimproved(squareNum))
            {
                //toggle house and number
                ToggleHouse(squareNum, m_myController.GetCurrentPlayerNumHouses(squareNum), true);
            }
            m_gameBoard.AddHouses(1);
         
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            RefreshPropertiesBox(m_myController.GetCurrentPlayerName());
            if (m_myController.HumanTurn())
            {
                m_myController.SetPropertyName("");
                ClosePropertyOptions();
            }
        }

        /// <summary>
        /// Player can sell hotel back to the bank.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SixButton_Click(object sender, EventArgs e)
        {
            //Sell a hotel back to the bank for half the price it was purchased for 
            TurnOffNumberButtons();

            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int sellPrice = m_gameBoard.GetSellHousePrice(squareNum);
            dialogBox.Text += "A hotel can be sold back to the bank for half its cost." +
                "This hotel can sell for $" + Convert.ToString(sellPrice) + Environment.NewLine;

            dialogBox.Text += "Are you sure you want to sell a hotel?" + Environment.NewLine;
            m_myController.SetSellHotel(true);
            SellButton.Visible = true;
            CancelButton.Visible = true;
        }

        /// <summary>
        /// The player decided to sell a hotel.
        /// Display houses again and get back money
        /// </summary>
        private void SellAHotel()
        {
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has sold a hotel on "
                + m_myController.GetPropertyName() + "." + Environment.NewLine;
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            m_myController.CurrentPlayerSoldHotel(m_gameBoard.GetSellHousePrice(squareNum), squareNum, 
                m_gameBoard.GetHouseCost(squareNum));
            m_gameBoard.SubtractHouses(4);
            m_gameBoard.AddHotels(1);

            //turn on house square with 4 houses
            ToggleHouse(squareNum, m_myController.GetCurrentPlayerNumHouses(squareNum), true);
            ToggleHotel(squareNum, false);       
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            RefreshPropertiesBox(m_myController.GetCurrentPlayerName());
            if (m_myController.HumanTurn())
            {
                m_myController.SetPropertyName("");
                ClosePropertyOptions();
            }
        }

        /// <summary>
        /// Player has the option to unmortgage a property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SevenButton_Click(object sender, EventArgs e)
        {
            TurnOffNumberButtons();
            //Unmortgage property
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int unmortgageCost = m_gameBoard.GetUnmortgageCost(squareNum);
            dialogBox.Text += "You can unmortgage " + m_myController.GetPropertyName() + " for $" 
                + Convert.ToString(unmortgageCost) + ", which is the mortgage cost plus 10%." + Environment.NewLine;
            if (m_myController.GetCurrentPlayerFunds() < unmortgageCost)
            {
                dialogBox.Text += "You do not have sufficient funds to unmortgage this property." + Environment.NewLine;
            }
            if (m_myController.GetCurrentPlayerFunds() >= unmortgageCost)
            {
                UnmortgageButton.Visible = true;
            }

            //player can press cancel button alternatively
        }

        /// <summary>
        /// The player decided to unmortage a property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnmortgageButton_Click(object sender, EventArgs e)
        {
            PlayerUnmortgagedProperty();
            ClosePropertyOptions();
        }

        /// <summary>
        /// The player decided to unmortage a property
        /// Pay fees and make the property unmortgaged
        /// </summary>
        private void PlayerUnmortgagedProperty ()
        {
            int squareNum = m_gameBoard.GetSquareNum(m_myController.GetPropertyName());
            int unmortgageCost = m_gameBoard.GetUnmortgageCost(squareNum);
            dialogBox.Text +=  m_myController.GetCurrentPlayerNameAndType() + " has unmortgaged " 
                + m_myController.GetPropertyName() + " for $" + Convert.ToString(unmortgageCost) + 
                "." + Environment.NewLine;
            m_myController.CurrentPlayerUnmortgageProperty(squareNum, unmortgageCost, 
                m_gameBoard.GetMortgageAmount(squareNum),
                m_gameBoard.GetPropertyCost(squareNum));

            UpdateFunds(m_myController.GetCurrentPlayerFunds());
        }

        /// <summary>
        /// Checking that the player hasn't selected themselves
        /// to sell a property or jail card to
        /// </summary>
        /// <param name="a_labelNum"></param>
        /// <returns></returns>
        private bool SellingToSelf(int a_labelNum)
        {
            if (a_labelNum == m_myController.GetCurrentPlayerPosition())
            {
                if (m_myController.GetSelectPlayerBool() || m_myController.GetSellCardBool())
                {
                    dialogBox.Text += "You can't sell to yourself." + Environment.NewLine;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checking that the player hasn't selected a bankrupt player
        /// to sell a property or jail card to
        /// </summary>
        /// <param name="a_labelNum"></param>
        /// <returns></returns>
        private bool BankruptPlayer(int a_labelNum)
        {
            if (m_myController.IsPlayerBankrupt(a_labelNum))
            {
                dialogBox.Text += "This player is bankrupt." + Environment.NewLine;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Current human player selected this player
        /// to sell property/jail card to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player1Label_Click(object sender, EventArgs e)
        {
            if (!SellingToSelf(0) && !BankruptPlayer(0))
            {
                SelectPlayerOptions(0);
                SellJailCard(0);
            }
        }

        /// <summary>
        /// Current human player selected this player
        /// to sell property/jail card to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player2Label_Click(object sender, EventArgs e)
        {
            if (!SellingToSelf(1) && !BankruptPlayer(1))
            {
                SelectPlayerOptions(1);
                SellJailCard(1);
            }
        }

        /// <summary>
        /// Current human player selected this player
        /// to sell property/jail card to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player3Label_Click(object sender, EventArgs e)
        {
            if (!SellingToSelf(2) && !BankruptPlayer(2))
            {
                SelectPlayerOptions(2);
                SellJailCard(2);
            }
        }

        /// <summary>
        /// Current human player selected this player
        /// to sell property/jail card to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player4Label_Click(object sender, EventArgs e)
        {
            if (!SellingToSelf(3) && !BankruptPlayer(3))
            {
                SelectPlayerOptions(3);
                SellJailCard(3);
            }
        }

        /// <summary>
        /// After the current player has selected
        /// a player to sell to, the player
        /// must now pick a price to sell the property/
        /// </summary>
        /// <param name="a_index"></param>
        //If selling a property to another player
        private void SelectPlayerOptions(int a_index)
        {
            CancelButton.Visible = false;
            if (m_myController.GetSelectPlayerBool())
            {
                string playerSelected = m_myController.WhichPlayer(a_index);
                if (playerSelected != m_myController.GetCurrentPlayerName()) 
                {
                    dialogBox.Text += "You have selected " + playerSelected + "." + Environment.NewLine;
                    dialogBox.Text += "Choose a price to sell the property." + Environment.NewLine;
                    m_myController.SetSelectedPlayer(playerSelected);
                    TurnOnPriceBox();
                    SellButton.Visible = true;
                }
                else
                {
                    dialogBox.Text += "Invalid player choice." + Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// The current human player will now select
        /// a price to sell the jail for after selecting
        /// a player to sell it to
        /// </summary>
        /// <param name="a_index"></param>
        private void SellJailCard(int a_index)
        {
            if (m_myController.GetSellCardBool())
            {
                CancelButton.Visible = false;
                dialogBox.Text += "You have selected " + m_myController.WhichPlayer(a_index) + 
                    "(" + m_myController.GetPlayerType(a_index) 
                    + ")." + Environment.NewLine;
                dialogBox.Text += "How much do you want to sell the card for?" + Environment.NewLine;
                TurnOnPriceBox();
                m_myController.SetCardBuyer(a_index);
                SellCardButton.Visible = true;
            }
        }

        /// <summary>
        /// Used by human player to sell jail card
        /// The player is first prompted to decide who
        /// to sell it to on first click.
        /// On second click, the buying player is prompted
        /// to buy the card at the chosen price ponit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SellCardButton_Click(object sender, EventArgs e)
        {
            //When player first presses the sell card button:
            if (!m_myController.GetSellCardBool())
            {
                SellCardButton.Visible = false;
                HandButton.Visible = false;
                m_myController.SetSellCardBool(true);
                dialogBox.Text += "Who do you want to sell the card to? Select from the players list." 
                    + Environment.NewLine;
                //or press cancel
            }
            else
            {
                //Player has decided who to sell jail card to
                SellJailCardPriceInput();
            }
        }

        /// <summary>
        /// Current human player sells jail card for a 
        /// chosen and valid price
        /// </summary>
        private void SellJailCardPriceInput()
        {
            try
            {
                //Sell jail card to selected player
                int playerInput = Convert.ToInt32(InputPriceBox.Text);
                if (playerInput >= 0 && playerInput <= 15140)
                {
                    MoneyLabel.Visible = false;
                    InputPriceBox.Visible = false;
                    ExitButton.Visible = false;
                    SellJailCard2(playerInput);
                }
                else
                {
                    if (playerInput < 0)
                    {
                        dialogBox.Text += "The amount you input is too small." + Environment.NewLine;
                    }
                    else
                    {
                        dialogBox.Text += "The amount you input is too large." + Environment.NewLine;
                    }
                }

            }
            catch (FormatException)
            {
                dialogBox.Text += "Invalid input." + Environment.NewLine;
            }
            catch (OverflowException)
            {
                dialogBox.Text += "Invalid input, system overflow." + Environment.NewLine;
            }
        }

        /// <summary>
        /// Buying player is prompted to decide whether or not to buy a jail card
        /// </summary>
        /// <param name="a_salePrice"></param>
        private void SellJailCard2 (int a_salePrice)
        {
            SellCardButton.Visible = false;
            CancelButton.Visible = false;
            UpdatePlayersLabel(m_myController.WhichPlayer(m_myController.GetCardBuyer()));
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " is selling a jail card" +
              " for $" + Convert.ToString(a_salePrice) + "." + Environment.NewLine;
            m_myController.SetCardPrice(a_salePrice);
            if (m_myController.GetPlayerType(m_myController.GetCardBuyer()) == "human")
            {
                if (m_myController.GetPlayerFunds(m_myController.GetCardBuyer()) < a_salePrice)
                {
                    dialogBox.Text += "You do not have enough funds to purchase a jail card from "
                        + m_myController.GetCurrentPlayerNameAndType() +
                        ". Prese 'No' to continue." + Environment.NewLine;
                }
                if (m_myController.GetPlayerFunds(m_myController.GetCardBuyer()) >= a_salePrice)
                {
                    dialogBox.Text += "Would you like to purchase a jail card?" + Environment.NewLine;
                    YesButton.Visible = true;
                }             
                NoButton.Visible = true;
            }
            else { CompPlayerBuyJailCard(); }
        }

        /// <summary>
        /// The computer player is prompted about whether to
        /// buy a jail card
        /// </summary>
        private void CompPlayerBuyJailCard ()
        {
            if (m_myController.GetPlayerFunds(m_myController.GetCardBuyer()) < m_myController.GetCardPrice())
            {
                dialogBox.Text += m_myController.WhichPlayer(m_myController.GetCardBuyer()) +
                    "(" + m_myController.GetPlayerType(m_myController.GetCardBuyer()) +
                    ") does not have enough funds to purchase a jail card from "
                    + m_myController.GetCurrentPlayerNameAndType() + Environment.NewLine;
            }
            else
            {
                if (m_compStrat.BuyJailCard(m_myController.GetCardPrice(), 
                    m_myController.GetPlayerFunds(m_myController.GetCardBuyer())))
                {
                    //buy the card
                    m_myController.SetJailCardBought(true);
                }                      
            }
            //don't buy the card/proceed to last step
            SoldJailCard();
        }

        /// <summary>
        /// After the buying player decided whether or not to buy the card
        /// this function will either transfer the card to the buyer or 
        /// inform the seller that the card was not sold.
        /// </summary>
        private void SoldJailCard ()
        {
            UpdatePlayersLabel(m_myController.GetCurrentPlayerName());
            if (m_myController.GetJailCardBought())
            {
                dialogBox.Text += m_myController.WhichPlayer(m_myController.GetCardBuyer()) + 
                    "(" + m_myController.GetPlayerType(m_myController.GetCardBuyer()) 
                    + ") purchased your jail card." + Environment.NewLine;
                m_myController.CurrentPlayerSoldJailCard(m_myController.GetCardBuyer(), m_myController.GetCardPrice());
                m_myController.SetJailCardBought(false);
                UpdateFunds(m_myController.GetCurrentPlayerFunds());
            }
            else
            {
                dialogBox.Text += m_myController.WhichPlayer(m_myController.GetCardBuyer()) + 
                    "(" + m_myController.GetPlayerType(m_myController.GetCardBuyer())
                    + ") did not purchase your jail card." + Environment.NewLine;               
            }
            m_myController.SetCardPrice(0);
            m_myController.SetCardBuyer(0);
            m_myController.SetSellCardBool(false);
            if (m_myController.HumanTurn() && m_myController.GetCurrentPlayerNumJailCards() != 0)
            {
                HandButton.Visible = true;
            }
            if (m_myController.GetOutstandingPayment() == 0) EndTurnButton.Visible = true;
        }

        /// <summary>
        /// The human player chose to pay a 
        /// flat rate of $200 in income tax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoHundredButton_Click(object sender, EventArgs e)
        {
            dialogBox.Text += "You have chosen to pay $200." + Environment.NewLine;
            m_myController.CurrentPlayerPaysMoney(200);
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            m_myController.SetTaxBool(false);
            TwoHundredButton.Visible = false;
            TenPercentButton.Visible = false;
            EndTurnButton.Visible = true;
            ShowJailCards();
        }

        /// <summary>
        /// The human player chose to pay  
        /// 10% of their assets in income tax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TenPercentButton_Click(object sender, EventArgs e)
        {        
            int totalAssets = m_myController.GetCurrentPlayerTotalAssets();
            int tenPercent = (totalAssets / 10);
            dialogBox.Text += "You have chosen to pay 10% of your assets: $"
                + Convert.ToString(tenPercent) + "." + Environment.NewLine;
            m_myController.CurrentPlayerPaysMoney(tenPercent);
            m_myController.SetTaxBool(false);
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            TwoHundredButton.Visible = false;
            TenPercentButton.Visible = false;
            EndTurnButton.Visible = true;
            ShowJailCards();
        }

        /// <summary>
        /// Human player declares bankruptcy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BankruptButton_Click(object sender, EventArgs e)
        {           
            BankruptButton.Visible = false;
            PayButton.Visible = false;

            PlayerBankrupt();
        }

        /// <summary>
        /// Set the player status to bankrupt
        /// Have a bankruptcy sale if the game isn't over
        /// </summary>
        private void PlayerBankrupt ()
        {
            m_myController.SetOutstandingPayment(0);
            m_myController.CurrentPlayerDeclaresBankrupt();
            dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " declared bankruptcy." 
                + Environment.NewLine;
            if (!m_myController.CheckIfGameover())
            {
                //sell player's remaining properties to other players
                BankruptcySale();               
            }
            else
            {
                GameOverMessage();
            }
        }

        /// <summary>
        /// Return remaining assets to the board and 
        /// auction any remaining properties to other players
        /// </summary>
        private async void BankruptcySale()
        {
            dialogBox.Text += Environment.NewLine + m_myController.GetCurrentPlayerNameAndType() + 
                " has declared bankruptcy."
            + Environment.NewLine;
            //Turn off player piece on board:
            TogglePlayerSquare(m_myController.GetCurrentPlayerSquare(), m_myController.GetCurrentPlayerColor(), 
                false);

            //liquidate bankrupt player's assets
            m_myController.LiquidateCurrentPlayerAssets();

            //Get out of jail free cards are put back into deck
            HandButton.Visible = false;
            m_myController.ResetCurrentPlayerJailCards();

            //Sell any remaining houses/hotels back to bank
            if (m_myController.GetCurrentPlayerTotalHotels() != 0)
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has " + 
                    Convert.ToString(m_myController.GetCurrentPlayerTotalHotels()) +
                    " hotels which will now be sold back to the bank." + Environment.NewLine;
                int[] hotelSquareNums = m_myController.GetCurrentPlayerHousesHotels().Keys.ToArray();
                int[] numHouseHotels = m_myController.GetCurrentPlayerHousesHotels().Values.ToArray();
                for (int i = 0; i < hotelSquareNums.Count(); i++)
                {
                    if (numHouseHotels[i] == 5)
                    {
                        //sell back to bank
                        m_myController.SetPropertyName(m_gameBoard.GetSquareMessage(hotelSquareNums[i]));
                        SellAHotel();
                    }
                }
            }

            if (m_myController.GetCurrentPlayerTotalHouses() != 0)
            {
                dialogBox.Text += m_myController.GetCurrentPlayerNameAndType() + " has " + 
                    Convert.ToString(m_myController.GetCurrentPlayerTotalHouses()) +
                    " houses which will now be sold back to the bank." + Environment.NewLine;
                int [] squareNums = m_myController.GetCurrentPlayerHousesHotels().Keys.ToArray();
                int [] numHouses = m_myController.GetCurrentPlayerHousesHotels().Values.ToArray();
                for (int j = 0; j < squareNums.Count(); j++)
                {
                    if (numHouses[j] < 5) //if not a hotel
                    {
                        //sell back to bank
                        m_myController.SetPropertyName(m_gameBoard.GetSquareMessage(squareNums[j]));
                        SellAHouse();
                    }
                }
            }

            if (m_myController.GetCurrentPlayerTotalProperties() != 0)
            {
                //If any properties are left, auction to other players or simply return to board
                dialogBox.Text += "All of " + m_myController.GetCurrentPlayerName() + 
                    "'s remaining properties will now be liquidated and auctioned."
                    + Environment.NewLine;
                int [] bankruptProperties = m_myController.GetCurrentPlayerPropertiesList().ToArray();
                if (bankruptProperties.Length > 0)
                {
                    dialogBox.Text += "The property is " + bankruptProperties[0] + "." + Environment.NewLine;
                    //remove from occupiedProperty list in gameBoard
                    m_gameBoard.RemoveProperty(bankruptProperties[0]);
                    m_myController.RemoveCurrentPlayerProperty(bankruptProperties[0]);
                    //auction it
                    m_myController.SetBuyPropertyBool(true);
                    m_myController.SetAuctionPropertySquare(bankruptProperties[0]);
                    m_myController.SetPlayerSale(true);
                    m_myController.SetBankruptcySaleBool(true);
                    auctionFinished = new TaskCompletionSource<bool>();
                    Auction();
                    await auctionFinished.Task;
                }            
            }

            //Finished all auctions
            m_myController.SetBuyPropertyBool(false);
            m_myController.SetPlayerSale(false);
            m_myController.SetBankruptcySaleBool(false);
            EndTurnButton.Visible = true;
        }

        /// <summary>
        /// This button is for paying
        /// off outstanding payments the 
        /// player owes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayButton_Click(object sender, EventArgs e)
        {
            if (MoneySufficient(m_myController.GetCurrentPlayerFunds(), m_myController.GetOutstandingPayment()))
            {
                BankruptButton.Visible = false;
                PayButton.Visible = false;
                PayOutstandingPayment();
            }
            if (!MoneySufficient(m_myController.GetCurrentPlayerFunds(), m_myController.GetOutstandingPayment()))
            {
                dialogBox.Text += "You do not have sufficient funds to pay the outstanding amount of $" + 
                    Convert.ToString(m_myController.GetOutstandingPayment()) + 
                    ". Sell your assets or get mortgages to cover the costs. " +
                    "If you have sold all your assets and/or mortgaged all your " +
                    "properties, you must declare bankruptcy." + Environment.NewLine;
            }
        }

        /// <summary>
        /// The player paid off an outstanding payment. 
        /// The player is now in the clear and can end their turn.
        /// </summary>
        private void PayOutstandingPayment ()
        {
            if (m_myController.HumanTurn()) dialogBox.Text += "You";
            else { dialogBox.Text += m_myController.GetCurrentPlayerNameAndType(); }
            dialogBox.Text += " paid off an outstanding payment of $";
            dialogBox.Text += Convert.ToString(m_myController.GetOutstandingPayment()) + "." + Environment.NewLine;
            m_myController.CurrentPlayerPaysMoney(m_myController.GetOutstandingPayment());
            m_myController.SetOutstandingPayment(0);
            UpdateFunds(m_myController.GetCurrentPlayerFunds());
            if(m_myController.HumanTurn()) EndTurnButton.Visible = true;
            if (!m_myController.HumanTurn()) CompPropertyOptions();
        }

        /// <summary>
        /// Announce that the game is over in classic mode.
        /// Show who won the game 
        /// </summary>
        private void GameOverMessage ()
        {
            if (m_gameTime != 0)
            {
                CancelStopwatch();
            }
            TurnOffAllButtons();
            PropertiesBox.Visible = false;
            dialogBox.Text += Environment.NewLine + "The game is over." + Environment.NewLine + 
                m_myController.GameWinnerName() + 
                " is the winner. All other players have declared bankruptcy." + Environment.NewLine;
            dialogBox.Text += "You may now close this window to end the game.";
        }

      
        /// <summary>
        /// If all but one player is bankrupt in a timed game,
        /// the stop watch will be stopped 
        /// </summary>
        //If game ends before the time's up
        private void CancelStopwatch()
        {
            timer1.Stop();
            timeLabel.Text = "Game over";
            TurnOffAllButtons();
            PropertiesBox.Visible = false;
        }

        /// <summary>
        /// In a timed game, there could be more than
        /// one winner at the end of the game. Show all the
        /// players' assets and calculate/show who won.
        /// </summary>
        private void TimeGameOver ()
        {
            //output total assets of each player
            int numWinners = m_myController.NumberOfWinners();
            string winnerMessage = "Assets are value of all assets + cash on hand." + Environment.NewLine 
                + m_myController.AllPlayersAssets();
            if (numWinners > 1)
            {
                winnerMessage += "There's been a tie. ";
                List<string> winnerList = m_myController.WinnerList();
                if (numWinners == 2)
                {
                    winnerMessage += winnerList[0] + " and " + winnerList[1] + " have equal assets.";
                }
                else if (numWinners == 3)
                {
                    winnerMessage += winnerList[0] + ", " + winnerList[1] + ", and " +
                        winnerList[2] + " have equal assets.";
                }
                else if (numWinners == 4)
                {
                    winnerMessage += "All the players have equal assets.";
                }
            }
            if (numWinners == 1)
            {
                string winnerName = m_myController.TimeGameWinnerName();
                winnerMessage += winnerName + "(" + m_myController.GetPlayerType(winnerName) +
                    ") has the largest amount of assets, " + winnerName + " is the winner!";
            }
            MessageBox.Show("Game Over! " + winnerMessage);
        }

        /// <summary>
        /// The exit button for exiting the options for
        /// a jail card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Exit the jail cards options menu
        private void ExitButton_Click(object sender, EventArgs e)
        {
            SellCardButton.Visible = false;
            ExitButton.Visible = false;
            EndTurnButton.Visible = true;
            m_myController.SetSellCardBool(false);
            InputPriceBox.Visible = false;
            MoneyLabel.Visible = false;
            if (m_myController.GetCurrentPlayerNumJailCards() != 0) HandButton.Visible = true;
        }
    }
}
