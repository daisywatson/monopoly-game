/// <summary>
/// Makes decisions for the computer player when playing the game. 
/// The computer's strategy is different depending on the difficulty of the game,
/// as well as if the game is in timed mode or not. 
/// Other factors such as the amount of funds the player has is used by some functions in this class.
/// </summary>
/// <author>Daisy Watson</author>
/// <date>5/15/20</date>
/// 
namespace DialogGame
{
    class Computer
    {
        bool m_timedGame = false; //if there's a time limit in the game
        bool m_easyDifficulty = true; //easy computer strategy (default) or hard strategy

        /// <summary>
        /// Set to a number if a timed game
        /// </summary>
        /// <param name="a_timeLimit"></param>
        /// <remarks> a_timeLimit is the number in minutes the user chose </remarks>
        public void SetTimeLimit(bool a_timeLimit)
        {
            m_timedGame = a_timeLimit;
        }

        /// <summary>
        /// True if timed game
        /// </summary>
        /// <returns>Return if a timed game or classic game</returns>
        public bool GetTimeLimit ()
        {
            return m_timedGame;
        }

        /// <summary>
        /// Set the difficulty level the user chose
        /// Easy is true, hard is false
        /// </summary>
        /// <param name="a_gameMode"></param>
        public void SetDifficulty (bool a_gameMode)
        {
            m_easyDifficulty = a_gameMode;
        }

        /// <summary>
        /// </summary>
        /// <returns>Get the difficulty level the user chose</returns>
        public bool GetDifficulty ()
        {
            return m_easyDifficulty;
        }

        /// <summary>
        /// Computer must decide whether to pay $200 or 10% of assets
        /// </summary>
        /// <param name="a_assetAmount"></param>
        /// <returns>Return true if paying $200, false if 10%</returns>
        public bool PayFlatIncomeTax(int a_assetAmount)
        {
            if ((a_assetAmount / 10) > 200) return true;

            return false;
        }

        /// <summary>
        /// whether the computer player should roll the dice rather than pay to
        /// get out of jail
        /// </summary>
        /// <param name="a_funds"></param>
        /// <param name="a_houses"></param>
        /// <param name="m_occupiedProperties"></param>
        /// <returns>True if rolling, false if paying</returns>
        public bool RollOutOfJail(int a_funds, int a_houses, int m_occupiedProperties)
        {
            if (GetTimeLimit())
            {
                if (RollOutOfJailTimedStrat(a_funds)) return true;

                return false;
            }

            //The computer player in easy mode tries to preserve funds
            //Also will try to roll if low on funds
            if (GetDifficulty() || a_funds < 50) return true;

            //In hard mode, the computer player will pay the fee to get back
            //into the game ASAP to buy more properties in the beginning of game
            //In the later half of the game, it's better to stay in jail to avoid paying rent
            if (m_occupiedProperties > 14 && a_houses < 32) return true;
            //There are 28 properties total

            return false;
        }

        /// <summary>
        /// Comp strategy in  a timed game
        /// </summary>
        /// <param name="a_funds"></param>
        /// <returns>True if rolling, false if paying</returns>
        private bool RollOutOfJailTimedStrat (int a_funds)
        {
            //In easy strategy, the computer will try to get out of jail and 
            //buy up as many properties as possible
            if (GetDifficulty() || a_funds < 50) return false;

            //The hard difficulty computer player will try to preserve funds
            return true;
        }

        /// <summary>
        /// If computer player lands on a property, whether or not to buy it
        /// </summary>
        /// <param name="a_propertyCost"></param>
        /// <param name="a_playerFunds"></param>
        /// <returns>True if buying</returns>
        public bool BuyProperty (int a_propertyCost, int a_playerFunds)
        {
            if (GetTimeLimit())
            {
                if (TimedBuyProperty(a_propertyCost, a_playerFunds)) return true;

                return false;
            }

            if (GetDifficulty())
            {
                //The easy player likes to buy cheap properties
                if (a_propertyCost < 300 && (a_propertyCost < a_playerFunds)) return true;
            }

            //Buying up all properties is best way to win the game
            if (a_playerFunds >= a_propertyCost) return true;

            return false;
        }

