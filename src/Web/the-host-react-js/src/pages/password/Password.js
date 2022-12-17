import { useState } from "react";
import MasterKey from "../../components/MasterKey";
import PasswordDetail from "./PasswordDetail";
import PasswordList from "./PasswordList";

function Password() {
    const [selectedPasswordId, setSelectedPasswordId] = useState('')

    function selectPassword(_passwordId) {
        setSelectedPasswordId(_passwordId)
    }

    return (
        <div className="container">
            <MasterKey />
            <div className="row">
                <div className="col-sm">
                    <PasswordList onClick={(selectedPasswordId) => selectPassword(selectedPasswordId)} />
                </div>
                <div className="col-sm">
                    <PasswordDetail id={selectedPasswordId} />
                </div>
            </div>
        </div>
    )
}

export default Password;