import React from 'react'
import { signinRedirect } from '../services/userService'
import { Redirect } from 'react-router-dom'
import { useSelector } from 'react-redux'

function Login() {
  const user = useSelector(state => state.auth.user)

  async function login() {
      await signinRedirect()
  }

  return (
    (user) ?
      (<Redirect to={'/'} />)
      :
      (
        <div>
          <h1>Hello!</h1>
          <p>Welcome to a great card game: "Auction".</p>
          <p>Start by signing in.</p>

          <button onClick={() => login()}>Login</button>
        </div>
      )
  )
}

export default Login
