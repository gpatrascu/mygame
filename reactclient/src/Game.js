import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { useSelector } from 'react-redux'
import {signoutRedirect, signoutRedirectCallback} from './services/userService'
import GameWindow from './GameWindow';
import GameInput from './GameInput';

const Game = () => {
    const user = useSelector(state => state.auth.user)
    const [ connection, setConnection ] = useState(null);
    const [ game, setGame ] = useState([]);
    const latestGame = useRef(null);
    
    latestGame.current = game;

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hubs/game')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('ReceiveGameMove', gameMove => {
                        const updatedGame = [...latestGame.current];
                        updatedGame.push(gameMove);

                        setGame(updatedGame);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    async function signOut() {
        await signoutRedirect();
    }
    
    const sendMove = async (user2, move) => {
        const gameMove = {
            user: user.profile.given_name,
            move: move
        };

        try {
            await  fetch('https://localhost:5001/game/moves', {
                method: 'POST',
                body: JSON.stringify(gameMove),
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }
    
    // return (<div>Nothing yet</div>)

    return (
        <div>
            <p>Hello, {user.profile.given_name}.</p>
            <button className="button button-clear" onClick={() => signOut()}>Sign Out</button>
            <GameInput sendMove={sendMove} />
            <hr />
            <GameWindow game={game}/>
        </div>
    );
};

export default  Game;