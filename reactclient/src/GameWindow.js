import React from 'react';

import Move from './Move';

const GameWindow = (props) => {
    console.log(props);
    const game = props.game
        .map(m => <Move
            key={Date.now() * Math.random()}
            user={m.user}
            move={m.move}/>);

    return(
        <div>
            {game}
        </div>
    )
};

export default GameWindow;