        /// <summary>
        /// If player buying property or not in a timed game
        /// </summary>
        /// <param name="a_propertyCost"></param>
        /// <param name="a_playerFunds"></param>
        /// <returns>True if buying</returns>
        private bool TimedBuyProperty (int a_propertyCost, int a_playerFunds)
        {
            if (!GetDifficulty())
            {
                //The hard mode player buys cheap properties to preserve funds in a timed game
                if (a_propertyCost < 300 && (a_propertyCost < a_playerFunds)) return true;
            }

            //Easy player will try to buy up all properties 
            if (a_playerFunds >= a_propertyCost) return true;

            return false;
        }

        /// <summary>
        /// Whether or not to bid in an auction
        /// The computer player will only increase a bid by the minimum $1 amount
        /// </summary>
        /// <param name="a_currentBid"></param>
        /// <param name="a_propertyCost"></param>
        /// <param name="a_playerFunds"></param>
        /// <param name="a_colorGroup"></param>
        /// <returns>True if buying</returns>
        public bool BidProperty (int a_currentBid, int a_propertyCost, int a_playerFunds, string a_colorGroup)
        {
            int compBid = a_currentBid + 1;
            if (compBid > a_playerFunds) return false;

            if(GetTimeLimit())
            {
                if (TimedBidProperty(compBid, a_propertyCost)) return true;

                return false;
            }

            if (GetDifficulty())
            {
                //The easy player won't spend more than a property's worth
                if (compBid > a_propertyCost) return false;
            }

            //Hard player spends more to either complete a set or prevent other player(s)
            //from completing a set of properties in the same color group
            if (!GetDifficulty() && a_colorGroup != "red" && a_colorGroup != "orange" && a_colorGroup != "yellow")
            {
                //Won't spend more than 2x a property's worth 
                //Unless the property is red, orange, yellow, the best properties in the game
                if (compBid > (2 * a_propertyCost)) return false;
            }
          
            return true;
        }

        /// <summary>
        /// If player should bid on property during a time game
        /// </summary>
        /// <param name="a_compBid"></param>
        /// <param name="a_propertyCost"></param>
        /// <returns>True if bidding</returns>
        private bool TimedBidProperty(int a_compBid, int a_propertyCost)
        {
            if (!GetDifficulty())
            {
                //The hard player won't spend more than a property's worth
                if (a_compBid > a_propertyCost) return false;
            }

            //The easy player will bid until winning
            return true;
        }
        
        /// <summary>
        /// If the player should buy a get out of jail free card or not
        /// </summary>
        /// <param name="a_sellPrice"></param>
        /// <param name="a_playerFunds"></param>
        /// <returns>True if buying</returns>
        public bool BuyJailCard (int a_sellPrice, int a_playerFunds)
        {
            if (a_sellPrice > a_playerFunds) return false;
            if (GetDifficulty())
            {
                //the easy player will buy jail cards for unreasonable prices
                return true;
            }
            //hard player will buy at a discount
            if (a_sellPrice < 50) return true;

            return false;
        }

        /// <summary>
        /// If a computer player should buy another player's property
        /// </summary>
        /// <param name="a_sellPrice"></param>
        /// <param name="a_propertyValue"></param>
        /// <param name="a_playerFunds"></param>
        /// <param name="a_colorGroup"></param>
        /// <returns>True if buying</returns>
        /// <remarks>Similar strategy as auctioning</remarks>
        public bool BuyPlayerProperty (int a_sellPrice, int a_propertyValue, int a_playerFunds, string a_colorGroup)
        {
            if (a_sellPrice > a_playerFunds) return false;

            if (GetTimeLimit())
            {
                if (TimedBuyPlayerProperty(a_sellPrice, a_propertyValue)) return true;

                return false;
            }

            if (GetDifficulty())
            {
                //Easy player prioritizes saving money
                if (a_sellPrice <= a_propertyValue) return true;

                return false;
            }

            //Hard player will spend more for a property, especially in a desired color group
            if (!GetDifficulty() && a_colorGroup != "red" && a_colorGroup != "orange" && a_colorGroup != "yellow")
            {
                //Won't spend more than 2x a property's worth 
                //Unless the property is red, orange, yellow, the best properties in the game
                if (a_sellPrice> (2 * a_propertyValue)) return false;
            }

            return true;
        }

