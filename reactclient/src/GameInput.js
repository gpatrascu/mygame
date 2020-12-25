import React, { useState } from 'react';

const GameInput = (props) => {
    const [user, setUser] = useState('');
    const [move, setMove] = useState('');

    const onSubmit = (e) => {
        e.preventDefault();

        const isUserProvided = user && user !== '';
        const isMoveProvided = move && move !== '';

        if (isUserProvided && isMoveProvided) {
            props.sendMove(user, move);
        }
        else {
            alert('Please insert a move and a user.');
        }
    }

    const onUserUpdate = (e) => {
        setUser(e.target.value);
    }

    const onMoveUpdate = (e) => {
        setMove(e.target.value);
    }

    return (
        <form
            onSubmit={onSubmit}>
            <label htmlFor="user">User:</label>
            <br />
            <input
                id="user"
                name="user"
                value={user}
                onChange={onUserUpdate} />
            <br/>
            <label htmlFor="message">Message:</label>
            <br />
            <input
                type="text"
                id="message"
                name="message"
                value={move}
                onChange={onMoveUpdate} />
            <br/><br/>
            <button>Submit</button>
        </form>
    )
};

export default GameInput;