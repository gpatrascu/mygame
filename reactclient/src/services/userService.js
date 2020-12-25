import { UserManager } from 'oidc-client';
import { storeUserError, storeUser } from '../actions/authActions'

const config = {
  authority: "https://localhost:5100/",
  client_id: "mygame",
  redirect_uri: "http://localhost:3000/signin-oidc",
  response_type: "code",
  scope:"openid profile mygame",
  silent_redirect_uri: 'http://localhost:3000/signin-oidc',
  post_logout_redirect_uri : "http://localhost:3000",
};

const userManager = new UserManager(config)

export async function loadUserFromStorage(store) {
  try {
    let user = await userManager.getUser()
    if (!user) { return store.dispatch(storeUserError()) }
    store.dispatch(storeUser(user))
  } catch (e) {
    console.error(`User not found: ${e}`)
    store.dispatch(storeUserError())
  }
}

export async function signinRedirect() {
  return await userManager.signinRedirect()
}

export function signinRedirectCallback() {
  return userManager.signinRedirectCallback()
}

export async function signoutRedirect() {
  await userManager.clearStaleState()
  await userManager.removeUser()
  return userManager.signoutRedirect()
}

export async function signoutRedirectCallback() {
  await userManager.clearStaleState()
  await userManager.removeUser()
  return userManager.signoutRedirectCallback()
}

export default userManager