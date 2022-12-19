import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import RequestButton from "../components/RequestButton";
import Translation from "../components/Translation";
import { USER_REGISTER_URL } from "../data/routes";

function Register() {

    const [registerModel, setRegisterModel] = useState({
        username: '',
        password: '',
        passwordAgain: ''
    });

    let navigate = useNavigate();

    const [onRegisterRequest, setOnRegisterRequest] = useState(false);

    function handleUsernameChange(event) {
        let updatedValue = {}
        updatedValue = { username: event.target.value }

        setRegisterModel(registerModel => ({
            ...registerModel,
            ...updatedValue
        }))
    }
    function handlePasswordChange(event) {
        let updatedValue = {}
        updatedValue = { password: event.target.value }

        setRegisterModel(registerModel => ({
            ...registerModel,
            ...updatedValue
        }))
    }

    function handlePasswordAgainChange(event) {
        let updatedValue = {}
        updatedValue = { passwordAgain: event.target.value }

        setRegisterModel(registerModel => ({
            ...registerModel,
            ...updatedValue
        }))
    }

    function isRegisterModelValid() {
        if (!registerModel) {
            toast.error(<Translation msg="ERR_WHILE_REGISTER" />);
            return false;
        }

        if (!registerModel.username || registerModel.username === '') {
            toast.warning(<Translation msg="USERNAME_CANNOT_EMPTY" />);
            return false;
        }

        if (!registerModel.password || registerModel.password === '') {
            toast.warning(<Translation msg="PASS_CANNOT_EMPTY" />);
            return false;
        }

        if (!registerModel.passwordAgain || registerModel.passwordAgain === '') {
            toast.warning(<Translation msg="PASS_AGAIN_CANNOT_EMPTY" />);
            return false;
        }

        if (registerModel.password !== registerModel.passwordAgain) {
            toast.warning(<Translation msg="SURE_PASS_MATCH" />);
            return false;
        }

        return true;
    }

    function registerationRequest() {
        if (!isRegisterModelValid()) {
            return;
        }

        setOnRegisterRequest(true)
        fetch(USER_REGISTER_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(registerModel)
        }).then(async (response) => {
            if (response.ok) {
                toast.success(<Translation msg="UESR_REGISTERED" />)
                navigate('/login')
            } else {
                const message = await response.text();
                toast.error(message)
            }
            setOnRegisterRequest(false)
        }).catch((err) => {
            toast.error(<Translation msg="UESR_CANNOT_REGISTERED" />)
            setOnRegisterRequest(false)
        })
    }

    return (
        <div className="container-lg d-flex justify-content-center align-items-center" style={{ flex: 1 }}>
            <div className="card w-100" style={{ width: "18rem" }}>
                <div className="card-body">
                    <h5 className="card-title text-center"><Translation msg="APP_NAME" /></h5>
                    <label><Translation msg="USERNAME" /></label>
                    <input type="text" className="form-control mb-2" value={registerModel.username} onChange={handleUsernameChange} ></input>
                    <label><Translation msg="PASS" /></label>
                    <input type="password" className="form-control mb-2" value={registerModel.password} onChange={handlePasswordChange} ></input>
                    <label><Translation msg="PASS_AGAIN" /></label>
                    <input type="password" className="form-control mb-2" value={registerModel.passwordAgain} onChange={handlePasswordAgainChange} ></input>
                    <RequestButton classNames='btn btn-primary' style={{ width: "100%" }} onClick={registerationRequest} isOnRequest={onRegisterRequest}><Translation msg="REGISTER" /></RequestButton>
                </div>
            </div>
        </div>
    )
}

export default Register;