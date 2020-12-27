import React, { useState, useEffect } from 'react'
import { signoutRedirect } from '../services/userService'
import { useSelector } from 'react-redux'
import * as gameService from '../services/gameService'
import { prettifyJson } from '../utils/jsonUtils'
import Game from "../Game";

function Home() {
  const user = useSelector(state => state.auth.user)
  const [games, setGames] = useState([])
    
  useEffect(() => getGames(), []);
    
  const signOut = async () => {
      await signoutRedirect()
  }
  
  const getGames = async () => {
      const games = await gameService.getGames();
      console.log(games);
      setGames(games);
  }

  const createGame = async () => {
      console.log("create game method pressed");
      await gameService.createGame(user.profile.name);
      await getGames();
  }
  
  const joinGame = async (gameId) => {
      await gameService.joinGame(gameId, user.profile.name);
      await getGames();
  }

    const leaveGame = async (gameId) => {
        await gameService.leaveGame(gameId, user.profile.sub);
        await getGames();
    }
    

    return (
    <div>
      <h1>Home</h1>
      <p>Hello, {user.profile.name}.</p>

      <button onClick={() => createGame()}>Create game</button>
      <button onClick={() => signOut()}>Sign Out</button>
      
      {games.map(game => {
         return (
             <div key={game.id}>
                 {game.players.map(p => {
                     return (
                         <div key={p.id}>{p.name}
                         </div>
                         
                     )
                 })} 
                 <button onClick={()=>joinGame(game.id)}>Join</button>
                 <button onClick={()=>leaveGame(game.id)}>Leave</button>
             </div>
             
         ); 
      })
      }  
        
    </div>
  )
}

export default Home
