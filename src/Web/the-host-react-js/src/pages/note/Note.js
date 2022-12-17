import { useState } from "react";
import NoteDetail from "./NoteDetial";
import NoteList from "./NoteList";

function Note() {
    const [selectedNoteId, setSelectedNoteId] = useState('')

    function selectNote(_noteId) {
        setSelectedNoteId(_noteId)
    }

    return (
        <div className="container">
            <NoteList onClick={(_selectedNoteId) => selectNote(_selectedNoteId)} />
            <NoteDetail id={selectedNoteId} />
        </div>
    )
}

export default Note;