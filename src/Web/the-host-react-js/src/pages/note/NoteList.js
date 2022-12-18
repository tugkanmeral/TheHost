import { useEffect, useState } from "react";
import { FaEdit, FaPlus, FaUndo } from "react-icons/fa";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import Paging from "../../components/Paging";
import Skeleton from "../../components/Skeleton";
import Translation from "../../components/Translation";
import { GET_NOTES_URL } from "../../data/routes";

function NoteList(props) {
    const token = useSelector((state) => state.auth.token)

    const [filters, setFilters] = useState([])
    const [notes, setNotes] = useState([])
    const [activePage, setActivePage] = useState([])
    const [pageCount, setPageCount] = useState([])
    const TAKE = 10

    useEffect(() => {
        getNotes(1)
    }, [])

    useEffect(() => {
        getNotes(1)
    }, [filters])

    const navigate = useNavigate()

    function getNotes(_page) {
        let url = GET_NOTES_URL + '?take=' + TAKE + '&skip='
        let skip = (_page - 1) * TAKE
        url += skip

        if (filters.length > 0) {
            url += '&tags=' + filters.join('&tags=')
        }

        fetch(url, {
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
            setNotes(data.items)

            let pagesCount = data.totalItemCount % data.take === 0 ? (data.totalItemCount / data.take) : (parseInt(data.totalItemCount / data.take) + 1)
            setPageCount(pagesCount)
            setActivePage(_page)
        }).catch((err) => {
            console.error(err);
        })
    }

    function pageChanged(_page) {

    }

    function refreshList() {
        getNotes(1)
    }

    function newNote(){
        navigate('/note/detail')
    }

    function selectNote(_id) {
        navigate('/note/detail/' + _id)
    }

    function filterBy(_filter) {
        if (filters.includes(_filter)) {
            removeFilter(_filter)
        }
        else {
            let filterList = filters
            filterList.push(_filter)
            setFilters([...new Set(filterList)])
        }
    }

    function removeFilter(_filter) {
        const filterList = filters
        const index = filterList.indexOf(_filter);

        if (index > -1) {
            filterList.splice(index, 1)
        }

        setFilters([...filterList])
    }

    function removeFilters() {
        setFilters([])
    }

    function ShowFilters() {
        if (filters.length > 3) {
            return (
                <span
                    style={{ cursor: 'pointer' }}
                    className="badge bg-warning text-dark mx-2"
                    onClick={() => removeFilters()}
                >
                    {filters.length}
                </span>
            )
        }

        const listItems = filters.map((filter, index) =>
            <span
                style={{ cursor: 'pointer' }}
                key={index}
                className="badge bg-warning text-dark mx-1"
                onClick={() => removeFilter(filter)}
            >
                {filter}
            </span>
        );

        return (
            listItems
        )
    }

    function NoteRows() {
        if (!notes)
            return;

        for (let index = 0; index < notes.length; index++) {
            notes[index].tagButtons = []
            if (notes[index].tags.length > 0) {
                for (let ind = 0; ind < notes[index].tags.length; ind++) {
                    let filter = notes[index].tags[ind]
                    notes[index].tagButtons.push(<button
                        key={index + '' + ind}
                        className={filters.includes(notes[index].tags[ind]) ? "btn btn-sm btn-warning mx-1" : "btn btn-sm btn-secondary mx-1"}
                        onClick={() => filterBy(filter)}>
                        {'#' + notes[index].tags[ind]}
                    </button>)

                }
            }
        }

        const listItems = notes.map((note, index) =>
            <tr key={index}>
                <th scope="row">{(activePage - 1) * TAKE + index + 1}</th>
                <td>{note.title}</td>
                <td>{note.tagButtons.length > 0 ? note.tagButtons : '-'}</td>
                <td>
                    <FaEdit style={{ cursor: 'pointer' }} onClick={() => selectNote(note.id)} />
                </td>
            </tr>
        );

        return listItems;
    }

    return (
        <div className='card p-3 mb-2' style={{ width: '100%' }}>
            {
                notes ?
                    <>
                        <table className="table table-sm">
                            <thead>
                                <tr>
                                    <th scope="col"><FaPlus style={{ cursor: 'pointer' }} onClick={newNote} /></th>
                                    <th scope="col"><Translation msg="TITLE" /></th>
                                    <th scope="col">
                                        <Translation msg="TAGS" />
                                        <ShowFilters />
                                    </th>
                                    <th scope="col"><FaUndo style={{ cursor: 'pointer' }} onClick={refreshList} />
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <NoteRows />
                            </tbody>
                        </table>
                        <Paging activePage={activePage} pageCount={pageCount} onClick={(page) => pageChanged(page)} />
                    </> :
                    <Skeleton />
            }
        </div>
    )
}

export default NoteList;