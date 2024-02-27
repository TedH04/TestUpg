import React from 'react';
import { render, act, waitFor, screen } from '@testing-library/react';
import { GameContext } from '../contexts/GameContext';
import { BrowserRouter } from 'react-router-dom';
import GameContextProvider from '../contexts/GameContext';
import { GetGameStateAsync, GetBattleStartAsync, GetReturnToTownAsync, GetAttackAsync, EquipItemAsync, GetEnterStoreAsync, SellItemAsync, BuyItemAsync } from '../services/GameService';

jest.mock('../services/GameService', () => ({
  GetBattleStartAsync: jest.fn(),
  GetGameStateAsync: jest.fn(),
  GetAttackAsync: jest.fn(),
  EquipItemAsync: jest.fn(),
  GetEnterStoreAsync: jest.fn(),
  GetReturnToTownAsync: jest.fn(),
  SellItemAsync: jest.fn(),
  BuyItemAsync: jest.fn(),
}));

//#region MockGameState
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
        "name": "Town"
    }
}
//#endregion
//#region MockBattleGameState
const mockBattleGameState = {
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
        "enemy": {
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
        "damageDoneLastTurn": 0,
        "damageTakenLastTurn": 0,
        "name": "Battle"
    }
}
//#endregion 
//#region Mocked store gamestate
const mockStoreGameState = {
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
//#region Mocked equipped items gamestate
const mockEquippedItemsGamestate = {
  "hero": {
    "xp": 30,
    "equippedWeapon": {
      "attackPower": 2,
      "name": "Sword",
      "price": 40
    },
    "equippedArmor": {
      "armorValue": 1,
      "name": "Breastplate",
      "price": 35
    },
    "equipmentInBag": [
      {
        "armorValue": 1,
        "name": "Breastplate",
        "price": 35
      }
    ],
    "money": 5,
    "id": 1,
    "name": "Ted",
    "description": null,
    "level": 3,
    "maxHP": 16,
    "currentHP": 16,
    "maxMana": 9,
    "currentMana": 9,
    "attackPower": 5,
    "armorValue": 1,
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
    "name": "Town"
  }
}
//#endregion

describe('GameContext methods tests', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  
    test('setGameState, Should set the game state', async () => {

      // Arrange
        GetGameStateAsync.mockResolvedValue(mockGameState);
        let contextValue;
        render(
          <BrowserRouter>
              <GameContextProvider>
                  <GameContext.Consumer>
                  {value => {
                      contextValue = value;
                      return <div data-testid="consumer" />;
                  }}
                  </GameContext.Consumer>
              </GameContextProvider>
          </BrowserRouter>
        );
        

        // Assert
        await waitFor(() => {
            expect(screen.getByTestId('consumer')).toBeInTheDocument();
            expect(contextValue.currentGameState).toEqual(mockGameState);
        });
    })

    test('enterBattle, Should set battle game state', async () => {

      // Arrange
        GetBattleStartAsync.mockResolvedValue(mockBattleGameState);
        let contextValue;
        render(
            <BrowserRouter>
                <GameContextProvider>
                    <GameContext.Consumer>
                    {value => {
                        contextValue = value;
                        return <div data-testid="consumer" />;
                    }}
                    </GameContext.Consumer>
                </GameContextProvider>
            </BrowserRouter>
        );

        // Act
        act(() => {
            contextValue.enterBattle();
        });

          // Assert
        await waitFor(() => {
            expect(screen.getByTestId('consumer')).toBeInTheDocument();
            expect(contextValue.currentGameState).toEqual(mockBattleGameState);
        });
    })

    test('returnToTown, Should return town game state', async () => {
      // Arrange
        GetReturnToTownAsync.mockResolvedValue(mockGameState);
        let contextValue;
        render(
          <BrowserRouter>
            <GameContextProvider>
              <GameContext.Consumer>
                {value => {
                  contextValue = value;
                  return <div data-testid="consumer" />;
                }}
              </GameContext.Consumer>
            </GameContextProvider>
          </BrowserRouter>
        );

        // Act
        act(() => {
            contextValue.returnToTown();
          });

          // Assert
        await waitFor(() => {
            expect(screen.getByTestId('consumer')).toBeInTheDocument();
            expect(contextValue.currentGameState).toEqual(mockGameState);
        });
    })

    test('attackEnemy, Should return game state', async () => {
      // Arrange
        GetAttackAsync.mockResolvedValue(mockBattleGameState);
        let contextValue;
        render(
          <BrowserRouter>
            <GameContextProvider>
              <GameContext.Consumer>
                {value => {
                  contextValue = value;
                  return <div data-testid="consumer" />;
                }}
              </GameContext.Consumer>
            </GameContextProvider>
          </BrowserRouter>
        );

        // Act
        act(() => {
            contextValue.attackEnemy();
          });

          // Assert
        await waitFor(() => {
            expect(screen.getByTestId('consumer')).toBeInTheDocument();
            expect(contextValue.currentGameState).toEqual(mockBattleGameState);
        });
    })

    test('equipItem, Should return equipped items game state', async () => {
      // Arrange
      EquipItemAsync.mockResolvedValue(mockEquippedItemsGamestate);
      let contextValue;
      render(
        <BrowserRouter>
          <GameContextProvider>
            <GameContext.Consumer>
              {value => {
                contextValue = value;
                return <div data-testid="consumer" />;
              }}
            </GameContext.Consumer>
          </GameContextProvider>
        </BrowserRouter>
      );

      // Act
      act(() => {
          contextValue.equipItem();
      });

        // Assert
      await waitFor(() => {
          expect(screen.getByTestId('consumer')).toBeInTheDocument();
          expect(contextValue.currentGameState).toEqual(mockEquippedItemsGamestate);
          expect(contextValue.currentGameState.hero.equippedArmor.name).toEqual("Breastplate");
          expect(contextValue.currentGameState.hero.equippedWeapon.name).toEqual("Sword");
      });
  })

  test('enterStore, Should return store game state', async () => {
    // Arrange
    GetEnterStoreAsync.mockResolvedValue(mockStoreGameState);
    let contextValue;
    render(
      <BrowserRouter>
        <GameContextProvider>
          <GameContext.Consumer>
            {value => {
              contextValue = value;
              return <div data-testid="consumer" />;
            }}
          </GameContext.Consumer>
        </GameContextProvider>
      </BrowserRouter>
    );

    // Act
    act(() => {
        contextValue.enterStore();
      });

      // Assert
    await waitFor(() => {
        expect(screen.getByTestId('consumer')).toBeInTheDocument();
        expect(contextValue.currentGameState).toEqual(mockStoreGameState);
        expect(contextValue.currentGameState.location.equipmentForSale).toBeTruthy();
    });
  })

  test('sellItem, Should return game state', async () => {
    // Arrange
    SellItemAsync.mockResolvedValue(mockStoreGameState);
    let contextValue;

    // Act
    render(
      <BrowserRouter>
        <GameContextProvider>
          <GameContext.Consumer>
            {value => {
              contextValue = value;
              return <div data-testid="consumer" />;
            }}
          </GameContext.Consumer>
        </GameContextProvider>
      </BrowserRouter>
    );

    act(() => {
        contextValue.sellItem();
    });

    // Assert
    await waitFor(() => {
        expect(screen.getByTestId('consumer')).toBeInTheDocument();
        expect(contextValue.currentGameState).toEqual(mockStoreGameState);
    });
  })

  test('buyItem, Should return game state', async () => {
    // Arrange
    BuyItemAsync.mockResolvedValue(mockStoreGameState);
    let contextValue;
        render(
          <BrowserRouter>
            <GameContextProvider>
              <GameContext.Consumer>
                {value => {
                  contextValue = value;
                  return <div data-testid="consumer" />;
                }}
              </GameContext.Consumer>
            </GameContextProvider>
          </BrowserRouter>
        );

    // Act
    act(() => {
        contextValue.buyItem();
      });

      // Assert
    await waitFor(() => {
        expect(screen.getByTestId('consumer')).toBeInTheDocument();
        expect(contextValue.currentGameState).toEqual(mockStoreGameState);
    });
  })
});

