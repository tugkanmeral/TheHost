import { useEffect, useState } from "react"
import { useSelector } from "react-redux";
import { toast } from "react-toastify";
import Translation from "../../components/Translation"
import {
    GET_PASSWORD_URL,
    DELETE_PASSWORD_URL,
    POST_PASSWORD_URL,
    PUT_PASSWORD_URL
} from "../../data/routes";
import { aesGcmDecrypt, aesGcmEncrypt } from "../../helpers/crypto";
import RequestButton from '../../components/RequestButton'

function PasswordDetail(props) {
    const passwordConst = {
        id: '',
        title: '',
        username: '',
        detail: '',
        pass: ''
    }
    const [password, setPassword] = useState(passwordConst)
    const token = useSelector((state) => state.auth.token)
    const masterKey = useSelector((state) => state.user.masterKey)

    useEffect(() => {
        if (props.id)
            getPassword(props.id);
    }, [props.id])

    function getPassword(_passwordId) {
        if (!_passwordId)
            return;

        fetch(GET_PASSWORD_URL + _passwordId, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then(async (response) => {
            if (!response.ok) {
                console.error(response.status + ": " + response.statusText);
                toast.error(<Translation msg="ERR_OCCURED" />)
                return;
            }

            const data = await response.json();

            if (!data.pass) {
                toast.warn(<Translation msg="PASS_IS_EMPTY" />)
            }

            if (!masterKey) {
                toast.warn(<Translation msg="MASTER_KEY_IS_EMPTY" />)
            }

            try {
                let password = await aesGcmDecrypt(data.pass, masterKey)
                data.pass = password;
            } catch (error) {
                console.warn(error)
            }

            setPassword(data)
        }).catch((err) => {
            console.error(err);
        })
    }

    function handleTitleChange(event) {
        let updatedValue = {}
        updatedValue = { title: event.target.value }

        setPassword(model => ({
            ...model,
            ...updatedValue
        }))
    }
    function handleUsernameChange(event) {
        let updatedValue = {}
        updatedValue = { username: event.target.value }

        setPassword(model => ({
            ...model,
            ...updatedValue
        }))
    }
    function handleDetailChange(event) {
        let updatedValue = {}
        updatedValue = { detail: event.target.value }

        setPassword(model => ({
            ...model,
            ...updatedValue
        }))
    }
    function handlePasswordChange(event) {
        let updatedValue = {}
        updatedValue = { pass: event.target.value }

        setPassword(model => ({
            ...model,
            ...updatedValue
        }))
    }

    async function save() {
        if (!masterKey) {
            toast.error(<Translation msg="MASTER_KEY_CANNOT_BE_EMPTY" />)
            return;
        }

        let encryptedPass = await aesGcmEncrypt(password.pass, masterKey)

        let postOrPutURL = ''
        let method = ''
        if (password.id) {
            postOrPutURL = PUT_PASSWORD_URL + password.id
            method = 'PUT'
        }
        else {
            postOrPutURL = POST_PASSWORD_URL
            method = 'POST'
        }

        let passwordModel = {
            password: password.id,
            title: password.title,
            username: password.username,
            detail: password.detail,
            pass: encryptedPass
        }

        fetch(postOrPutURL, {
            method: method,
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(passwordModel)
        }).then(async (response) => {
            if (!response.ok) {
                console.error(response.status + ": " + response.statusText);
                toast.error(<Translation msg="ERR_OCCURED" />)
                return;
            }

            toast.success(<Translation msg="SAVED" />)
            cancel();
        }).catch((err) => {
            console.error(err);
        })

        cancel();
    }

    function remove() {
        fetch(DELETE_PASSWORD_URL + password.id, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then(async (response) => {
            if (!response.ok) {
                console.error(response.status + ": " + response.statusText);
                toast.error(<Translation msg="ERR_OCCURED" />)
                return;
            }

            toast.success(<Translation msg="PASSWORD_DELETED" />)
            cancel();
        }).catch((err) => {
            console.error(err);
        })
    }

    function cancel() {
        setPassword(passwordConst)
    }

    return (
        <div className='card p-3' style={{ width: '100%' }}>
            <div className="card-body">
                <h5 className="card-title text-center">{password?.title ? password.title : <Translation msg="NEW_PASS" />}</h5>
                <label><Translation msg="TITLE" /></label>
                <input type="text" className="form-control mb-2" value={password?.title} onChange={handleTitleChange} ></input>
                <label><Translation msg="USERNAME" /></label>
                <input type="text" className="form-control mb-2" value={password?.username} onChange={handleUsernameChange} ></input>
                <label><Translation msg="DETAIL" /></label>
                <input type="text" className="form-control mb-2" value={password?.detail} onChange={handleDetailChange} ></input>
                <label><Translation msg="PASS" /></label>
                <input type="password" className="form-control mb-2" value={password?.pass} onChange={handlePasswordChange} ></input>
                <div className="d-flex flex-row-reverse">
                    <RequestButton
                        classNames="btn btn-primary mx-1"
                        onClick={save}>
                        <Translation msg="SAVE" />
                    </RequestButton >
                    <RequestButton
                        classNames="btn btn-danger mx-1"
                        onClick={remove}>
                        <Translation msg="DELETE" />
                    </RequestButton >
                    <div
                        className="btn btn-secondary mx-1"
                        onClick={cancel}>
                        <Translation msg="CANCEL" />
                    </div>
                </div>
            </div>
        </div>
    )
}

export default PasswordDetail