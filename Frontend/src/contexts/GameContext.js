import React from 'react'
import { createContext, useState, useEffect } from 'react'
import {
  GetBattleStartAsync,
  GetGameStateAsync,
  GetAttackAsync,
  EquipItemAsync,
  GetEnterStoreAsync,
  GetReturnToTownAsync,
  SellItemAsync,
  BuyItemAsync,
} from '../services/GameService'
import { useNavigate } from "react-router-dom";

export const GameContext = createContext()
export const GameContextProvider = ({ children }) => {
  const [currentGameState, setCurrentGameState] = useState()
  const [currentShopItems, setCurrentShopItems] = useState([])
  const [currentItems, setCurrentItems] = useState([])
  const navigate = useNavigate();

  useEffect(() => {
    setGameState()
  }, [])

  useEffect(() => {
    getEquipmentForSale()
  }, [currentGameState])

  useEffect(() => {
    getInventory()
  }, [currentGameState])

  const setGameState = async () => {
    let currentState = await GetGameStateAsync()
    setCurrentGameState(currentState)
  }

  const enterBattle = async () => {
    let currentState = await GetBattleStartAsync()
    setCurrentGameState(currentState)
  }

  const returnToTown = async () => {
    handleNavigateSite("/PlayGame");
    let currentState = await GetReturnToTownAsync()
    setCurrentGameState(currentState)
  }

  const attackEnemy = async () => {
    let currentState = await GetAttackAsync()
    setCurrentGameState(currentState)
  }

  //Kan nog göra om denna till getEquipment och ändra if satser för att minska repetition
  //så kan man ta bort metoden getEquipmentForSale som jag (ted) Gjorde nedanför
  const getInventory = async () => {
    if (currentGameState && currentGameState.hero) {
      await setCurrentItems(currentGameState.hero.equipmentInBag)
    } else {
      console.log('Game state or hero not available')
    }
  }

  const equipItem = async (index) => {
    let currentState = await EquipItemAsync(index)
    setCurrentGameState(currentState)
  }

  const enterStore = async () => {
    handleNavigateSite("/Shop");
    let currentState = await GetEnterStoreAsync()
    setCurrentGameState(currentState)
  }

  const getEquipmentForSale = async () => {
    if (currentGameState && currentGameState.location.name === 'Shop') {
      setCurrentShopItems(currentGameState.location.equipmentForSale)
    } else {
      console.log('Game state or store not available')
    }
  }

  const sellItem = async (index) => {
    let currentState = await SellItemAsync(index)
    setCurrentGameState(currentState)
  }

  const buyItem = async (index) => {
    let currentState = await BuyItemAsync(index)
    setCurrentGameState(currentState)
  }

  const handleNavigateSite = (site) => {
    navigate(`${site}`); 
  }
  
  return (
    <GameContext.Provider
      value={{
        currentGameState,
        setCurrentGameState,
        enterBattle,
        attackEnemy,
        getInventory,
        equipItem,
        enterStore,
        getEquipmentForSale,
        returnToTown,
        sellItem,
        buyItem,
        currentItems,
        handleNavigateSite,
        currentShopItems,
      }}
    >
      {children}
    </GameContext.Provider>
  )
}

export default GameContextProvider
