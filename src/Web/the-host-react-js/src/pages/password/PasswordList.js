import { useState, useEffect, useRef } from "react";
import { useSelector } from "react-redux";
import Translation from '../../components/Translation'
import { GET_PASSWORDS_URL, GET_PASSWORD_URL } from '../../data/routes';
import Skeleton from '../../components/Skeleton'
import { FaCopy } from 'react-icons/fa'
import { aesGcmDecrypt } from "../../helpers/crypto";
import { toast } from 'react-toastify';
import Paging from "../../components/Paging";

function PasswordList(props) {
    const [passwords, setPasswords] = useState()
    const [activePage, setActivePage] = useState(1)
    const [pageCount, setPageCount] = useState(1)
    const TAKE = 10

    useEffect(() => {
        getPasswords(1)
    }, []);

    const token = useSelector((state) => state.auth.token)
    const masterKey = useSelector((state) => state.user.masterKey)

    function getPasswords(_page) {
        let skip = (_page - 1) * TAKE

        fetch(GET_PASSWORDS_URL + '?take=' + TAKE + '&skip=' + skip, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then(async (response) => {
            if (!response.ok) {
                console.error(response.status + ": " + response.statusText);
                return;
            }

            const data = await response.json();
            setPasswords(data.items)

            let pagesCount = data.totalItemCount % data.take == 0 ? (data.totalItemCount / data.take) : (parseInt(data.totalItemCount / data.take) + 1)
            setPageCount(pagesCount)
        }).catch((err) => {
            console.error(err);
        })
    }

    function copyPassword(_id) {
        fetch(GET_PASSWORD_URL + _id, {
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
                return
            }

            if (!masterKey) {
                toast.warn(<Translation msg="MASTER_KEY_IS_EMPTY" />)
                return
            }

            var password = await aesGcmDecrypt(data.pass, masterKey)
            navigator.clipboard.writeText(password);
            toast.success(<Translation msg="COPIED" />)
        }).catch((err) => {
            console.error(err);
        })
    }

    function PasswordRows() {
        if (!passwords)
            return;

        const listItems = passwords.map((password, index) =>
            <tr key={index}>
                <th scope="row">{(activePage - 1) * TAKE + index + 1}</th>
                <td>{password.title}</td>
                <td>{password.username}</td>
                <td>{password.detail}</td>
                <td>
                    <FaCopy style={{ cursor: 'pointer' }} onClick={() => copyPassword(password.id)} />
                </td>
            </tr>
        );
        
        return listItems;
    }

    function pageChanged(_page) {
        setActivePage(_page)
        getPasswords(_page)
    }

    return (
        <div className='card p-3' style={{ width: '100%' }}>
            {
                passwords ?
                    <>
                        <table className="table table-sm">
                            <thead>
                                <tr>
                                    <th scope="col">#</th>
                                    <th scope="col"><Translation msg="TITLE" /></th>
                                    <th scope="col"><Translation msg="USERNAME" /></th>
                                    <th scope="col"><Translation msg="DETAIL" /></th>
                                    <th scope="col">
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <PasswordRows />
                            </tbody>
                        </table>
                        <Paging activePage={activePage} pageCount={pageCount} onClick={(page) => pageChanged(page)} />
                    </> :
                    <Skeleton />
            }

        </div>
    )
}

export default PasswordList;