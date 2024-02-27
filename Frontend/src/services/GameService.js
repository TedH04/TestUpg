const GetGameStateAsync = async () => {
  try {
    const res = await fetch('https://localhost:7003/Game/state', {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const GetBattleStartAsync = async () => {
  try {
    const res = await fetch('https://localhost:7003/Game/battle', {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const GetAttackAsync = async () => {
  try {
    const res = await fetch('https://localhost:7003/Game/attack', {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const EquipItemAsync = async (index) => {
  try {
    const res = await fetch(
      `https://localhost:7003/Game/equip?index=${index}`,
      {
        method: 'GET',
      }
    )
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const GetEnterStoreAsync = async () => {
  try {
    const res = await fetch('https://localhost:7003/Game/enterStore', {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const GetReturnToTownAsync = async () => {
  try {
    const res = await fetch('https://localhost:7003/Game/returnToTown', {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const SellItemAsync = async (index) => {
  try {
    const res = await fetch(`https://localhost:7003/Game/sell?index=${index}`, {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

const BuyItemAsync = async (index) => {
  try {
    const res = await fetch(`https://localhost:7003/Game/buy?index=${index}`, {
      method: 'GET',
    })
    const data = await res.json()
    return data
  } catch (err) {
    console.error(err)
  }
}

export {
  GetGameStateAsync,
  GetBattleStartAsync,
  GetAttackAsync,
  EquipItemAsync,
  GetEnterStoreAsync,
  GetReturnToTownAsync,
  SellItemAsync,
  BuyItemAsync,
}
