import React from 'react'
import { Routes, Route } from 'react-router-dom'
import WelcomePage from './components/views/WelcomePage'
import PlayGame from './components/views/PlayGame'
import Shop from './components/views/Shop'

const RouteElements = () => {
  return (
    <Routes>
      <Route exact path="/" element={<WelcomePage />} />
      <Route path="/PlayGame" element={<PlayGame />} />
      <Route path="/Shop" element={<Shop />} />
    </Routes>
  )
}

export default RouteElements
