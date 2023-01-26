import { useEffect, useState } from "react";
import Modal from 'react-modal';

import './NoteModal.css'

const customStyles = {
    content: {
        width: '60vw',
        height: '80vh',
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)',
        padding: '25px',
        display: 'flex',
        flexDirection: 'column',
        zIndex: 999999999999
    },
};

function NoteModal(props) {
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        setIsOpen(props.isOpen)
    }, [props.isOpen])

    function Tags(){
        props.note.tagButtons = []
        if (props.note.tags.length > 0) {
            for (let ind = 0; ind < props.note.tags.length; ind++) {
                props.note.tagButtons.push(<button
                    key={ind}
                    className="btn btn-sm btn-secondary mx-1 my-1"
                    >
                    {'#' + props.note.tags[ind]}
                </button>)

            }
        }

        return (
            props.note.tagButtons
        )
    }

    return (
        <Modal
            isOpen={isOpen}
            style={customStyles}
            contentLabel="Note Modal"
        >
            <div className="note-modal-header">
                <h2>{props.note.title}</h2>
                {props.children}
            </div>
            <hr />
            <div className="note-modal-body" dangerouslySetInnerHTML={{ __html: props.note.text }} />
            <hr />
            <div className="note-modal-footer">
                <Tags />
            </div>
        </Modal>
    )
}

export default NoteModal;