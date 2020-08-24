/// <summary>
/// The template for each player in the game. 
/// Each player object holds information about the player playing the game. 
/// The player objects are generated and held in a player array in the Controller class. 
/// </summary>
/// <author>Daisy Watson</author>
/// <date>5/15/20</date>

using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    private string m_playerColor = "white";
    private string m_playerName = "John Doe"; 
    private bool m_isHumanPlayer = true;
    private int m_playerMoney = 1500;
    private int m_squareNum = 0; //player's current location on board
    private bool m_inJail = false;
    private int m_jailTurns = 0; //how many turns player has been in jail
    private int m_jailFreeCards = 0; //how many Get Out of Jail Free cards
    private bool m_auctionPass = false; //Whether player passed during an auction or not
    //List of owned property, lists their squareNum
    private List<int> m_properties = new List<int>();
    private List<int> m_mortgages = new List<int>();
    //Dictionary of owned houses/hotels. hotels are marked as #5. <squareNum, num of houses/hotels>
    private Dictionary<int, int> m_houseHotels = new Dictionary<int, int>();
    private int m_assetsValue = 0; //value of all properties
    private bool m_bankrupt = false;


    /// <summary>
    /// Set player name, type, and game piece color
    /// </summary>
    /// <param name="a_name"></param>
    /// <param name="a_humanPlayer"></param>
    /// <param name="a_color"></param>
    public Player(string a_name, bool a_humanPlayer, string a_color)
	{
        m_playerName = a_name;
        m_playerColor = a_color;
        m_isHumanPlayer = a_humanPlayer;
	}

    /// <summary>
    /// </summary>
    /// <returns>the name of the player i.e. Player 1</returns>
    public string GetPlayerName()
    {
        return m_playerName;
    }

    /// <summary>
    /// Which color the player selected at beginning of game, set it
    /// </summary>
    /// <param name="selectColor"></param>
    public void SetPlayerColor(string selectColor)
    {
        m_playerColor = selectColor;
    }

    /// <summary>
    /// </summary>
    /// <returns>Player color i.e. blue, purple, yellow, orange</returns>
    public string GetPlayerColor()
    {
        return m_playerColor;
    }

    /// <summary>
    /// </summary>
    /// <returns> player type</returns>
    public string GetPlayerType()
    {
        if (m_isHumanPlayer) return "human";

        return "computer";
    }

    /// <summary>
    /// True if human, false if computer
    /// </summary>
    /// <returns> if human player or not </returns>
    public bool PlayerTypeHuman ()
    {
        return m_isHumanPlayer;
    }

    /// <summary>
    /// Set current square to new location
    /// </summary>
    /// <param name="a_newNum"></param>
    /// <remarks> a_newNum is the new location the player has landed </remarks>
    public void SetSquare(int a_newNum)
    {
        m_squareNum = a_newNum;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Current square where player is</returns>
    public int GetSquare()
    {
        return m_squareNum;
    }

    /// <summary>
    /// $$$
    /// </summary>
    /// <returns>the player's funds</returns>
    public int GetMoney()
    {
        return m_playerMoney;
    }

    /// <summary>
    /// Add more money to player funds
    /// </summary>
    /// <param name="a_newMoney"></param>
    public void AddMoney(int a_newMoney)
    {
        m_playerMoney += a_newMoney;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>How many get out of jail free cards the player has in hand </returns>
    public int GetJailCards()
    {
        return m_jailFreeCards;
    }

    /// <summary>
    /// Pay a fee, subtract from player funds
    /// </summary>
    /// <param name="a_moneyAmt"></param>
    public void PayMoney(int a_moneyAmt)
    {
        m_playerMoney = m_playerMoney - a_moneyAmt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Total num houses player owns on all properties</returns>
    public int TotalNumHouses()
    {
        int totalNum = 0;
        if (TotalNumProperties() == 0) return totalNum;
        if (m_houseHotels.Count == 0) return totalNum;

        foreach (KeyValuePair<int, int> pair in m_houseHotels)
        {
            if (pair.Value != 0 && pair.Value < 5) //if not a hotel
            {
                totalNum += pair.Value;
            }
        }

        return totalNum;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Total num hotels player owns on all properties</returns>
    public int TotalNumHotels()
    {
        int totalNum = 0;
        if (TotalNumProperties() == 0) return totalNum;
        if (m_houseHotels.Count == 0) return totalNum;

        foreach (KeyValuePair<int, int> pair in m_houseHotels)
        {
            if (pair.Value == 5)
            {
                totalNum++;
            }
        }

        return totalNum;
    }

    /// <summary>
    /// Add another get out of jail free card to player's hand
    /// </summary>
    public void AddJailCards()
    {
        m_jailFreeCards++;
    }

    /// <summary>
    /// if the player has been in jail for three turns or not
    /// </summary>
    /// <returns>True if in jail for 3 turns </returns>
    public bool ThreeJailTurns()
    {
        if (m_jailTurns == 3)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Send player to jail
    /// </summary>
    public void GoToJail()
    {
        m_inJail = true;
    }

    /// <summary>
    /// Get out of jail
    /// </summary>
    public void LeaveJail()
    {
        m_inJail = false;
    }

    /// <summary>
    /// Whether player is in jail or not
    /// </summary>
    /// <returns>True if in jail</returns>
    public bool JailStatus()
    {
        return m_inJail;
    }

    /// <summary>
    /// Add 1 more turn to total number of turns player has been in jail
    /// </summary>
    public void AddJailTurns()
    {
        m_jailTurns++;
    }

    /// <summary>
    /// After player has left jail,
    /// reset the number of turns in jail to 0
    /// </summary>
    public void ResetJailTurns()
    {
        m_jailTurns = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>How many turns player has been in jail</returns>
    public int GetJailTurns()
    {
        return m_jailTurns;
    }

    /// <summary>
    /// Player used/sold one get out of jail free card
    /// </summary>
    public void SubtractJailCards()
    {
        m_jailFreeCards--;
    }

    /// <summary>
    /// If a particular property is mortgaged or not
    /// </summary>
    /// <param name="a_squareNum"></param>
    /// <returns>True if mortgaged</returns>
    public bool IsPropertyMortgaged(int a_squareNum)
    {
        if (m_mortgages.Contains(a_squareNum))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// If the player has declared bankruptcy or not
    /// </summary>
    /// <returns>True if bankrupt</returns>
    public bool IsBankrupt ()
    {
        return m_bankrupt;
    }

    /// <summary>
    /// Player has declared bankruptcy
    /// </summary>
    public void DeclareBankrupt()
    {
        m_bankrupt = true;
    }

    /// <summary>
    /// Mortgage a particular property
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void MortgageProperty (int a_squareNum)
    {
        m_mortgages.Add(a_squareNum);
    }

    /// <summary>
    /// Unmortgage a particular property
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void UnmortgageProperty (int a_squareNum)
    {
        m_mortgages.Remove(a_squareNum);
    }

    /// <summary>
    /// Set if the player passed during an auction
    /// Used to see if auction is over not
    /// True if player passed
    /// </summary>
    public void TurnOnAuctionPass ()
    {
        m_auctionPass = true;
    }

    /// <summary>
    /// If the player bid during an auction
    /// False if the player bid
    /// </summary>
    public void TurnOffAuctionPass ()
    {
        m_auctionPass = false;
    }

    /// <summary>
    /// If the player passed during an auction
    /// </summary>
    /// <returns></returns>
    public bool GetAuctionPass ()
    {
        return m_auctionPass;
    }

    /// <summary>
    /// Set player's name i.e. a_newName = "Player 1"
    /// </summary>
    /// <param name="a_newName"></param>
    public void SetPlayerName (string a_newName)
    {
        m_playerName = a_newName;
    }

    /// <summary>
    /// Set the number of get out of jail free cards the player has on hand
    /// </summary>
    /// <param name="a_numCards"></param>
    public void SetJailCards (int a_numCards)
    {
        m_jailFreeCards = a_numCards;
    }

    /// <summary>
    /// Add the property to the player's list of owned properties
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void AddProperty (int a_squareNum)
    {
        m_properties.Add(a_squareNum);
    }

    /// <summary>
    /// Remove the property from the player's list of owned properties
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void RemoveProperty(int a_squareNum)
    {
        m_properties.Remove(a_squareNum);
    }

    /// <summary>
    /// get list of properties the player owns
    /// </summary>
    /// <returns>List of all owned properties</returns>
    public List<int> GetPropertiesList()
    {
        return m_properties;
    }

    /// <summary>
    /// get properties with houses or hotels built
    /// hotels are marked as #5. 
    /// </summary>
    /// <returns>List of owned properties that have been improved </returns>
    // <squareNum, num of houses/hotels>
    public Dictionary<int, int> GetHouseHotels()
    {
        return m_houseHotels;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>List of the properties with hotels the player owns </returns>
    public List<int> GetHotels()
    {
        List<int> PropertiesWithHotels = new List<int>();
        foreach (KeyValuePair<int, int> pair in m_houseHotels)
        {
            if (pair.Value == 5)
            {
                PropertiesWithHotels.Add(pair.Key);
            }
        }
        return PropertiesWithHotels;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>List of the properties with houses the player owns </returns>
    public List<int> GetHouses()
    {
        List<int> PropertiesWithHouses = new List<int>();
        foreach (KeyValuePair<int, int> pair in m_houseHotels)
        {
            if (pair.Value != 5)
            {
                PropertiesWithHouses.Add(pair.Key);
            }
        }
        return PropertiesWithHouses;
    }

    /// <summary>
    /// Add a house to a particular property the player owns
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void AddNewHouse(int a_squareNum)
    {
        if (UnimprovedProperty(a_squareNum) == true)
        {
            m_houseHotels.Add(a_squareNum, 1);
        }
        else
        {
            //get value at squareNum
            int numHouses = GetNumHouses(a_squareNum);
            //++
            numHouses++;
            //reset/insert into dictionary
            SetNumHouses(a_squareNum, numHouses);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a_squareNum"></param>
    /// <returns>Num houses built on a property </returns>
    public int GetNumHouses(int a_squareNum)
    {
        //number of houses on a property
        int numHouses = 0;
        if (UnimprovedProperty(a_squareNum) != true)
        {
            numHouses = m_houseHotels[a_squareNum];
        }
        return numHouses;
    }

    /// <summary>
    /// Set the number of houses on a particular property
    /// </summary>
    /// <param name="a_squareNum"></param>
    /// <param name="a_numHouses"></param>
    public void SetNumHouses(int a_squareNum, int a_numHouses)
    {
        m_houseHotels[a_squareNum] = a_numHouses;
    }

    /// <summary>
    /// Remove a house on a particular property
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void RemoveHouse(int a_squareNum)
    {
        if (GetNumHouses(a_squareNum) == 1)
        {
            m_houseHotels.Remove(a_squareNum);
        }
        else
        {
            int numHouses = GetNumHouses(a_squareNum);
            numHouses--;
            SetNumHouses(a_squareNum, numHouses);
        }
    }

    /// <summary>
    /// if any houses have been built on a property
    /// </summary>
    /// <param name="a_squareNum"></param>
    /// <returns>True if no houses have been built </returns>
    public bool UnimprovedProperty(int a_squareNum)
    {
        //search houseHotels to see if houses/hotels exist
        //on that square
        if (m_houseHotels.ContainsKey(a_squareNum))
        {
            return false;
        }
        return true;      
    }

    /// <summary>
    /// If a hotel has been built on a particular property
    /// </summary>
    /// <param name="a_squareNum"></param>
    /// <returns>True if hotel has been built </returns>
    public bool HotelExists(int a_squareNum)
    {
        //if hotel exists at a property
        if (UnimprovedProperty(a_squareNum) == true)
        {
            return false;
        }
        if (GetNumHouses(a_squareNum) == 5)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// If four houses have been built on a property or not
    /// </summary>
    /// <param name="a_squareNum"></param>
    /// <returns>True if 4 houses have been built already </returns>
    public bool FourHouses(int a_squareNum)
    {
        //if 4 houses exist, a hotel can be built
        if (UnimprovedProperty(a_squareNum) == true)
        {
            return false;
        }
        if (GetNumHouses(a_squareNum) == 4)
        {
            return true;
        }

        return false;       
    }

    /// <summary>
    /// if the player can build/sell a house on the chosen property evenly
    /// </summary>
    /// <param name="a_otherSquareNums"></param>
    /// <param name="a_squareNum"></param>
    /// <param name="a_build"></param>
    /// <remarks> a_otherSquareNums are the other properties in the color set</remarks>
    /// <remarks> a_build is if building or selling a house, true if building </remarks>
    /// <returns>True if a house can be built/sold </returns>
    public bool IsEvenBuild(List <int> a_otherSquareNums, int a_squareNum, bool a_build)
    {
        int desiredBuildNum = GetNumHouses(a_squareNum);
        if (a_build)
        {
            desiredBuildNum++;
        }
        else
        {
            //selling the house
            desiredBuildNum--;
        }

        List<int> numHousesOtherSquares = new List<int>();
        for (int i = 0; i < a_otherSquareNums.Count; i++)
        {
            int numHouses = GetNumHouses(a_otherSquareNums[i]);
            //if the difference between desiredBuidNum and any other number of houses is > 1, 
            //the player can't build/sell the house
            if (Math.Abs(numHouses - desiredBuildNum) > 1)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Add a hotel to the property
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void AddHotel(int a_squareNum)
    {
        m_houseHotels[a_squareNum] = 5;
    }

    /// <summary>
    /// Remove a hotel from the property 
    /// </summary>
    /// <param name="a_squareNum"></param>
    public void RemoveHotel(int a_squareNum)
    {
        m_houseHotels[a_squareNum] = 4;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Total num of properties the player owns </returns>
    public int TotalNumProperties ()
    {
        return m_properties.Count;
    }

    /// <summary>
    /// If any properties can be mortgaged
    /// </summary>
    /// <returns>True if 1 or more propeties can be mortgaged </returns>
    public bool MortgagesAvailable()
    {
        if (TotalNumProperties() == 0) return false;

        if (TotalNumProperties() == m_mortgages.Count) return false;
       
        return true;
    }

    /// <summary>
    /// Array of properties that don't have mortgages
    /// </summary>
    /// <returns>List of properties without mortgages</returns>
    public List <int> UnmortgagedProperties()
    {
        return m_properties.Except(m_mortgages).ToList();
    }

    /// <summary>
    /// Set amount of funds the player has
    /// </summary>
    /// <param name="a_newMoney"></param>
    public void SetMoney(int a_newMoney)
    {
        m_playerMoney = a_newMoney;
    }

    /// <summary>
    /// Get total amount of assets the player has
    /// </summary>
    /// <returns> Value of player's assets </returns>
    public int GetAssets ()
    {
        return m_assetsValue;
    }

    /// <summary>
    /// Set how much assets the player has
    /// </summary>
    /// <param name="a_num"></param>
    public void SetAssets (int a_num)
    {
        m_assetsValue = a_num;
    }

    /// <summary>
    /// Add to player's asseets
    /// </summary>
    /// <param name="a_num"></param>
    public void AddAssets(int a_num)
    {
        m_assetsValue += a_num;
    }

    /// <summary>
    /// Subtract from player's assets
    /// </summary>
    /// <param name="a_num"></param>
    public void SubtractAssets (int a_num)
    {
        m_assetsValue -= a_num;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>assets plus cash</returns>
    public int GetTotalAssets ()
    {
        return (m_assetsValue + m_playerMoney);
    }     
}
