import axios from 'axios'

export async function CreateGame(name) {
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
