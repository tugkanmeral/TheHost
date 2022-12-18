import { createSlice } from '@reduxjs/toolkit'

export const userReducer = createSlice({
    name: 'user',
    initialState: {
        lang: 'TR',
        masterKey: null
    },
    reducers: {
        setLang: (state, action) => {
            state.lang = action.payload;

            // changes html lang attribute
            document.documentElement.lang = action.payload;
        },
        setMasterKey: (state, action) => {
            state.masterKey = action.payload;
        }
    },
})

// Action creators are generated for each case reducer function
export const { setLang, setMasterKey } = userReducer.actions

export default userReducer.reducer