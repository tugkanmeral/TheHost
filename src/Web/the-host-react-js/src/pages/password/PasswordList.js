import { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import Translation from '../../components/Translation'
import { GET_PASSWORDS_URL, GET_PASSWORD_URL } from '../../data/routes';
import Skeleton from '../../components/Skeleton'
import { FaUndo, FaCopy, FaEdit } from 'react-icons/fa'
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
            setActivePage(_page)
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

            let password = await aesGcmDecrypt(data.pass, masterKey)
            navigator.clipboard.writeText(password);
            toast.success(<Translation msg="COPIED" />)
        }).catch((err) => {
            console.error(err);
            toast.error(<Translation msg="ERR_OCCURED" />)
        })
    }

    function PasswordRows() {
        if (!passwords)
            return;

        const listItems = passwords.map((password, index) =>
            <tr key={index}>
                <th scope="row" className="col-1">{(activePage - 1) * TAKE + index + 1}</th>
                <td className="col-3">{password.title}</td>
                <td className="col-3">{password.username}</td>
                <td className="col-2">
                    {masterKey ? <FaEdit style={{ cursor: 'pointer' }} onClick={() => selectPassword(password.id)} /> : null}
                    {masterKey ? <FaCopy style={{ cursor: 'pointer' }} onClick={() => copyPassword(password.id)} /> : null}
                </td>
            </tr>
        );

        return listItems;
    }

    function pageChanged(_page) {
        setActivePage(_page)
        getPasswords(_page)
    }

    function selectPassword(_id) {
        props.onClick(_id)
    }

    function refreshList() {
        getPasswords(1)
    }

    return (
        <div className='card p-3 mb-2' style={{ width: '100%' }}>
            {
                passwords ?
                    <>
                        <table className="table table-sm">
                            <thead>
                                <tr>
                                    <th scope="col" className="col-1">#</th>
                                    <th scope="col" className="col-3"><Translation msg="TITLE" /></th>
                                    <th scope="col" className="col-3"><Translation msg="USERNAME" /></th>
                                    <th scope="col" className="col-2"><FaUndo style={{ cursor: 'pointer' }} onClick={refreshList} />
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