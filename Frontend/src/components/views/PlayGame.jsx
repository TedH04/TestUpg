import "./PlayGame.css";
import React, { useContext, useState } from "react";
import { GameContext } from "../../contexts/GameContext";

const PlayGame = () => {

    const {currentGameState, enterBattle, attackEnemy, getInventory, currentItems, equipItem, enterStore} = useContext(GameContext);

    const [showItems, setShowItems] = useState(false);

    let enemyImage = "";

    if(currentGameState.location.name !== "Town"){
        if (currentGameState.location.enemy.name === "Slime"){
            enemyImage = "https://cdn.discordapp.com/attachments/1024651946721824768/1164949923725316096/slimepicfixxed.png?ex=65451329&is=65329e29&hm=e29ccf5814c4ff09a6d1ecf2285877fc2b8036f8d41062fe6b9e0a2e2053a0bd&"
        }else if(currentGameState.location.enemy.name === "Rat"){
            enemyImage = "https://i.pinimg.com/originals/2f/7c/78/2f7c78c4378b7aae8d237e083732884f.png"
        }
    }

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
    <div>
        {currentGameState ? (
            <div className="game-container" id="game-container" data-testid="game-container-test">
                <div className="hero-card" id="hero-card">
                    <img className="fighterImage" id="hero-image" src={heroImage} alt="Hero" />
                    <h3>{currentGameState.hero.name} The Hero</h3>
                    <p>Level: {currentGameState.hero.level}</p>
                    <p id="money-tag">Money: {currentGameState.hero.money}</p>
                    <p>HP: {currentGameState.hero.currentHP}/{currentGameState.hero.maxHP}</p>
                    <p>Mana: {currentGameState.hero.currentMana}/{currentGameState.hero.maxMana}</p>
                    {currentGameState.hero.equippedWeapon ? (
                        <p>Weapon: {currentGameState.hero.equippedWeapon.name} + {currentGameState.hero.equippedWeapon.attackPower}</p>
                    ) : (
                        <p>Weapon: Unarmed</p>
                    )}
                    {currentGameState.hero.equippedArmor ? (
                        <p>Armor: {currentGameState.hero.equippedArmor.name} + {currentGameState.hero.equippedArmor.armorValue}</p>
                    ) : (
                        <p>Armor: Unarmored</p>
                    )}
                    <p>Armor Value: {currentGameState.hero.armorValue}</p>
                    <p id="attackpower-tag">Attack Power: {currentGameState.hero.attackPower}</p>
                    <p>Experience: {currentGameState.hero.xp}</p>
                </div>
            
                <div id="game-info">
                    <h1 className="game-area-text" id="location-text">You are currently in: {currentGameState.location.name}</h1>
                    <ul id="text-display" data-testid="text-display-test" className="action-flow">
                        {showItems === true && 
                            currentItems.map(listItem => {
                                var listEquipItemName ="equip-"+ listItem.name;
                                return <li key={currentItems.indexOf(listItem)}>{listItem.name} + {listItem.attackPower}{listItem.armorValue} 
                                <button id={listEquipItemName}
                                onClick={() => {
                                    equipItem(currentItems.indexOf(listItem));
                                    setShowItems(false);
                                }}>Equip</button></li>
                            })
                        }
                        
                    </ul>
                    <div className="button-container" id="game-buttons">
                        {currentGameState.location.name !== "Battle" &&
                            <button className="game-button" id="startbattle-button" data-testid="startbattle-button-test" onClick={() =>{
                                enterBattle();
                                setShowItems(false);
                            }}>Challenge an enemy</button>
                        }
                        {currentGameState.location.name === "Battle" &&
                            <button className="game-button" id="attack-button" data-testid="attack-button-test" onClick={() => attackEnemy()}>Attack</button>
                        }
                        {currentGameState.location.name !== "Battle" &&
                            <button className="game-button" id="checkinventory-button" data-testid="checkinventory-button-test" onClick={() => {
                                getInventory();
                                setShowItems(true);
                            }}>Check inventory</button>
                        }

                        {currentGameState.location.name !== "Battle" &&
                            <button className="game-button" id="enterStore-button" data-testid="enterStore-button-test" onClick={() =>{
                                enterStore();
                            }}>Enter Store</button>
                        }
                        
                    </div>
                </div>
                
                {currentGameState.location && currentGameState.location.name !== "Town" &&
                    <div className="enemy-card" id="enemy-card">
                        <img className="fighterImage" id="enemy-image" src={enemyImage} alt="Enemy" />
                        <h3 id="enemy-type-name">{currentGameState.location.enemy.name}</h3>
                        <p>Level: {currentGameState.location.enemy.level}</p>
                        <p>HP: {currentGameState.location.enemy.currentHP}/{currentGameState.location.enemy.maxHP}</p>
                        <p>Mana: {currentGameState.location.enemy.currentMana}/{currentGameState.location.enemy.maxMana}</p>
                        <p>Armor: {currentGameState.location.enemy.armorValue}</p>
                        <p>Attack: {currentGameState.location.enemy.attackPower}</p>
                    </div>
                }
                
                
            </div>
            
        ) : (
            <p id="error-message">Error Loading game</p>
        )}
    </div>
  )
}

export default PlayGame
