import axios from 'axios'

export async function leaveGame(gameId, userId) {
  console.log(userId)
  await  axios.delete(`https://localhost:5001/games/${gameId}/players/${userId}`);
}


export async function joinGame(gameId, name) {
  await  axios.post(`https://localhost:5001/games/${gameId}/players`, {
    Name:name
  });
}


export async function createGame(name) {
  await  axios.post('https://localhost:5001/games', {
    Name:name
  });
}


async function getGames() {
  const response = await axios.get('https://localhost:5001/games');
  return response.data;
}

export {
  getGames
}
