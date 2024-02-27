import React from "react";
import { render, screen, act } from "@testing-library/react";
import userEvent from '@testing-library/user-event';
import PlayGame from '../components/views/PlayGame'
import GameContextProvider from "../contexts/GameContext";
import WelcomePage from "../components/views/WelcomePage";
import { BrowserRouter } from "react-router-dom";

describe('Render tests', () => {
    test('Should render welcome page', () => {
        render(
            <BrowserRouter>
                <GameContextProvider>
                    <WelcomePage />
                </GameContextProvider>
            </BrowserRouter>
            
        )
        const startGameButton = screen.getByTestId('startgame-button-test');
        expect(startGameButton).toBeInTheDocument();
    })
})

describe('Function tests', () => {
    test('Button click should run "handleClick" method', async () => {
        // Arrange
        const mockHandleNavigate = jest.fn();
            render(
                <BrowserRouter>
                    <GameContextProvider value={{handleNavigateSite: mockHandleNavigate('/PlayGame')}}>
                        <WelcomePage />
                    </GameContextProvider>
                </BrowserRouter>
            )

        // Act
        act(() => {
            const startGameButton = screen.getByTestId('startgame-button-test');
            userEvent.click(startGameButton);
        })
        



        // Assert
        expect(mockHandleNavigate).toHaveBeenCalled();
    })
})