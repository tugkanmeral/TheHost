import { createSlice } from '@reduxjs/toolkit'
import { getTokenObj, isObjNullOrEmpty, getToken } from '../../helpers';

let tokenObj = getTokenObj();
let appToken = getToken();

export const authReducer = createSlice({
    name: 'auth',
    initialState: {
        token: !isObjNullOrEmpty(tokenObj) ? appToken : null,
        isLoggedIn: !isObjNullOrEmpty(tokenObj) ? true : false 
    },
    reducers: {
        setToken: (state, action) => {
            state.token = action.payload
            state.isLoggedIn = true

            localStorage.setItem('appToken', action.payload)
        },
        clearToken: (state) => {
            state.token = ''
            state.isLoggedIn = false

            localStorage.removeItem('appToken')
        }
    },
})

// Action creators are generated for each case reducer function
export const { setToken, clearToken } = authReducer.actions

export default authReducer.reducer