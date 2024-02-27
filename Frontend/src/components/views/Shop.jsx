import React, { useContext } from 'react';
import { GameContext } from '../../contexts/GameContext';
import './Shop.css';


const Shop = () => {
    const { currentGameState, returnToTown, buyItem, sellItem, currentShopItems} = useContext(GameContext);


    let heroImage = "";

    if (currentGameState.hero.equippedWeapon === null)
    {
        heroImage = "https://cdn.discordapp.com/attachments/1157246873334198305/1169956091904405535/transbacknosword.png?ex=65574985&is=6544d485&hm=b42e0e6378b250ffb24e2d50420d914321d8e6af37d3e6aba0dd7afe7bf9866b&"
    }
    else if(currentGameState.hero.equippedArmor !== null)
    {
        heroImage = "https://cdn.discordapp.com/attachments/1157246873334198305/1170024137863999599/transbackwithswordandshield.png?ex=655788e4&is=654513e4&hm=87a44396802aa4b833110e57747b9f31a612360e842237a63dad920d91b44b3f&"
    }
    else if(currentGameState.hero.equippedWeapon !== null)
    {
        heroImage = "https://cdn.discordapp.com/attachments/1157246873334198305/1169956106152448010/transbackwithsword.png?ex=65574988&is=6544d488&hm=4e9c93c8307974210d2aca8490be96deb4a7ef49f783037c9d526d643105887e&&"
    }

    return (
        <div className="game-container">

        <div className="hero-card" id="hero-card">
                <img className="fighterImage" id="fighter-image" src={heroImage} alt="Hero" />
                <h3 id="hero-name">{currentGameState.hero.name} The Hero</h3>
                <p id="hero-level">Level: {currentGameState.hero.level}</p>
                <p id="hero-money">Money: {currentGameState.hero.money}</p>
                {currentGameState.hero.equippedWeapon ? (
                    <p id="hero-weapon">Weapon: {currentGameState.hero.equippedWeapon.name} + {currentGameState.hero.equippedWeapon.attackPower}</p>
                ) : (
                    <p id="hero-weapon">Weapon: Unarmed</p>
                )}
                {currentGameState.hero.equippedArmor ? (
                    <p id="hero-armor">Armor: {currentGameState.hero.equippedArmor.name} + {currentGameState.hero.equippedArmor.armorValue}</p>
                ) : (
                    <p id="hero-armor">Armor: Unarmored</p>
                )}
                <p id="hero-armor-value">Armor Value: {currentGameState.hero.armorValue}</p>
                <p id="hero-attack-power">Attack Power: {currentGameState.hero.attackPower}</p>
            </div>
            
        {currentGameState ? (
            <div className="shop-container" id="shop-container">
            <div>
                <h1 className="game-area-text" id="current-location">You are currently in: {currentGameState.location.name}</h1>
                <h2 id="shop-inventory-title">Shop Inventory</h2>
                <ul id="shop-text-display" className="action-flow">
                {currentShopItems.map(listItem => {
                    var listBuyItemName ="buy-"+ listItem.name;
                    return <li key={currentShopItems.indexOf(listItem)}>{listItem.name} + {listItem.attackPower}{listItem.armorValue} 
                        <button id={listBuyItemName} data-testid="buy-button-test"
                        onClick={() => {
                            buyItem(currentShopItems.indexOf(listItem));
                        }}>Buy</button></li>
                    })
                }
                </ul>

                <h2 id="player-inventory-title">Your Inventory</h2>
                <ul id="player-text-display" className="action-flow">
                    {currentGameState.hero.equipmentInBag.map((item, index) => (
                        <li key={index}>
                            {item.name} + {item.attackPower}{item.armorValue}
                            <button id='sell-button' data-testid="sell-button-test" onClick={() => {
                                sellItem(index);
                                }}>Sell</button>
                        </li>
                    ))}
                </ul>
            </div>
            <button className="button" id="leave-button" data-testid="leave-button-test" onClick={() => {
                returnToTown();
                currentGameState.location.name = "Town";
                }}>Leave</button>
        </div>
            
        ) : (
            <p id="error-message">Error Loading game</p>
        )}
      
    </div>
  )
}

export default Shop;
