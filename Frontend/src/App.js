import './App.css'
import React from 'react'
import GameContextProvider from './contexts/GameContext'
import RouteElements from './RouteElements'
import AudioPlayer from './components/music/AudioPlayer'
import { BrowserRouter } from 'react-router-dom'

function App() {
  return (
    <BrowserRouter>
      <AudioPlayer />
      <GameContextProvider>
        <RouteElements />
      </GameContextProvider>
    </BrowserRouter>
  )
}

export default App
