function NoteDetail(props) {
    return (
        <div className="container-lg d-flex justify-content-center align-items-center" style={{ flex: 1 }}>
           Note : {props.id}
        </div>
    )
}

export default NoteDetail;