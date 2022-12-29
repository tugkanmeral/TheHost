import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom'
import { DELETE_NOTE_URL, GET_NOTE_URL, INSERT_NOTE_URL, UPDATE_NOTE_URL } from '../../data/routes';

import { EditorState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import { convertToHTML } from 'draft-convert';
import { convertFromHTML } from 'draft-convert'
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import Translation from '../../components/Translation';
import RequestButton from '../../components/RequestButton';
import { toast } from 'react-toastify';

function NoteDetail() {
    const token = useSelector((state) => state.auth.token)

    const opts = {
        styleToHTML: (style) => {
            if (style === 'STRIKETHROUGH') {
                return {
                    start: '<s>',
                    end: '</s>'
                };
            }
        }
    }
    const toHTML = convertToHTML(opts);

    let { id } = useParams();

    const navigate = useNavigate()

    useEffect(() => {
        if (id)
            getNote(id)
    }, [id])

    const [note, setNote] = useState({
        title: '',
        text: '',
        tags: [],
        tagsStr: ''
    })

    const [editorState, setEditorState] = useState(
        () => EditorState.createEmpty(),
    );

    const convertContentToHTML = () => {
        let currentContentAsHTML = toHTML(editorState.getCurrentContent());

        let updatedValue = {}
        updatedValue = { text: currentContentAsHTML }

        setNote(noteItem => ({
            ...noteItem,
            ...updatedValue
        }))
    }
    const handleEditorChange = (state) => {
        setEditorState(state);
        convertContentToHTML();
    }
    function handleTitleChange(event) {
        let updatedValue = {}
        updatedValue = { title: event.target.value }

        setNote(model => ({
            ...model,
            ...updatedValue
        }))
    }
    function handleTagsChange(event) {
        let updatedValue = {}
        updatedValue = { tagsStr: event.target.value }

        setNote(model => ({
            ...model,
            ...updatedValue
        }))
    }

    function getNote(_id) {
        fetch(GET_NOTE_URL + id, {
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

            setNote(data)

            const editorState = EditorState.createWithContent(convertFromHTML(data.text));
            setEditorState(editorState)

        }).catch((err) => {
            console.error(err);
        })
    }

    function save() {
        if (!note.title) {
            return toast.warn(<Translation msg="NOTE_VALIDATION_MSG" />)
        }

        let url = ''
        let method = ''
        let noteModel = {
            title: note.title,
            text: note.text,
            tags: []
        }

        if (note.id) {
            url = UPDATE_NOTE_URL + note.id
            method = 'PUT'
        } else {
            url = INSERT_NOTE_URL
            method = 'POST'
        }

        if (note.tagsStr.length > 0) {
            noteModel.tags = note.tagsStr.substring(1).replace(" ", "").split('#')
        }

        fetch(url, {
            method: method,
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(noteModel)
        }).then(async (response) => {
            if (!response.ok) {
                console.error(response.status + ": " + response.statusText);
                return;
            }

            toast.success(<Translation msg="SAVED" />)
        }).catch((err) => {
            console.error(err);
        })
    }

    function remove(_id) {
        fetch(DELETE_NOTE_URL + _id, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then(async (response) => {
            if (!response.ok) {
                console.error(response.status + ": " + response.statusText);
                return;
            }

            toast.success(<Translation msg="NOTE_DELETED" />)
            navigate('/note')
        }).catch((err) => {
            console.error(err);
        })
    }

    function cancel() {
        navigate('/note')
    }

    return (
        <div className='container'>

            <div className='card p-3 mb-2' style={{ width: '100%' }}>
                <div className='card-body'>
                    <div>
                        <label><Translation msg="TITLE" /></label>
                        <input type="text" className="form-control mb-2" value={note.title || ''} onChange={handleTitleChange} ></input>

                        <label htmlFor="bioInput" className="form-label"><Translation msg="NOTE" /></label>
                        <Editor
                            className="form-control mb-2"
                            id="bioInput"
                            editorState={editorState}
                            onEditorStateChange={handleEditorChange}
                            wrapperClassName="wrapper-class"
                            editorClassName="editor-class"
                            toolbarClassName="toolbar-class"
                            toolbar={{
                                options: ['inline', 'blockType', 'list', 'textAlign', 'history'],
                                inline: {
                                    options: ['bold', 'italic', 'strikethrough', 'underline', 'monospace']
                                },
                                blockType: {
                                    inDropdown: true
                                },
                                list: { inDropdown: true },
                                textAlign: { inDropdown: true },
                                link: { inDropdown: true },
                            }}
                        />

                        <label htmlFor="bioInput" className="form-label mt-2"><Translation msg="TAGS" /></label>
                        <input type="text" className="form-control mb-2" value={note.tagsStr || ''} onChange={handleTagsChange} ></input>

                        <div className="d-flex flex-row-reverse mt-4">
                            <RequestButton
                                classNames="btn btn-primary mx-1"
                                onClick={save}>
                                <Translation msg="SAVE" />
                            </RequestButton >
                            {note.id ? <RequestButton
                                classNames="btn btn-danger mx-1"
                                onClick={() => remove(note.id)}>
                                <Translation msg="DELETE" />
                            </RequestButton > : null}
                            <div
                                className="btn btn-secondary mx-1"
                                onClick={cancel}>
                                <Translation msg="CANCEL" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    )
}

export default NoteDetail;