        /// <summary>
        /// If buying another player's property during a timed game
        /// </summary>
        /// <param name="a_sellPrice"></param>
        /// <param name="a_propertyValue"></param>
        /// <returns>True if buying</returns>
        private bool TimedBuyPlayerProperty (int a_sellPrice, int a_propertyValue)
        {
            if (GetDifficulty())
            {
                //Hard player prioritizes saving money
                if (a_sellPrice <= a_propertyValue) return true;

                return false;
            }

            //The easy player will spend a lot to purchase any property
            return true;
        }


        /// <summary>
        /// If player should sell jail card
        /// </summary>
        /// <param name="a_numCards"></param>
        /// <param name="a_currentFunds"></param>
        /// <returns>True if selling</returns>
        public bool SellJailCard (int a_numCards, int a_currentFunds)
        {
            if (a_currentFunds < 50) return true;

            //the easy player prefers preserving cards
            if (!GetDifficulty())
            {
                if (a_numCards > 1) return true;

                //If player is low on funds, 
                if (a_currentFunds < 400) return true;
            }

            return false;
        }

        /// <summary>
        /// How much to sell a jail card for
        /// </summary>
        /// <param name="a_numCards"></param>
        /// <param name="a_playerFunds"></param>
        /// <returns>Selling price</returns>
        public int JailCardSellPrice(int a_numCards, int a_playerFunds)
        {
            if (a_playerFunds < 50) return 50;

            if (GetDifficulty())
            {
                //Easy player will sell for under value
                return 49;
            }
            //Hard player will sell for its value
            return 50;
        }

        /// <summary>
        /// If player should buy a house on a property or not
        /// </summary>
        /// <returns>True to buy</returns>
        public bool BuyHouse ()
        {
            //If player has enough funds, do it
            return true; 
        }

        /// <summary>
        /// If player should buy a hotel on a property or not
        /// </summary>
        /// <returns>True if buying</returns>
        public bool BuyHotel ()
        {
            //The easy player will buy when it's an option
            if (GetDifficulty()) return true;

            //Buying a hotel is not the best way to win the game
            //It's better to hoard houses from other players
            return false;
        }

        /// <summary>
        /// If player should mortgage a property or not
        /// </summary>
        /// <param name="a_playerFunds"></param>
        /// <returns>True if mortgaging</returns>
        public bool MortgageProperty(int a_playerFunds)
        {
            //mortgage if player funds are low
            if (a_playerFunds < 400) return true;
            return false;
        }

        /// <summary>
        /// If player should unmortgage a property or not
        /// </summary>
        /// <returns>True if unmortgaging </returns>
        public bool UnmortgageProperty()
        {
            //if player has enough funds, do it
            return true;
        }

        /// <summary>
        /// If player should sell a house or hotel on a property
        /// </summary>
        /// <param name="a_playerFunds"></param>
        /// <returns>True if selling</returns>
        public bool SellHouseHotel(int a_playerFunds)
        {
            //if player funds are low
            if (a_playerFunds < 400) return true;
            return false;
        }

        /// <summary>
        /// If player should sell an owned property or not
        /// </summary>
        /// <param name="a_playerFunds"></param>
        /// <returns>True if selling</returns>
        public bool SellPlayerProperty(int a_playerFunds)
        {
            //if player funds are low
            if (a_playerFunds < 400) return true;
            return false;
        }

    }
}
