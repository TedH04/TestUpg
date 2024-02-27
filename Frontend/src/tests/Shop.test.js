import React from "react";
import { render, screen, act } from "@testing-library/react";
import userEvent from '@testing-library/user-event';
import {GameContext} from "../contexts/GameContext";
import { BrowserRouter } from 'react-router-dom'
import Shop from "../components/views/Shop";

//#region Mocked gamestate
const mockGameState = {
    "hero": {
      "xp": 10,
      "equippedWeapon": null,
      "equippedArmor": null,
      "equipmentInBag": [
        {
          "attackPower": 2,
          "name": "Sword",
          "price": 40
        }
      ],
      "money": 0,
      "id": 1,
      "name": "Ted",
      "description": null,
      "level": 1,
      "maxHP": 10,
      "currentHP": 10,
      "maxMana": 5,
      "currentMana": 5,
      "attackPower": 1,
      "armorValue": 0,
      "critChance": 0,
      "dodgeChance": 0
    },
    "enemyList": [
      {
        "noDropChance": 80,
        "xpValue": 5,
        "moneyValue": 10,
        "lootTable": [
          {
            "attackPower": 2,
            "name": "Sword",
            "price": 40
          },
          {
            "armorValue": 1,
            "name": "Breastplate",
            "price": 35
          }
        ],
        "id": 2,
        "name": "Slime",
        "description": "Japanese first enemy",
        "level": 1,
        "maxHP": 5,
        "currentHP": 5,
        "maxMana": 5,
        "currentMana": 0,
        "attackPower": 1,
        "armorValue": 0,
        "critChance": 0,
        "dodgeChance": 0
      },
      {
        "noDropChance": 80,
        "xpValue": 5,
        "moneyValue": 10,
        "lootTable": [
          {
            "armorValue": 1,
            "name": "Breastplate",
            "price": 35
          }
        ],
        "id": 3,
        "name": "Rat",
        "description": "European first enemy",
        "level": 1,
        "maxHP": 3,
        "currentHP": 3,
        "maxMana": 3,
        "currentMana": 0,
        "attackPower": 2,
        "armorValue": 0,
        "critChance": 0,
        "dodgeChance": 0
      }
    ],
    "location": {
      "equipmentForSale": [
        {
          "armorValue": 1,
          "name": "Breastplate",
          "price": 35
        }
      ],
      "name": "Shop"
    }
  }
//#endregion

describe("Render component tests", () => {
  test('Renders the component without errors', () => {
    // Arrange
    render(
        <BrowserRouter>
            <GameContext.Provider value={{ currentGameState: mockGameState, currentShopItems: mockGameState.location.equipmentForSale }}>
                <Shop />
            </GameContext.Provider>
        </BrowserRouter>
    );

    // Assert
    expect(screen.getByText(/You are currently in:/)).toBeInTheDocument();
  });

  test('Renders the hero card with correct values', () => {
      // Arrange
      render(
          <BrowserRouter>
              <GameContext.Provider value={{ currentGameState: mockGameState, currentShopItems: mockGameState.location.equipmentForSale }}>
                  <Shop />
              </GameContext.Provider>
          </BrowserRouter>
      );

      // Assert
      expect(screen.getByText(`${mockGameState.hero.name} The Hero`)).toBeInTheDocument();
      expect(screen.getByText(`Level: ${mockGameState.hero.level}`)).toBeInTheDocument();
      expect(screen.getByText(`Money: ${mockGameState.hero.money}`)).toBeInTheDocument();
      expect(screen.getByText(`Attack Power: ${mockGameState.hero.attackPower}`)).toBeInTheDocument();
      expect(screen.getByText('Weapon: Unarmed')).toBeInTheDocument();
      expect(screen.getByText('Armor: Unarmored')).toBeInTheDocument();
  });

})

describe("Call functions tests", () => {
  test('Should handle "Buy button" button click ', () => {
    // Arrange
    const mockBuyItem = jest.fn();
    render(
        <BrowserRouter>
            <GameContext.Provider value={{ currentGameState: mockGameState, currentShopItems: mockGameState.location.equipmentForSale, buyItem: mockBuyItem }}>
                <Shop />
            </GameContext.Provider>
        </BrowserRouter>
    );

    // Act
    const buyButton = screen.getByTestId("buy-button-test");
    act(() => {
        userEvent.click(buyButton);
    });

    // Assert
    expect(mockBuyItem).toHaveBeenCalled();
  })

  test('Should handle "Sell button" button click ', () => {
      // Arrange
      const mockSellItem = jest.fn(); 
      render(
          <BrowserRouter>
              <GameContext.Provider value={{ currentGameState: mockGameState, currentShopItems: mockGameState.location.equipmentForSale, sellItem: mockSellItem }}>
                  <Shop />
              </GameContext.Provider>
          </BrowserRouter>
      );

      // Act
      const mockSellButton = screen.getByTestId("sell-button-test")
      act(() => {
          userEvent.click(mockSellButton);
      })

      // Assert
      expect(mockSellItem).toHaveBeenCalled();
  })

  test('Should handle "Leave" button click', () => {
      // Arrange
      const mockReturnToTown = jest.fn();

      render(
          <BrowserRouter>
              <GameContext.Provider value={{ currentGameState: mockGameState, currentShopItems: mockGameState.location.equipmentForSale, returnToTown: mockReturnToTown }}>
                  <Shop />
              </GameContext.Provider>
          </BrowserRouter>
      );

      // Act
      const mockLeaveButton = screen.getByTestId("leave-button-test") 
      act(() => {
          userEvent.click(mockLeaveButton);
      })

      // Assert
      expect(mockReturnToTown).toHaveBeenCalled();
  })
})
