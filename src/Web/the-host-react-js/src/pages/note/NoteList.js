import { useEffect, useState } from "react";
import { FaEdit, FaEye, FaPlus, FaTimes, FaUndo } from "react-icons/fa";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import Paging from "../../components/Paging";
import Skeleton from "../../components/Skeleton";
import Translation from "../../components/Translation";
import { GET_NOTES_URL, GET_NOTE_URL } from "../../data/routes";
import NoteModal from "./NoteModal"

function NoteList(props) {
    const token = useSelector((state) => state.auth.token)

    const [filters, setFilters] = useState([])
    const [searchText, setSearchText] = useState("")
    const [notes, setNotes] = useState([])
    const [activePage, setActivePage] = useState([])
    const [pageCount, setPageCount] = useState([])
    const [previewNote, setPreviewNote] = useState({})
    const [modalIsOpen, setModalIsOpen] = useState(false);

    const TAKE = 10

    useEffect(() => {
        getNotes(1)
    }, [filters])

    useEffect(() => {
        getNotes(1)
    }, [searchText])

    const navigate = useNavigate()

    function getNotes(_page) {
        let url = GET_NOTES_URL + '?take=' + TAKE + '&skip='
        let skip = (_page - 1) * TAKE
        url += skip

        if (filters.length > 0) {
            url += '&tags=' + filters.join('&tags=')
        }

        if (searchText.length > 0) {
            url += '&searchText=' + searchText
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
        setActivePage(_page)
        getNotes(_page)
    }

    function refreshList() {
        getNotes(1)
    }

    function newNote() {
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

    function handleSearchTextChange(event) {
        setSearchText(event.target.value)
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
                    className="badge bg-warning text-dark mx-2 my-1"
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
                className="badge bg-warning text-dark mx-1 my-1"
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
                        className={filters.includes(notes[index].tags[ind]) ? "btn btn-sm btn-warning mx-1 my-1" : "btn btn-sm btn-secondary mx-1 my-1"}
                        onClick={() => filterBy(filter)}>
                        {'#' + notes[index].tags[ind]}
                    </button>)

                }
            }
        }

        const listItems = notes.map((note, index) =>
            <tr key={index}>
                <th scope="row" className="col-1">{(activePage - 1) * TAKE + index + 1}</th>
                <td className="col-5">{note.title}</td>
                <td className="col-5">{note.tagButtons.length > 0 ? note.tagButtons : '-'}</td>
                <td className="col-1">
                    <FaEdit style={{ cursor: 'pointer', marginRight: '15px' }} onClick={() => selectNote(note.id)} />
                    <FaEye style={{ cursor: 'pointer', }} onClick={() => openNoteModal(note.id)} />
                </td>
            </tr>
        );

        return listItems;
    }

    function openNoteModal(_id) {
        fetch(GET_NOTE_URL + _id, {
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

            if (data && data.tags && data.tags.length > 0) {
                data.tagsStr = '#' + data.tags.join(' #')
            }

            setPreviewNote(data)
            setModalIsOpen(true);
        }).catch((err) => {
            console.error(err);
        })

    }

    return (
        <div className='card p-3 mb-2' style={{ width: '100%' }}>
            {
                notes ?
                    <>
                        <div className="row align-items-center justify-content-center">
                            <div className="col-sm-2">
                                <Translation msg="TEXT_SEARCH" /> :
                            </div>
                            <div className="col-sm-10">
                                <input
                                    type="text"
                                    className="form-control float-right"
                                    value={searchText || ''}
                                    onChange={handleSearchTextChange} >
                                </input>
                            </div>
                        </div>
                        <hr />
                        <table className="table table-sm">
                            <thead>
                                <tr>
                                    <th scope="col" className="col-1"><FaPlus style={{ cursor: 'pointer' }} onClick={newNote} /></th>
                                    <th scope="col" className="col-5"><Translation msg="TITLE" /></th>
                                    <th scope="col" className="col-5">
                                        <Translation msg="TAGS" />
                                        <ShowFilters />
                                    </th>
                                    <th scope="col" className="col-1"><FaUndo style={{ cursor: 'pointer' }} onClick={refreshList} />
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

            <NoteModal note={previewNote} isOpen={modalIsOpen} >
                <div
                    style={{ cursor: 'pointer' }}
                    onClick={() => setModalIsOpen(false)}>
                    <FaTimes size='1.3em' />
                </div>
            </NoteModal>
        </div>
    )
}

export default NoteList;