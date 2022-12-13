import React, { useState } from 'react';
import { useDispatch } from 'react-redux'
import { setToken } from './../store/reducers/authReducer'
import { useNavigate } from "react-router-dom";
import { GET_TOKEN_URL } from '../data/routes';
import Translation from '../components/Translation';
import { toast } from 'react-toastify';
import RequestButton from '../components/RequestButton';

function Login() {
    const dispatch = useDispatch()
    // const isLoggedIn = useSelector((state) => state.auth.isLoggedIn)
    let navigate = useNavigate();

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const [onLoginRequest, setOnLoginRequest] = useState(false);

    function handleUsernameChange(event) {
        setUsername(event.target.value);
    }

    function handlePasswordChange(event) {
        setPassword(event.target.value);
    }

    function sendLoginRequest() {
        if (!username || !password) {
            toast.warning(<Translation msg="LOGIN_VALIDATION_WARN" />);
            return;
        }

        setOnLoginRequest(true)
        fetch(GET_TOKEN_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Username: username,
                Password: password
            })
        }).then(async (response) => {
            if (!response.ok) {
                toast.error(<Translation msg="LOGIN_ERR" />)
                setPassword('')
                setOnLoginRequest(false)
                return;
            }

            const data = await response.text();
            if (data) {
                dispatch(setToken(data))
                navigate('/')
            }
            else {
                toast.error(<Translation msg="LOGIN_ERR" />)
                setPassword('')
                setOnLoginRequest(false)
                return;
            }

            setOnLoginRequest(false)
        }).catch((err) => {
            toast.error(<Translation msg="LOGIN_ERR" />)
            setPassword('')
            setOnLoginRequest(false)
        })
    }

    return (
        <div className="container-lg d-flex justify-content-center align-items-center" style={{ flex: 1 }}>
            <div className="card w-100" style={{ width: "18rem" }}>
                <div className="card-body">
                    <h5 className="card-title text-center"><Translation msg="APP_NAME" /></h5>
                    <label><Translation msg="EMAIL" /></label>
                    <input type="text" className="form-control mb-2" value={username} onChange={handleUsernameChange} ></input>
                    <label><Translation msg="PASS" /></label>
                    <input type="password" className="form-control mb-2" value={password} onChange={handlePasswordChange} ></input>
                    <RequestButton classNames='btn btn-primary' style={{ width: "100%" }} onClick={sendLoginRequest} isOnRequest={onLoginRequest}><Translation msg="LOGIN" /></RequestButton>
                    <button type="button" className="btn btn-link" style={{ width: "100%" }} onClick={() => navigate('/register')}><Translation msg="REGISTER" /></button>
                </div>
            </div>
        </div>
    )
}

export default Login;


