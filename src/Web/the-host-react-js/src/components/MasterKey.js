import { useEffect, useState } from "react";
import { FaLock, FaLockOpen } from "react-icons/fa";
import { useDispatch, useSelector } from "react-redux";
import { setMasterKey } from './../store/reducers/userReducer'
import Translation from "./Translation";

function MasterKey() {
    const [masterKey, setMasterKeyState] = useState()
    const [masterKeyLock, setMasterKeyLock] = useState(false)

    const masterKeyState = useSelector((state) => state.user.masterKey)
    const dispatch = useDispatch()

    useEffect(() => {
        setMasterKeyFromState()
    }, []);

    function setMasterKeyFromState() {
        if (masterKeyState) {
            setMasterKeyLock(true)
            setMasterKeyState(masterKeyState)
        }
    }

    function handleMasterKeyChange(event) {
        setMasterKeyState(event.target.value)
    }

    function lockMasterKey() {
        if (masterKeyLock) {
            setMasterKeyLock(false)
            setMasterKeyState(null)
            dispatch(setMasterKey(null))
        }
        else if (masterKey) {
            dispatch(setMasterKey(masterKey))
            setMasterKeyLock(true)
        }
        // lock and unlock if there is no writen masterKey 
        else {
            setMasterKeyLock(true)
            setTimeout(() => {
                setMasterKeyLock(false)
            }, 250)
        }
    }

    return (
        <div className="card p-2 mb-2">
            <div style={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center'
            }}>
                <label
                    style={{
                        whiteSpace: 'pre',
                        marginRight: '15px'
                    }}
                >
                    <Translation msg="MASTER_KEY" />
                </label>
                <input
                    type="password"
                    className="form-control"
                    onChange={handleMasterKeyChange}
                    disabled={masterKeyLock}
                    value={masterKey || ''}
                ></input>
                <button className="btn btn-sm" onClick={lockMasterKey}>
                    {
                        masterKeyLock ?
                            <FaLock /> :
                            <FaLockOpen />
                    }
                </button>
            </div>
        </div>
    )
}

export default MasterKey