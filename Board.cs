/// <summary>
/// Holds the data for the game board: 
/// namely the properties, their associated prices and costs, the number of hotels/houses in rotation, 
/// the community chest/chance card decks, and if a particular property is owned by a player. 
/// Used by the Game class to get information about squares the player lands on on the board.  
/// </summary>
/// <author>Daisy Watson</author>
/// <date>5/15/20</date>

using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGame
{
    class Board
    {
        //List of purchased properties
        private Dictionary<int, string> m_occupiedProperty = new Dictionary<int, string>(); //square num, who owns it
        private int m_chestIndex = 0; //points to top of deck of community chest cards
        private int m_chanceIndex = 0;
        int[] m_shuffledCards = new int[16]; //shuffle the community chest/chance cards
        private int m_numHotels = 12;
        private int m_numHouses = 32;
        private Random m_rand = new Random();

        /// <summary>
        /// Initialize the community chest/chance cards decks
        /// Shuffle the decks
        /// </summary>
        public Board()
        {
            //start on Go, lower right square, which is square 0
            //40 squares total, can be in jail or just visiting 
            //Square #40 is in jail
            //0-40 square numbers range
            //player doesn't stay on Go to Jail Square

            SetCardShuffle();
            ShuffleCards();
        }

        /// <summary>
        /// Adds the community chest/chance cards to the decks
        /// Initialize the values in the array
        /// Shuffle cards
        /// </summary>
        public void SetCardShuffle()
        {
            for (int i = 0; i < 16; i++)
            {
                m_shuffledCards[i] = i;
            }
        }

        /// <summary>
        /// Shuffle the community chest/chance decks
        /// </summary>
        public void ShuffleCards()
        {
            m_shuffledCards = m_shuffledCards.OrderBy(x => m_rand.Next()).ToArray();
        }

        /// <summary>
        /// Set number of hotels on the board that
        /// are available/haven't been bought by a player
        /// </summary>
        /// <param name="a_num"></param>
        public void SetNumHotels(int a_num)
        {
            m_numHotels = a_num;
        }

        /// <summary>
        /// Set number of houses on the board that
        /// are available/haven't been bought by a player
        /// </summary>
        /// <param name="a_num"></param>
        public void SetNumHouses(int a_num)
        {
            m_numHouses = a_num;
        }

        /// <summary>
        /// Add a_num of hotels back to the board 
        /// </summary>
        /// <param name="a_num"></param>
        public void AddHotels (int a_num)
        {
            m_numHotels += a_num;
        }

        /// <summary>
        /// Subtract hotels purchased by a player
        /// </summary>
        /// <param name="a_minusNum"></param>
        public void SubtractHotels (int a_minusNum)
        {
            m_numHotels -= a_minusNum;
        }

        /// <summary>
        /// Add houses sold back to the board by the player
        /// </summary>
        /// <param name="a_num"></param>
        public void AddHouses (int a_num)
        {
            m_numHouses += a_num;
        }

        /// <summary>
        /// Subtract houses purchases by the player from the board
        /// </summary>
        /// <param name="a_num"></param>
        public void SubtractHouses (int a_num)
        {
            m_numHouses -= a_num;
        }

        /// <summary>
        /// If there are any houses that 
        /// are still available on the board
        /// that haven't been purchased by players
        /// </summary>
        /// <returns> true if house available to purchase </returns>
        public bool HouseAvailable ()
        {
            if (GetNumHouses() != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// How many houses are still available on the board
        /// that haven't been purchased by players
        /// </summary>
        /// <returns> num of houses on the board/available </returns>
        public int GetNumHouses()
        {
            return m_numHouses;
        }

        /// <summary>
        /// If there are hotels left on the board
        /// players can purchase
        /// </summary>
        /// <returns> true if hotels still available </returns>
        public bool HotelAvailable ()
        {
            if (GetNumHotels() != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Num hotels available to be purchased </returns>
        public int GetNumHotels()
        {
            return m_numHotels;
        }

        /// <summary>
        /// there must be 4 houses available to sell a hotel
        /// </summary>
        /// <returns> true if a hotel can be sold </returns>
        public bool HotelSellable ()
        {
            if (GetNumHouses() < 4)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// move used chance/community chest card to bottom of deck
        /// </summary>
        /// <param name="a_whichDeck"></param>
        public void SetCardIndex(string a_whichDeck)
        {
            if (a_whichDeck == "chest")
            {
                //move on to the next card
                m_chestIndex++;
                if (m_chestIndex == 16)
                {
                    //Wrap around if at bottom of deck
                    m_chestIndex = 0;
                }
            }
            else
            {
                m_chanceIndex++;
                if (m_chanceIndex == 16)
                {
                    m_chanceIndex = 0;
                }
            }
        }

        /// <summary>
        /// Each community chest/chance card has a corresponding number
        /// This is used by board class
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <returns> The community chest/chance card number </returns>
        private int GetCardNum(string a_whichDeck)
        {
            if (a_whichDeck == "chest")
            {
                return m_shuffledCards[m_chestIndex];
            }
            else
            {
                return m_shuffledCards[m_chanceIndex];
            }
        }

        /// <summary>
        /// If a player purchased a property,
        /// add it to the list of occupied(owned)
        /// properites on the board
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_playerName"></param>
        public void AddProperty(int a_squareNum, string a_playerName)
        {
            m_occupiedProperty.Add(a_squareNum, a_playerName);
        }

        /// <summary>
        /// If a property no longer has an owner 
        /// it's returned to the board
        /// </summary>
        /// <param name="a_squareNum"></param>
        public void RemoveProperty(int a_squareNum)
        {
            m_occupiedProperty.Remove(a_squareNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>How many properties have been bought by players </returns>
        public int NumOccupiedProperties()
        {
            return m_occupiedProperty.Count;
        }

        /// <summary>
        /// If a particular property is owned by a player
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> True if property is owned by a player </returns>
        public bool IsOccupied(int a_squareNum)
        {
            if (m_occupiedProperty.ContainsKey(a_squareNum))
            {
                return true;
            }

            return false;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>who owns an occupied property</returns>
        public string WhoOwns(int a_squareNum)
        {
            return m_occupiedProperty[a_squareNum];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> the name of a particular square </returns>
        public string GetSquareMessage(int a_squareNum)
        {
            string squareMessage = "unassigned";
            switch (a_squareNum)
            {
                case 0:
                    squareMessage = "Go";
                    break;
                case 1:
                    squareMessage = "Mediterranean Avenue";
                    break;
                case 2:
                    squareMessage = "Community Chest";
                    break;
                case 3:
                    squareMessage = "Baltic Avenue";
                    break;
                case 4:
                    squareMessage = "Income Tax";
                    break;
                case 5:
                    squareMessage = "Reading Railroad";
                    break;
                case 6:
                    squareMessage = "Oriental Avenue";
                    break;
                case 7:
                    squareMessage = "Chance";
                    break;
                case 8:
                    squareMessage = "Vermont Avenue";
                    break;
                case 9:
                    squareMessage = "Connecticut Avenue";
                    break;
                case 10:
                    squareMessage = "Jail";
                    break;
                case 11:
                    squareMessage = "St. Charles Place";
                    break;
                case 12:
                    squareMessage = "Electric Company";
                    break;
                case 13:
                    squareMessage = "States Avenue";
                    break;
                case 14:
                    squareMessage = "Virginia Avenue";
                    break;
                case 15:
                    squareMessage = "Pennsylvania Railroad";
                    break;
                case 16:
                    squareMessage = "St. James Place";
                    break;
                case 17:
                    squareMessage = "Community Chest";
                    break;
                case 18:
                    squareMessage = "Tennessee Avenue";
                    break;
                case 19:
                    squareMessage = "New York Avenue";
                    break;
                case 20:
                    squareMessage = "Free Parking";
                    break;
                case 21:
                    squareMessage = "Kentucky Avenue";
                    break;
                case 22:
                    squareMessage = "Chance";
                    break;
                case 23:
                    squareMessage = "Indiana Avenue";
                    break;
                case 24:
                    squareMessage = "Illinois Avenue";
                    break;
                case 25:
                    squareMessage = "B. & O. Railroad";
                    break;
                case 26:
                    squareMessage = "Atlantic Avenue";
                    break;
                case 27:
                    squareMessage = "Ventnor Avenue";
                    break;
                case 28:
                    squareMessage = "Water Works";
                    break;
                case 29:
                    squareMessage = "Marvin Gardens";
                    break;
                case 30:
                    squareMessage = "Go to Jail";
                    break;
                case 31:
                    squareMessage = "Pacific Avenue";
                    break;
                case 32:
                    squareMessage = "North Carolina Avenue";
                    break;
                case 33:
                    squareMessage = "Community Chest";
                    break;
                case 34:
                    squareMessage = "Pennsylvania Avenue";
                    break;
                case 35:
                    squareMessage = "Short Line";
                    break;
                case 36:
                    squareMessage = "Chance";
                    break;
                case 37:
                    squareMessage = "Park Place";
                    break;
                case 38:
                    squareMessage = "Luxury Tax";
                    break;
                case 39:
                    squareMessage = "Boardwalk";
                    break;
                default:
                    squareMessage = "Error";
                    break;
            }
            return squareMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <returns>the square number of a property </returns>
        public int GetSquareNum(string a_propertyName)
        {
            int squareNum;
            switch (a_propertyName)
            {
                case "Go":
                    squareNum = 0;
                    break;
                case "Mediterranean Avenue":
                    squareNum = 1;
                    break;
                case "Baltic Avenue":
                    squareNum = 3;
                    break;
                case "Income Tax":
                    squareNum = 4;
                    break;
                case "Reading Railroad":
                    squareNum = 5;
                    break;
                case "Oriental Avenue":
                    squareNum = 6;
                    break;
                case "Vermont Avenue":
                    squareNum = 8;
                    break;
                case "Connecticut Avenue":
                    squareNum = 9;
                    break;
                case "Jail":
                    squareNum = 10;
                    break;
                case "St. Charles Place":
                    squareNum = 11;
                    break;
                case "Electric Company":
                    squareNum = 12;
                    break;
                case "States Avenue":
                    squareNum = 13;
                    break;
                case "Virginia Avenue":
                    squareNum = 14;
                    break;
                case "Pennsylvania Railroad":
                    squareNum = 15;
                    break;
                case "St. James Place":
                    squareNum = 16;
                    break;
                case "Tennessee Avenue":
                    squareNum = 18;
                    break;
                case "New York Avenue":
                    squareNum = 19;
                    break;
                case "Free Parking":
                    squareNum = 20;
                    break;
                case "Kentucky Avenue":
                    squareNum = 21;
                    break;
                case "Indiana Avenue":
                    squareNum = 23;
                    break;
                case "Illinois Avenue":
                    squareNum = 24;
                    break;
                case "B. & O. Railroad":
                    squareNum = 25;
                    break;
                case "Atlantic Avenue":
                    squareNum = 26;
                    break;
                case "Ventnor Avenue":
                    squareNum = 27;
                    break;
                case "Water Works":
                    squareNum = 28;
                    break;
                case "Marvin Gardens":
                    squareNum = 29;
                    break;
                case "Go to Jail":
                    squareNum = 30;
                    break;
                case "Pacific Avenue":
                    squareNum = 31;
                    break;
                case "North Carolina Avenue":
                    squareNum = 32;
                    break;
                case "Pennsylvania Avenue":
                    squareNum = 34;
                    break;
                case "Short Line":
                    squareNum = 35;
                    break;
                case "Park Place":
                    squareNum = 37;
                    break;
                case "Luxury Tax":
                    squareNum = 38;
                    break;
                case "Boardwalk":
                    squareNum = 39;
                    break;
                default:
                    squareNum = 100;
                    break;
            }
            return squareNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> what color group a property is in</returns>
        public string GetColorGroup(int a_squareNum)
        {
            string colorGroup = "";

            if (a_squareNum == 1 || a_squareNum == 3)
            {
                colorGroup = "brown";
            }
            else if (a_squareNum == 5 || a_squareNum == 15 || a_squareNum == 25 || a_squareNum == 35)
            {
                colorGroup = "railroad";
            }
            else if (a_squareNum == 6 || a_squareNum == 8 || a_squareNum == 9)
            {
                colorGroup = "sky";
            }
            else if (a_squareNum == 11 || a_squareNum == 13 || a_squareNum == 14)
            {
                colorGroup = "pink";
            }
            else if (a_squareNum == 12 || a_squareNum == 28)
            {
                colorGroup = "utilities";
            }
            else if (a_squareNum == 16 || a_squareNum == 18 || a_squareNum == 19)
            {
                colorGroup = "orange";
            }
            else if (a_squareNum == 21 || a_squareNum == 23 || a_squareNum == 24)
            {
                colorGroup = "red";
            }
            else if (a_squareNum == 26 || a_squareNum == 27 || a_squareNum == 29)
            {
                colorGroup = "yellow";
            }
            else if (a_squareNum == 31 || a_squareNum == 32 || a_squareNum == 34)
            {
                colorGroup = "green";
            }
            else if (a_squareNum == 37 || a_squareNum == 39)
            {
                colorGroup = "blue";
            }

            return colorGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>List of the square numbers of all the properties in a color group</returns>
        public List <int> GetColorGroupSquareNums (int a_squareNum)
        {
            string colorGroup = GetColorGroup(a_squareNum);
            List<int> squareNums = new List<int>();

            if (colorGroup == "brown" )
            {
                squareNums.Add(1);
                squareNums.Add(3);
            }
            else if (colorGroup == "sky")
            {
                squareNums.Add(11);
                squareNums.Add(13);
                squareNums.Add(14);
            }
            else if (colorGroup == "pink") 
            {
                squareNums.Add(12);
                squareNums.Add(28);
            }
            else if (colorGroup == "orange")
            {
                squareNums.Add(16);
                squareNums.Add(18);
                squareNums.Add(19);
            }
            else if (colorGroup == "red") 
            {
                squareNums.Add(21);
                squareNums.Add(23);
                squareNums.Add(24);
            }
            else if (colorGroup == "yellow")
            {
                squareNums.Add(26);
                squareNums.Add(27);
                squareNums.Add(29);
            }
            else if (colorGroup == "green") 
            {
                squareNums.Add(31);
                squareNums.Add(32);
                squareNums.Add(34);
            }
            else if (colorGroup == "blue")
            {
                squareNums.Add(37);
                squareNums.Add(39);
            }

            return squareNums;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_colorGroup"></param>
        /// <returns>Amount a house costs of a particular color set</returns>
        public int GetHouseCost(string a_colorGroup)
        {
            int cost = 0;

            if (a_colorGroup == "brown" || a_colorGroup == "sky")
            {
                cost = 50;
            }
            else if (a_colorGroup == "pink" || a_colorGroup == "orange")
            {
                cost = 100;
            }
            else if (a_colorGroup == "red" || a_colorGroup == "yellow")
            {
                cost = 150;
            }
            else if (a_colorGroup == "green" || a_colorGroup == "blue")
            {
                cost = 200;
            }

            return cost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> Amount a house costs of a particular square </returns>
        public int GetHouseCost(int a_squareNum)
        {          
            string colorGroup = GetColorGroup(a_squareNum);
            int cost = 0;

            if (colorGroup == "brown" || colorGroup == "sky")
            {
                cost = 50;
            }
            else if (colorGroup == "pink" || colorGroup == "orange")
            {
                cost = 100;
            }
            else if (colorGroup == "red" || colorGroup == "yellow")
            {
                cost = 150;
            }
            else if (colorGroup == "green" || colorGroup == "blue")
            {
                cost = 200;
            }

            return cost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>Price of selling a house on a particular square</returns>
        public int GetSellHousePrice(int a_squareNum)
        {
            int houseCost = GetHouseCost(a_squareNum);

            return (houseCost / 2);
        }

        /// <summary>
        /// if a house can be built on a player's property
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>true if a house can be built</returns>
        public bool HouseBuildable (int a_squareNum)
        {
            if (AllColorsHeld(a_squareNum))
            {
                string colorGroup = GetColorGroup(a_squareNum);
                if (colorGroup != "railroad" && colorGroup != "utilities")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// check if all colors held by the same player
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> True if player owns all properties in a color set </returns>
        public bool AllColorsHeld(int a_squareNum)
        {
            string colorGroup = GetColorGroup(a_squareNum);
            if (colorGroup == "brown" && ColorGroupAllPropertiesOccupied("brown"))
            {
                if (WhoOwns(1) == WhoOwns(3)) return true;
            }
            if (colorGroup == "railroad" && ColorGroupAllPropertiesOccupied("railroad"))
            {
                if (WhoOwns(5) == WhoOwns(15) && WhoOwns(5) == WhoOwns(25)
                && WhoOwns(25) == WhoOwns(35))
                { return true; }                
            }
            if (colorGroup == "sky" && ColorGroupAllPropertiesOccupied("sky"))
            {
                if (WhoOwns(6) == WhoOwns(8) && WhoOwns(8) == WhoOwns(9)) return true; 
            }
            if (colorGroup == "pink" && ColorGroupAllPropertiesOccupied("pink"))
            {
                if (WhoOwns(11) == WhoOwns(13) && WhoOwns(13) == WhoOwns(14)) return true;
            }
            if (colorGroup == "utilities" && ColorGroupAllPropertiesOccupied("utilities"))
            {
                if (WhoOwns(12) == WhoOwns(28)) return true;
            }
            if (colorGroup == "orange" && ColorGroupAllPropertiesOccupied("orange"))
            {
                if (WhoOwns(16) == WhoOwns(18) && WhoOwns(18) == WhoOwns(19)) return true; 
            }
            if (colorGroup == "red" && ColorGroupAllPropertiesOccupied("red"))
            {
                if (WhoOwns(21) == WhoOwns(23) && WhoOwns(21) == WhoOwns(24)) return true; 
            }
            if (colorGroup == "yellow" && ColorGroupAllPropertiesOccupied("yellow"))
            {
                if (WhoOwns(26) == WhoOwns(27) && WhoOwns(27) == WhoOwns(29)) return true; 
            }
            if (colorGroup == "green" && ColorGroupAllPropertiesOccupied("green"))
            { 
                if (WhoOwns(31) == WhoOwns(32) && WhoOwns(31) == WhoOwns(34)) return true;                
            }
            if (colorGroup == "blue" && ColorGroupAllPropertiesOccupied("blue"))
            {
                if (WhoOwns(37) == WhoOwns(39)) return true; 
            }

            return false;
        }

        /// <summary>
        /// If all properties in a particular color group are owned or not
        /// </summary>
        /// <param name="a_colorGroup"></param>
        /// <returns> True if all properties in a color group have been bought </returns>
        public bool ColorGroupAllPropertiesOccupied (string a_colorGroup)
        {
            if (a_colorGroup == "brown" && IsOccupied(1) && IsOccupied(3))
            {
                return true; 
            }
            if (a_colorGroup == "railroad" && IsOccupied(5) && IsOccupied(15) 
                && IsOccupied(25) && IsOccupied(35))
            {              
                return true;               
            }
            if (a_colorGroup == "sky" && IsOccupied(6) && IsOccupied(8)  && IsOccupied(9))
            {
                return true;
            }
            if (a_colorGroup == "pink" && IsOccupied(11) && IsOccupied(13) && IsOccupied(14))
            {
                return true;
            }
            if (a_colorGroup == "utilities" && IsOccupied(12) && IsOccupied(28))
            {
                return true;
            }
            if (a_colorGroup == "orange" && IsOccupied(16) && IsOccupied(18) && IsOccupied(19))
            {
                return true;
            }
            if (a_colorGroup == "red" && IsOccupied(21) && IsOccupied(23) && IsOccupied(24))
            {
                return true;
            }
            if (a_colorGroup == "yellow" && IsOccupied(26) && IsOccupied(27) && IsOccupied(29))
            {
                return true;
            }
            if (a_colorGroup == "green" && IsOccupied(31) && IsOccupied(32) && IsOccupied(34))
            {
                return true;
            }
            if (a_colorGroup == "blue" && IsOccupied(37) && IsOccupied(39))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get how much it costs to buy a property 
        /// for its price listed on the board
        /// Using the property name
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <returns>The cost of a property </returns>
        public int GetPropertyCost (string a_propertyName)
        {
            return GetPropertyCost(GetSquareNum(a_propertyName));
        }

        /// <summary>
        /// /How much it costs to buy a property for its price listed on the board
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> property cost </returns>
        public int GetPropertyCost(int a_squareNum)
        {
            int cost = 0;

            if (a_squareNum == 1 || a_squareNum == 3)
            {
                cost = 60;
            }
            if (a_squareNum == 5 || a_squareNum == 15 || a_squareNum == 25 || 
                a_squareNum == 35 || a_squareNum == 19)
            {
                cost = 200;
            }
            if (a_squareNum == 6 || a_squareNum == 8)
            {
                cost = 100;
            }
            if (a_squareNum == 9)
            {
                cost = 120;
            }
            if (a_squareNum == 11 || a_squareNum == 13)
            {
                cost = 140;
            }
            if (a_squareNum == 12 || a_squareNum == 28)
            {
                cost = 150;
            }
            if (a_squareNum == 14)
            {
                cost = 160;
            }
            if (a_squareNum == 16 || a_squareNum == 18)
            {
                cost = 180;
            }
            if (a_squareNum == 21 || a_squareNum == 23)
            {
                cost = 220;
            }
            if (a_squareNum == 24)
            {
                cost = 240;
            }
            if (a_squareNum == 26 || a_squareNum == 27)
            {
                cost = 260;
            }
            if (a_squareNum == 29)
            {
                cost = 280;
            }
            if (a_squareNum == 31 || a_squareNum == 32)
            {
                cost = 300;
            }
            if (a_squareNum == 34)
            {
                cost = 320;
            }
            if (a_squareNum == 37)
            {
                cost = 350;
            }
            if (a_squareNum == 39)
            {
                cost = 400;
            }

            return cost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>mortgage amount of a particular property </returns>
        public int GetMortgageAmount (int a_squareNum)
        {
            int cost = GetPropertyCost(a_squareNum);

            return (cost/2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>unmortgage cost of a particular property </returns>
        public int GetUnmortgageCost (int a_squareNum)
        {
            int cost = GetMortgageAmount(a_squareNum);
            int tenPercent = cost / 10;

            if (cost % 10 != 0)
            {
                //round up
                cost++;
            }

            return (cost + tenPercent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_houses"></param>
        /// <param name="a_ownerName"></param>
        /// <returns>rent cost of a particular property </returns>
        public int GetRentCost(int a_squareNum, int a_houses, string a_ownerName)
        {
            int cost = 0;          

            if (a_squareNum == 1)
            {
                if (a_houses == 0)
                {
                    cost = 2;
                }
                if (a_houses == 1)
                {
                    cost = 10;
                }
                if (a_houses == 2)
                {
                    cost = 30;
                }
                if (a_houses == 3)
                {
                    cost = 90;
                }
                if (a_houses == 4)
                {
                    cost = 160;
                }
                if (a_houses == 5)
                {
                    cost = 250;
                }
            }
            if (a_squareNum == 3)
            {
                if (a_houses == 0)
                {
                    cost = 4;
                }
                if (a_houses == 1)
                {
                    cost = 20;
                }
                if (a_houses == 2)
                {
                    cost = 60;
                }
                if (a_houses == 3)
                {
                    cost = 180;
                }
                if (a_houses == 4)
                {
                    cost = 320;
                }
                if (a_houses == 5)
                {
                    cost = 450;
                }
            }
            if (a_squareNum == 5 || a_squareNum == 15 || a_squareNum == 25 || a_squareNum == 35)
            {
                cost = GetRailRoadCost(a_ownerName);
            }
            if (a_squareNum == 6 || a_squareNum == 8)
            {
                if (a_houses == 0)
                {
                    cost = 4;
                }
                if (a_houses == 1)
                {
                    cost = 30;
                }
                if (a_houses == 2)
                {
                    cost = 90;
                }
                if (a_houses == 3)
                {
                    cost = 270;
                }
                if (a_houses == 4)
                {
                    cost = 400;
                }
                if (a_houses == 5)
                {
                    cost = 550;
                }
            }
            if (a_squareNum == 9)
            {
                if (a_houses == 0)
                {
                    cost = 8;
                }
                if (a_houses == 1)
                {
                    cost = 40;
                }
                if (a_houses == 2)
                {
                    cost = 100;
                }
                if (a_houses == 3)
                {
                    cost = 300;
                }
                if (a_houses == 4)
                {
                    cost = 450;
                }
                if (a_houses == 5)
                {
                    cost = 600;
                }
            }
            if (a_squareNum == 11 || a_squareNum == 13)
            {
                if (a_houses == 0)
                {
                    cost = 10;
                }
                if (a_houses == 1)
                {
                    cost = 50;
                }
                if (a_houses == 2)
                {
                    cost = 150;
                }
                if (a_houses == 3)
                {
                    cost = 450;
                }
                if (a_houses == 4)
                {
                    cost = 625;
                }
                if (a_houses == 5)
                {
                    cost = 750;
                }
            }
            if (a_squareNum == 12 || a_squareNum == 28)
            {
                cost = GetUtilitiesCost(a_ownerName);
            }
            if (a_squareNum == 14)
            {
                if (a_houses == 0)
                {
                    cost = 12;
                }
                if (a_houses == 1)
                {
                    cost = 60;
                }
                if (a_houses == 2)
                {
                    cost = 180;
                }
                if (a_houses == 3)
                {
                    cost = 500;
                }
                if (a_houses == 4)
                {
                    cost = 700;
                }
                if (a_houses == 5)
                {
                    cost = 900;
                }
            }
            if (a_squareNum == 16 || a_squareNum == 18)
            {
                if (a_houses == 0)
                {
                    cost = 14;
                }
                if (a_houses == 1)
                {
                    cost = 70;
                }
                if (a_houses == 2)
                {
                    cost = 200;
                }
                if (a_houses == 3)
                {
                    cost = 550;
                }
                if (a_houses == 4)
                {
                    cost = 750;
                }
                if (a_houses == 5)
                {
                    cost = 950;
                }
            }
            if (a_squareNum == 19)
            {
                if (a_houses == 0)
                {
                    cost = 16;
                }
                if (a_houses == 1)
                {
                    cost = 80;
                }
                if (a_houses == 2)
                {
                    cost = 220;
                }
                if (a_houses == 3)
                {
                    cost = 600;
                }
                if (a_houses == 4)
                {
                    cost = 800;
                }
                if (a_houses == 5)
                {
                    cost = 1000;
                }
            }
            if (a_squareNum == 21 || a_squareNum == 23)
            {
                if (a_houses == 0)
                {
                    cost = 18;
                }
                if (a_houses == 1)
                {
                    cost = 90;
                }
                if (a_houses == 2)
                {
                    cost = 250;
                }
                if (a_houses == 3)
                {
                    cost = 700;
                }
                if (a_houses == 4)
                {
                    cost = 875;
                }
                if (a_houses == 5)
                {
                    cost = 1050;
                }
            }
            if (a_squareNum == 24)
            {
                if (a_houses == 0)
                {
                    cost = 20;
                }
                if (a_houses == 1)
                {
                    cost = 100;
                }
                if (a_houses == 2)
                {
                    cost = 300;
                }
                if (a_houses == 3)
                {
                    cost = 750;
                }
                if (a_houses == 4)
                {
                    cost = 925;
                }
                if (a_houses == 5)
                {
                    cost = 1100;
                }
            }
            if (a_squareNum == 26 || a_squareNum == 27)
            {
                if (a_houses == 0)
                {
                    cost = 22;
                }
                if (a_houses == 1)
                {
                    cost = 110;
                }
                if (a_houses == 2)
                {
                    cost = 330;
                }
                if (a_houses == 3)
                {
                    cost = 800;
                }
                if (a_houses == 4)
                {
                    cost = 975;
                }
                if (a_houses == 5)
                {
                    cost = 1150;
                }
            }
            if (a_squareNum == 29)
            {
                if (a_houses == 0)
                {
                    cost = 24;
                }
                if (a_houses == 1)
                {
                    cost = 120;
                }
                if (a_houses == 2)
                {
                    cost = 360;
                }
                if (a_houses == 3)
                {
                    cost = 850;
                }
                if (a_houses == 4)
                {
                    cost = 1025;
                }
                if (a_houses == 5)
                {
                    cost = 1200;
                }
            }
            if (a_squareNum == 31 || a_squareNum == 32)
            {
                if (a_houses == 0)
                {
                    cost = 26;
                }
                if (a_houses == 1)
                {
                    cost = 130;
                }
                if (a_houses == 2)
                {
                    cost = 390;
                }
                if (a_houses == 3)
                {
                    cost = 900;
                }
                if (a_houses == 4)
                {
                    cost = 1100;
                }
                if (a_houses == 5)
                {
                    cost = 1275;
                }
            }
            if (a_squareNum == 34)
            {
                if (a_houses == 0)
                {
                    cost = 28;
                }
                if (a_houses == 1)
                {
                    cost = 150;
                }
                if (a_houses == 2)
                {
                    cost = 450;
                }
                if (a_houses == 3)
                {
                    cost = 1000;
                }
                if (a_houses == 4)
                {
                    cost = 1200;
                }
                if (a_houses == 5)
                {
                    cost = 1400;
                }
            }
            if (a_squareNum == 37)
            {
                if (a_houses == 0)
                {
                    cost = 35;
                }
                if (a_houses == 1)
                {
                    cost = 175;
                }
                if (a_houses == 2)
                {
                    cost = 500;
                }
                if (a_houses == 3)
                {
                    cost = 1100;
                }
                if (a_houses == 4)
                {
                    cost = 1300;
                }
                if (a_houses == 5)
                {
                    cost = 1500;
                }
            }
            if (a_squareNum == 39)
            {
                if (a_houses == 0)
                {
                    cost = 50;
                }
                if (a_houses == 1)
                {
                    cost = 200;
                }
                if (a_houses == 2)
                {
                    cost = 600;
                }
                if (a_houses == 3)
                {
                    cost = 1400;
                }
                if (a_houses == 4)
                {
                    cost = 1700;
                }
                if (a_houses == 5)
                {
                    cost = 2000;
                }
            }

            if (a_houses == 0 && AllColorsHeld(a_squareNum))
            {
                cost = cost * 2;
            }
            return cost;
        }

        /// <summary>
        /// a_playername is the name of the owner of the railroad
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns>How much a player pays for landing on a railroad</returns>
        public int GetRailRoadCost(string a_playerName)
        {
            int cost = 25;
            int numRailRoads = 0;

            if (IsOccupied(5))
            {
                if (PropertyOwnedBy(5, a_playerName))
                {
                    numRailRoads++;
                }
            }
            if (IsOccupied(15))
            {
                if (PropertyOwnedBy(15, a_playerName))
                {
                    numRailRoads++;
                }
            }
            if (IsOccupied(25))
            {
                if (PropertyOwnedBy(25, a_playerName))
                {
                    numRailRoads++;
                }
            }
            if (IsOccupied(35) == true)
            {
                if (PropertyOwnedBy(35, a_playerName))
                {
                    numRailRoads++;
                }
            }

            if (numRailRoads == 2)
            {
                cost = 50;
            }
            if (numRailRoads == 3)
            {
                cost = 100;
            }
            if (numRailRoads == 4)
            {
                cost = 200;
            }

            return cost;
        }

        /// <summary>
        /// If a property is owned by a particular player
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <param name="a_playerName"></param>
        /// <returns> True if property owned by a_playerName </returns>
        public bool PropertyOwnedBy(int a_squareNum, string a_playerName)
        {
            if (WhoOwns(a_squareNum) == a_playerName) return true;

            return false;
        }

        /// <summary>
        /// a_playerName is the name of the owner of the utility 
        /// </summary>
        /// <param name="a_playerName"></param>
        /// <returns> How much a player pays if landing on a utilities square </returns>
        public int GetUtilitiesCost(string a_playerName)
        {
            int[] rndArr = new int[2];
            for (int i = 0; i < 2; i++)
            {
                rndArr[i] = m_rand.Next(1, 6);
            }
            int cost = rndArr[0] + rndArr[1];

            int numUtilities = 0;
            if (IsOccupied(12))
            {
                if (PropertyOwnedBy(12, a_playerName))
                {
                    numUtilities++;
                }
            }
            if (IsOccupied(28))
            {
                if (PropertyOwnedBy(28, a_playerName))
                {
                    numUtilities++;
                }
            }

            //1 owned: 4 times dice
            if (numUtilities == 1)
            {
                cost = cost * 4;
            }
            else
            {
                //2 owned: 10 times dice
                cost = cost * 10;
            }
            
            return cost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The message of the community chest card drawn </returns>
        public string GetChestMessage()
        {
            string message = "";

            switch (GetCardNum("chest"))
            {
                case 0:
                    message = "Holiday fund matures. Collect $100.";
                    break;
                case 1:
                    message = "Life insurance matures. Collect $100.";
                    break;
                case 2:
                    message = "School fees. Pay $50.";
                    break;
                case 3:
                    message = "Income tax refund. Collect $20.";
                    break;
                case 4:
                    message = "Hospital fees. Pay $100.";
                    break;
                case 5:
                    message = "Go to jail. Go directly to jail. Do not pass Go. Do not collect $200.";
                    break;
                case 6:
                    message = "Collect $25. Consultancy fee.";
                    break;
                case 7:
                    message = "It's your birthday. Collect $10 from each player.";
                    break;
                case 8:
                    message = "You inherit $100.";
                    break;
                case 9:
                    message = "From sale of stock, you get $50.";
                    break;
                case 10:
                    message = "Get out of jail free. This card may be kept until needed, traded, or sold.";
                    break;
                case 11:
                    message = "Doctor's fees. Pay $50.";
                    break;
                case 12:
                    message = "You have won second prize in a beauty contest. Collect $10.";
                    break;
                case 13:
                    message = "You are assessed for street repairs. Pay $40 per house and $115 per hotel you own.";
                    break;
                case 14:
                    message = "Advance to Go. Collect $200.";
                    break;
                case 15:
                    message = "Bank error in your favor. Collect $200.";
                    break;
                default:
                    message = "Error";
                    break;
            }
            return message;
        }


        /// <summary>
        /// If a community chest card drawn 
        /// will make the player move somehwhere else on the board
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <remarks> a_whichDeck is either "chest" or "chance" </remarks>
        /// <returns> true if the player has to move </returns>       
        public bool ChestChanceMovement(string a_whichDeck)
        {
            bool moving = false;
            if (a_whichDeck == "chest")
            {
                if (GetCardNum("chest") == 5 || GetCardNum("chest") == 14)
                {
                    moving = true;
                }
            }
            else
            {
                int chanceCard = GetCardNum("chance");
                if (chanceCard != 1 && chanceCard != 2 && chanceCard != 4 &&
                    chanceCard != 6 && chanceCard != 9 && chanceCard != 12)
                {
                    moving = true;
                }
            }

            return moving;
        }

        /// <summary>
        /// Where a player must move if drawing a particular chest/chance card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_squareNum"></param>
        /// <remarks> a_squareNum is where the player currently is on the board </remarks>
        /// <returns> The square number where the player must move to </returns>
        public int ChestChanceMoveSquares(string a_whichDeck, int a_squareNum)
        {
            int finalSquare = a_squareNum;

            if (a_whichDeck == "chest")
            {
                if (GetCardNum("chest") == 5)
                {
                    //go directly to jail
                    finalSquare = 40;
                }
                if (GetCardNum("chest") == 14)
                {
                    //advance to go
                    finalSquare = 0;
                }
            }

            if (a_whichDeck == "chance")
            {
                finalSquare = ChanceMoveSquares(a_squareNum);
            }

            return finalSquare;
        }

        /// <summary>
        /// Where a player must move if drawing a particular chance card
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns> The square number where the player must move to </returns>
        private int ChanceMoveSquares (int a_squareNum)
        {
            int finalSquare = a_squareNum;

            if (GetCardNum("chance") == 0)
            {
                finalSquare = 0;
            }
            if (GetCardNum("chance") == 3)
            {
                finalSquare = 11;
            }
            if (GetCardNum("chance") == 5)
            {
                //go back 3 spaces
                finalSquare = (a_squareNum - 3);
            }
            if (GetCardNum("chance") == 7 || GetCardNum("chance") == 8)
            {
                //advance to next/nearest railroad
                if (a_squareNum == 7)
                {
                    finalSquare = 15;
                }
                if (a_squareNum == 22)
                {
                    finalSquare = 25;
                }
                if (a_squareNum == 36)
                {
                    finalSquare = 35;
                }
            }
            if (GetCardNum("chance") == 10)
            {
                finalSquare = 39;
            }
            if (GetCardNum("chance") == 11)
            {
                finalSquare = 5;
            }
            if (GetCardNum("chance") == 13)
            {
                finalSquare = 24;
            }
            if (GetCardNum("chance") == 14)
            {
                //advance to nearest utility
                if (a_squareNum == 7 || a_squareNum == 36)
                {
                    finalSquare = 12;
                }
                if (a_squareNum == 22)
                {
                    finalSquare = 28;
                }
            }
            if (GetCardNum("chance") == 15)
            {
                //go to jail
                finalSquare = 40;
            }

            return finalSquare;
        }

        /// <summary>
        /// Whether or not going to jail from drawing a chest/chance card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <returns>true if going to jail </returns>      
        public bool ChestChanceJail(string a_whichDeck)
        {
            //Whether or not going to jail
            if (a_whichDeck == "chest" && GetCardNum("chest") == 5)
            {
                return true;
            }
            if (a_whichDeck == "chance" && GetCardNum("chance") == 15)
            {
                return true;
            }
          
            return false;
        }

        /// <summary>
        /// Whether or not passing go from drawing a chest/chance card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_formerSquare"></param>
        /// <returns> true if passing go </returns>
        public bool ChestChancePassGo(string a_whichDeck, int a_formerSquare)
        {
            if (a_whichDeck == "chest")
            {
                //only pass go when going to jail or landing on Go square
                return false;
            }
            if (a_whichDeck == "chance")
            {
                //card no. 7, 8, 14 can advance past go
                //but do not say to collect $200
                if (GetCardNum("chance") == 3 && a_formerSquare > 11)
                {
                    return true;
                }
                if (GetCardNum("chance") == 11 && a_formerSquare > 5)
                {
                    return true;
                }
                if (GetCardNum("chance") == 13 && a_formerSquare > 24)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// If player collects more money from event in chest/chance card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <returns>true if player collecting money </returns>
        public bool ChestChanceCollectMoney(string a_whichDeck)
        {
            bool collectMoney = false;

            if (a_whichDeck == "chest")
            {
                int chestNum = GetCardNum("chest");
                if (chestNum == 0 || chestNum == 1 || chestNum == 3
                    || chestNum == 6 || chestNum == 7 || chestNum == 8 ||
                    chestNum == 9 || chestNum == 12 || chestNum == 15)
                {
                    collectMoney = true;
                }
            }
            if (a_whichDeck == "chance")
            {
                if (GetCardNum("chance") == 9 || GetCardNum("chance") == 12)
                {
                    collectMoney = true;
                }
            }

            return collectMoney;
        }

        /// <summary>
        /// How much money the player collects from event in chest/chance card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_numPlayers"></param>
        /// <remarks> a_numPlayers is the number of players in the game </remarks>
        /// <returns> amount of money player is collecting </returns>
        public int ChestChanceGetMoney(string a_whichDeck, int a_numPlayers)
        {
            int howMuch = 0;
            if (a_whichDeck == "chest")
            {
                if (GetCardNum("chest") == 0 || GetCardNum("chest") == 1 
                    || GetCardNum("chest") == 8)
                {
                    howMuch = 100;
                }
                if (GetCardNum("chest") == 3)
                {
                    howMuch = 20;
                }
                if (GetCardNum("chest") == 6)
                {
                    howMuch = 25;
                }
                if (GetCardNum("chest") == 7)
                {
                    howMuch = (10 * (a_numPlayers - 1)); //don't include the player receiving the money
                }
                if (GetCardNum("chest") == 9)
                {
                    howMuch = 50;
                }
                if (GetCardNum("chest") == 12)
                {
                    howMuch = 10;
                }
                if (GetCardNum("chest") == 15)
                {
                    howMuch = 200;
                }
            }
            if (a_whichDeck == "chance")
            {
                if (GetCardNum("chance") == 9)
                {
                    howMuch = 50;
                }
                if (GetCardNum("chance") == 12)
                {
                    howMuch = 150;
                }
            }

            return howMuch;
        }

        /// <summary>
        /// If the player has to pay a fee
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <returns> true if player has to pay a fee </returns>
        public bool ChestChancePayMoney(string a_whichDeck)
        {
            bool payMoney = false;
            if (a_whichDeck == "chest")
            {
                if (GetCardNum("chest") == 2 || GetCardNum("chest") == 4
                   || GetCardNum("chest") == 11 || GetCardNum("chest") == 13)
                {
                    payMoney = true;
                }
            }
            else
            {
                if (GetCardNum("chance") == 1 || GetCardNum("chance") == 4 ||
                    GetCardNum("chance") == 6)
                {
                    payMoney = true;
                }
            }

            return payMoney;
        }

        /// <summary>
        /// How much the player has to pay in fees
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <param name="a_houses"></param>
        /// <param name="a_numHotels"></param>
        /// <param name="a_numPlayers"></param>
        /// <returns> amount player has to pay in fees </returns>
        public int ChestChancePayFees(string a_whichDeck, int a_houses, int a_numHotels, int a_numPlayers)
        {
            int payment = 0;
            if (a_whichDeck == "chest")
            {
                if (GetCardNum("chest") == 2 || GetCardNum("chest") == 11)
                {
                    payment = 50;
                }
                if (GetCardNum("chest") == 4)
                {
                    payment = 100;
                }
                if (GetCardNum("chest") == 13)
                {
                    //40 per house, 115 per hotel
                    payment = (40 * a_houses) + (115 * a_numHotels);
                }
            }
            if (a_whichDeck == "chance")
            {
                if (GetCardNum("chance") == 1)
                {
                    //$25 for each house, 100 for each hotel
                    payment = (25 * a_houses) + (100 * a_numHotels);
                }
                if (GetCardNum("chance") == 4)
                {
                    //each player 50
                    payment = 50 * a_numPlayers;
                }
                if (GetCardNum("chance") == 6)
                {
                    payment = 15;
                }
            }

            return payment;
        }

        /// <summary>
        /// If player drew a Get Out of Jail Free card
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <returns> true if player drew jail card </returns>
        public bool ChestChanceJailCard(string a_whichDeck)
        {
            bool jailFree = false;

            if (a_whichDeck == "chest" && GetCardNum("chest") == 10)
            {
                jailFree = true;
            }
            if (a_whichDeck == "chance" && GetCardNum("chance") == 2)
            {
                jailFree = true;
            }

            return jailFree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Message on chance card drawn</returns>
        public string GetChanceMessage()
        {
            string message = "";
            switch (GetCardNum("chance"))
            {
                case 0:
                    message = "Advance to Go. Collect $200.";
                    break;
                case 1:
                    message = "Make general repairs on all your property: for each house, pay $25, " +
                        "for each hotel, pay $100.";
                    break;
                case 2:
                    message = "Get out of jail free. This card may be kept until needed, traded, or sold.";
                    break;
                case 3:
                    message = "Advance to St. Charles Place. If you pass Go, collect $200.";
                    break;
                case 4:
                    message = "You have been elected chairman of the board. Pay each player $50.";
                    break;
                case 5:
                    message = "Go back three spaces.";
                    break;
                case 6:
                    message = "Speeding fine. Pay $15.";
                    break;
                case 7:
                    message = "Advance to the next railroad. If unowned, you may buy it from the bank." +
                    "If owned, pay the owner twice the rent to which they are are otherwise entitled.";
                    break;
                case 8:
                    message = "Advance to the nearest railroad. If unowned, you may buy it from the bank." +
                    "If owned, pay the owner twice the rent to which they are are otherwise entitled.";
                    break;
                case 9:
                    message = "Bank pays you dividend of $50.";
                    break;
                case 10:
                    message = "Advance to Boardwalk.";
                    break;
                case 11:
                    message = "Take a trip to Reading Railroad. If you pass Go, collect $200.";
                    break;
                case 12:
                    message = "Your building loan matures. Collect $150.";
                    break;
                case 13:
                    message = "Advance to Illinois Avenue. If you pass Go, collect $200.";
                    break;
                case 14:
                    message = "Advance to the nearest utility If unowned, you may buy it from the bank." +
                    "If owned, roll the dice, and pay the owner 10 times your roll.";
                    break;
                case 15:
                    message = "Go to jail. Go directly to jail. Do not pass Go. Do not collect $200.";
                    break;
                default:
                    message = "Error";
                    break;
            }
            return message;
        }

        /// <summary>
        /// These particular chance cards change how the player pays rent
        /// </summary>
        /// <param name="a_whichDeck"></param>
        /// <returns> True if player must pay special rent </returns>
        public bool SpecialRentPay(string a_whichDeck)
        {
            //Get the current card number the player drew
            int cardNum = GetCardNum(a_whichDeck);
            if (a_whichDeck == "chance")
            {
                if (cardNum == 7 || cardNum == 8 || cardNum == 14)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This function is used for determining the amount of rent for drawing certain chance cards
        /// </summary>
        /// <returns> Number of the last chance card drawn </returns>
        public int GetLastCard()
        {
            int chnceIndex = GetChanceIndex();
            chnceIndex--;
            if (chnceIndex < 0)
            {
                chnceIndex = 15;
            }
            return GetChanceCardNum(chnceIndex);
        }

        /// <summary>
        /// Used by GetLastCard()
        /// </summary>
        /// <returns>Index of chance cards deck</returns>
        private int GetChanceIndex()
        {
            return m_chanceIndex;
        }

        /// <summary>
        /// Used by GetLastCard()
        /// </summary>
        /// <param name="a_index"></param>
        /// <returns>Card number at a particular index in the chance deck</returns>
        private int GetChanceCardNum (int a_index)
        {
            return m_shuffledCards[a_index];
        }

        /// <summary>
        /// this function is used for determining the amount of rent for drawing certain chance cards
        /// it determines if the player needs to roll dice
        /// </summary>
        /// <returns> true if the player doesn't need to roll dice</returns>
        public bool NoDiceRoll ()
        {
            int theLastCard = GetLastCard();
            if (theLastCard == 7 || theLastCard == 8)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// this function is used for determining the amount of rent for drawing certain chance cards
        /// </summary>
        /// <param name="a_regularRent"></param>
        /// <returns>amount of the special rent </returns>
        public int GetSpecialRent (int a_regularRent)
        {
            return (2 * a_regularRent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>type of square a player landed on</returns>
        public string GetSquareType(int a_squareNum)
        {
            string squareType = "property";

            if (a_squareNum == 0)
            {
                squareType = "go";
            }
            if (a_squareNum == 20)
            {
                squareType = "parking";
            }
            if (a_squareNum == 4 || a_squareNum == 38)
            {
                squareType = "tax";
            }
            if (a_squareNum == 12 || a_squareNum == 28)
            {
                squareType = "utilities";
            }
            if (a_squareNum == 7 || a_squareNum == 22 || a_squareNum == 36)
            {
                squareType = "chance";
            }
            if (a_squareNum == 2 || a_squareNum == 17 || a_squareNum == 33)
            {
                squareType = "chest";
            }
            if (a_squareNum == 10 || a_squareNum == 30 || a_squareNum == 40)
            {
                squareType = "jail";
            }
            if (a_squareNum == 5 || a_squareNum == 15 || a_squareNum == 25 || 
                a_squareNum == 35)
            {
                squareType = "railroad";
            }
           
            return squareType;
        }

        /// <summary>
        /// If player lands on a tax square
        /// </summary>
        /// <param name="a_squareNum"></param>
        /// <returns>Tax amount player must pay </returns>
        public int GetTaxAmount (int a_squareNum)
        {
            int txAmt = 100;
            if (a_squareNum == 4)
            {
                txAmt = 200;
            }            
            return txAmt;
        }

    }
}
