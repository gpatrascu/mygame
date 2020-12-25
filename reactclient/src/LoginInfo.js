import React, {useEffect, useState} from 'react';
import {UserManager, WebStorageStateStore} from 'oidc-client';

const config = {
    authority: "https://localhost:5100/",
    client_id: "mygame",
    redirect_uri: "http://localhost:3000/login_complete",
    response_type: "code",
    scope:"openid profile mygame",
    post_logout_redirect_uri : "http://localhost:3000/logout",
};

const LoginInfo = async (props) => {
    let manager = new UserManager(config);

    let user = await manager.getUser();

    let userName = 'not logged in';
    if(user){
        userName = user.profile.name;
    }
    
    const login = () => {
        console.log("Login")
    }

    return (
        <div>
            not auth
            <button onClick={login}>Login</button>
        </div>
    );
};

export default LoginInfo;