/// <summary>
/// Holds most of the game data for the main game loop and most of the game logic, 
/// holds most of the variables for the UI for the Game class, 
/// and also provides an interface for the Game class to access functions/data from the Player class. 
/// </summary>
/// <author>Daisy Watson</author>
/// <date>5/15/20</date>

using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGame
{
    class Controller
    {
        private int m_players = 0; //total number of players
        private int m_numHumans = 0;
        private int m_numComp = 0;
        //whether mixed players, all human players, or all computer players
        private string m_playerTypeGame = "mixed"; 
        private Random m_randDice = new Random(); //random num generator for dice
        //whose turn to roll the dice at beginning of game to determine player order
        private int m_rollDicePlayer = 1;
        //Dice sums each player rolled:
        private int[] m_diceSums;
        //list of the order the players play consisting of their names: Player 1, 2, etc
        private List<string> m_orderList = new List<string>(); 
        private string m_currentPlayerName = "";
        //current player's position in the player order list/playerArray, 0-3
        private int m_currentPlayerPosition = 0; 
        private Player[] m_playerArray; //array of the players in the game
        private int m_auctionPropertySquare = 0; //property being auctioned
        //if the current player has to pay rent in a special way according to a chance/chest card:
        private bool m_specialRentPay = false;
        //if the current player has any outstanding fees:
        private int m_outstandingPayment = 0;
        //If the current player has the opportunity to purchase a property
        private bool m_buyPropertyBool = false;
        //If a player won an auction
        private string m_auctionWinnerName = "";
        //If a human player has selected another player to sell something to
        private bool m_selectPlayerBool = false;
        //Buying player confirms purchasing the property for sale
        private bool m_sellPropertyBool = false;
        //The human seller wants to sell the property through property options
        private bool m_sellPlayerProperty = false;
        //when a player sells their owned property to another player
        private bool m_playerSale = false;
        //Property in question for being sold, property options
        private string m_propertyName = "";
        //Amount players bid in an auction
        private int m_bidValue = 10; 
        private string m_auctionPlayer = "";
        private string m_selectedPlayer = "";
        //Price of a property player is selling
        private int m_saleAmount = 0;
        private bool m_hotelPurchase = false;
        private bool m_sellHouse = false;
        private bool m_sellHotel = false;
        private bool m_sellCardBool = false;
        //Index of player buying a get out of jail card
        private int m_cardBuyer = 0;
        //Price of buying a get out of jail card
        private int m_cardSalePrice = 0;
        private bool m_jailCardBought = false;
        //when the player must pay tax before doing anything else
        private bool m_taxBool = false; 
        private bool m_bankruptcySaleBool = false;
        //when the computer player is selling cards to raise funds
        private bool m_compPlayerSellJailCard = false;
        //when computer player is selling properties to raise funds for insufficient funds
        private bool m_compPropertySale = false;
        //Computer selling property through its property options
        private bool m_compPropOptionsSale = false;
        //When the current player is in jail and hasn't rolled dice or paid a fee to get out yet
        private bool m_playerJailRoll = false;

        /// <summary>
        /// Set total number of players in game
        /// Create new array of player objects
        /// Create new array for the sums of dice each player will roll
        /// </summary>
        /// <param name="a_number"></param>
        public void SetNumPlayers(int a_number)
        {
            m_players = a_number;
            m_playerArray = new Player[GetNumPlayers()];
            m_diceSums = new int[GetNumPlayers()];
        }

        /// <summary>
        /// Set number of human players
        /// </summary>
        /// <param name="a_number"></param>
        public void SetHumanPlayers (int a_number)
        {
            m_numHumans = a_number;
        }

        /// <summary>
        /// Get number of human players
        /// </summary>
        /// <returns></returns>
        public int GetNumHumans()
        {
            return m_numHumans;
        }

        /// <summary>
        /// Set number of computer players
        /// </summary>
        /// <param name="a_number"></param>
        public void SetCompPlayers (int a_number)
        {
            m_numComp = a_number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Number of computer players</returns>
        public int GetNumComp()
        {
            return m_numComp;
        }

        /// <summary>
        /// All human, all computer players, or mixed
        /// </summary>
        public void SetPlayerTypeGame ()
        {
            m_playerTypeGame = WhatPlayerTypeGame();
        }

        /// <summary>
        /// if players in game are mixed computer/human players,
        /// only human players, or only computer players
        /// This is for setting the game type
        /// </summary>
        /// <returns>game type i.e. "human" for all human game </returns>
        private string WhatPlayerTypeGame ()
        {
            if (GetNumHumans() > 0 && GetNumComp() == 0)
            {
                return "human";
            }
            if (GetNumComp() > 0 && GetNumHumans() == 0)
            {
                return "computer";
            }
            return "mixed";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>What type of game it is: all human, computer, mixed </returns>
        public string GetPlayerTypeGame ()
        {
            return m_playerTypeGame;
        }

        /// <summary>
        /// computer player(s) roll the dice at the beginning of the game
        /// to determine who goes first
        /// </summary>
        /// <returns> What dice the player(s) rolled, if the player rerolled </returns>
        public string ComputerRollsDice()
        {
            string toDisplay = "";

            //While the current player rolling dice is <= the number of players, roll the dice 
            while (GetRollDicePlayer() <= GetNumPlayers()) 
            {
                toDisplay += DiceOutput(); //what dice numbers the player rolled
                toDisplay += RerollOrNext(); //reroll or move onto the next player
            }

            return toDisplay;
        }

        /// <summary>
        /// For determining the player order
        /// </summary>
        /// <returns>Which player's turn it is to roll the dice in the beginning of the game</returns>
        private int GetRollDicePlayer()
        {
            return m_rollDicePlayer;
        }

        /// <summary>
        /// For determinig the player order
        /// human players roll dice first, before computer players
        /// </summary>
        /// <returns> Rolled dice numbers, if rerolled or not </returns>
        public string HumanRollsDice()
        {
            string toDisplay = "";
            //While the current player rolling dice is <= the number of human players, roll the dice 
            while (GetRollDicePlayer() <= GetNumHumans())
            {
                toDisplay += DiceOutput(); //what dice numbers the player rolled
                toDisplay += RerollOrNext(); //reroll or move onto the next player
            }

            return toDisplay;
        }


        /// <summary>
        /// What dice each player rolled when deciding the order the players play
        /// </summary>
        /// <returns>The rolled dice and their sum </returns>
        public string DiceOutput()
        {
            int[] diceArray = RollDice();
            string textToDisplay = "Player " + Convert.ToString(GetRollDicePlayer()) +
            "(" + HumanOrComputerPlayer(GetRollDicePlayer()) + ") rolled a " + Convert.ToString(diceArray[0]) +
            " and a " + Convert.ToString(diceArray[1]) + "." + Environment.NewLine;
            int diceSum = diceArray[0] + diceArray[1];
            textToDisplay += "The dice sum is " + Convert.ToString(diceSum) + "." + Environment.NewLine;
            SetDice((GetRollDicePlayer() - 1), diceSum);

            return textToDisplay;
        }

        /// <summary>
        /// randomly rolls 2 dice
        /// </summary>
        /// <returns>  2 numbers between 1 and 6 </returns>
        public int[] RollDice()
        {
            int[] rndArr = new int[2];
    
            for (int i = 0; i < 2; i++)
            {
                rndArr[i] = m_randDice.Next(1, 6);
            }
            return rndArr;
        }

        /// <summary>
        /// playerNum is the player's name: Player 1,2,3, or 4
        /// return if human or computer player
        /// This only applies at the beginniner of the game, before the player
        /// order has been determined.
        /// </summary>
        /// <param name="a_playerNum"></param>
        /// <returns> The player type: "human" or "computer" </returns>
        public string HumanOrComputerPlayer(int a_playerNum)
        {
            string playerType = "computer";
            if (HumanOnlyGame()) playerType = "human";

            if (a_playerNum <= GetNumHumans()) playerType = "human";

            return playerType;
        }

        /// <summary>
        /// If an all human game or not
        /// </summary>
        /// <returns> True if only human players </returns>
        public bool HumanOnlyGame()
        {
            if (GetNumHumans() > 0 && GetNumComp() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the rolled dice sum for each player at the beginning of the game
        /// </summary>
        /// <param name="a_index"></param>
        /// <param name="a_diceSum"></param>
        /// <remarks> a_index is the index of the particular player in the diceSums array </remarks>
        private void SetDice(int a_index, int a_diceSum)
        {
            m_diceSums[a_index] = a_diceSum;
        }

        /// <summary>
        /// In the beginning of the game
        /// Whose turn to roll next or if the current player needs to reroll
        /// </summary>
        /// <returns>If the player rerolled or if moving on to next player </returns>
        public string RerollOrNext()
        {
            string toDisplay = "";
            if (GetRollDicePlayer() == 1) //The first player rolling
            {
                IncreaseRollDicePlayer();
                toDisplay += "Player " + Convert.ToString(GetRollDicePlayer()) + "(" +
                     HumanOrComputerPlayer(GetRollDicePlayer()) + ") rolls next." + Environment.NewLine;
            }
            else //all other players
            {
                if (RepeatDiceSum(GetRollDicePlayer())) //check to see if the dice sum is a repeat
                {
                    toDisplay += "Player " + Convert.ToString(GetRollDicePlayer()) + 
                        "(" + HumanOrComputerPlayer(GetRollDicePlayer()) +
                    ") rolled the same sum as " + WhichDiceRepeat(GetRollDicePlayer()) + "." +
                    Environment.NewLine + "Player " + Convert.ToString(GetRollDicePlayer()) + "(" +
                    HumanOrComputerPlayer(GetRollDicePlayer()) + ") must roll again." + Environment.NewLine;
                }
                else //move on to next player
                {
                    IncreaseRollDicePlayer();
                    if (GetRollDicePlayer() <= GetNumPlayers()) //not finished rolling yet
                    {
                        toDisplay += "Player " + Convert.ToString(GetRollDicePlayer()) + "(" +
                            HumanOrComputerPlayer(GetRollDicePlayer()) + ") rolls next." + Environment.NewLine;
                    }
                }
            }

            return toDisplay;
        }

        /// <summary>
        /// Increment to the next player to roll the dice in the beginning of the game
        /// to determine the player order
        /// </summary>
        private void IncreaseRollDicePlayer()
        {
            m_rollDicePlayer++;
        }

        /// <summary>
        /// if the rolled dice sum is a repeat of the dice sum rolled by a previous player
        /// </summary>
        /// <param name="a_diceNum"></param>
        /// <remarks> a_diceNum corresponds to the position of the player in the player order </remarks>
        /// <returns>True if there's been a repeat </returns>
        public bool RepeatDiceSum(int a_diceNum)
        {
            if (a_diceNum == 2)
            {
                if (m_diceSums[0] == m_diceSums[1]) { return true; }
            }
            if (a_diceNum == 3)
            {
                if (m_diceSums[2] == m_diceSums[0]) { return true; }
                if (m_diceSums[2] == m_diceSums[1]) { return true; }
            }
            if (a_diceNum == 4)
            {
                if (m_diceSums[3] == m_diceSums[0]) { return true; }
                if (m_diceSums[3] == m_diceSums[1]) { return true; }
                if (m_diceSums[3] == m_diceSums[2]) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Which player rolled the same dice sum as another player
        /// </summary>
        /// <param name="a_diceNum"></param>
        /// <returns> The player name that had the same dice sum </returns>
        public string WhichDiceRepeat(int a_diceNum)
        {
            if (a_diceNum == 3)
            {
                if (m_diceSums[2] == m_diceSums[1])
                {
                    return "Player 2" + "(" + HumanOrComputerPlayer(2) + ")";
                }
            }
            if (a_diceNum == 4)
            {
                if (m_diceSums[3] == m_diceSums[1])
                {
                    return "Player 2" + "(" + HumanOrComputerPlayer(2) + ")";
                }
                if (m_diceSums[3] == m_diceSums[2])
                {
                    return "Player 3" + "(" + HumanOrComputerPlayer(3) + ")";
                }
            }
            return "Player 1" + "(" + HumanOrComputerPlayer(1) + ")"; //else Player 2 rolled same as Player 1
        }

        /// <summary>
        /// when the human players have finished rolling the dice
        /// in the beginning of the game to determine player order
        /// </summary>
        /// <returns> True if finished rolling </returns>
        public bool FinishedRolling()
        {
            //Human players always roll first
            if (GetRollDicePlayer() > GetNumHumans())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Print the orderList
        /// Add the players in the determined order to a new array of player objects
        /// </summary>
        /// <returns>the order the players will play</returns>
        public string DisplayPlayerOrder()
        {
            string outputResult = "";
            if (GetNumPlayers() == 2)
            {
                string firstPlayer = SetTwoPlayerOrder();
                outputResult +=  "Player " + firstPlayer + "(" +
                     HumanOrComputerPlayer(Convert.ToInt32(firstPlayer)) + ") goes first." + Environment.NewLine;
            }
            if (GetNumPlayers() >= 3)
            {
                SetThreeOrMorePlayersOrder();
                outputResult += Convert.ToString(GetPlayerNameInOrderList(0)) + "(" +
                     HumanOrComputerPlayer(GetPlayerNum(GetPlayerNameInOrderList(0))) + 
                     ") goes first." + Environment.NewLine;
                outputResult += Convert.ToString(GetPlayerNameInOrderList(1)) + "(" +
                     HumanOrComputerPlayer(GetPlayerNum(GetPlayerNameInOrderList(1))) + 
                     ") goes second." + Environment.NewLine;
                outputResult += Convert.ToString(GetPlayerNameInOrderList(2)) + "(" +
                     HumanOrComputerPlayer(GetPlayerNum(GetPlayerNameInOrderList(2))) + 
                     ") goes third." + Environment.NewLine;
                if (GetNumPlayers() == 4)
                {
                    outputResult += Convert.ToString(GetPlayerNameInOrderList(3)) + "(" +
                     HumanOrComputerPlayer(GetPlayerNum(GetPlayerNameInOrderList(3))) + 
                     ") goes fourth." + Environment.NewLine;
                };
            }

            CreatePlayerArray(); 
            return outputResult;
        }

        /// <summary>
        /// Set the player order if there are only 2 players playing the game
        /// </summary>
        /// <returns> The name of the player going first </returns>
        private string SetTwoPlayerOrder()
        {
            string firstPlayer = "0";
            if (m_diceSums[0] > m_diceSums[1])
            {
                firstPlayer = "1";
                m_orderList.Add("Player 1");
                m_orderList.Add("Player 2");
            }
            else
            {
                firstPlayer = "2";
                m_orderList.Add("Player 2");
                m_orderList.Add("Player 1");
            }

            return firstPlayer;
        }

        /// <summary>
        /// Set the player order if there are 3-4 players playing the game
        /// </summary>
        private void SetThreeOrMorePlayersOrder()
        {
            Dictionary<int, string> playerOrder = new Dictionary<int, string>();
            //Add dice sums and player names
            playerOrder.Add(m_diceSums[0], "Player 1");
            playerOrder.Add(m_diceSums[1], "Player 2");
            playerOrder.Add(m_diceSums[2], "Player 3");
            if (GetNumPlayers() == 4) { playerOrder.Add(m_diceSums[3], "Player 4"); }

            //Order by dice sums, biggest to smallest
            IEnumerable<KeyValuePair<int, string>> orderedPlayers = playerOrder.OrderByDescending(i => i.Key);

            //Add to the m_orderList
            foreach (KeyValuePair<int, string> player in orderedPlayers)
            {
                m_orderList.Add(player.Value);
            }
        }

        /// <summary>
        /// Set the current player's turn to the first player in the player list
        /// This is the start the game
        /// </summary>
        public void SetFirstPlayer()
        {
            SetCurrentPlayer(m_orderList[0]);
        }

        /// <summary>
        /// </summary>
        /// <param name="a_index"></param>
        /// <returns> Which player is at a particular index in the orderList </returns>
        private string GetPlayerNameInOrderList (int a_index)
        {
            return m_orderList[a_index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns> the number in the player's name i.e. 1 for Player 1 </returns>
        public int GetPlayerNum (string a_playerName)
        {
            return Convert.ToInt32(Convert.ToString(a_playerName[7]));
        }

        /// <summary>
        /// Add the players to the player array
        /// After determining their order using the orderList
        /// </summary>
        public void CreatePlayerArray ()
        {
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                //set color to black because player haven't chosen the color of their pieces yet
                m_playerArray[i] = new Player(GetPlayerNameInOrderList(i), 
                    IsHumanPlayer(GetPlayerNameInOrderList(i)), "black");
            }
        }

        /// <summary>
        /// Get if the player is human or not using the player name
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns> True if human </returns>
        public bool IsHumanPlayer (string a_playerName)
        {
            return IsHumanPlayer(GetPlayerNum(a_playerName));
        }

        /// <summary>
        /// determine if player is a human player or computer player 
        /// playerNum is the current player's position in the orderList: 1-4
        /// This is only used in the beginning of the game when rolling dice
        /// to determine the player order
        /// </summary>
        /// <param name="a_playerNum"></param>
        /// <returns> True if human </returns>
        public bool IsHumanPlayer(int a_playerNum)
        {
            if (HumanOnlyGame())
            {
                return true;
            }
            if (CompOnlyGame())
            {
                return false;
            }

            //Because human players roll first in a mixed game, 
            //any number greater than the number of human players 
            //is a computer player
            if (a_playerNum > GetNumHumans()) return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> True if all computer game </returns>
        public bool CompOnlyGame()
        {
            if (GetNumComp() > 0 && GetNumHumans() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> True if both human and computer players in game </returns>
        public bool MixedGame()
        {
            if (GetNumComp() > 0 && GetNumHumans() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Computer players chooses the colors of their game pieces
        /// </summary>
        /// <returns> outputs what color pieces the computer players chose in an all computer game </returns>
        public string ComputerPlayersChooseColor()
        {
            string textOutput = "";
            List<string> availableColors;  

            for (int i = 0; i < GetNumComp(); i++)
            {
                availableColors = ChoosablePieceColors();
                m_playerArray[GetCurrentPlayerPosition()].SetPlayerColor(availableColors[0]);
                textOutput += Environment.NewLine + GetCurrentPlayerNameAndType() + 
                    " picked " + availableColors[0] + ".";
                //increment to next player to choose color:
                m_currentPlayerPosition++;
            }

            return textOutput;
        }

        /// <summary>
        /// what color pieces are available for the computer player to choose
        /// </summary>
        /// <returns> Available color pieces that haven't been chosen yet by other players </returns>
        public List<string> ChoosablePieceColors()
        {
            //all available colors
            List<string> choosableColors = new List<string>() {
                "yellow", "blue", "orange", "purple" };

            //delete colors already chosen by other m_players 
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                choosableColors.Remove(m_playerArray[i].GetPlayerColor());
            }

            return choosableColors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> current player position in the orderList/playerArray </returns>
        public int GetCurrentPlayerPosition()
        {
            return m_currentPlayerPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> the player name + whether human or computer player </returns>
        public string GetCurrentPlayerNameAndType()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetPlayerName() + "(" +
                m_playerArray[GetCurrentPlayerPosition()].GetPlayerType() + ")";
        }

        /// <summary>
        /// if human or computer's turn when playing the game
        /// </summary>
        /// <returns> True if human player's turn </returns>
        public bool HumanTurn()
        {
            return m_playerArray[GetCurrentPlayerPosition()].PlayerTypeHuman();         
        }

        /// <summary>
        /// output what color the computer player choses in a mixed game
        /// </summary>
        /// <returns> Color of game piece chosen by computer player </returns>
        public string ComputerPlayerColorChoice()
        {
            List<string> availableColors = ChoosablePieceColors();
            //Choose the first available color:
            m_playerArray[GetCurrentPlayerPosition()].SetPlayerColor(availableColors[0]); 

            return availableColors[0];
        }

        /// <summary>
        /// Set the game piece color the human player chose
        /// Move onto next player
        /// </summary>
        /// <param name="a_color"></param>
        public void SetHumanPlayerColor(string a_color)
        {
            m_playerArray[GetCurrentPlayerPosition()].SetPlayerColor(a_color);
            IncrementCurrentPlayerPosition();
        }

        /// <summary>
        /// Move onto the next player
        /// </summary>
        public void IncrementCurrentPlayerPosition()
        {
            m_currentPlayerPosition += 1;
        }

        /// <summary>
        /// If all the players have chosen colors for their game pieces
        /// </summary>
        /// <returns> True if finished picking colors </returns>
        public bool FinishedColorPicking()
        {
            if (m_currentPlayerPosition >= GetNumPlayers()) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Name of current player whose turn it is </returns>
        public string GetCurrentPlayerName()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetPlayerName();
        }

        /// <summary>
        /// Player type of current player, whether human or computer
        /// </summary>
        /// <returns> human or computer </returns>
        public string GetCurrentPlayerType()
        {
            return "(" + m_playerArray[GetCurrentPlayerPosition()].GetPlayerType() + ")";
        }

        /// <summary>
        /// Set the name of and position of the current player
        /// </summary>
        /// <param name="a_playerName"></param>
        public void SetCurrentPlayer(string a_playerName)
        {
            m_currentPlayerName = a_playerName;
            //current position in the order list/m_playerArray, 0-3
            m_currentPlayerPosition = GetPlayerPosition(a_playerName); 
        }

        /// <summary>
        /// Get a copy of the playerArray for the UI 
        /// </summary>
        /// <returns> Array of player objects </returns>
        public Player[] GetPlayerArray ()
        {
            return m_playerArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> color of current player's game piece </returns>
        public string GetCurrentPlayerColor()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetPlayerColor();
        }

        /// <summary>
        /// store what current square the player has landed on
        /// </summary>
        /// <param name="a_squareNum"></param>
        public void SetCurrentPlayerSquare (int a_squareNum)
        {
            m_playerArray[GetCurrentPlayerPosition()].SetSquare(a_squareNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Which square the current player is at </returns>
        public int GetCurrentPlayerSquare()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetSquare();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_playerPosition"></param>
        /// <returns> Which square a particular player is at </returns>
        public int GetPlayerSquare (int a_playerPosition)
        {
            return m_playerArray[a_playerPosition].GetSquare();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Current player's funds, a $ amount </returns>
        public int GetCurrentPlayerFunds()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetMoney();
        }
        
        /// <summary>
        /// Add a_amount to current player's funds
        /// </summary>
        /// <param name="a_amount"></param>
        public void AddCurrentPlayerFunds (int a_amount)
        {
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(a_amount);
        }

        /// <summary>
        /// Set m_auctionPropertySquare to the square # of 
        /// the property being auctioned
        /// </summary>
        /// <param name="a_squareNum"></param>
        public void SetAuctionPropertySquare (int a_squareNum)
        {
            m_auctionPropertySquare = a_squareNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Square num of the current property being auctioned </returns>
        public int GetAuctionPropertySquare()
        {
            return m_auctionPropertySquare;
        }

        /// <summary>
        /// Used by the UI when a player can buy a property
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetBuyPropertyBool(bool a_turnOn)
        {
            m_buyPropertyBool = a_turnOn;
        }

        /// <summary>
        /// Whether a player is buying a property
        /// </summary>
        /// <returns> True if buying property in process </returns>
        public bool GetBuyPropertyBool()
        {
            return m_buyPropertyBool;
        }

        /// <summary>
        /// Set to whoever won an auction
        /// </summary>
        /// <param name="a_name"></param>
        public void SetAuctionWinner(string a_name)
        {
            m_auctionWinnerName = a_name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> The name of whoever won the auction </returns>
        public string GetAuctionWinner()
        {
            return m_auctionWinnerName;
        }
        
        /// <summary>
        /// When the player is selecting another player in the UI
        /// This bool is turned on
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSelectPlayerBool (bool a_turnOn)
        {
            m_selectPlayerBool = a_turnOn;
        }

        /// <summary>
        /// If the current player is selecting another player in the UI
        /// </summary>
        /// <returns> True if selecting </returns>
        public bool GetSelectPlayerBool()
        {
            return m_selectPlayerBool;
        }

        /// <summary>
        /// Set true if player is selling property to another player
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetPlayerSale(bool a_turnOn)
        {
            m_playerSale = a_turnOn;
        }

        /// <summary>
        /// if player is selling property to another player
        /// </summary>
        /// <returns> True if player to player sale of property is happening </returns>
        public bool GetPlayerSale()
        {
            return m_playerSale;
        }

        /// <summary>
        /// If computer player is selling a property through property options 
        /// </summary>
        /// <returns> True if selling throuhg property options </returns>
        public bool GetCompPropOptionsSale()
        {
            return m_compPropOptionsSale;
        }

        /// <summary>
        /// Set true when computer wants to sell a property through its property options
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetCompPropOptionsSale(bool a_turnOn)
        {
            m_compPropOptionsSale = a_turnOn;
        }

        /// <summary>
        /// Set true if the player has to roll or pay a jail fee in jail
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetPlayerJailRoll(bool a_turnOn)
        {
            m_playerJailRoll = a_turnOn;
        }

        /// <summary>
        /// If the player has to roll or pay a jail fee in jail
        /// </summary>
        /// <returns> True if player hasn't rolled/paid yet </returns>
        public bool GetPlayerJailRoll()
        {
            return m_playerJailRoll;
        }

        /// <summary>
        /// Set true when computer player is selling property
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetCompPropertySale(bool a_turnOn)
        {
            m_compPropertySale = a_turnOn;
        }

        /// <summary>
        /// If computer player is selling a property
        /// </summary>
        /// <returns></returns>
        public bool GetCompPropertySale()
        {
            return m_compPropertySale;
        }

        /// <summary>
        /// Set true if the computer is selling a get out of jail free card
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetCompSellJailCard(bool a_turnOn)
        {
            m_compPlayerSellJailCard = a_turnOn;
        }

        /// <summary>
        /// if the computer is selling a get out of jail free card
        /// </summary>
        /// <returns> True if selling jail card </returns>
        public bool GetCompSellJailCard()
        {
            return m_compPlayerSellJailCard;
        }

        /// <summary>
        /// Set true if a jail card being sold was bought by another player
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetJailCardBought(bool a_turnOn)
        {
            m_jailCardBought = a_turnOn;
        }

        /// <summary>
        /// If another player bought a jail card being sold
        /// </summary>
        /// <returns> True if player aggreed to buy a jail card from another player </returns>
        public bool GetJailCardBought()
        {
            return m_jailCardBought;
        }

        /// <summary>
        /// Set how much a player is selling a jail card for
        /// </summary>
        /// <param name="a_val"></param>
        public void SetCardPrice(int a_val)
        {
            m_cardSalePrice = a_val;
        }

        /// <summary>
        /// how much a player is selling a jail card for
        /// </summary>
        /// <returns> The price of the jail card being sold </returns>
        public int GetCardPrice()
        {
            return m_cardSalePrice;
        }

        /// <summary>
        /// The selected player who has the opportunity to purchase
        /// the jail card the current player is selling
        /// </summary>
        /// <param name="a_val"></param>
        /// <remarks> a_val is the index of the selected player in the player array </remarks>
        public void SetCardBuyer(int a_val)
        {
            m_cardBuyer = a_val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The index of the selected player who's buying a jail card </returns>
        public int GetCardBuyer()
        {
            return m_cardBuyer;
        }

        /// <summary>
        /// If current player is selling a jail card, set to true
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSellCardBool(bool a_turnOn)
        {
            m_sellCardBool = a_turnOn;
        }

        /// <summary>
        /// If current player is selling a jail card
        /// </summary>
        /// <returns> True if current player is selling jail card </returns>
        public bool GetSellCardBool()
        {
            return m_sellCardBool;
        }

        /// <summary>
        /// Set to true if current player has decide to sell a hotel
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSellHotel(bool a_turnOn)
        {
            m_sellHotel = a_turnOn;
        }

        /// <summary>
        /// if current player has decide to sell a hotel
        /// </summary>
        /// <returns> true if selling hotel </returns>
        public bool GetSellHotel()
        {
            return m_sellHotel;
        }

        /// <summary>
        /// Set true if current player selling a house
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSellHouse(bool a_turnOn)
        {
            m_sellHouse = a_turnOn;
        }

        /// <summary>
        /// if current player selling a house
        /// </summary>
        /// <returns> true if selling house </returns>
        public bool GetSellHouse()
        {
            return m_sellHouse;
        }

        /// <summary>
        /// Set true if current player buying a hotel
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetHotelPurchase(bool a_turnOn)
        {
            m_hotelPurchase = a_turnOn;
        }

        /// <summary>
        /// if current player buying a hotel
        /// </summary>
        /// <returns> true if buying a hotel</returns>
        public bool GetHotelPurchase()
        {
            return m_hotelPurchase;
        }

        /// <summary>
        /// Price player is selling proeprty for
        /// </summary>
        /// <param name="a_val"></param>
        public void SetSaleAmount(int a_val)
        {
            m_saleAmount = a_val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Price player is selling proeprty for </returns>
        public int GetSaleAmount()
        {
            return m_saleAmount;
        }

        /// <summary>
        /// Set the player the current player selected to sell to
        /// </summary>
        /// <param name="a_name"></param>
        public void SetSelectedPlayer(string a_name)
        {
            m_selectedPlayer = a_name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Which player the current player decided to sell to </returns>
        public string GetSelectedPlayer()
        {
            return m_selectedPlayer;
        }

        /// <summary>
        /// Set the player's turn in an auction
        /// </summary>
        /// <param name="a_name"></param>
        public void SetAuctionPlayer(string a_name)
        {
            m_auctionPlayer = a_name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Whose turn it is during an auction</returns>
        public string GetAuctionPlayer()
        {
            return m_auctionPlayer;
        }

        /// <summary>
        /// Set amount player bid during an auction
        /// </summary>
        /// <param name="a_val"></param>
        public void SetBidValue(int a_val)
        {
            m_bidValue = a_val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Amount of last bid in auction </returns>
        public int GetBidValue()
        {
            return m_bidValue;
        }

        /// <summary>
        /// Set property name being sold or selected
        /// </summary>
        /// <param name="a_name"></param>
        public void SetPropertyName(string a_name)
        {
            m_propertyName = a_name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Property name being sold or selected </returns>
        public string GetPropertyName()
        {
            return m_propertyName;
        }

        /// <summary>
        /// Set true if human player wants to sell property 
        /// to another player 
        /// Used by the UI
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSellPlayerProperty(bool a_turnOn)
        {
            m_sellPlayerProperty = a_turnOn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> True if human player desires to sell property to another player </returns>
        public bool GetSellPlayerProperty()
        {
            return m_sellPlayerProperty;
        }

        /// <summary>
        /// Set true if selected buyer decides to purchase
        /// human player's property
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSellPropertyBool(bool a_turnOn)
        {
            m_sellPropertyBool = a_turnOn;
        }

        /// <summary>
        /// If the selected buyer decided to buy the current player's property
        /// </summary>
        /// <returns> True if buying the property </returns>
        public bool GetSellPropertyBool()
        {
            return m_sellPropertyBool;
        }

        /// <summary>
        /// If the current player has to pay a tax/
        /// landed on a tax square
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetTaxBool(bool a_turnOn)
        {
            m_taxBool = a_turnOn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> True if current player has to pay a tax</returns>
        public bool GetTaxBool()
        {
            return m_taxBool;
        }

        /// <summary>
        /// Set true if current player is having a bankruptcy sale
        /// of all assets
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetBankruptcySaleBool(bool a_turnOn)
        {
            m_bankruptcySaleBool = a_turnOn;
        }

        /// <summary>
        /// If player is having a bankruptcy sale
        /// </summary>
        /// <returns> True if player is bankrupt and selling all assets </returns>
        public bool GetBankruptcySaleBool()
        {
            return m_bankruptcySaleBool;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> how many jail cards current player has </returns>
        public int GetCurrentPlayerNumJailCards()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetJailCards();
        }

        /// <summary>
        /// Current player pays a fee of a_howMuch
        /// which is deducted from the player's funds
        /// </summary>
        /// <param name="a_howMuch"></param>
        public void CurrentPlayerPaysMoney (int a_howMuch)
        {
            m_playerArray[GetCurrentPlayerPosition()].PayMoney(a_howMuch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Total num of houses on all properties the player owns </returns>
        public int GetCurrentPlayerTotalHouses ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].TotalNumHouses();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> List of properties that the player owns that have houses built </returns>
        public List<int> GetCurrentPlayerHouses()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetHouses();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns> Total num of hotels on all properties the player owns </returns>
        public int GetCurrentPlayerTotalHotels()
        {
            return m_playerArray[GetCurrentPlayerPosition()].TotalNumHotels();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Total num of properties the player owns  </returns>
        public int GetCurrentPlayerTotalProperties ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].TotalNumProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> List of all properties the player owns with hotels built  </returns>
        public List<int> GetCurrentPlayerHotels ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetHotels();
        }

        /// <summary>
        /// Add one more jail card to current player's hand
        /// </summary>
        public void AddCurrentPlayerJailCards ()
        {
            m_playerArray[GetCurrentPlayerPosition()].AddJailCards();
        }

        /// <summary>
        /// Set player's status to be in jail
        /// </summary>
        public void SendCurrentPlayerToJail ()
        {
            m_playerArray[GetCurrentPlayerPosition()].GoToJail();
        }

        /// <summary>
        /// Current player rolled the dice but didn't Get out of jail
        /// Increment number of turns player has been in jail
        /// </summary>
        public void CurrentPlayerAddJailTurns()
        {
            m_playerArray[GetCurrentPlayerPosition()].AddJailTurns();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> How many turns the player has been in jail </returns>
        public int GetCurrentPlayerNumJailTurns()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetJailTurns();
        }

        /// <summary>
        /// has the player been in jail for three turns?
        /// </summary>
        /// <returns> True if player has been in jail for 3 turns </returns>
        public bool CurrentPlayerJailedThreeTurns ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].ThreeJailTurns();
        }

        /// <summary>
        /// player leaves jail. Also reset jail turns
        /// </summary>
        public void CurrentPlayerLeavesJail()
        {
            m_playerArray[GetCurrentPlayerPosition()].LeaveJail();
            m_playerArray[GetCurrentPlayerPosition()].ResetJailTurns();
        }

        /// <summary>
        /// Current player uses a jail card
        /// Subtract one jail card from hand
        /// </summary>
        public void CurrentPlayerUsesJailCard()
        {
            m_playerArray[GetCurrentPlayerPosition()].SubtractJailCards();
        }

        /// <summary>
        /// The total amount of assets the player has
        /// </summary>
        /// <returns> The total amount of assets the player has </returns>
        public int GetCurrentPlayerTotalAssets()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetTotalAssets();
        }

        /// <summary>
        /// Get player funds using position in the player array
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <returns> How much $$$ the player has </returns>
        public int GetPlayerFunds (int a_whichPlayer)
        {
            return m_playerArray[a_whichPlayer].GetMoney();
        }

        /// <summary>
        /// Find out how much $$$ a player has using the
        /// player's name
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <returns> How much $$$ the player has </returns>
        public int GetPlayerFunds (string a_whichPlayer)
        {
            return m_playerArray[GetPlayerPosition(a_whichPlayer)].GetMoney();
        }

        /// <summary>
        /// Add one more jail card to a specified player's hand
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        public void AddPlayerJailCards (int a_whichPlayer)
        {
            m_playerArray[a_whichPlayer].AddJailCards();
        }

        /// <summary>
        /// A particular player pays money
        /// This uses the player's index in the player array
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_howMuch"></param>
        public void PlayerPaysMoney (int a_whichPlayer, int a_howMuch)
        {
            m_playerArray[a_whichPlayer].PayMoney(a_howMuch);
        }

        /// <summary>
        /// A particular player pays money
        /// This uses the player's name
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_howMuch"></param>
        public void PlayerPaysMoney(string a_whichPlayer, int a_howMuch)
        {
            m_playerArray[GetPlayerPosition(a_whichPlayer)].PayMoney(a_howMuch);
        }

        /// <summary>
        /// Set amount of outstanding fee player has to pay
        /// </summary>
        /// <param name="a_amount"></param>
        public void SetOutstandingPayment(int a_amount)
        {
            m_outstandingPayment = a_amount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>How much of the fee the player has to pay </returns>
        public int GetOutstandingPayment()
        {
            return m_outstandingPayment;
        }

        /// <summary>
        /// Player status set to bankrupt
        /// </summary>
        public void CurrentPlayerDeclaresBankrupt ()
        {
            m_playerArray[GetCurrentPlayerPosition()].DeclareBankrupt();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_boardSquareNum"></param>
        /// <returns> True if property is mortgaged </returns>
        public bool IsPropertyMortgaged (string a_whichPlayer, int a_boardSquareNum)
        {
            return m_playerArray[GetPlayerPosition(a_whichPlayer)].IsPropertyMortgaged(a_boardSquareNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns> what position the player is in the m_orderList </returns>
        public int GetPlayerPosition(string a_playerName)
        {
            return m_orderList.IndexOf(a_playerName);
        }

        /// <summary>
        /// Get if player is human or computer player
        /// using player's name
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns> "human" or "computer" </returns>
        public string GetPlayerType(string a_playerName)
        {
            return m_playerArray[GetPlayerPosition(a_playerName)].GetPlayerType();
        }

        /// <summary>
        /// Get if player is human or computer player
        /// using player's position in the player array
        /// </summary>
        /// <param name="a_playerPosition"></param>
        /// <returns> "human" or "computer" </returns>
        public string GetPlayerType(int a_playerPosition)
        {
            return m_playerArray[a_playerPosition].GetPlayerType();
        }

        /// <summary>
        /// Add money to a specified player's funds
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <param name="a_amount"></param>
        public void AddPlayerFunds(string a_playerName, int a_amount)
        {
            m_playerArray[GetPlayerPosition(a_playerName)].AddMoney(a_amount);
        }

        /// <summary>
        /// if a particular property a player owns has houses built on it or not
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <param name="a_squareNum"></param>
        /// <returns> true if property hasn't had houses built on it </returns>
        public bool PlayerPropertyUnimproved (string a_playerName, int a_squareNum)
        {
            return m_playerArray[GetPlayerPosition(a_playerName)].UnimprovedProperty(a_squareNum);
        }

        /// <summary>
        /// if a particular property current player owns has houses built on it or not
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> true if property hasn't had houses built on it </returns>
        public bool CurrentPlayerPropertyUnimproved  (int a_squareNum)
        {
            return m_playerArray[GetCurrentPlayerPosition()].UnimprovedProperty(a_squareNum);
        }
        /// <summary>
        /// Get the number of houses built on a property the player owns
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <param name="a_squareNum"></param>
        /// <returns> num houses built on a property the player owns </returns>
        public int GetNumHouses (string a_playerName, int a_squareNum)
        {
            return m_playerArray[GetPlayerPosition(a_playerName)].GetNumHouses(a_squareNum);
        }

        /// <summary>
        /// Get the number of houses built on a property the current player owns
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>num houses built on a property the player owns </returns>
        public int GetCurrentPlayerNumHouses (int a_squareNum)
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetNumHouses(a_squareNum);
        }

        /// <summary>
        /// Actions taken when current player buys a property
        /// </summary>
        /// <param name="a_propertyCost"></param>
        public void CurrentPlayerBuysProperty (int a_propertyCost)
        {
            //subtract money
            CurrentPlayerPaysMoney(a_propertyCost);
            //Add property value to player's Assets
            m_playerArray[GetCurrentPlayerPosition()].AddAssets(a_propertyCost);
            //Add property to player's list of bought properties
            m_playerArray[GetCurrentPlayerPosition()].AddProperty(
                m_playerArray[GetCurrentPlayerPosition()].GetSquare());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> True if particular property is mortgaged </returns>
        public bool IsCurrentPlayerPropertyMortgaged (int a_squareNum)
        {
            return m_playerArray[GetCurrentPlayerPosition()].IsPropertyMortgaged(a_squareNum);
        }

        /// <summary>
        /// Get player bankruptcy status using player's name
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns> True if player is bankrupt </returns>
        public bool IsPlayerBankrupt (string a_playerName)
        {
            return m_playerArray[GetPlayerPosition(a_playerName)].IsBankrupt();
        }

        /// <summary>
        /// Get player bankruptcy status using player's position in the player array
        /// </summary>
        /// <param name="a_playerPosition"></param>
        /// <returns> True if player is bankrupt </returns>
        public bool IsPlayerBankrupt(int a_playerPosition)
        {
            return m_playerArray[a_playerPosition].IsBankrupt();
        }

        /// <summary>
        /// the player bid during an auction
        /// </summary>
        /// <param name="a_playerName"></param>
        public void TurnOffPlayerAuctionPass (string a_playerName)
        {
            m_playerArray[GetPlayerPosition(a_playerName)].TurnOffAuctionPass();
        }

        /// <summary>
        /// the player passed during an auction 
        /// </summary>
        /// <param name="a_playerName"></param>
        public void TurnOnPlayerAuctionPass(string a_playerName)
        {
            m_playerArray[GetPlayerPosition(a_playerName)].TurnOnAuctionPass();
        }

        /// <summary>
        /// If the player passed during their turn during an auction
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <returns> True if player passed </returns>
        public bool GetPlayerAuctionPass (int a_whichPlayer)
        {
            return m_playerArray[a_whichPlayer].GetAuctionPass();
        }

        /// <summary>
        /// How many players aren't bankrupt
        /// </summary>
        /// <returns> Number of not-bankrupt players </returns>
        public int NotBankruptPlayers()
        {
            int bankruptCounter = 0;
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                if (m_playerArray[i].IsBankrupt())
                {
                    bankruptCounter++;
                }
            }
            int numPlayers = GetNumPlayers() - bankruptCounter;
            return numPlayers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> num of players in the game </returns>
        public int GetNumPlayers()
        {
            return m_players;
        }

        /// <summary>
        /// What happens when a player wins a property auction
        /// </summary>
        /// <param name="a_winnerName"></param>
        /// <param name="a_bidValue"></param>
        /// <param name="a_auctionSquareNum"></param>
        public void PlayerWinsPropertyAuction (string a_winnerName, int a_bidValue, int a_auctionSquareNum)
        {
            PlayerPaysMoney(a_winnerName, a_bidValue);
            //Add property to players' list
            m_playerArray[GetPlayerPosition(a_winnerName)].AddProperty(a_auctionSquareNum);
        }

        /// <summary>
        /// What happens when a player wins a card auction
        /// </summary>
        /// <param name="a_winnerName"></param>
        /// <param name="a_bidValue"></param>
        public void PlayerWinsCardAuction(string a_winnerName, int a_bidValue)
        {
            PlayerPaysMoney(a_winnerName, a_bidValue);
            m_playerArray[GetPlayerPosition(a_winnerName)].AddJailCards();
            m_playerArray[GetCurrentPlayerPosition()].SubtractJailCards();
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(a_bidValue);
        }

        /// <summary>
        /// Add a_amount to player's assets
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_amount"></param>
        public void PlayerAddsAssets(string a_whichPlayer, int a_amount)
        {
            m_playerArray[GetPlayerPosition(a_whichPlayer)].AddAssets(a_amount);
        }

        /// <summary>
        /// Subtract a_amount from player's assets
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_amount"></param>
        public void PlayerSubtractsAssets (string a_whichPlayer, int a_amount)
        {
            m_playerArray[GetPlayerPosition(a_whichPlayer)].SubtractAssets(a_amount);
        }

        /// <summary>
        /// Subtract a_amount from current player's assets
        /// </summary>
        /// <param name="a_amount"></param>
        public void CurrentPlayerSubstractsAssets (int a_amount)
        {
            m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_amount);
        }

        /// <summary>
        /// When a player buys a mortgaged property from another player
        /// </summary>
        /// <param name="a_buyingPlayer"></param>
        /// <param name="a_auctionPropertySquare"></param>
        /// <param name="a_mortgageAmount"></param>
        public void MortgagedPropertySale (string a_buyingPlayer, 
            int a_auctionPropertySquare, int a_mortgageAmount)
        {
            //Add property to Assets of buying player
            PlayerAddsAssets(a_buyingPlayer, a_mortgageAmount);
            //Add mortgage to buying player
            MortgageProperty(a_buyingPlayer, a_auctionPropertySquare);
            //Subtract mortgage from selling player
            UnmortgageProperty(m_currentPlayerName, a_auctionPropertySquare);
            //Subtract mortgage value from Assets of selling player
            PlayerSubtractsAssets(m_currentPlayerName, a_mortgageAmount);
        }

        /// <summary>
        /// When a player buys a mortgaged property from a bankrupted player
        /// </summary>
        /// <param name="a_buyingPlayer"></param>
        /// <param name="a_auctionPropertySquare"></param>
        /// <param name="a_mortgageAmount"></param>
        public void BankruptMortgagedPropertySale(string a_buyingPlayer,
            int a_auctionPropertySquare, int a_mortgageAmount)
        {
            //Add property to Assets of buying player
            PlayerAddsAssets(a_buyingPlayer, a_mortgageAmount);
            //Add mortgage to buying player
            MortgageProperty(a_buyingPlayer, a_auctionPropertySquare);
        }

        /// <summary>
        /// Mortgage a particular player's property
        /// Only used in Controller class
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_squareNum"></param>
        public void MortgageProperty(string a_whichPlayer, int a_squareNum)
        {
            m_playerArray[GetPlayerPosition(a_whichPlayer)].MortgageProperty(a_squareNum);
        }

        /// <summary>
        /// When current player mortgages a property
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_mortgageAmount"></param>
        /// <param name="a_propertyCost"></param>
        public void CurrentPlayerMortgageProperty (int a_squareNum, int a_mortgageAmount, int a_propertyCost)
        {
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(a_mortgageAmount);
            m_playerArray[GetCurrentPlayerPosition()].MortgageProperty(a_squareNum);
            m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_propertyCost);
            m_playerArray[GetCurrentPlayerPosition()].AddAssets(a_mortgageAmount);
        }

        /// <summary>
        /// Unmortgage a particular player's property
        /// </summary>
        /// <param name="a_whichPlayer"></param>
        /// <param name="a_squareNum"></param>
        public void UnmortgageProperty (string a_whichPlayer, int a_squareNum)
        {
            m_playerArray[GetPlayerPosition(a_whichPlayer)].UnmortgageProperty(a_squareNum);
        }

        /// <summary>
        /// Unmortgage current player's property
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_unmortgageCost"></param>
        /// <param name="a_mortgageAmount"></param>
        /// <param name="a_propertyCost"></param>
        public void CurrentPlayerUnmortgageProperty(int a_squareNum, int a_unmortgageCost, 
            int a_mortgageAmount, int a_propertyCost)
        {
            m_playerArray[GetCurrentPlayerPosition()].PayMoney(a_unmortgageCost);
            m_playerArray[GetCurrentPlayerPosition()].UnmortgageProperty(a_squareNum);
            m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_mortgageAmount);
            m_playerArray[GetCurrentPlayerPosition()].AddAssets(a_propertyCost);
        }

        /// <summary>
        /// What happens when current player buys a house
        /// </summary>
        /// <param name="a_houseCost"></param>
        /// <param name="a_squareNum"></param>
        public void CurrentPlayerBoughtHouse (int a_houseCost, int a_squareNum)
        {
            m_playerArray[GetCurrentPlayerPosition()].AddAssets(a_houseCost);
            m_playerArray[GetCurrentPlayerPosition()].PayMoney(a_houseCost);
            m_playerArray[GetCurrentPlayerPosition()].AddNewHouse(a_squareNum);
        }

        /// <summary>
        /// What happens when current player buys a house
        /// </summary>
        /// <param name="a_hotelCost"></param>
        /// <param name="a_squareNum"></param>
        public void CurrentPlayerBoughtHotel(int a_hotelCost, int a_squareNum)
        {
            m_playerArray[GetCurrentPlayerPosition()].AddAssets(a_hotelCost);
            m_playerArray[GetCurrentPlayerPosition()].PayMoney(a_hotelCost);
            m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(4 * a_hotelCost);
            m_playerArray[GetCurrentPlayerPosition()].AddHotel(a_squareNum);
        }

        /// <summary>
        /// What happens when current player sells a house
        /// </summary>
        /// <param name="a_sellPrice"></param>
        /// <param name="a_squareNum"></param>
        /// <param name="a_houseCost"></param>
        public void CurrentPlayerSoldHouse (int a_sellPrice, int a_squareNum, int a_houseCost)
        {
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(a_sellPrice);
            m_playerArray[GetCurrentPlayerPosition()].RemoveHouse(a_squareNum);
            m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_houseCost);
        }

        /// <summary>
        /// What happens when current player sells a hotel
        /// </summary>
        /// <param name="a_sellPrice"></param>
        /// <param name="a_squareNum"></param>
        /// <param name="a_hotelCost"></param>
        public void CurrentPlayerSoldHotel(int a_sellPrice, int a_squareNum, int a_hotelCost)
        {
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(a_sellPrice);
            m_playerArray[GetCurrentPlayerPosition()].RemoveHotel(a_squareNum);
            m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_hotelCost); //hotel value
            m_playerArray[GetCurrentPlayerPosition()].AddAssets(4 * a_hotelCost); //value of houses gained
        }

        /// <summary>
        /// Remove a particular property from current player's
        /// list of owned properties
        /// </summary>
        /// <param name="a_squareNum"></param>
        public void RemoveCurrentPlayerProperty (int a_squareNum)
        {
            m_playerArray[GetCurrentPlayerPosition()].RemoveProperty(a_squareNum);
        }

        /// <summary>
        /// At the end of an auction
        /// Reset all the player's auction passes
        /// </summary>
        /// <param name="a_auctionPlayer"></param>
        public void ResetPlayersAuctionPass (string a_auctionPlayer)
        {
            //reSet all players' auction pass booleans
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                a_auctionPlayer = WhoseTurn(a_auctionPlayer);
                m_playerArray[GetPlayerPosition(a_auctionPlayer)].TurnOffAuctionPass();
            }
        }

        /// <summary>
        /// Whose turn is it in an auction, this wraps around
        /// </summary>
        /// <param name="a_lastPlayer"></param>
        /// <returns> name of the next player in the auction</returns>
        public string WhoseTurn(string a_lastPlayer)
        {
            int index = m_orderList.IndexOf(a_lastPlayer);
            if (index == (GetNumPlayers() - 1))
            {
                return GetPlayerNameInOrderList(0);
            }
            else
            {
                return GetPlayerNameInOrderList(index + 1);
            }
        }

        /// <summary>
        /// Set the current player to the next player whose turn it is in the game
        /// </summary>
        public void SetNextPlayer()
        {
            SetCurrentPlayer(NextPlayerName());
        }

        /// <summary>
        /// Next player after the current one, this wraps around
        /// </summary>
        /// <returns> Name of next player in the orderList</returns>
        public string NextPlayerName()
        {
            if (m_currentPlayerPosition == (GetNumPlayers() - 1))
            {
                return GetPlayerNameInOrderList(0);
            }
            else
            {
                return GetPlayerNameInOrderList(m_currentPlayerPosition + 1);
            }
        }

        /// <summary>
        /// If the game is over and all but one player is bankrupt
        /// </summary>
        /// <returns> True if game is over </returns>
        public bool CheckIfGameover()
        {
            int bankruptCounter = 0;
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                if (m_playerArray[i].IsBankrupt())
                {
                    bankruptCounter++;
                }
            }
            //if all players but one are bankrupt
            if (bankruptCounter == (GetNumPlayers() - 1))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns></returns>
        public List<int> GetPlayerPropertiesList(string a_playerName)
        {
            return m_playerArray[GetPlayerPosition(a_playerName)].GetPropertiesList(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> List of current player's owned properties </returns>
        public List<int> GetCurrentPlayerPropertiesList()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetPropertiesList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> List of current player's unmortgaged properties </returns>
        public List<int> GetCurrentPlayerUnmortgagedProperties()
        {
            return m_playerArray[GetCurrentPlayerPosition()].UnmortgagedProperties();
        }

        /// <summary>
        /// If player owns any unmortgaged properties
        /// </summary>
        /// <returns> True if mortgages are available for current player </returns>
        public bool CurrentPlayerMortgagesAvailable ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].MortgagesAvailable();
        }

        /// <summary>
        /// If player in jail or not
        /// </summary>
        /// <returns> True if player is in jail </returns>
        public bool GetCurrentPlayerJailStatus ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].JailStatus();
        }

        /// <summary>
        /// Current player's jail cards are set to 0 after bankruptcy
        /// </summary>
        public void ResetCurrentPlayerJailCards()
        {
            m_playerArray[GetCurrentPlayerPosition()].SetJailCards(0);
        }

        /// <summary>
        /// if current player's move goes past Go
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_diceSum"></param>
        /// <returns> True if move goes past Go </returns>
        public bool WrapAround(int a_squareNum, int a_diceSum)
        {
            if ((a_squareNum + a_diceSum) > 39)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// The player's position after wrapping around go
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_diceSum"></param>
        /// <returns> the correct square number when wrapping around Go </returns>
        public int WrapSquare(int a_squareNum, int a_diceSum)
        {
            return ((a_squareNum + a_diceSum) - 40);
        }

        /// <summary>
        /// collect $200 from passing go
        /// </summary>
        public void CurrentPlayerPassedGo()
        {
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(200);
        }

        /// <summary>
        /// Set on or off if player has to
        /// pay special rent amount because of 
        /// drawing certain chance/community chest cards
        /// </summary>
        /// <param name="a_turnOn"></param>
        public void SetSpecialRentPay (bool a_turnOn)
        {
            m_specialRentPay = a_turnOn;
        }

        /// <summary>
        /// if player has to
        /// pay special rent amount because of 
        /// drawing certain chance/community chest cards
        /// </summary>
        /// <returns></returns>
        public bool GetSpecialRentPay()
        {
            return m_specialRentPay;
        }

        /// <summary>
        /// If current player has declared bankrupty
        /// </summary>
        public void LiquidateCurrentPlayerAssets()
        {
            m_playerArray[GetCurrentPlayerPosition()].SetMoney(0);
            m_playerArray[GetCurrentPlayerPosition()].SetAssets(0);
            m_playerArray[GetCurrentPlayerPosition()].SetJailCards(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> List of current player's properties with houses/hotels built </returns>
        //<property square num, num of houses/hotels>
        public Dictionary<int, int> GetCurrentPlayerHousesHotels ()
        {
            return m_playerArray[GetCurrentPlayerPosition()].GetHouseHotels();
        }

        /// <summary>
        /// Check if current player has enough funds to pay off outstanding payment
        /// </summary>
        /// <param name="a_outstandingPayment"></param>
        /// <returns> True if player's funds enough to pay off amount </returns>
        public bool CurrentPlayerEnoughFunds (int a_outstandingPayment)
        {
            if (m_playerArray[GetCurrentPlayerPosition()].GetMoney() >= a_outstandingPayment) return true;

            return false;
        }

        /// <summary>
        /// In a classic game, if all other players have bankrupt
        /// </summary>
        /// <returns> Name of winner in a classic game</returns>
        public string GameWinnerName ()
        {
            string winnerName = "";
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                if (!m_playerArray[i].IsBankrupt())
                {
                    winnerName = m_playerArray[i].GetPlayerName();
                }
            }
            return winnerName;
        }

        /// <summary>
        /// output the total amount of assets each player has at the end of a time limit game
        /// </summary>
        /// <returns> List of all the players' total assets </returns>
        public string AllPlayersAssets ()
        {
            string playerStats = "The total assets of each player:" + Environment.NewLine;
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                playerStats += m_playerArray[i].GetPlayerName() + ": $" +
                    Convert.ToString(m_playerArray[i].GetTotalAssets()) + Environment.NewLine;            
            }

            return playerStats;
        }

        /// <summary>
        /// /If there's been a tie in a time limit game, how many players have tied 
        /// </summary>
        /// <returns> how many winners at end of game </returns>
        public int NumberOfWinners ()
        {
            int winnerPosition = 0;
            int winnerAssetValue = m_playerArray[0].GetTotalAssets();
            //Compare value of all players' assets, find the player with biggest
            for (int i = 1; i < GetNumPlayers(); i++)
            {
                if (m_playerArray[i].GetTotalAssets() > winnerAssetValue)
                {
                    winnerAssetValue = m_playerArray[i].GetTotalAssets();
                    winnerPosition = i;
                }
            }

            int numWinners = 1;
            //See if there are any ties
            for (int j = 0; j < GetNumPlayers(); j++)
            {
                if (m_playerArray[j].GetTotalAssets() == winnerAssetValue && j != winnerPosition)
                {
                    numWinners++;
                }
            }

            return numWinners;
        }

        /// <summary>
        /// if there's a tie, list the players who won
        /// </summary>
        /// <returns> list of winners in a time limit game </returns>
        public List<string> WinnerList ()
        {
            List<string> listOfWinners = new List<string>();
            int winnerAsSetValue = m_playerArray[GetPlayerPosition(TimeGameWinnerName())].GetTotalAssets();
            for (int i = 0; i < GetNumPlayers(); i++)
            {
                if (m_playerArray[i].GetTotalAssets() == winnerAsSetValue)
                {
                    listOfWinners.Add(m_playerArray[i].GetPlayerName() + "(" + 
                        GetPlayerType(m_playerArray[i].GetPlayerName()) + ")");
                }
            }

            return listOfWinners;
        }

        /// <summary>
        /// In time limit game, player with most assets at the end of the game wins
        /// </summary>
        /// <returns>Winner name in a time limit game</returns>
        public string TimeGameWinnerName ()
        {
            string winnerName = m_playerArray[0].GetPlayerName();
            int winnerAsSetValue = m_playerArray[0].GetTotalAssets();
            //Find player with most Assets
            for (int i = 1; i < GetNumPlayers(); i++)
            {
                if (m_playerArray[i].GetTotalAssets() > winnerAsSetValue)
                {
                    winnerAsSetValue = m_playerArray[i].GetTotalAssets();
                    winnerName = m_playerArray[i].GetPlayerName();
                }
            }

            return winnerName;
        }

        /// <summary>
        /// if 4 houses have been built on a property
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> true if 4 houses have been built </returns>
        public bool CurrentPlayerFourHousesBuilt (int a_squareNum)
        {
            return m_playerArray[GetCurrentPlayerPosition()].FourHouses(a_squareNum);
        }

        /// <summary>
        /// if hotel exists on a property
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> true if a hotel has been built </returns>
        public bool CurrentPlayerHotelBuilt (int a_squareNum)
        {
            return m_playerArray[GetCurrentPlayerPosition()].HotelExists(a_squareNum);
        }

        /// <summary>
        /// If a house being built/sold is an even build. Houses must be built 
        /// evenly on properties of the same color.
        /// </summary>
        /// <param name="otherSquareNums"></param>
        /// <param name="a_squareNum"></param>
        /// <param name="a_build"></param>
        /// <remarks> a_build = true if building, false if selling </remarks>
        /// <returns> true if it's an even build/sell </returns>    
        public bool CurrentPlayerIsEvenBuild (List<int> otherSquareNums, int a_squareNum, bool a_build)
        {
            return m_playerArray[GetCurrentPlayerPosition()].IsEvenBuild(otherSquareNums, a_squareNum, a_build);
        }

        /// <summary>
        /// which player is at the index in the order list
        /// </summary>
        /// <param name="a_index"></param>
        /// <returns> player name at particular index </returns>
        public string WhichPlayer(int a_index)
        {
            try
            {
                return GetPlayerNameInOrderList(a_index);
            }
            catch (IndexOutOfRangeException)
            {
                return "player not found";
            }
            catch (ArgumentOutOfRangeException)
            {
                return "player not found";
            }
        }

        /// <summary>
        /// Actions that happen when a player sells a jila card
        /// </summary>
        /// <param name="a_cardBuyer"></param>
        /// <param name="a_cardSalePrice"></param>
        public void CurrentPlayerSoldJailCard (int a_cardBuyer, int a_cardSalePrice)
        {
           CurrentPlayerUsesJailCard(); //subtract from player's hand
           AddCurrentPlayerFunds(a_cardSalePrice);
           AddPlayerJailCards(a_cardBuyer);
           PlayerPaysMoney(a_cardBuyer, a_cardSalePrice);
        }

        /// <summary>
        /// Actions taken when current player sells a property
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_saleAmount"></param>
        /// <param name="a_propertyCost"></param>
        /// <param name="a_mortgageAmount"></param>
        /// <param name="selectedPlayer"></param>        
        public void CurrentPlayerPropertySold (int a_squareNum, int a_saleAmount, int a_propertyCost, 
            int a_mortgageAmount, string selectedPlayer)
        {
            int index = GetPlayerPosition(selectedPlayer);
            //subtract funds from buying player
            m_playerArray[index].PayMoney(a_saleAmount);
            //Add funds to seller
            m_playerArray[GetCurrentPlayerPosition()].AddMoney(a_saleAmount);
            if (m_playerArray[GetCurrentPlayerPosition()].IsPropertyMortgaged(a_squareNum))
            {
                //Add property to Assets of buying player
                m_playerArray[index].AddAssets(a_mortgageAmount);
                //Add mortgage to buying player
                m_playerArray[index].MortgageProperty(a_squareNum);
                //Subtract mortgage from selling player
                m_playerArray[GetCurrentPlayerPosition()].UnmortgageProperty(a_squareNum);
                //Subtract mortgage value from Assets of selling player
                m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_mortgageAmount);
            }
            else
            {
                //Add property to Assets of buying player
                m_playerArray[index].AddAssets(a_propertyCost);
                //Subtract property from Assets of selling player
                m_playerArray[GetCurrentPlayerPosition()].SubtractAssets(a_propertyCost);
            }
            //remove property from selling player             
            m_playerArray[GetCurrentPlayerPosition()].RemoveProperty(a_squareNum);       
            //Add property to buying player
            m_playerArray[index].AddProperty(a_squareNum);
        }

        /// <summary>
        /// Used in Game's auction2 function to 
        /// determine the position of the last player in the
        /// player array.
        /// This accounts for bankrupt players
        /// </summary>
        /// <param name="m_currentPlayerPosition"></param>
        /// <returns> previous player position/index in the player array </returns>
        public int PreviousPlayerPosition(int m_currentPlayerPosition)
        {
            int prevPosition = SimplePrevPlayerPosition(m_currentPlayerPosition);
            
            while (m_playerArray[prevPosition].IsBankrupt())
            {
                prevPosition = SimplePrevPlayerPosition(prevPosition);
            }

            return prevPosition;
        }

        /// <summary>
        /// if there are no bankrupt players, get the previous player's position
        /// </summary>
        /// <param name="m_currentPlayerPosition"></param>
        /// <returns> previous player position/index in the player array </returns>
        public int SimplePrevPlayerPosition(int m_currentPlayerPosition)
        {
            if (m_currentPlayerPosition == 0)
            {
                return (GetNumPlayers() - 1);
            }
            else
            {
                return (m_currentPlayerPosition - 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_position"></param>
        /// <returns> which player is at what position in the player array/orderlist </returns>
        private string GetPlayerName(int a_position)
        {
            return Convert.ToString(m_orderList[a_position]);
        }
    }
}
