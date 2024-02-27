import React, {useContext} from 'react'
import './WelcomePage.css';
import { GameContext } from '../../contexts/GameContext';

const WelcomePage = () => {
    const {handleNavigateSite} = useContext(GameContext);

    return (
        <div className="welcome-container">
            <button className="button" id='startgame-button' data-testid="startgame-button-test" onClick={() => handleNavigateSite('/PlayGame')}>Start Game</button>
        </div>
    )
}

export default WelcomePage
